# Notes API v2

Assignment 2. Same Notes API as before but now split into layers, with categories and DTOs.
Data is kept in memory so it is lost when the app restarts.

## Layers

```
        Client (Swagger)
              |
              |  DTOs in / DTOs out
              v
   +----------------------+
   |     Controllers      |   NotesController, CategoriesController
   |  (no logic, only     |   takes the request, returns status code
   |   http stuff)        |
   +----------------------+
              |
              v
   +----------------------+
   |       Services       |   NoteService, CategoryService
   |  (business rules,    |   validation, max notes limit, sorting,
   |   dto mapping)       |   throws NotFound / Validation / Conflict
   +----------------------+
              |
              |  entities only
              v
   +----------------------+
   |     Repositories     |   NoteRepository, CategoryRepository
   |  (only ones touching |   GetAll, GetById, Add, Update, Delete,
   |   the data store)    |   GetByCategoryId, Search
   +----------------------+
              |
              v
   +----------------------+
   |  InMemoryDataStore   |   2 dictionaries + 2 id counters
   |  (the "database")    |   seeded with Personal, Work, Study
   +----------------------+
```

Middleware order: RequestLoggingMiddleware -> ExceptionHandlingMiddleware -> controllers.

## Endpoints

Notes
- GET /notes?categoryId=&search=&includeArchived=
- GET /notes/{id}
- POST /notes
- PUT /notes/{id}  (archive / unarchive is done by sending isArchived here)
- DELETE /notes/{id}

Categories
- GET /categories
- POST /categories
- DELETE /categories/{id}
- GET /categories/summary

Errors always come back like this:

```json
{ "statusCode": 404, "message": "Note with id 9 was not found." }
```

| Code | When |
|------|------|
| 200 | get / update worked |
| 201 | created, Location header has the new url |
| 204 | deleted |
| 400 | empty title, category does not exist, max notes reached, deleting a category that still has notes |
| 404 | note or category id not found |
| 409 | category name already exists |

Max notes is set in appsettings.json under NoteSettings:MaxNotes.

## How to run

```
dotnet run
```

Then open /swagger.

## DI lifetimes

### Why InMemoryDataStore must be Singleton

The store is the only place where the data lives. Singleton means one object is created and
every request gets that same object.

If it was Scoped or Transient, a new store would be created for every request. So a note
added with POST would be gone in the next GET, because that GET gets a fresh empty store.
The constructor would also run again every time, so only the 3 seeded categories would exist.

### Why repositories and services can be Scoped

They do not keep any data themselves. They only have a reference to the store (or to a
repository). So it does not matter that a new one is made for each request, they all end up
pointing to the same singleton store. Scoped is also what will be needed later with EF Core,
because DbContext is scoped.

### What goes wrong if a Singleton depends on a Scoped service

The singleton is created once and lives for the whole app. If it takes a scoped service in
its constructor it keeps holding that one object forever, even after the request it belonged
to is finished. So the scoped service is no longer really scoped, it behaves like a singleton.
This is called a captive dependency. With a real DbContext this would mean one context shared
by all requests, which is not thread safe and keeps old data.

ASP.NET Core checks this in Development and throws an error at startup like
"Cannot consume scoped service from singleton".

That is why the dependency goes only one way here: scoped (repository) -> singleton (store).

### Bonus: two writes at the same time

The store is shared by all requests and Dictionary is not thread safe. If two POST requests
come at the same time:

- both can read the same NextNoteId before either one increases it, so two notes get the
  same id and one of them fails or overwrites the other
- the dictionary can get corrupted inside if two threads add at the same moment
- a GET that is looping over the dictionary while another request adds to it throws
  "Collection was modified"

To protect it the store has one `Lock` object and every repository method does its work
inside `lock (_store.Lock)`. So only one request can read or write the data at a time.

One thing the lock does not fully cover: the max notes check is in the service
(count first, then add). Two requests at the exact same time could both pass the check.
To fix it properly the check and the add would have to be inside the same lock.

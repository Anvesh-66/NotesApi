using NotesApi.Services;
using NotesApi.Middleware;
using NotesApi.Model;
using NotesApi.Data;
using NotesApi.Repositories;
var builder = WebApplication.CreateBuilder(args);

//without this option asp.net itself rejects empty strings before my service validation runs
builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.AddSwaggerGen();

//store is singleton because it holds the data, rest are scoped (explained in README)
builder.Services.AddSingleton<InMemoryDataStore>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.Configure<NoteSettings>(builder.Configuration.GetSection("NoteSettings"));


var app = builder.Build();

//timing is first so it also measures the error handling
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
app.UseSwagger();
app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

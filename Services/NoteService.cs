namespace NotesApi.Services;
using NotesApi.Model;
using NotesApi.Dtos;
using NotesApi.Mappers;
using NotesApi.Exceptions;
using NotesApi.Repositories;
using Microsoft.Extensions.Options;
public class NoteService : INoteService{
    private readonly INoteRepository _noteRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly int _maxNotes;

    public NoteService(INoteRepository noteRepository, ICategoryRepository categoryRepository, IOptions<NoteSettings> options){
        _noteRepository = noteRepository;
        _categoryRepository = categoryRepository;
        _maxNotes = options.Value.MaxNotes;
    }

    public List<NoteResponseDto> GetAll(int? categoryId, string? search, bool includeArchived){
        List<Note> notes;
        if (!string.IsNullOrWhiteSpace(search))
        {
            notes = _noteRepository.Search(search.Trim());
        }
        else if (categoryId != null)
        {
            notes = _noteRepository.GetByCategoryId(categoryId.Value);
        }
        else
        {
            notes = _noteRepository.GetAll();
        }

        //search and categoryId can come together, and archived ones are hidden by default
        var filtered = new List<Note>();
        foreach (var note in notes)
        {
            if (!includeArchived && note.IsArchived)
            {
                continue;
            }
            if (categoryId != null && note.CategoryId != categoryId.Value)
            {
                continue;
            }
            filtered.Add(note);
        }

        filtered.Sort(CompareNewestFirst);

        //getting all category names once instead of asking repository for every note
        var categoryNames = new Dictionary<int, string>();
        foreach (var category in _categoryRepository.GetAll())
        {
            categoryNames[category.Id] = category.Name;
        }

        var result = new List<NoteResponseDto>();
        foreach (var note in filtered)
        {
            string name = "";
            if (categoryNames.ContainsKey(note.CategoryId))
            {
                name = categoryNames[note.CategoryId];
            }
            result.Add(DtoMapper.ToNoteResponse(note, name));
        }
        return result;
    }

    public NoteResponseDto GetById(int id){
        var note = FindNote(id);
        return ToResponse(note);
    }

 public NoteResponseDto Create(CreateNoteDto dto){
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ValidationException("Title is required.");
        }
        if (dto.Content == null)
        {
            dto.Content = "";
        }
        var category = _categoryRepository.GetById(dto.CategoryId);
        if (category == null)
        {
            throw new ValidationException("Category with id " + dto.CategoryId + " does not exist.");
        }
        //same limit as assignment 1, value comes from appsettings
        if (_noteRepository.GetAll().Count >= _maxNotes)
        {
            throw new ValidationException("Maximum number of notes reached.");
        }

        var note = DtoMapper.ToNote(dto);
        note.CreatedAt = DateTime.Now;
        var created = _noteRepository.Add(note);
        return DtoMapper.ToNoteResponse(created, category.Name);
    }

    public NoteResponseDto Update(int id, UpdateNoteDto dto){
        var note = FindNote(id);
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ValidationException("Title is required.");
        }
        if (dto.Content == null)
        {
            dto.Content = "";
        }
        //category must exist before the note is moved to it
        var category = _categoryRepository.GetById(dto.CategoryId);
        if (category == null)
        {
            throw new ValidationException("Category with id " + dto.CategoryId + " does not exist.");
        }

        note.Title = dto.Title;
        note.Content = dto.Content;
        note.CategoryId = dto.CategoryId;
        note.IsArchived = dto.IsArchived;
        note.UpdatedAt = DateTime.Now;
        _noteRepository.Update(note);
        return DtoMapper.ToNoteResponse(note, category.Name);
    }

    public void Delete(int id){
        var deleted = _noteRepository.Delete(id);
        if (!deleted)
        {
            throw new NotFoundException("Note with id " + id + " was not found.");
        }
    }

    public NoteResponseDto Archive(int id){
        return SetArchived(id, true);
    }
    public NoteResponseDto Unarchive(int id){
        return SetArchived(id, false);
    }

    private NoteResponseDto SetArchived(int id, bool isArchived){
        var note = FindNote(id);
        note.IsArchived = isArchived;
        note.UpdatedAt = DateTime.Now;
        _noteRepository.Update(note);
        return ToResponse(note);
    }

    private Note FindNote(int id){
        var note = _noteRepository.GetById(id);
        if (note == null)
        {
            throw new NotFoundException("Note with id " + id + " was not found.");
        }
        return note;
    }

    private NoteResponseDto ToResponse(Note note){
        string name = "";
        var category = _categoryRepository.GetById(note.CategoryId);
        if (category != null)
        {
            name = category.Name;
        }
        return DtoMapper.ToNoteResponse(note, name);
    }

    //for List.Sort, b before a gives newest first
    private static int CompareNewestFirst(Note a, Note b){
        int result = b.CreatedAt.CompareTo(a.CreatedAt);
        if (result == 0)
        {
            result = b.Id.CompareTo(a.Id);
        }
        return result;
    }
}

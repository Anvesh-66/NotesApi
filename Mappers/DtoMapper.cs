namespace NotesApi.Mappers;
using NotesApi.Model;
using NotesApi.Dtos;


public static class DtoMapper
{
    public static Note ToNote(CreateNoteDto dto){
        var note = new Note();
        note.Title = dto.Title;
        note.Content = dto.Content;
        note.CategoryId = dto.CategoryId;
        return note;
    }

    //category name is passed from service because note only has the CategoryId
    public static NoteResponseDto ToNoteResponse(Note note, string categoryName){
        var dto = new NoteResponseDto();
        dto.Id = note.Id;
        dto.Title = note.Title;
        dto.Content = note.Content;
        dto.CategoryName = categoryName;
        dto.CreatedAt = note.CreatedAt;
        dto.UpdatedAt = note.UpdatedAt;
        return dto;
    }

    public static Category ToCategory(CreateCategoryDto dto){
        var category = new Category();
        category.Name = dto.Name;
        return category;
    }
    public static CategoryResponseDto ToCategoryResponse(Category category){
        var dto = new CategoryResponseDto();
        dto.Id = category.Id;
        dto.Name = category.Name;
        return dto;
    }
    public static CategorySummaryDto ToCategorySummary(Category category, int noteCount){
        var dto = new CategorySummaryDto();
        dto.Id = category.Id;
        dto.Name = category.Name;
        dto.NoteCount = noteCount;
        return dto;
    }
}

namespace NotesApi.Services;
using NotesApi.Dtos;

public interface ICategoryService{
    List<CategoryResponseDto> GetAll();
    CategoryResponseDto Create(CreateCategoryDto dto);
    void Delete(int id);
    List<CategorySummaryDto> GetSummary();
}

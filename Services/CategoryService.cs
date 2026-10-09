namespace NotesApi.Services;
using NotesApi.Dtos;
using NotesApi.Mappers;
using NotesApi.Exceptions;
using NotesApi.Repositories;
public class CategoryService : ICategoryService{
    private readonly ICategoryRepository _categoryRepository;
    private readonly INoteRepository _noteRepository;

    public CategoryService(ICategoryRepository categoryRepository, INoteRepository noteRepository){
        _categoryRepository = categoryRepository;
        _noteRepository = noteRepository;
    }

    public List<CategoryResponseDto> GetAll(){
        var result = new List<CategoryResponseDto>();
        foreach (var category in _categoryRepository.GetAll())
        {
            result.Add(DtoMapper.ToCategoryResponse(category));
        }
        return result;
    }

    public CategoryResponseDto Create(CreateCategoryDto dto){
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ValidationException("Category name is required.");
        }
        dto.Name = dto.Name.Trim();

        //"work" and "Work" should be treated as same name
        foreach (var existing in _categoryRepository.GetAll())
        {
            if (existing.Name.ToLower() == dto.Name.ToLower())
            {
                throw new ConflictException("Category " + dto.Name + " already exists.");
            }
        }

        var category = DtoMapper.ToCategory(dto);
        var created = _categoryRepository.Add(category);
        return DtoMapper.ToCategoryResponse(created);
    }

    public void Delete(int id){
        var category = _categoryRepository.GetById(id);
        if (category == null)
        {
            throw new NotFoundException("Category with id " + id + " was not found.");
        }
        //cannot delete if notes are still using this category
        var notes = _noteRepository.GetByCategoryId(id);
        if (notes.Count > 0)
        {
            throw new ValidationException("Category still has notes, so it cannot be deleted.");
        }
        _categoryRepository.Delete(id);
    }

    public List<CategorySummaryDto> GetSummary(){
        //key = category id, value = how many notes are in it
        var counts = new Dictionary<int, int>();
        foreach (var note in _noteRepository.GetAll())
        {
            if (counts.ContainsKey(note.CategoryId))
            {
                counts[note.CategoryId]++;
            }
            else
            {
                counts[note.CategoryId] = 1;
            }
        }

        var result = new List<CategorySummaryDto>();
        foreach (var category in _categoryRepository.GetAll())
        {
            int count = 0;
            if (counts.ContainsKey(category.Id))
            {
                count = counts[category.Id];
            }
            result.Add(DtoMapper.ToCategorySummary(category, count));
        }
        return result;
    }
}

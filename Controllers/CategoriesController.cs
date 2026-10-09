using Microsoft.AspNetCore.Mvc;
using NotesApi.Dtos;
using NotesApi.Services;

namespace NotesApi.Controllers;
[ApiController]
[Route("categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

public CategoriesController(ICategoryService categoryService)
{
    _categoryService = categoryService;
}

[HttpGet]
[ProducesResponseType(typeof(List<CategoryResponseDto>), StatusCodes.Status200OK)]
public IActionResult GetAll(){
    return Ok(_categoryService.GetAll());
}

[HttpPost]
[ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status201Created)]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
public IActionResult Create(CreateCategoryDto dto)
{
    var created = _categoryService.Create(dto);
    //there is no GET /categories/{id} so giving the location url directly
    return Created("/categories/" + created.Id, created);
}
[HttpDelete("{id}")]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
public IActionResult Delete(int id)
{
    _categoryService.Delete(id);
    return NoContent();
}

[HttpGet("summary")]
[ProducesResponseType(typeof(List<CategorySummaryDto>), StatusCodes.Status200OK)]
public IActionResult GetSummary(){
    return Ok(_categoryService.GetSummary());
}
}

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserManager.Application.Contracts.Pagination;
using UserManager.Application.Contracts.Products;
using UserManager.Application.Services;

namespace UserManager.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(IProductService productService, IValidator<CreateProductRequest> validator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PageResponse<ProductResponse>>> GetPaged([FromQuery] ProductQueryParams query, CancellationToken cancellationToken)
        => Ok(await productService.GetPagedAsync(query, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdAsync(id, cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid) return BadRequest(validation.ToDictionary());
        var product = await productService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        => await productService.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}

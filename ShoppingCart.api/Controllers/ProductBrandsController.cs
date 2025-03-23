using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.api.Attributes;
using ShoppingCart.data.DataModels.Dtos;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Models;
using ShoppingCart.data.Services.Implementations;
using ShoppingCart.data.Services.Interfaces;
using System.Text.Json;

namespace ShoppingCart.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBrandsController : ControllerBase
    {
        private readonly IProductBrandService brandService;
        private readonly IMapper mapper;

        private readonly int maxPageSize = 20;

        public ProductBrandsController(IProductBrandService brandService, IMapper mapper)
        {
            this.brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Cache(864000)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetAllProductBrandsAsync
           (string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var (productBrandEntities, paginationMetadata) =
                await brandService.GetAllProductBrandsAsync(searchQuery, pageNumber, pageSize);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(mapper.Map<IEnumerable<ProductBrand>>(productBrandEntities));
        }

        [Cache(864000)]
        [HttpGet("{id}", Name = "GetProductBrand")]
        public async Task<ActionResult<ProductBrand>> GetProductBrandById(int id)
        {
            ProductBrandEntity? productBrandEntity = await brandService.GetProductBrandByIdAsync(id);
            if (productBrandEntity == null)
            {
                return NotFound("Brand with the provided id was not found");
            }
            return Ok(mapper.Map<ProductBrand>(productBrandEntity));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductBrand>> AddProductBrand(ProductBrandDto productBrandDto)
        {
            if (productBrandDto == null)
            {
                return BadRequest("Product brand to add was not provided");
            }

            ProductBrandEntity productBrandEntity = mapper.Map<ProductBrandEntity>(productBrandDto);

            await brandService.AddProductBrandAsync(productBrandEntity);
            await brandService.SaveChangesAsync();

            ProductBrand productBrandToReturn = mapper.Map<ProductBrand>(productBrandEntity);

            return CreatedAtRoute("GetProductBrand", new
            {
                Id = productBrandEntity.Id,
            }, productBrandToReturn);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateProductBrand(int id, ProductBrandDto updatedProductBrand)
        {
            ProductBrandEntity? productBrandEntity = await brandService.GetProductBrandByIdAsync(id);
            if (productBrandEntity == null)
            {
                return BadRequest("Product brand with provided id was not found");
            }
            mapper.Map(updatedProductBrand, productBrandEntity);
            return Ok(await brandService.SaveChangesAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteProductBrand(int id)
        {
            ProductBrandEntity? productBrandEntity = await brandService.GetProductBrandByIdAsync(id);
            if (productBrandEntity == null)
            {
                return BadRequest("Product brand with provided id was not found");
            }
            await brandService.DeleteProductBrandAsync(id);
            return Ok(await brandService.SaveChangesAsync());
        }
    }
}

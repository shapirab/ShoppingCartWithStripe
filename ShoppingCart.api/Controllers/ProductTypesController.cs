using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.data.DataModels.Dtos;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Models;
using ShoppingCart.data.Services.Interfaces;
using System.Text.Json;

namespace ShoppingCart.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
        private readonly IProductTypeService typeService;
        private readonly IMapper mapper;

        private readonly int maxPageSize = 20;

        public ProductTypesController(IProductTypeService typeService, IMapper mapper)
        {
            this.typeService = typeService ?? throw new ArgumentNullException(nameof(typeService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetAllProductTypesAsync
          (string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var (productTypesEntities, paginationMetadata) =
                await typeService.GetAllProductTypesAsync(searchQuery, pageNumber, pageSize);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(mapper.Map<IEnumerable<ProductType>>(productTypesEntities));
        }

        [HttpGet("{id}", Name = "GetProductType")]
        public async Task<ActionResult<ProductType>> GetProductTypeById(int id)
        {
            ProductTypeEntity? productTypeEntity = await typeService.GetProductTypeByIdAsync(id);
            if (productTypeEntity == null)
            {
                return NotFound("Type with the provided id was not found");
            }
            return Ok(mapper.Map<ProductType>(productTypeEntity));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductType>> AddProductType(ProductTypeDto productTypeDto)
        {
            if (productTypeDto == null)
            {
                return BadRequest("Product type to add was not provided");
            }

            ProductTypeEntity productTypeEntity = mapper.Map<ProductTypeEntity>(productTypeDto);

            await typeService.AddProductTypeAsync(productTypeEntity);
            await typeService.SaveChangesAsync();

            ProductType productTypeToReturn = mapper.Map<ProductType>(productTypeEntity);

            return CreatedAtRoute("GetProductType", new
            {
                Id = productTypeEntity.Id,
            }, productTypeToReturn);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateProductType(int id, ProductTypeDto updatedProductType)
        {
            ProductTypeEntity? productTypeEntity = await typeService.GetProductTypeByIdAsync(id);
            if (productTypeEntity == null)
            {
                return BadRequest("Product type with provided id was not found");
            }
            mapper.Map(updatedProductType, productTypeEntity);
            return Ok(await typeService.SaveChangesAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteProductType(int id)
        {
            ProductTypeEntity? productTypeEntity = await typeService.GetProductTypeByIdAsync(id);
            if (productTypeEntity == null)
            {
                return BadRequest("Product type with provided id was not found");
            }
            await typeService.DeleteProductTypeAsync(id);
            return Ok(await typeService.SaveChangesAsync());
        }
    }
}

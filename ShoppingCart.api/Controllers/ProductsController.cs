﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.api.Attributes;
using ShoppingCart.data.DataModels.Dtos;
using ShoppingCart.data.DataModels.Entities;
using ShoppingCart.data.DataModels.Models;
using ShoppingCart.data.Services.Interfaces;
using System.Text.Json;

namespace ShoppingCart.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;

        private readonly int maxPageSize = 20;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Cache(600)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetAllProductsAsync
            ([FromQuery]string? searchQuery, [FromQuery] List<int>? brandIds, [FromQuery] List<int>? typeIds, 
            [FromQuery] string? sort, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
       {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var (productEntities, paginationMetadata) =
                await productService.GetAllProductsAsync(searchQuery, brandIds, typeIds, sort, pageNumber, pageSize);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(mapper.Map<IEnumerable<ProductToReturnDto>>(productEntities));
        }

        [Cache(600)]
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
            ProductEntity? productEntity = await productService.GetProductByIdAsync(id);
            if (productEntity == null)
            {
                return NotFound("Product with the provided id was not found");
            }
            return Ok(mapper.Map<ProductToReturnDto>(productEntity));
        }

        [InvalidateCache("api/products|")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(ProductDto product)
        {
            if (product == null)
            {
                return BadRequest("Product to add was not provided");
            }

            ProductEntity productEntity = mapper.Map<ProductEntity>(product);

            await productService.AddProductAsync(productEntity);
            await productService.SaveChangesAsync();

            Product productToReturn = mapper.Map<Product>(productEntity);

            return CreatedAtRoute("GetProduct", new
            {
                Id = productEntity.Id,
            }, productToReturn);
        }

        [InvalidateCache("api/products|")]
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateProduct(int id, ProductDto updatedProduct)
        {
            ProductEntity? productEntity = await productService.GetProductByIdAsync(id);
            if (productEntity == null)
            {
                return BadRequest("Product with provided id was not found");
            }
            mapper.Map(updatedProduct, productEntity);
            return Ok(await productService.SaveChangesAsync());
        }

        [InvalidateCache("api/products|")]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            ProductEntity? productEntity = await productService.GetProductByIdAsync(id);
            if (productEntity == null)
            {
                return BadRequest("Product with provided id was not found");
            }
            await productService.DeleteProductAsync(id);
            return Ok(await productService.SaveChangesAsync());
        }
    }
}


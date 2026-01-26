using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.BuildingBlocks.Results;
using Product.API.Features.Products.DTOs;
using Product.API.Features.Products.Requests.Commands.AddProduct;
using Product.API.Features.Products.Requests.Commands.DeleteProduct;
using Product.API.Features.Products.Requests.Commands.UpdateProduct;
using Product.API.Features.Products.Requests.Queries.GetProductById;
using Product.API.Features.Products.Requests.Queries.GetProducts;

namespace Product.API.Features.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public ProductController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<Result<IEnumerable<ProductResponseDto>>> Get(CancellationToken cancellationToken)
        {
            var query = new GetProductQuery();
            return await _sender.Send(query, cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<Result<ProductResponseDto>> Get(int id, CancellationToken cancellationToken)
        {
            var query = new GetProductByIdQuery { Id = id };
            return await _sender.Send(query, cancellationToken);
        }

        [HttpPost]
        public async Task<Result<ProductResponseDto>> Post([FromBody] ProductRequestDto model, CancellationToken cancellationToken)
        {
            // Direct mapping or using command directly if DTO matches?
            // Existing code used _mapper to Map DTO -> Command. 
            // Better: Command takes DTO or properties.
            // Let's assume Command takes DTO or properties.
            // For now, I'll instantiate command. 
            // Note: Old code: _mapper.Map<ProductRequestDto, AddProductCommand>(model);
            
             var command = new AddProductCommand
             {
                 Name = model.Name,
                 Description = model.Description,
                 Price = model.Price,
                 Category = model.Category,
                 ImageUrl = model.ImageUrl,
                 NumberofProduct = model.NumberofProduct
             };
             
            return await _sender.Send(command, cancellationToken);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<Result<ProductResponseDto>> Put([FromRoute] int id, [FromBody] ProductRequestDto model, CancellationToken cancellationToken)
        {
            var command = new UpdateProductCommand
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Category = model.Category,
                ImageUrl = model.ImageUrl,
                NumberofProduct = model.NumberofProduct
            };
            return await _sender.Send(command, cancellationToken);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<Result<bool>> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteProductCommand { Id = id };
            return await _sender.Send(command, cancellationToken);
        }
    }
}

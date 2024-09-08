using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnMapper;
using Product.API.Common.Handler;
using Product.API.Features.Products.DTOs;
using Product.API.Features.Products.Requests.Commands.AddProduct;
using Product.API.Features.Products.Requests.Commands.DeleteProduct;
using Product.API.Features.Products.Requests.Commands.UpdateProduct;
using Product.API.Features.Products.Requests.Queries.GetProductById;
using Product.API.Features.Products.Requests.Queries.GetProducts;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Product.API.Features.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly OnMapping _mapper;
        public ProductController(ISender sender, OnMapping mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<Result<IEnumerable<ProductResponseDto>>> Get(CancellationToken cancellationToken)
        {
            var command = new GetProductQuery();
            var result = await _sender.Send(command, cancellationToken);
            return await Result<IEnumerable<ProductResponseDto>>.SuccessAsync(result.Data, "Viewed Successfully", true);
        }

        [HttpGet("{id}")]
        public async Task<Result<ProductResponseDto>> Get(int id, CancellationToken cancellationToken)
        {
            var command = new GetProductByIdQuery
            {
                Id = id,
            };
            var result = await _sender.Send(command, cancellationToken);
            return await Result<ProductResponseDto>.SuccessAsync(result.Data, "Found Successfully", true);
        }

        [HttpPost]
        public async Task<Result<ProductResponseDto>> Post([FromBody] ProductRequestDto model, CancellationToken cancellationToken)
        {

            var mappedCommand = await _mapper.Map<ProductRequestDto, AddProductCommand>(model);
            var result = await _sender.Send(mappedCommand.Data, cancellationToken);

            return await Result<ProductResponseDto>.SuccessAsync(result.Data, "Created Successfully", true);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<Result<ProductResponseDto>> Put([FromRoute] int id, [FromBody] ProductRequestDto model, CancellationToken cancellationToken)
        {
            try
            {
                var mappedRequestModel = await _mapper.Map<ProductRequestDto, UpdateProductCommand>(model);
                mappedRequestModel.Data.Id = id;
                var updateResult = await _sender.Send(mappedRequestModel.Data, cancellationToken);
                return await Result<ProductResponseDto>.SuccessAsync(updateResult.Data, "Updated Successfully", true);
            }
            catch (Exception)
            {
                return await Result<ProductResponseDto>.FaildAsync(true, "Not Updated");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<Result<bool>> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                var command = new DeleteProductCommand
                {
                    Id = id
                };
                var delete = await _sender.Send(command, cancellationToken);
                if (delete.IsSuccess)
                {
                    return await Result<bool>.SuccessAsync(delete.Data, "Deleted Successfully", true);
                }
                return await Result<bool>.FaildAsync(false, "Not Deleted");
            }
            catch (Exception)
            {
                throw new ArgumentNullException(nameof(id));
            }
        }

    }
}

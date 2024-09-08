using MediatR;
using Microsoft.EntityFrameworkCore;
using OnMapper;
using ShoppingCart.API.Common.Handler;
using ShoppingCart.API.Entities;
using ShoppingCart.API.Features.DTOs.CartDetailsDTOs.Response;
using ShoppingCart.API.Features.DTOs.CartDTOs;
using ShoppingCart.API.Features.DTOs.CartHeaderDTOs.Response;
using ShoppingCart.API.DataBase;

namespace ShoppingCart.API.Features.Carts.Requests.Command.AddCart
{
    public class AddCartCommandHandler : IRequestHandler<AddCartCommand, Result<CartDto>>
    {
        private readonly AppDbContext _context;
        private readonly OnMapping _mapper;
        public AddCartCommandHandler(AppDbContext context, OnMapping mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<CartDto>> Handle(AddCartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == request.CartHeaderResponse.UserId);
                if (cartHeader == null)
                {
                    //Create header cart
                    var cartHeadermodel = await _mapper.Map<CartHeaderResponseDto, CartHeader>(request.CartHeaderResponse);
                    await _context.CartHeaders.AddAsync(cartHeadermodel.Data);
                    await _context.SaveChangesAsync();
                    request.CartDetailsResponse.First().CartHeaderId = cartHeadermodel.Data.Id;
                    var cartDetailsmodel = await _mapper.Map<CartDetailsResponseDto, CartDetails>(request.CartDetailsResponse.First());
                    await _context.AddAsync(cartDetailsmodel.Data);
                    await _context.SaveChangesAsync();

                    var mappedCommand = new AddCartCommand
                    {
                        CartHeaderResponse = new CartHeaderResponseDto
                        {
                            Id = cartDetailsmodel.Data.CartHeaderId,
                            UserId = cartDetailsmodel.Data.CartHeader.UserId,
                        },
                        CartDetailsResponse = new List<CartDetailsResponseDto>
                        {
                                new CartDetailsResponseDto
                                {
                                    Id = cartDetailsmodel.Data.Id,
                                    CartHeaderId = cartDetailsmodel.Data.CartHeaderId,
                                    ProductId = cartDetailsmodel.Data.ProductId,
                                    Count = cartDetailsmodel.Data.Count
                                }
                        }
                    };
                    var mappedCart = new CartDto
                    {
                        CartDetailsResponse = mappedCommand.CartDetailsResponse,
                        CartHeaderResponse = mappedCommand.CartHeaderResponse
                    };
                    return await Result<CartDto>.SuccessAsync(mappedCart, "Added Successfully", true);
                }
                else
                {
                    //Not null
                    //Check if details has same product
                    var cartDetails = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(u => u.ProductId == request.CartDetailsResponse.First().ProductId && u.CartHeaderId == cartHeader.Id);
                    if (cartDetails == null)
                    {
                        //Create cart details

                        request.CartDetailsResponse.First().CartHeaderId = cartHeader.Id;
                        var cartDetailsmodel = await _mapper.Map<CartDetailsResponseDto, CartDetails>(request.CartDetailsResponse.First());
                        await _context.AddAsync(cartDetailsmodel.Data);
                        await _context.SaveChangesAsync();

                        var mappedCommand = new AddCartCommand
                        {
                            CartHeaderResponse = new CartHeaderResponseDto
                            {
                                Id = cartDetailsmodel.Data.CartHeaderId,
                                UserId = request.CartHeaderResponse.UserId,
                                //UserId = cartDetailsmodel.Data.CartHeader.UserId,
                            },
                            CartDetailsResponse = new List<CartDetailsResponseDto>
                            {
                                new CartDetailsResponseDto
                                {
                                    Id = cartDetailsmodel.Data.Id,
                                    CartHeaderId = cartDetailsmodel.Data.CartHeaderId,
                                    ProductId = cartDetailsmodel.Data.ProductId,
                                    Count = cartDetailsmodel.Data.Count
                                }
                            }
                        };
                        var mappedCart = new CartDto
                        {
                            CartDetailsResponse = mappedCommand.CartDetailsResponse,
                            CartHeaderResponse = mappedCommand.CartHeaderResponse
                        };

                        return await Result<CartDto>.SuccessAsync(mappedCart, "Added Successfully", true);
                    }
                    else
                    {
                        //Update count in cart details
                        request.CartDetailsResponse.First().Count += cartDetails.Count;
                        request.CartDetailsResponse.First().CartHeaderId = cartDetails.CartHeaderId;
                        request.CartDetailsResponse.First().Id = cartDetails.Id;
                        var cartDetailsmodel = await _mapper.Map<CartDetailsResponseDto, CartDetails>(request.CartDetailsResponse.First());
                        _context.Update(cartDetailsmodel.Data);
                        await _context.SaveChangesAsync();

                        var mappedCommand = new AddCartCommand
                        {
                            CartHeaderResponse = new CartHeaderResponseDto
                            {
                                Id = cartDetailsmodel.Data.CartHeaderId,
                                UserId = cartHeader.UserId
                            },
                            CartDetailsResponse = new List<CartDetailsResponseDto>
                            {
                                new CartDetailsResponseDto
                                {
                                    Id = cartDetailsmodel.Data.Id,
                                    CartHeaderId = cartDetailsmodel.Data.CartHeaderId,
                                    ProductId = cartDetailsmodel.Data.ProductId,
                                    Count = cartDetailsmodel.Data.Count
                                }
                            }
                        };
                        var mappedCart = new CartDto
                        {
                            CartDetailsResponse = mappedCommand.CartDetailsResponse,
                            CartHeaderResponse = mappedCommand.CartHeaderResponse
                        };
                        return await Result<CartDto>.SuccessAsync(mappedCart, "Added Successfully", true);
                    }
                }
            }
            catch
            {
                return await Result<CartDto>.FaildAsync(false, "Not added");
            }

        }
    }
}

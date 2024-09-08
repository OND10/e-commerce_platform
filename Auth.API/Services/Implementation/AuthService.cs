using Auth.API.Common.Handler;
using Auth.API.Contract.Interfaces;
using Auth.API.Models.Domain;
using Auth.API.Models.DTOs;
using Auth.API.Models.DTOs.Request;
using Auth.API.Models.DTOs.Response;
using Auth.API.Services.Interface;
using OnMapper;

namespace Auth.API.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserManagerRepository _repository;
        private readonly ITokenRepository _trepository;
        public AuthService(IUserManagerRepository repository, ITokenRepository trepository)
        {
            _repository = repository;
            _trepository = trepository;
        }

        public async Task<Result<bool>> AddUserToRole(UserRoleRequestDTO request)
        {
            var result = await _repository.AssignRoleToUser(request.Email, request.roleName);
            if(result is true)
            {
                return await Result<bool>.SuccessAsync(result, "Role is Created Successfully", true);
            }
            return await Result<bool>.FaildAsync(false, "Role is Created Successfully");
        }

        public async Task<Result<LoginResponseDTO>> Login(LoginRequestDTO request)
        {
            var mapper = new OnMapping();
            var userlog = await _repository.FindUserByNameAsync(request.UserName);
            if (userlog is not null)
            {
                //Checking password
                var checkPasswordresult = await _repository.CheckUserPasswordAsync(userlog, request.Password);
                if (checkPasswordresult)
                {
                    var roles = await _repository.GetUserRolesAsync(userlog);

                    //Creating JWT
                    var tokens = _trepository.CreateJWTTokenAsync(userlog, roles.ToList());
                    
                    //Mapping ApplicationUser to the LoginResponseDTO
                    var response = new LoginResponseDTO
                    {
                        User = new UserDTO
                        {
                            Email = userlog.Email,
                            Name = userlog.Name,
                            PhoneNumber = userlog.PhoneNumber,
                        },
                        Token = tokens
                    };
                    return await Result<LoginResponseDTO>.SuccessAsync(response, "Logged Successfully", true);
                }
                return await Result<LoginResponseDTO>.FaildAsync(false, "Email or Password are incorrect");
            }
            return await Result<LoginResponseDTO>.FaildAsync(false, "Email or Password are incorrect");
        }

        public async Task<Result<UserDTO>> Register(RegisterRequestDTO request)
        {
            var mapper = new OnMapping();
            var modelMapped = await mapper.Map<RegisterRequestDTO, ApplicationUser>(request);
            modelMapped.Data.Name = request.UserName;
            var result = await _repository.CreateUserAsync(modelMapped.Data, request.Password);

            if (result.Succeeded)
            {
                var userReturn = await _repository.FindUserByEmailAsync(request.Email);

                ///
                /// Here I should use mapping but still I am in debugging mode.
                ///
                var userMappedResult = await mapper.Map<ApplicationUser, UserDTO>(userReturn);
                
                return await Result<UserDTO>.SuccessAsync(userMappedResult.Data, "Registered Successfully", true);
            }
            return await Result<UserDTO>.FaildAsync(false, $"{result.Errors}");
        }
    }

    
}

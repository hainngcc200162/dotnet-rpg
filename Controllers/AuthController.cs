using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository authRepo, IMapper mapper)
        {
            _authRepo = authRepo;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            var response = await _authRepo.Register(
                new User { Username = request.Username }, request.Password
            );
            if (response.Success)
            {
                return Ok(response); // Trả về Ok nếu đăng ký thành công
            }
            return BadRequest(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDto request)
        {
            var response = await _authRepo.Login(request.Username, request.Password);
            if (response.Success)
            {
                return Ok(response); // Trả về Ok nếu đăng nhập thành công
            }
            return BadRequest(response);
        }

        [HttpPost("ChangePassword")]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword(UserChangePasswordDto request)
        {
            var response = await _authRepo.ChangePassword(request.Username, request.OldPassword, request.NewPassword);
            if (!response.Success)
            {
                 return Ok(response);
            }
                return BadRequest(response);    
        }

    }
}

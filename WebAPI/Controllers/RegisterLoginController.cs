using Application.Interfaces.Security;
using Application.Interfaces.Users;
using AutoMapper;
using Core.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Users;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class RegisterLoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterLoginController> _logger;

        public RegisterLoginController(
            IUserRepository userRepository, 
            IPasswordHasher passwordHasher, 
            IJwtService jwtService, 
            ILogger<RegisterLoginController> logger,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserModel registerUserModel)
        {
            
            try
            {
                _logger.LogInformation("Registration attempt for {Email} / {PhoneNumber}", registerUserModel.Email, registerUserModel.Phone);

                var user = _mapper.Map<User>(registerUserModel);
                user.PasswordHash = _passwordHasher.HashPassword(registerUserModel.Password);

                var addedUser = _userRepository.AddUser(user);
                _logger.LogInformation("User with {Email} / {PhoneNumber} registered successfully", registerUserModel.Email, registerUserModel.Phone);

                return Ok(addedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        [HttpPost("login/email")]
        public IActionResult LoginWithEmail([FromBody] LoginEmailModel loginEmailModel)
        {
            try
            {
                _logger.LogInformation("Logging attempt with email {Email}", loginEmailModel.Email);
                var user = _userRepository.GetUserByEmail(loginEmailModel.Email);
                if(user == null || !_passwordHasher.VerifyPassword(loginEmailModel.Password, user.PasswordHash))
                {
                    _logger.LogInformation("Unathorized access attempt with email: {Email}", loginEmailModel.Email);
                    return Unauthorized();
                }
                var token = _jwtService.GenerateToken(user);
                _logger.LogInformation("User with email {Email} logged in successfully", loginEmailModel.Email);

                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "An error occurred while logging in");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("login/phone")]
        public IActionResult LoginWithPhone([FromBody] LoginPhoneModel loginEmailModel)
        {
            _logger.LogInformation("Logging attempt with Phone Number {PhoneNumber}", loginEmailModel.Phone);

            var user = _userRepository.GetUserByPhone(loginEmailModel.Phone);
            if (user == null || !_passwordHasher.VerifyPassword(loginEmailModel.Password, user.PasswordHash))
            {
                _logger.LogInformation("Unathorized access attempt with Phone Number: {PhoneNumber}", loginEmailModel.Phone);
                return Unauthorized();
            }

            var token = _jwtService.GenerateToken(user);
            _logger.LogInformation("User with Phone Number {PhoneNumber} logged in successfully", loginEmailModel.Phone);

            return Ok(token);
        }
    }
}

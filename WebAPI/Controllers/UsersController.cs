﻿using Application.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserModel registerUserModel)
        {
            _logger.LogInformation("Registering user with email {Email}", registerUserModel.Email);
            var hashedPassword = _passwordHasher.HashPassword(registerUserModel.Password);
            var user = new User
            {
                Username = registerUserModel.Username,
                Email = registerUserModel.Email,
                Phone = registerUserModel.PhoneNumber,
                PasswordHash = hashedPassword
            };
            
            var addedUser = _userRepository.AddUser(user);
            _logger.LogInformation("User with email {Email} registered successfully", registerUserModel.Email);

            return Ok(addedUser);
        }
        [HttpPost("login/email")]
        public IActionResult LoginWithEmail([FromBody] LoginEmailModel loginEmailModel)
        {
            _logger.LogInformation("Logging attempt with email {Email}", loginEmailModel.Email);

            var user = _userRepository.GetUserByEmail(loginEmailModel.Email);
            if ( user == null || !_passwordHasher.VerifyPassword(loginEmailModel.Password, user.PasswordHash))
            {
                _logger.LogInformation("Unathorized access attempt with email: {Email}", loginEmailModel.Email);
                return Unauthorized();
            }

            var token = _jwtService.GenerateToken(user);
            _logger.LogInformation("User with email {Email} logged in successfully", loginEmailModel.Email);

            return Ok(token);
        }
    }
}

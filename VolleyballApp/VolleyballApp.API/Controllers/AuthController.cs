using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VolleyballApp.API.Data;
using VolleyballApp.API.Dtos;
using VolleyballApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;

namespace VolleyballApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IVolleyballRepository _volley;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper, IVolleyballRepository volley)
        {
            _mapper = mapper;
            _config = config;
            _repo = repo;
            _volley = volley;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username)) return BadRequest("Username already exists");

            var userToCreate = _mapper.Map<User>(userForRegisterDto);
            if(userToCreate.UserType != "player" && userToCreate.UserType != "referee" && userToCreate.UserType != "organiser")
            {
                userToCreate.UserType = "player";
            }
            userToCreate.RankingPoints = 900;
            userToCreate.OwnedTeam = false;
            userToCreate.IsMailActivated = false;
            userToCreate.Description = "";
            userToCreate.Positions = "";
            
            var createdUser = await _repo.Regiser(userToCreate, userForRegisterDto.Password);
            Helpers.MailSender.sendMessage("Account activation", "<h3>To activate your account click this link http://localhost:4200/activate/" + createdUser.Id +"</h3>", createdUser.Mail);

            var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);

            return CreatedAtRoute("GetUser", new { controller = "Users", id = createdUser.Id }, userToReturn);
        }

        [HttpGet("activate/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var userFromRepo = await _volley.GetUser(id);
            if (userFromRepo == null) return Unauthorized();
            if (userFromRepo.IsMailActivated) return BadRequest("User already activated");
            userFromRepo.IsMailActivated = true;
            var userToReturn = _mapper.Map<UserForListDto>(userFromRepo);
            if (await _volley.saveAll()) return Ok(userToReturn);
            throw new Exception($"Activating user with {id} failed on save.");

        }

        [HttpGet("resend/{id}")]
        public async Task<IActionResult> ResendMail(int id)
        {
            var user = await _volley.GetUser(id);
            if (user == null) return Unauthorized();
            if(user.IsMailActivated) return BadRequest("Mail already confirmed");
            Helpers.MailSender.sendMessage("Account activation", "<h3>To activate your account click this link http://localhost:4200/activate/" + user.Id +"</h3>", user.Mail);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null) return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var user = _mapper.Map<UserForListDto>(userFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }
    }
}
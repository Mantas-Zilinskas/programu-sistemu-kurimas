using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using EmocineSveikataServer.Dto;
using EmocineSveikataServer.Models;
using EmocineSveikataServer.Repositories.UserRepository;
using Microsoft.IdentityModel.Tokens;

namespace EmocineSveikataServer.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsername(loginDto.Username);
            
            if (user == null)
            {
                throw new KeyNotFoundException("Neteisingas naudotojo vardas arba slaptažodis");
            }

            if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new KeyNotFoundException("Neteisingas naudotojo vardas arba slaptažodis");
            }

            string token = CreateToken(user);
            
            var userDto = _mapper.Map<UserDto>(user);
            
            return new AuthResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        public async Task<AuthResponseDto> Register(RegisterDto registerDto)
        {
            if (await _userRepository.UserExists(registerDto.Username))
            {
                throw new InvalidOperationException("Toks naudotojo vardas jau egzistuoja");
            }

            if (await _userRepository.EmailExists(registerDto.Email))
            {
                throw new InvalidOperationException("Toks el. pašto adresas jau užregistruotas");
            }

            CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = registerDto.Role
            };

            await _userRepository.CreateUser(user);

            string token = CreateToken(user);
            
            var userDto = _mapper.Map<UserDto>(user);
            
            return new AuthResponseDto
            {
                Token = token,
                User = userDto
            };
        }

        public async Task<bool> UserExists(string username)
        {
            return await _userRepository.UserExists(username);
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}

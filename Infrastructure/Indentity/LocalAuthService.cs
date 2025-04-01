using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Dtos.User;
using Core.Entities;
using Core.Specifications;
using System.Transactions;

namespace Infrastructure.Indentity
{
    public class LocalAuthService(IGenericRepository<UserEntity> userRepository,
            IGenericRepository<LoginEntity> loginRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService,
             IUnitOfWork unitOfWork) : IAuthService
    {
        private readonly IGenericRepository<UserEntity> _userRepository = userRepository;
        private readonly IGenericRepository<LoginEntity> _loginRepository = loginRepository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<AuthTokenDto> Login(LoginDto loginDto)
        {
            var userSpec = new EntityByUniqueProperty<UserEntity>("Email", loginDto.Email);
            var existingUser = await _userRepository.GetEntityWithSpecAsync(userSpec);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("Email or password is incorrect");
            }
            var loginSpec = new EntityByUniqueProperty<LoginEntity>("Email", loginDto.Email);
            var existingLogin = await _loginRepository.GetEntityWithSpecAsync(loginSpec);
            if (existingLogin == null)
            {
                throw new KeyNotFoundException("Email or password is incorrect");
            }
            var isPasswordValid = _passwordHasher.VerifyPassword(existingLogin.Password, loginDto.Password);
            if (!isPasswordValid)
            {
                throw new KeyNotFoundException("Email or password is incorrect");
            }
            var token = _jwtTokenService.GenerateToken(existingUser);
            return new AuthTokenDto { Token = token };
        }

        public async Task<string> Register(RegisterDto registerDto)
        {
            var spec = new EntityByUniqueProperty<UserEntity>("Email", registerDto.Email);
            var existingUser = await _userRepository.GetEntityWithSpecAsync(spec);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists");
            }
            var hashedPassword = _passwordHasher.HashPassword(registerDto.Password);
            var user = new UserEntity
            {
                Email = registerDto.Email,
                Provider = "Local",
                Name = registerDto.Name
            };


            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _userRepository.CreateAsync(user);
                var login = new LoginEntity
                {
                    Email = registerDto.Email,
                    Password = hashedPassword,
                    UserId = user.Id
                };
                await _loginRepository.CreateAsync(login);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new TransactionAbortedException(ex.Message);
            }

            return "Register success";
        }
    }
}
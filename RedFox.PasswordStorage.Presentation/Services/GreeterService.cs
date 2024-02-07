using Grpc.Core;
using Password_Storage_Server;
using RedFox.PasswordStorage.Infrastructure.Models;
using RedFox.PasswordStorage.Infrastructure.Services;

namespace Password_Storage_Server.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override  Task<RegisterReply> UserRegister(RegisterRequest request, ServerCallContext context)
        {
            RegisterService registerService = new RegisterService();
            bool registerSuccess =  registerService.Register(new RegisterModel(request.Login, request.Password)).Result;

            if (registerSuccess)
            {
                return Task.FromResult(new RegisterReply
                {
                    ReplyCode = RegisterReply.Types.StatusCode.Ok,
                });
            }
            return Task.FromResult(new RegisterReply
            {
                ReplyCode = RegisterReply.Types.StatusCode.LoginAlreadyUse,
            });
        }

        public override Task<TokenReply> UserAuth(UserDataRequest request, ServerCallContext context)
        {
            AuthService authService = new();
            bool isAuth = authService.Auth(request.Login, request.Password).Result;
            if (isAuth)
            {
                return Task.FromResult(new TokenReply
                {
                    AccessToken = authService.GetAccessToken(),
                    RefreshToken = authService.GetRefreshToken(),
                    Login = request.Login,
                    ReplyCode = TokenReply.Types.StatusCode.Ok,
                });
            }
            return Task.FromResult(new TokenReply
            {
                AccessToken = "",
                RefreshToken = "",
                Login = request.Login,
                ReplyCode = TokenReply.Types.StatusCode.IncorrectPassword,
            });
        }

        public override Task<InfoReply> UserGetInfo(InfoRequest request, ServerCallContext context)
        {
            UserInfoService accountService = new();
            string info = accountService.GetInfo(request.AccessToken, request.RefreshToken, request.Login).Result;

            if (Equals(info, "Non"))
            {
                return Task.FromResult(new InfoReply
                {
                    List = info,
                    ReplyCode = InfoReply.Types.StatusCode.Unlogin,
                    AccessToken = "Non",
                    RefreshToken = "Non",
                });
            }

            return Task.FromResult(new InfoReply
            {
                List = info,
                ReplyCode = InfoReply.Types.StatusCode.Ok,
                AccessToken = accountService.GetNewAccessToken().Result,
                RefreshToken = accountService.GetNewRefreshToken().Result,
            });
        }

        public override Task<UpdateReply> UserUpdateInfo(UpdateRequest request, ServerCallContext context)
        {
            UserInfoService accountService = new();
            bool flag = accountService.UpdateInfo(request.AccessToken, request.RefreshToken, request.Login, request.List).Result;

            return Task.FromResult(new UpdateReply
            {
                AccessToken = accountService.GetNewAccessToken().Result,
                RefreshToken = accountService.GetNewRefreshToken().Result,
            });
        }
    }
}
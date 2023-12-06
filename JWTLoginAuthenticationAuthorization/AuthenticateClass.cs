using DAL;
using Models;

namespace JWTLoginAuthenticationAuthorization
{
    public class AuthenticateClass
    {
        private readonly IConfiguration _config;
        public AuthenticateClass(IConfiguration config)
        {
            _config = config;
        }
        public UsersModel Authenticate(LoginModel userLogin, ref string Log)
        {
            try
            {
                string log = string.Empty;
                UsersDAL usersDAL = new UsersDAL(_config);

                var user = usersDAL.QUserLogin(userLogin.Name, userLogin.Password, ref log);
                if (!string.IsNullOrEmpty(log))
                {
                    Log = log;
                }
                if (user != null)
                {
                    Log = "User: " + user.Name + " successfully logged in.";
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log = "An exception has uncured while authenticating the user. Error: " + ex.Message.ToString() + "";
                return null;
            }
        }
    }
}

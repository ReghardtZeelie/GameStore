using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public interface  IUsersDAL  
    {

        UsersModel QUserLogin(string UserName, string Password, ref string log);
        UsersModel IUser(AddUserModel newuser, ref string log);
    }
}

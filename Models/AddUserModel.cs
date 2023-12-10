using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AddUserModel
    {
        
        public string Name { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime DOB { get; set; }
    }
}

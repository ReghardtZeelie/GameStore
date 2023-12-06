using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ItemsDAL
    {
        private readonly IConfiguration _configuration;

        public ItemsDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Models
{
    public class User
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string PositionId { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string PositionName { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string Password { get; set; }
        public string Grade { get; set; }
        public string Email { get; set; }

        protected string _hostName;
        protected string _hostIP;

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        

    }
}

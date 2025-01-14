using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account
{
    public class UserDto
    {
        public string username { get; set; } 
        public string email { get; set; }
        public string AppUserId {get; set;} 
    }
}
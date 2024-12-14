using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Models;

namespace api.Mapper
{
    public static class UserDtoMapper
    {
        public static UserDto MapToUserDto(this AppUser appUser)
        {
            return new UserDto
                {
                username = appUser.UserName,
                email = appUser.Email,
                };
            ;
        }
        public static List<UserDto> MapToUserDtos(this IEnumerable<AppUser> appUsers)
        {
            return appUsers.Select(x => x.MapToUserDto()).ToList();
        }
    }
}
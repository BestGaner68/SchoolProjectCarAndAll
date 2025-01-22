using api.Dtos;
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
                AppUserId = appUser.Id,
                };
            ;
        }
        public static List<UserDto> MapToUserDtos(this IEnumerable<AppUser> appUsers)
        {
            return appUsers.Select(x => x.MapToUserDto()).ToList();
        }
    }
}
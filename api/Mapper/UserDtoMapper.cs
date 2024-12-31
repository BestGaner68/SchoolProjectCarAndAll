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
                };
            ;
        }
        public static List<UserDto> MapToUserDtos(this IEnumerable<AppUser> appUsers)
        {
            return appUsers.Select(x => x.MapToUserDto()).ToList();
        }

        public static WagenParkDataDto MapToWagenParkDto (this WagenParkVerzoek verzoek)
        {
            return new WagenParkDataDto{
                AppUserId = verzoek.AppUserId,
                WagenparkId = verzoek.WagenparkId,
                WagenparkVerzoekId = verzoek.wagenparkverzoekId,
            };
        }
        public static List<WagenParkDataDto> MapToWagenParkDtos (this IEnumerable<WagenParkVerzoek> verzoeken)
        {
            return verzoeken.Select(x => x.MapToWagenParkDto()).ToList();
        }
    }
}
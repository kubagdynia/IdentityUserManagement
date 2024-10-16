using IdentityUserManagement.Application.Dto;
using IdentityUserManagement.Core.Entities;

namespace IdentityUserManagement.Application.Mappers;

public static class UserRegistrationDtoMapper
{
    public static User MapToUser(this UserRegistrationDto userRegistrationDto)
    {
        return new User
        {
            UserName = userRegistrationDto.Email,
            FirstName = userRegistrationDto.FirstName,
            LastName = userRegistrationDto.LastName,
            Email = userRegistrationDto.Email
        };
    }
}
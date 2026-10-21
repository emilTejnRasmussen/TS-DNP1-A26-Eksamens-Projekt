using DTO.User;
using Entities;

namespace Mapper;

public static class UserMapper
{
    public static User ToEntity(CreateUserDto dto)
    {
        return new User(
            dto.Username,
            dto.Password
        );
    }

    public static UserDto ToDto(User user)
    {
        return new UserDto(
            user.Id,
            user.Username
            );
    }
}
using System.ComponentModel.DataAnnotations;

namespace DTO.User;

public record UpdateUserRequest(
    [StringLength(50, MinimumLength = 3)]
    string Username
);
using System.ComponentModel.DataAnnotations;

namespace DTO.User;

public record ChangePasswordRequest(
    [Required] 
    string CurrentPassword,
    
    [Required, MinLength(8)]
    string NewPassword
);
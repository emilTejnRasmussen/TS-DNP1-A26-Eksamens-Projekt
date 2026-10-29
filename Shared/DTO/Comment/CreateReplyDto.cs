using System.ComponentModel.DataAnnotations;

namespace DTO.Comment;

public record CreateReplyDto(
    [Required, MinLength(1)] 
    string Body
);
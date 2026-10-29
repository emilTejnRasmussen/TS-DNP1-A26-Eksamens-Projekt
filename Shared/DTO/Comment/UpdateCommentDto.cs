using System.ComponentModel.DataAnnotations;

namespace DTO.Comment;

public record UpdateCommentDto(
    [Required, MinLength(1)]
    string Body
);
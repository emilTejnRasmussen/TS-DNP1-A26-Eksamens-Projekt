using System.ComponentModel.DataAnnotations;

namespace DTO.Comment;

public record CreateCommentDto(
    [Required, MinLength(1)]
    string Body,
    int? ParentCommentId
);
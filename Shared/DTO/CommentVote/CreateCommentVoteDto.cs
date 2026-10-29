using System.ComponentModel.DataAnnotations;
using Entities;

namespace DTO.CommentVote;

public record CreateCommentVoteDto(
    [Required]
    VoteType VoteType
);
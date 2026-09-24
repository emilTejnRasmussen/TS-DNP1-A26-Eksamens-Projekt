using DTO.PostVote;
using Entities;

namespace WebApi.Mappers;

public static class PostVoteMapper
{
    public static PostVoteDto ToDto(PostVote postVote)
    {
        return new PostVoteDto(
            postVote.Id,
            postVote.UserId,
            postVote.VoteType
        );
    }
}
using System.Text.Json.Serialization;

namespace Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VoteType
{
    Like,
    Dislike
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ForumThreads.Model
{
    public partial class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string? CommentedThreadId { get; set; }
        public string? OriginalCommentId { get; set; }
        public string CommentBody { get; set; }
        public int CommentScore { get; set; }
        public DateTime CommentDate { get; set; } = DateTime.UtcNow;
        public string UserName { get; set; }
        public int UserId { get; set; }
        public List<Comment> Comments { get; set; } = [];
    }
}

using MongoDB.Bson.Serialization.Attributes;

namespace ForumThreads.Model
{
    public partial class Comment
    {
        [BsonId]
        public int _id { get; set; }
        public string CommentBody { get; set; }
        public int CommentScore { get; set; }
        public DateTime CommentDate { get; set; } = DateTime.UtcNow;
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}

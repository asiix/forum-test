using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Diagnostics.Metrics;

namespace ForumThreads.Model
{
    public partial class Thread
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Flair { get; set; }
        public int ThreadScore { get; set; }
        public DateTime ThreadCreationDate { get; set; } = DateTime.UtcNow;
        public string CreatorName { get; set; }
        public int CreatorId { get; set; }
        public List<Comment> Comments { get; set; } = [];
    }
}

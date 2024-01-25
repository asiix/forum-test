using MongoDB.Bson.Serialization.Attributes;

namespace ForumThreads.Model
{
    public class User
    {
        [BsonId]
        public int Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public string ProfilePhoto { get; set; }
        public string ProfileBanner { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}

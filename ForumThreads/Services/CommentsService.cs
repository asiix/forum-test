using ForumThreads.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.Design;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ForumThreads.Services
{
    public class CommentsService
    {
        private readonly IMongoCollection<ForumThreads.Model.Thread> _threadsCollection;

        public CommentsService(
            IOptions<ForumDatabaseSettings> forumDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                forumDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                forumDatabaseSettings.Value.DatabaseName);

            _threadsCollection = mongoDatabase.GetCollection<ForumThreads.Model.Thread>(
                forumDatabaseSettings.Value.CollectionName);
        }

        //Get Comment by threadId and commentId
        public async Task<ForumThreads.Model.Comment> GetCommentAsync(string threadId, string commentId)
        {
            var threadFilter = Builders<ForumThreads.Model.Thread>.Filter.Eq(x => x._id, threadId);

            // Find the thread matching the threadId
            var thread = await _threadsCollection.Find(threadFilter).FirstOrDefaultAsync();

            // If thread is null, return null
            if (thread == null)
            {
                return null;
            }

            // Find the comment within the thread matching the commentId
            var comment = thread.Comments.FirstOrDefault(c => c._id == commentId);

            return comment;
        }

        //Add comment to a thread
        public async Task CreateCommentAsync(string threadId, Comment comment, string? originalCommentId = null)
        {
            var filter = Builders<ForumThreads.Model.Thread>.Filter.Eq(x => x._id, threadId);
            var thread = await _threadsCollection.Find(filter).FirstOrDefaultAsync();

            if (thread == null)
            {
                throw new ArgumentException("Thread not found.");
            }

            if (comment._id == null)
            {
                comment._id = ObjectId.GenerateNewId().ToString();
            }

            comment.CommentedThreadId = threadId;
            comment.OriginalCommentId = originalCommentId;

            if (originalCommentId != null)
            {
                var originalComment = FindCommentById(thread.Comments, originalCommentId);

                if (originalComment != null)
                {
                    originalComment.Comments ??= new List<Comment>();
                    originalComment.Comments.Add(comment);
                }
                else
                {
                    throw new ArgumentException("Original comment not found.");
                }
            }
            else
            {
                thread.Comments ??= new List<Comment>();
                thread.Comments.Add(comment);
            }

            var update = Builders<ForumThreads.Model.Thread>.Update.Set(x => x.Comments, thread.Comments);
            var result = await _threadsCollection.UpdateOneAsync(filter, update);

            if (result.ModifiedCount != 1)
            {
                throw new InvalidOperationException("Failed to add comment to the thread.");
            }
        }

        public async Task ModifyCommentAsync(string threadId, string commentId, Comment updatedComment)
        {
            var filter = Builders<ForumThreads.Model.Thread>.Filter.Eq(x => x._id, threadId);
            var thread = await _threadsCollection.Find(filter).FirstOrDefaultAsync();
            if (thread == null)
            {
                throw new ArgumentException("Thread not found.");
            }

            var existingComment = thread.Comments?.FirstOrDefault(c => c._id == commentId);

            if (existingComment == null)
            {
                throw new ArgumentException("Comment not found.");
            }

            existingComment.CommentBody = updatedComment.CommentBody;
            existingComment.CommentScore = updatedComment.CommentScore;

            var update = Builders<ForumThreads.Model.Thread>.Update.Set(x => x.Comments, thread.Comments);
            var result = await _threadsCollection.UpdateOneAsync(filter, update);

            if (result.ModifiedCount != 1)
            {
                throw new InvalidOperationException("Failed to modify the comment.");
            }
        }

        public async Task DeleteCommentAsync(string threadId, string commentId)
        {
            var filter = Builders<ForumThreads.Model.Thread>.Filter.Eq(x => x._id, threadId);
            var thread = await _threadsCollection.Find(filter).FirstOrDefaultAsync();
            if (thread == null)
            {
                throw new ArgumentException("Thread not found.");
            }

            var existingComment = thread.Comments?.FirstOrDefault(c => c._id == commentId);

            if (existingComment == null)
            {
                throw new ArgumentException("Comment not found.");
            }

            existingComment.CommentBody = null;
            existingComment.CommentScore = 0;
            existingComment.UserName = null;
            existingComment.UserId = 0;

            var update = Builders<ForumThreads.Model.Thread>.Update.Set(x => x.Comments, thread.Comments);
            var result = await _threadsCollection.UpdateOneAsync(filter, update);

            if (result.ModifiedCount != 1)
            {
                throw new InvalidOperationException("Failed to modify the comment.");
            }
        }

        private Comment FindCommentById(List<Comment>? comments, string commentId)
        {
            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    if (comment._id == commentId)
                    {
                        return comment;
                    }

                    var foundInReplies = FindCommentById(comment.Comments, commentId);
                    if (foundInReplies != null)
                    {
                        return foundInReplies;
                    }
                }
            }

            return null;
        }
    }
}

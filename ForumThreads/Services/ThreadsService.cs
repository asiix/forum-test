using ForumThreads.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ForumThreads.Services;

public class ThreadsService
{
    private readonly IMongoCollection<ForumThreads.Model.Thread> _threadsCollection;

    public ThreadsService(
        IOptions<ForumDatabaseSettings> forumDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            forumDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            forumDatabaseSettings.Value.DatabaseName);

        _threadsCollection = mongoDatabase.GetCollection<ForumThreads.Model.Thread>(
            forumDatabaseSettings.Value.CollectionName);
    }

    public async Task<List<ForumThreads.Model.Thread>> GetAsync() =>
        await _threadsCollection.Find(_ => true).ToListAsync();

    public async Task<ForumThreads.Model.Thread> GetAsync(string id) =>
        await _threadsCollection.Find(x => x._id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(ForumThreads.Model.Thread thread)
    {
        thread.ThreadId = GetNextAutoIncrementalId();
        await _threadsCollection.InsertOneAsync(thread);
    }

    public async Task UpdateAsync(string id, ForumThreads.Model.Thread thread) =>
        await _threadsCollection.ReplaceOneAsync(x => x._id == id, thread);

    public async Task RemoveAsync(string id) =>
        await _threadsCollection.DeleteOneAsync(x => x._id == id);

    public int GetNextAutoIncrementalId()
    {
        var sort = Builders<ForumThreads.Model.Thread>.Sort.Descending(model => model.ThreadId);
        var lastDocument = _threadsCollection.Find(Builders<ForumThreads.Model.Thread>.Filter.Empty)
                                     .Sort(sort)
                                     .Limit(1)
                                     .FirstOrDefault();

        return lastDocument?.ThreadId + 1 ?? 1;
    }
}
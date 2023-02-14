using gamification_backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace gamification_backend.Service;

public class TasksService
{
    private readonly IMongoCollection<SessionRecord> _tasksCollection;

    public TasksService(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _tasksCollection = mongoDatabase.GetCollection<SessionRecord>(
            databaseSettings.Value.TasksCollectionName);
    }

    public async Task<List<SessionRecord>> GetAsync()
    {
        return await _tasksCollection.Find(_ => true).ToListAsync();
    }

    public async Task<SessionRecord?> GetAsync(int id)
    {
        return await _tasksCollection.Find(x => x.Id == Convert.ToInt32(id)).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(SessionRecord newBook)
    {
        await _tasksCollection.InsertOneAsync(newBook);
    }

    public async Task UpdateAsync(int id, SessionRecord updatedBook)
    {
        await _tasksCollection.ReplaceOneAsync(x => x.Id == Convert.ToInt32(id), updatedBook);
    }

    public async Task RemoveAsync(int id)
    {
        await _tasksCollection.DeleteOneAsync(x => x.Id == Convert.ToInt32(id));
    }
}
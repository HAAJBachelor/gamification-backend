using gamification_backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace gamification_backend.Service;

public class TasksService
{
    private readonly IMongoCollection<GameTask> _tasksCollection;

    public TasksService(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _tasksCollection = mongoDatabase.GetCollection<GameTask>(
            databaseSettings.Value.TasksCollectionName);
    }

    public async Task<List<GameTask>> GetAsync()
    {
        return await _tasksCollection.Find(_ => true).ToListAsync();
    }

    public async Task<GameTask?> GetAsync(string id)
    {
        return await _tasksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(GameTask newBook)
    {
        await _tasksCollection.InsertOneAsync(newBook);
    }

    public async Task UpdateAsync(string id, GameTask updatedBook)
    {
        await _tasksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);
    }

    public async Task RemoveAsync(string id)
    {
        await _tasksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
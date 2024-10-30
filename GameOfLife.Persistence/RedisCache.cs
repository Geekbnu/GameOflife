using StackExchange.Redis;
using System.Text.Json;

namespace GameOfLife.Persistence
{
    public class RedisCache : IPersistence
    {
        private IDatabase _redis;

        public RedisCache(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public bool Save(Guid id, List<List<int>> matrix)
        {
            return _redis.StringSet(id.ToString(), JsonSerializer.Serialize(matrix));
        }

        public List<List<int>>? Get(Guid id)
        {
            var boardData = _redis.StringGet(id.ToString());
            var matrix = JsonSerializer.Deserialize<List<List<int>>>(boardData);
            return matrix;
        }
    }
}

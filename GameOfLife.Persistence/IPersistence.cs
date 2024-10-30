namespace GameOfLife.Persistence
{
    public interface IPersistence
    {
        public bool Save(Guid id, List<List<int>> matrix);
        public List<List<int>> Get(Guid id);
    }
}
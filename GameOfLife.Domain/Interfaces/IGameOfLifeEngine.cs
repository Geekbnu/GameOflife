namespace GameOfLife.Domain.Interfaces
{
    public interface IGameOfLifeEngine
    {
        public List<List<int>> Generate(List<List<int>> matrix);
    }
}

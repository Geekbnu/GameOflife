namespace GameOfLife.Domain.Entities
{
    public class GameOfLife
    {
        public List<List<int>>? Matrix { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
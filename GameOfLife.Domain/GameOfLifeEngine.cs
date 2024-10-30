using GameOfLife.Domain.Enum;
using GameOfLife.Domain.Interfaces;

namespace GameOfLife.Domain
{
    public class GameOfLifeEngine : IGameOfLifeEngine
    {
        private Entities.GameOfLife _gameOfLife = new();
        
        public List<List<int>> Generate(List<List<int>> matrix)
        {
            SetMatrix(matrix);
            var copyMatrix = matrix.Select(row => row.ToList()).ToList();

            for (int x = 0; x < _gameOfLife.Width; x++)
            {
                for (int y = 0; y < _gameOfLife.Height; y++)
                {
                    var liveNeighbours = LiveNeighbourCount(x, y);
                    var cellState = (State)_gameOfLife.Matrix[x][y];

                    if (cellState == State.Alive)
                    {
                        if (liveNeighbours < 2 || liveNeighbours > 3)
                        {
                            KillCell(copyMatrix, x, y);
                        }
                    }
                    else if (cellState == State.Dead && liveNeighbours == 3)
                    {
                        ReviveCell(copyMatrix, x, y);
                    }
                }
            }

            return copyMatrix;
        }

        private int LiveNeighbourCount(int x, int y)
        {
            int count = 0;
            foreach (Neighbour neighbour in System.Enum.GetValues(typeof(Neighbour)))
            {
                if (NeighbourCell(x, y, neighbour) == State.Alive)
                {
                    count++;
                }
            }
            return count;
        }

        private State CellState(int x, int y)
        {
            if (x < 0 || x >= _gameOfLife.Width || y < 0 || y >= _gameOfLife.Height)
            {
                return State.None;
            }
            return (State)_gameOfLife.Matrix[x][y];
        }

        private State NeighbourCell(int x, int y, Neighbour neighbour)
        {
            return neighbour switch
            {
                Neighbour.Top => CellState(x, y - 1),
                Neighbour.TopRight => CellState(x + 1, y - 1),
                Neighbour.Right => CellState(x + 1, y),
                Neighbour.BottomRight => CellState(x + 1, y + 1),
                Neighbour.Bottom => CellState(x, y + 1),
                Neighbour.BottomLeft => CellState(x - 1, y + 1),
                Neighbour.Left => CellState(x - 1, y),
                Neighbour.TopLeft => CellState(x - 1, y - 1),
                _ => State.None
            };
        }

        private void KillCell(List<List<int>> matrix, int x, int y)
        {
            matrix[x][y] = (int)State.Dead;
        }

        private void ReviveCell(List<List<int>> matrix, int x, int y)
        {
            matrix[x][y] = (int)State.Alive;
        }

        private void SetMatrix(List<List<int>> matrix)
        {
            _gameOfLife.Matrix = matrix;
            _gameOfLife.Width = matrix.Count;
            _gameOfLife.Height = matrix[0].Count;
        }
    }
}
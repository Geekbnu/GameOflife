using GameOfLife.Application.Shared.Exceptions;
using GameOfLife.Domain.Interfaces;
using GameOfLife.Persistence;

namespace GameOfLife.Application.Services
{
    public class GameOfLIfeService : IGameOfLifeService
    {
        private IPersistence _persistence;
        private IGameOfLifeEngine _gameEngine;

        public GameOfLIfeService(IGameOfLifeEngine gameEngine, IPersistence cache)
        {
            _persistence = cache;
            _gameEngine = gameEngine;
        }

        public Board Generate(Guid boardId)
        {
            var boardData = _persistence.Get(boardId);

            if (boardData == null)
            {
                throw new EngineException("Board not found.");
            }

            var currentMatrix = boardData;
            var updatedMatrix = _gameEngine.Generate(currentMatrix);

            _persistence.Save(boardId, updatedMatrix);

            return new Board { Id = boardId, Matrix = updatedMatrix };
        }

        public Board Upload(List<List<int>> matrix)
        {
            if (matrix == null || matrix.Count == 0)
            {
                throw new EngineException("Invalid matrix input.");
            }

            var boardId = Guid.NewGuid();
            _persistence.Save(boardId, matrix);

            return new Board { Id = boardId, Matrix = matrix, Message = "Board was successfully uploaded." };
        }

        public Board GetFutureStates(Guid boardId, int steps)
        {
            if (steps < 1)
            {
                throw new EngineException("Invalid request.");
            }

            var boardData = _persistence.Get(boardId);

            if (boardData == null)
            {
                throw new EngineException("Board not found.");
            }

            var currentMatrix = boardData;
            for (int i = 0; i < steps; i++)
            {
                currentMatrix = _gameEngine.Generate(currentMatrix);
            }

            _persistence.Save(boardId, currentMatrix);

            return new Board { Id = boardId, Matrix = currentMatrix };
        }

        public Board GetFinalState(Guid boardId, int maxAttempts)
        {
            string _boardId = boardId.ToString();

            if (maxAttempts < 1)
            {
                throw new EngineException("Invalid request.");
            }

            var boardData = _persistence.Get(boardId);

            if (boardData == null)
            {
                throw new EngineException("Board not found.");
            }

            var currentMatrix = boardData;
            var previousStates = new HashSet<string>();
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                var stateString = string.Join("|", currentMatrix.Select(row => string.Join(",", row)));
                if (previousStates.Contains(stateString))
                {
                    return new Board { Id = boardId, Matrix = currentMatrix, Message = "Board reached a stable state." };
                }

                previousStates.Add(stateString);
                currentMatrix = _gameEngine.Generate(currentMatrix);
                attempts++;
            }

            return new Board { Id = boardId, Matrix = currentMatrix, Message = "Board did not reach a conclusion within the maximum number of attempts." };
        }
    }

    public struct Board
    {
        public Guid Id { get; set; }
        public List<List<int>> Matrix { get; set; }
        public string Message { get; set; }
    }
}

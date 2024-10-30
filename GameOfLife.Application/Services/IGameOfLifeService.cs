namespace GameOfLife.Application.Services
{
    public interface IGameOfLifeService
    {
        public Board Upload(List<List<int>> matrix);

        public Board Generate(Guid boardId);

        public Board GetFutureStates(Guid boardId, int steps);

        public Board GetFinalState(Guid boardId, int maxAttempts);
    }
}
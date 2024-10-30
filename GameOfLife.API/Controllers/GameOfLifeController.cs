using GameOfLife.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GameOfLife.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameOfLifeController : ControllerBase
    {
        private readonly IGameOfLifeService _service;

        public GameOfLifeController(IGameOfLifeService gameOfLifeservice)
        {
            _service = gameOfLifeservice;
        }

        [HttpPost("upload")]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult Upload([FromBody] List<List<int>> matrix)
        {
            var board = _service.Upload(matrix);
            return Ok(board);
        }

        [HttpPost("generate")]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<Board> Generate([FromBody] Guid boardId)
        {
            var board = _service.Generate(boardId);
            return Ok(board);
        }

        [HttpPost("states/{steps}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<object> GetFutureStates([FromBody] Guid boardId, int steps)
        {
            var board = _service.GetFutureStates(boardId, steps);
            return Ok(board);
        }

        [HttpPost("final/{maxAttempts}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<object> GetFinalState([FromBody] Guid boardId, int maxAttempts)
        {
            var board = _service.GetFinalState(boardId, maxAttempts);
            return Ok(board);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using ShipsAPI.DTOs;
using ShipsAPI.Interfaces;

namespace ShipsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class GameController : ControllerBase
    {
     
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            Console.WriteLine("GameController vytvořen");
            _gameService = gameService;
        }

        // Vytvoření nové hry
        [HttpPost("new")]
        public IActionResult NewGame([FromBody] GameInitRequest request)
        {
            _gameService.NewGame(request.Player1, request.Player2, request.BoardSize);
            return Ok("Hra byla vytvořena.");
        }

        // Hráč vystřelí na dané souřadnice
        [HttpPost("fire")]
        public IActionResult Fire([FromBody] FireRequest request)
        {
            var result = _gameService.Fire(request.PlayerName, request.X, request.Y);
            return Ok(result);
        }

        // Vystřel hráče
        [HttpGet("status")]
        public IActionResult IsGameOver()
        {
            return Ok(new {IsGameOver = _gameService.IsGameOver() });
        }

        // Výherce 
        [HttpGet("winner")]
        public IActionResult GetWinner()
        {
            var winner = _gameService.GetWinner();
            if (winner == null)
                return Ok("Hra ještě neskončila");
            return Ok($"Výherce je {winner}");
        }




    }
}

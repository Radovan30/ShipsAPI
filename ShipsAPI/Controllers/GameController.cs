using Microsoft.AspNetCore.Mvc;
using ShipsAPI.DTOs;
using ShipsAPI.Interfaces;
using ShipsAPI.Services;

namespace ShipsAPI.Controllers
{
    [ApiController]
    [Route("api/battleship")]

    public class GameController : ControllerBase
    {
     
        private readonly IGameService _gameService;
        private readonly GameServiceHelper _helper;

        public GameController(IGameService gameService, GameServiceHelper helper)
        {
            // Console.WriteLine("GameController vytvořen");
            _gameService = gameService;
            _helper = helper;
        }

        // Vytvoření nové hry
        [HttpPost("games")]
        public IActionResult NewGame([FromBody] GameInitRequest request)
        {
            _gameService.NewGame(request.Player1, request.Player2, request.BoardSize);
            return Ok("Hra byla vytvořena.");
        }

        // Vystřel hráče
        [HttpPost("fire")]
        public IActionResult Fire([FromBody] FireRequest request)
        { 
            var result = _gameService.Fire(request.PlayerName, request.X, request.Y);
            _helper.PrintResult(result, request.PlayerName);
            return Ok(result);
        }

        // Kontrola konce hry
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

        // Aktivní hráč
        [HttpGet("turn")]
        public IActionResult GetCurrentPlayer()
        {
            return Ok($"Na tahu je: {_gameService.GetCurrentPlayer()}");
        }



    }
}

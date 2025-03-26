using Microsoft.AspNetCore.Mvc;
using ShipsAPI.Services;

namespace ShipsAPI.Interfaces
{
    public interface IGameService
    {
        void NewGame(string player1, string player2, int boardSize);
        string Fire(string playerName, int x, int y);
        bool IsGameOver();
        string? GetWinner();
        string GetCurrentPlayer();
    }
}

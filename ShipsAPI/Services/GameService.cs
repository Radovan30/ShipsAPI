using ShipsAPI.Interfaces;
using ShipsAPI.Models;

namespace ShipsAPI.Services
{
    public class GameService : IGameService
    {
        private Player _player1 = null!;
        private Player _player2 = null!;
        private Player _activePlayer = null!;
        private Player _waitingPlayer = null!;

        private bool _gameOver;

        // Vytvoří hru - hrače a vygeneruje lodě
        public void NewGame(string player1Name, string player2Name, int boardSize)
        {
            Console.WriteLine("Byla vytvořena nová hra!");

            _player1 = new Player(player1Name, boardSize);
            _player2 = new Player(player2Name, boardSize);

            _player1.SetActive(true);
            _player2.SetActive(false);

            _activePlayer = _player1;
            _waitingPlayer = _player2;

            _gameOver = false;

            Console.WriteLine($"Hraje {_activePlayer.GetName()}");
        }

        // Útok - vyhodnotí zasáha/vodu/výhru
        public string Fire(string playerName, int x, int y)
        {
            Console.WriteLine("Střílel hráč: " + playerName);

            if (_gameOver) 
                return "Hra skončila!";

            if (_activePlayer.GetName() != playerName)
                return "Nejsi na tahu";

            var targeBoard = _waitingPlayer.GetBoard();
            var cell = targeBoard.GetCell(x, y);

            if (cell.IsHit)
                return "Toto pole už bylo zasaženo";

            cell.MarkHit();

            if (!cell.HasShip())
            {
                SwitchTurns();
                return "Voda";
            }

            var ship = cell.GetShip;

            if (ship!.IsSunk())
            {
                if (targeBoard.AllShipsSunk())
                {
                    _gameOver = true;
                    return $"Zásah a potopeno! Hráč {playerName} vyhrál";
                }

                return "Zásah a potopeno!";
            }

            return "Zásah";
        }

        // Přepne hráče
        private void SwitchTurns()
        {
            _activePlayer.SetActive(false);
            _waitingPlayer.SetActive(true);

            var temp = _activePlayer;
            _activePlayer = _waitingPlayer;
            _waitingPlayer = temp;
        }

        // Vrací stav hry
        public bool IsGameOver() => _gameOver;

        // Vrací jméno vítěze
        public string? GetWinner() => _gameOver ? _activePlayer.GetName() : null;

    }
}

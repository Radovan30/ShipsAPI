namespace ShipsAPI.Models
{
    public class Player
    {

        private string _name;
        private Board _gameBoard;
        private bool _isActive;


        public Player(string name, int boardSize)
        {
            _name = name;
            _gameBoard = new Board(boardSize);
            _isActive = false;
        }


        // Get a Set pro jméno – se základní validací
        public string GetName() 
        { 
            return _name; 
        }

        public void SetName(string name) 
        { 
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("Jméno hráče nesmí být prázdné");
            _name = name; 
        }


        // Get pro Board
        public Board GetBoard()
        { 
            return _gameBoard;
        }


        // Get a Set pro aktivitu hráče
        public bool IsActive()
        {
            return _isActive;
        }


        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }


        // Pomocná metoda pro ladění/logování
        public override string ToString()
        {
            return $"{_name} ({_gameBoard.GetWidth}x{_gameBoard.GetHeight})";
        }



    }
}

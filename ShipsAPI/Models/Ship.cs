namespace ShipsAPI.Models
{
    public abstract class Ship
    {

        // Seznam buněk, které loď obsazuje na herním poli
        protected List<Cell> _occupiedCells = new();

        // Vrací seznam obsazených buněk této lodě
        public List<Cell> GetOccupiedCells() { return _occupiedCells; }

        // Vrací true, pokud jsou všechny buňky lodě zasažené
        public bool IsSunk()
        {
            foreach (var cell in _occupiedCells)
            {
                if (!cell.IsHit) 
                    return false;
            }
            return true;
        }

        // Abstraktní metoda – každá podtřída (typ lodě) musí vrátit svůj název
        public abstract string GetName();
    }
}

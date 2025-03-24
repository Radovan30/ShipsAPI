namespace ShipsAPI.Models
{
    // Buňka 
    public class Cell
    {

        private int _x;
        private int _y;
        private bool _isHit; // byla buňka zasažena
        private Ship? ship; // odkaz na instanci Lodě - ?může být null


        public Cell(int x, int y)
        {
            _x = x;
            _y = y;
            _isHit = false;
        }


        // Metody Get pro vracení souřadnic X a Y
        public int GetX { get { return _x; } }
        public int GetY { get { return _y; } }

        // Stavy pro zasažení buněk
        public bool IsHit { get { return _isHit; } }

        // Nastaví buňku jako zasaženou
        public void MarkHit()
        {
            _isHit = true;
        }
        
        
        // Ziskání reference na loď, která je v dané buňce - může být i null (když tam žádná není)
        public Ship? Ship { get { return ship; } }

        // Nastaví v dné buňce část lodi
        public void SetShip(Ship ships)
        {
            ship = ships;
        }

        // Kontrola zda buňka obsahuje loď
        public bool HasShip()
        {
            return ship != null;
        }

    }
}
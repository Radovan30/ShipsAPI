namespace ShipsAPI.Models.Ships
{
    public class TwoBlockShip : Ship
    {
        public TwoBlockShip(Cell cell) 
        { 
            _occupiedCells = new List<Cell>();
            cell.SetShip(this);
        }

        public override string GetName()
        {
            return "1x2";
        }

    }
}

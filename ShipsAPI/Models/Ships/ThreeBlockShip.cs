namespace ShipsAPI.Models.Ships
{
    public class ThreeBlockShip : Ship
    {
        public ThreeBlockShip(Cell cell) 
        { 
            _occupiedCells = new List<Cell>();
            cell.SetShip(this);
        }

        public override string GetName()
        {
            return "1x3";
        }
    }
}

namespace ShipsAPI.Models.Ships
{
    public class OneBlockShip : Ship
    {
        public OneBlockShip(Cell cell) 
        { 
            _occupiedCells.Add(cell);
            cell.SetShip(this);
        }

        public override string GetName()
        {
            return "1x1";
        }

    }
}

namespace ShipsAPI.Models.Ships
{
    public class ThreeBlockShip : Ship
    {

        public ThreeBlockShip(List<Cell> cells) 
        {
            if (cells.Count != 3)
                throw new ArgumentException("3x1 loď musí mít 3 buňky");

            _occupiedCells.AddRange(cells);

            foreach (var cell in cells)
            {
                cell.SetShip(this);
            }
            
        }

        public override string GetName()
        {
            return "3x1";
        }
    }
}

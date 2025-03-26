namespace ShipsAPI.Models.Ships
{
    public class OneBlockShip : Ship
    {

        public OneBlockShip(List<Cell> cells) 
        {
            if (cells.Count != 1)
                throw new ArgumentNullException("1x1 loď musí mít 1 buňku");

            _occupiedCells.AddRange(cells);

            foreach (var cell in cells)
            {
                cell.SetShip(this);
            }
        }

        public override string GetName()
        {
            return "1x1";
        }

    }
}

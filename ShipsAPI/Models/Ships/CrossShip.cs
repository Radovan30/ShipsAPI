namespace ShipsAPI.Models.Ships
{
    public class CrossShip : Ship
    {

       public CrossShip(List<Cell> cells)
       {
            if (cells.Count != 5)
                throw new ArgumentException("CrossShip musí mít 5 buněk (tvar kříže)");

            _occupiedCells.AddRange(cells);

            foreach (var cell in cells)
            {
                cell.SetShip(this);
            }
       }

        public override string GetName()
        {
            return "Cross";
        }
    }
}

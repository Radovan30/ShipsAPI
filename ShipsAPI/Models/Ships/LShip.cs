namespace ShipsAPI.Models.Ships
{
    public class LShip : Ship
    {

        public LShip(List<Cell> cells)
        {
            if (cells.Count != 5)
                throw new ArgumentException("LShip musí mít 5 buněk (tvar L)");

            _occupiedCells.AddRange(cells);

            foreach (var cell in cells)
            {
                cell.SetShip(this);
            }
        }

        public override string GetName()
        {
            return "L-Shape";
        }



    }
}

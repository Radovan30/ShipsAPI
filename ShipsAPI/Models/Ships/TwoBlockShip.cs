﻿namespace ShipsAPI.Models.Ships
{
    public class TwoBlockShip : Ship
    {

        public TwoBlockShip(List<Cell> cells)
        {
            if (cells.Count != 2)
                throw new ArgumentException("2x1 loď musí mít 2 buňky");

            _occupiedCells.AddRange(cells);

            foreach (var cell in cells)
            {
                cell.SetShip(this);
            }
        }

        public override string GetName()
        {
            return "2x1";
        }

    }
}

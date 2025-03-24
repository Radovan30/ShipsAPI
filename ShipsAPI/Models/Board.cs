using System;
using System.Collections.Generic;
using ShipsAPI.Models.Ships;

namespace ShipsAPI.Models
{
    public class Board
    {
        private int _width;
        private int _height;
        private Cell[,] _grid;
        private List<Ship> _ship;

        public Board(int size)
        {
            _grid = new Cell[_width, _height];
            _ship = new List<Ship>();

            InitializeGrid();
        }


        // Metoda pro nastavení velikosti hrací plochy
        private void SetSize(int size) 
        { 
            if (size < 10 || size > 20 )
                throw new ArgumentOutOfRangeException(nameof(size), "Velikost hracího pole musí být mezi 10 a 20");

            _width = size;
            _height = size;
        }

        public int GetWidth() => _width;
        public int GetHeight() => _height;


        // Metoda pro vytvoření hrací plochy 
        private void InitializeGrid() 
        {
            for (int x = 0; x < _width; x++)
            { 
                for (int y = 0; y < _height; y++)
                {
                    _grid[x, y] = new Cell(x, y);
                }
            }
        }


        // Metoda pro přidání lodě
        public void AddShip(Ship ship)
        {
            _ship.Add(ship);
        }


        // Kontrola zda jsou všechny lodě potopené
        public bool AllShipsSunk()
        {
            foreach (var ship in _ship)
            {
                if (!ship.IsSuck())
                    return false;
            }
            return true;
        }


        // Metoda pro generování lodí
        public void GenerateShips()
        {
            PlaceShips<OneBlockShip>(2, 1);
            PlaceShips<TwoBlockShip>(2, 2);
            PlaceShips<ThreeBlockShip>(1, 3);
        }


        private void PlaceShips<T>(int count, int size) where T : Ship
        {
            Random rnd = new Random();
            int attempts = 0;

            while (count > 0 && attempts < 1000)
            {
                int x = rnd.Next(_width);
                int y = rnd.Next(_height);
                bool horizontal = rnd.Next(2) == 0;
            }
        }







    }

}

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using ShipsAPI.Models.Ships;

namespace ShipsAPI.Models
{
    public class Board
    {
        private int _width;
        private int _height;
        private Cell[,] _grid;
        private List<Ship> _ships;

        public Board(int size)
        {
            SetSize(size);

            _grid = new Cell[_width, _height];
            _ships = new List<Ship>();

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
        public Cell[,] GetGrid() => _grid;
        public List<Ship> GetShip() => _ships;


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

        // Práce s buňkami
        public Cell GetCell(int x, int y)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height)
                throw new ArgumentOutOfRangeException("Souřadnice mimo hrací pole.");
            return _grid[x, y];
        }


        // Metoda pro přidání lodě
        public void AddShip(Ship ship)
        {
            _ships.Add(ship);
        }


        // Kontrola zda jsou všechny lodě potopené
        public bool AllShipsSunk()
        {
            foreach (var ship in _ships)
            {
                if (!ship.IsSunk())
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


        // Metoda se pokouší vložit loď do náhodné pozice, kontoluje že je místo volné a v okolí není žádná jiná loď
        private void PlaceShips<T>(int count, int size) where T : Ship
        {
            Random rnd = new();
            int attempts = 0;

            while (count > 0 && attempts < 1000)
            {
                int x = rnd.Next(_width);
                int y = rnd.Next(_height);
                bool horizontal = rnd.Next(2) == 0;

                var cells = TryGetEmptyCells(x, y, size, horizontal);
                if (cells == null || !IsSafePlacement(cells))
                {
                    attempts++;
                    continue;
                }

                Ship ship = (Ship)Activator.CreateInstance(typeof(T), cells)!;
                _ships.Add(ship);
                count--;
            }

            if (attempts >= 1000)
                throw new Exception("Nepodařilo se umístit všecny lodě.");

        }


        // Metoza ziskava volné buňky pro loď 
        private List<Cell>? TryGetEmptyCells(int x, int y, int size, bool horizontal)
        {
            var cells = new List<Cell>();

            for (int i = 0; i < size; i++)
            {
                // Podle směru vypočet souřadnic každé buňky lodě
                int xi = horizontal ? x + i : x; // posun vpravo
                int yi = horizontal ? y : y + i; // posun dolů

                // Ošetření mimo hrací pole
                if (xi >= _width || yi >= _height)
                    return null;

                // Získa buňku na pozici
                var cell = GetCell(xi, yi);

                // Jestli buňka obsahuje jinou loď, nemůže ji použít
                if (cell.HasShip()) 
                    return null;

                // Přidání buňky do seznamu
                cells.Add(cell);
            }

            return cells;
        }


        // Metoda zabraňuje doteku lodí
        private bool IsSafePlacement(List<Cell> cells)
        {
            foreach (var cell in cells)
            {
                // Projde všechny okolní buńky 
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int nx = cell.GetX + dx;
                        int ny = cell.GetY + dy;

                        // Kontrola že žádná sousední buňka neobsahuje loď
                        if (nx >= 0 && nx < _width && ny >= 0 && ny < _height)
                        {
                            var neighbor = GetCell(nx, ny);
                            if (neighbor.HasShip() && !cells.Contains(neighbor))
                                return false;
                        }
                    }
                }
            }

            // Všechny okolní buňky jsou prazdné
            return true;
        }

    }

}

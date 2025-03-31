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
        public List<Ship> GetShips() => _ships;


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
            if (x < 0 || x >= _width)
                throw new ArgumentOutOfRangeException(nameof(x), "Souřadnice x je mimo hrací pole");

            if (y < 0 || y >= _height)
                throw new ArgumentOutOfRangeException(nameof(y), "Souřadnice y je mimo hrací pole");

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
            foreach (var (shipType, count, shape) in _shipConfigs)
            {
                for (int i = 0; i < count; i++)
                {
                    bool success = TryPlaceShip(shipType, shape);
                    Console.WriteLine($"{shipType.Name} umístění: {(success ? "OK" : "FAIL")}");
                }
            }

            // Počet lodí a zobrazení hracích ploch
            Console.WriteLine($"Celkem lodí: {_ships.Count}");
            PrintBoard();
        }


        // Konfigurace lodi
        private readonly List<(Type shipType, int count, List<(int dx, int dy)> shape)> _shipConfigs =
        [
            (typeof(OneBlockShip), 2, new List<(int, int)> { (0, 0) }),
            (typeof(TwoBlockShip), 2, new List<(int, int)> { (0, 0), (1, 0) }),
            (typeof(ThreeBlockShip), 1, new List<(int, int)> { (0, 0), (1, 0), (2, 0) }),
            (typeof(CrossShip), 1, new List<(int, int)> { (0, 0), (0, -1), (0, 1), (-1, 0), (1, 0) }),
            (typeof(LShip), 1, new List<(int, int)> { (0, 0), (0, 1), (0, 2), (0, 3), (1, 3) })
        ];


        //// Metoda se pokouší vložit loď do náhodné pozice, kontroluje volné místo v okolí
        private bool TryPlaceShip(Type shipType,  List<(int dx, int dy)> baseShape)
        {
            Random rnd = new();
            int attempts = 0;

            while (attempts < 1000)
            {
                // Náhodné souřadnice x, y a rotace lodě
                int x = rnd.Next(_width);
                int y = rnd.Next(_height);
                int rotation = shipType == typeof(OneBlockShip) ? 0 : rnd.Next(4) * 90; // Kontrola jestli loď obsahuje 1 blok, pokud ano tak se neprovede otáčení jínak se otáčení provede 

                var rotatedShape = RotateShape(baseShape, rotation); // Otočení původního tvaru lodě podle uhlu
                var cells = TryPlaceCustomShip(x, y, rotatedShape);  // Umístění lodě na náhodou pozici

                if (cells != null)
                {
                    try
                    {
                        var ship = (Ship?)Activator.CreateInstance(shipType, cells); 

                        if (ship != null)
                        {
                            _ships.Add(ship);
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"Nelze vytvořit loď {shipType.Name} (null)");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Výjimka při vytváření {shipType.Name}: {ex.Message}");
                    }
                }

                attempts++;

            }

            return false;
           
        }


        // Rotace lodí, vrací nový seznam souřadnic který reprezentuje otočený tvar lodě
        private static List<(int dx, int dy)> RotateShape(List<(int dx, int dy)> shape, int angel)
        {
            return shape.ConvertAll(p => angel switch
            {
                90 => (-p.dy, p.dx),
                180 => (-p.dx, -p.dy),
                270 => (p.dy, -p.dx),
                _ => p
            });
        }


        // Metoda, která zjišťuje jestli jde umístit loď na souřadnice aniž by přesahovala hrací pole
        private List<Cell>? TryPlaceCustomShip(int originX, int originY, List<(int dx, int dy)> shape)
        {
            var cells = new List<Cell>();

            foreach ( var (dx, dy) in shape)
            {
                int x = originX + dx;
                int y = originY + dy;

                // Ověření souřadnic jestli nejsou mimo herní plochu
                if (x < 0 || x >= _width || y < 0 || y >= _height)
                    return null;

                var cell = GetCell(x, y);
                if (cell.HasShip())
                    return null;

                cells.Add(cell);
            }

            return IsSafePlacement(cells) ? cells : null;
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


        // Kontrolní výpis hrací plochy 
        public void PrintBoard()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var cell = GetCell(x, y);
                    Console.Write(cell.HasShip() ? "■ " : "_ ");
                }
                Console.WriteLine();
            }
        }



        // Stará funkčnost pro jednoduché lodě bez rotace 
        //
        //// Metoda se pokouší vložit loď do náhodné pozice, kontoluje že je místo volné a v okolí není žádná jiná loď
        //private void PlaceShips<T>(int count, int size) where T : Ship
        //{
        //    Random rnd = new();
        //    int attempts = 0;

        //    while (count > 0 && attempts < 1000)
        //    {
        //        // Náhodné souřadnice x, y
        //        int x = rnd.Next(_width);
        //        int y = rnd.Next(_height);
        //        bool horizontal = rnd.Next(2) == 0;

        //        // Získání volných buněk
        //        var cells = TryGetEmptyCells(x, y, size, horizontal);
        //        // Jestli nejsou buňky volné a nebo se dotýkájí tak se nastavý nový pokus
        //        if (cells == null || !IsSafePlacement(cells))
        //        {
        //            attempts++;
        //            continue;
        //        }

        //        // Vytvoření instance lodě a přidání do seznamu
        //        Ship ship = (Ship)Activator.CreateInstance(typeof(T), cells)!;
        //        _ships.Add(ship);
        //        count--;
        //    }

        //    // 1000 pokusů pro umístění
        //    if (attempts >= 1000)
        //        throw new Exception("Nepodařilo se umístit všecny lodě.");

        //}


        //// Metoza ziskava volné buňky pro loď 
        //private List<Cell>? TryGetEmptyCells(int x, int y, int size, bool horizontal)
        //{
        //    var cells = new List<Cell>();

        //    for (int i = 0; i < size; i++)
        //    {
        //        // Podle směru vypočet souřadnic každé buňky lodě
        //        int xi = horizontal ? x + i : x; // posun vpravo
        //        int yi = horizontal ? y : y + i; // posun dolů

        //        // Ošetření mimo hrací pole
        //        if (xi >= _width || yi >= _height)
        //            return null;

        //        // Získa buňku na pozici
        //        var cell = GetCell(xi, yi);

        //        // Jestli buňka obsahuje jinou loď, nemůže ji použít
        //        if (cell.HasShip()) 
        //            return null;

        //        // Přidání buňky do seznamu
        //        cells.Add(cell);
        //    }

        //    return cells;
        //}



    }

}

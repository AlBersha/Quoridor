using System.Collections.Generic;
using System.Linq;
using System;

namespace Quoridor.Model
{
    using Cells = List<Cell>;

    public struct Wall
    {
        public Wall(Cell c1, Cell c2, Cell c3, Cell c4, bool isVertical)
        {
            cells = new List<Cell>();

            cells.Add(c1);
            cells.Add(c2);
            cells.Add(c3);
            cells.Add(c4);

            this.isVertical = isVertical;
        }

        public List<List<Cell>> GetPairs()
        {
            if (!isVertical)
                cells.Sort((Cell left, Cell right) => {
                    int res = left.X.CompareTo(right.X);
                    return res != 0 ? res : left.Y.CompareTo(right.Y);
                });
            else
                cells.Sort((Cell left, Cell right) => {
                    int res = left.Y.CompareTo(right.Y);
                    return res != 0 ? res : left.X.CompareTo(right.X);
                });

            return new List<List<Cell>>()
            {
                new List<Cell>()
                {
                    cells[0],
                    cells[1]
                },
                new List<Cell>()
                {
                    cells[2],
                    cells[3]
                },
            };
        }

        public Wall Reverse()
        {
            return new Wall(cells[0], cells[1], cells[2], cells[3], !isVertical);
        }

        public static bool operator ==(Wall l, Wall r)
        {
            var cellsL = l.cells;
            cellsL.Sort((Cell left, Cell right) => {
                int res = left.X.CompareTo(right.X);
                return res != 0 ? res : left.Y.CompareTo(right.Y);
            });

            var cellsR = r.cells;
            cellsR.Sort((Cell left, Cell right) => {
                int res = left.X.CompareTo(right.X);
                return res != 0 ? res : left.Y.CompareTo(right.Y);
            });
            
            return Enumerable.SequenceEqual(cellsL, cellsR) && l.isVertical == r.isVertical;
        }

        public static bool operator !=(Wall l, Wall r)
        {
            return !(l == r);
        }

        public static Wall operator +(Wall wall, Cell cell)
        {
            var newWall = new Wall(wall.cells[0] + cell, wall.cells[1] + cell, wall.cells[2] + cell, wall.cells[3] + cell, wall.isVertical);
            foreach (var c in newWall.cells)
                if (c.X < 0 || c.X > GameField.fieldSize || c.Y < 0 || c.Y > GameField.fieldSize)
                    return wall;
            
            return newWall;
        }

        public Cells cells { get; }
        public bool isVertical { get; }
    }
}

using System.Collections.Generic;

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
            return new Wall(this.cells[0], this.cells[1], this.cells[2], this.cells[3], !this.isVertical);
        }

        public static Wall operator +(Wall wall, Cell cell)
        {
            return new Wall(wall.cells[0] + cell, wall.cells[1] + cell, wall.cells[2] + cell, wall.cells[3] + cell, wall.isVertical);
        }

        public Cells cells { get; }
        public bool isVertical { get; }
    }
}

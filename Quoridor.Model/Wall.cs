using System.Collections.Generic;

namespace Quoridor.Model
{
    using Cells = List<Cell>;

    public struct Wall
    {
        public Wall(Cell c1, Cell c2, Cell c3, Cell c4)
        {
            cells = new List<Cell>();

            cells.Add(c1);
            cells.Add(c2);
            cells.Add(c3);
            cells.Add(c4);
        }

        public Cells cells { get; }
    }
}

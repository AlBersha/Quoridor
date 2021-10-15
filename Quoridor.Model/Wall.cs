using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Model
{
    using Cells = List<Cell>;

    public class Wall
    {
        public Wall(Cell c1, Cell c2, Cell c3, Cell c4)
        {
            cells.Add(c1);
            cells.Add(c2);
            cells.Add(c3);
            cells.Add(c4);
        }

        public Cells cells { get; }
    }
}

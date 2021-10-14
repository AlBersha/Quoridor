using System.Collections.Generic;
using System.Numerics;

namespace Quoridor.Model
{
    public class GameField
    {
        public GameField()
        {
            cells = new List<Cell>[fieldSize, fieldSize];
            wallsList = new List<Wall>();
        }

        public void RemovePassage(Cell from, Cell passage)
        {
            cells[from.X, from.Y].Remove(passage);
        }

        public bool AddWall(Wall wall)
        {
            if (wallsList.Contains(wall))
                return false;
            else
                wallsList.Add(wall);



            return true;
        }

        public const int fieldSize = 9;
        private List<Cell>[,] cells;
        private List<Wall> wallsList;
    }
}

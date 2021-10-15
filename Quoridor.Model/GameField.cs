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

        private void RemovePassage(Cell from, Cell passage)
        {
            cells[from.X, from.Y].Remove(passage);
        }

        private void RemovePassages(Wall wall)
        {
            RemovePassage(wall.cells[0], wall.cells[1]);
            RemovePassage(wall.cells[1], wall.cells[0]);

            RemovePassage(wall.cells[2], wall.cells[3]);
            RemovePassage(wall.cells[3], wall.cells[2]);
        }

        public bool AddWall(Wall wall)
        {
            if (wallsList.Contains(wall))
                return false;
            else
                wallsList.Add(wall);

            RemovePassages(wall);

            return true;
        }

        public const int fieldSize = 9;
        private List<Cell>[,] cells;
        private List<Wall> wallsList;
    }
}

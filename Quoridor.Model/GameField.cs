using System.Collections.Generic;

namespace Quoridor.Model
{
    public class GameField
    {
        public GameField()
        {
            cells = new List<Cell>[fieldSize, fieldSize];

            for (int columnIndex = 0; columnIndex < fieldSize; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < fieldSize; rowIndex++)
                {
                    cells[columnIndex, rowIndex] = GeneratePassages(new Cell(columnIndex, rowIndex));
                }
            }

            wallsList = new List<Wall>();
        }

        private List<Cell> GeneratePassages(Cell from)
        {
            List<Cell> result = new List<Cell>();

            if (from.X > 0) result.Add(new Cell(from.X - 1, from.Y));
            if (from.Y > 0) result.Add(new Cell(from.X, from.Y - 1));
            if (from.X < fieldSize - 1) result.Add(new Cell(from.X + 1, from.Y));
            if (from.Y < fieldSize - 1) result.Add(new Cell(from.X, from.Y + 1));

            return result;
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

        public List<Cell> GetPassages(Cell from)
        {
            return cells[from.X, from.Y];
        }

        public const int fieldSize = 9;
        private List<Cell>[,] cells;
        private List<Wall> wallsList;
    }
}

using System.Collections.Generic;

namespace Quoridor.Model
{
    using Vector2Int = Cell;
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

        private bool WallExists(Cell from, Vector2Int direction)
        {
            Cell possibleWallCell = from + direction;

            return cells[from.X, from.Y].Contains(possibleWallCell);
        }

        private bool IsWallBehindSecondPlayer(Cell secondPlayerPosition, Vector2Int direction)
        {
            return WallExists(secondPlayerPosition, direction);
        }
        
        private List<Cell> GetPossibleDiagonalSpecialMoves(Cell secondPlayerPosition, Vector2Int direction)
        {
            List<Cell> result = new List<Cell>();

            if (direction == Cell.UnaryDown || direction == Cell.UnaryUp)
            {
                if (!WallExists(secondPlayerPosition, direction + Cell.UnaryLeft)) result.Add(secondPlayerPosition + direction + Cell.UnaryLeft);
                if (!WallExists(secondPlayerPosition, direction + Cell.UnaryRight)) result.Add(secondPlayerPosition + direction + Cell.UnaryRight);
            }
            else
            {
                if (!WallExists(secondPlayerPosition, direction + Cell.UnaryUp)) result.Add(secondPlayerPosition + direction + Cell.UnaryUp);
                if (!WallExists(secondPlayerPosition, direction + Cell.UnaryDown)) result.Add(secondPlayerPosition + direction + Cell.UnaryDown);
            }

            return result;
        }

        private List<Cell> ReplaceOnPossibleSpecialMoves(List<Cell> availableMoves, Cell firstPlayerPosition, Cell secondPlayerPosition)
        {
            List<Cell> result = availableMoves;

            bool isSpecialMovePossible = availableMoves.Contains(secondPlayerPosition);
            if (isSpecialMovePossible)
            {
                Vector2Int direction = secondPlayerPosition - firstPlayerPosition;

                if (IsWallBehindSecondPlayer(secondPlayerPosition, direction))
                {
                    result.AddRange(GetPossibleDiagonalSpecialMoves(secondPlayerPosition, direction));
                }
                else
                {
                    result.Remove(secondPlayerPosition);
                    result.Add(secondPlayerPosition + direction);
                }
            }

            return result;
        }

        public List<Cell> GeneratePossibleMoves(Cell firstPlayerPosition, Cell secondPlayerPosition)
        {
            List<Cell> result = cells[firstPlayerPosition.X, firstPlayerPosition.Y];
            result = ReplaceOnPossibleSpecialMoves(result, firstPlayerPosition, secondPlayerPosition);

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
            
            RemovePassage(wall.cells[0], wall.cells[1]);
            RemovePassage(wall.cells[1], wall.cells[0]);
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

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

            return !cells[from.X, from.Y].Contains(possibleWallCell);
        }

        private bool IsInFieldBorders(Cell cell)
        {
            return cell.X >= 0 && cell.X <= fieldSize - 1 &&
                cell.Y >= 0 && cell.Y <= fieldSize - 1;
        }
        
        private List<Cell> GetPossibleDiagonalSpecialMoves(Cell secondPlayerPosition, Vector2Int direction)
        {
            List<Cell> result = new List<Cell>();

            if (direction == Cell.UnaryDown || direction == Cell.UnaryUp)
            {
                if (!WallExists(secondPlayerPosition + direction, Cell.UnaryLeft) && IsInFieldBorders(secondPlayerPosition + direction + Cell.UnaryLeft))
                    result.Add(secondPlayerPosition + direction + Cell.UnaryLeft);
                if (!WallExists(secondPlayerPosition + direction, Cell.UnaryRight) && IsInFieldBorders(secondPlayerPosition + direction + Cell.UnaryRight))
                    result.Add(secondPlayerPosition + direction + Cell.UnaryRight);
            }
            else
            {
                if (!WallExists(secondPlayerPosition + direction, Cell.UnaryUp) && IsInFieldBorders(secondPlayerPosition + direction + Cell.UnaryUp))
                    result.Add(secondPlayerPosition + direction + Cell.UnaryUp);
                if (!WallExists(secondPlayerPosition + direction, Cell.UnaryDown) && IsInFieldBorders(secondPlayerPosition + direction + Cell.UnaryDown))
                    result.Add(secondPlayerPosition + direction + Cell.UnaryDown);
            }

            return result;
        }

        private List<Cell> ReplaceOnPossibleSpecialMoves(List<Cell> availableMoves, Cell firstPlayerPosition, Cell secondPlayerPosition)
        {
            List<Cell> result = availableMoves;

            bool isSpecialMovePossible = availableMoves.Contains(secondPlayerPosition);
            if (isSpecialMovePossible)
            {
                result.Remove(secondPlayerPosition);
                Vector2Int direction = secondPlayerPosition - firstPlayerPosition;
                
                if (WallExists(secondPlayerPosition, direction))
                    result.AddRange(GetPossibleDiagonalSpecialMoves(secondPlayerPosition, direction));
                else
                    result.Add(secondPlayerPosition + direction);
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
            var pairs = wall.GetPairs();
            foreach (var pair in pairs)
            {
                RemovePassage(pair[0], pair[1]);
                RemovePassage(pair[1], pair[0]);
            }
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

        public List<Cell> GetPassages(Cell from) => cells[from.X, from.Y];

        public const int fieldSize = 9;
        private List<Cell>[,] cells;
        private List<Wall> wallsList;
    }
}

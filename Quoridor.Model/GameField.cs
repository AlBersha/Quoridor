using System;
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
            Func<Cell, Vector2Int, bool> moveExists =
                (Cell from, Vector2Int dir) => (!WallExists(from, dir) && IsInFieldBorders(from + dir));

            if (direction == Cell.UnaryDown || direction == Cell.UnaryUp)
            {
                if (moveExists(secondPlayerPosition, Cell.UnaryLeft)) result.Add(secondPlayerPosition + Cell.UnaryLeft);
                if (moveExists(secondPlayerPosition, Cell.UnaryRight)) result.Add(secondPlayerPosition + Cell.UnaryRight);
            }
            else
            {
                if (moveExists(secondPlayerPosition, Cell.UnaryUp)) result.Add(secondPlayerPosition + Cell.UnaryUp);
                if (moveExists(secondPlayerPosition, Cell.UnaryDown)) result.Add(secondPlayerPosition + Cell.UnaryDown);
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

        private void AddPassage(Cell from, Cell passage)
        {
            cells[from.X, from.Y].Add(passage);
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

        private void AddPassages(Wall wall)
        {
            var pairs = wall.GetPairs();
            foreach (var pair in pairs)
            {
                AddPassage(pair[0], pair[1]);
                AddPassage(pair[1], pair[0]);
            }
        }

        public bool AddWall(Wall wall, Cell firstPlayerPosition, Cell secondPlayerPosition, Cell target)
        {
            RemovePassages(wall);

            bool waysFound = WinningWaysExist(firstPlayerPosition, secondPlayerPosition, target);
            if (wallsList.Contains(wall) || !waysFound)
            {
                AddPassages(wall);
                return false;
            }
            else
                wallsList.Add(wall);

            return true;
        }

        private bool WayExists(Cell from, Cell to, ref List<Cell> visitedCells)
        {
            foreach (var cell in cells[from.X, from.Y])
            {
                if (!visitedCells.Contains(cell))
                {
                    if (cell == to) return true;
                    else
                    {
                        visitedCells.Add(cell);
                        if (WayExists(cell, to, ref visitedCells)) return true;
                    }
                }
            }
            return false;
        }

        private bool WinningWaysExist(Cell firstPlayerPosition, Cell secondPlayerPosition, Cell target)
        {
            var visitedCells = new List<Cell>() { firstPlayerPosition };
            bool firstPlayerWayExists = WayExists(firstPlayerPosition, target, ref visitedCells);

            visitedCells = new List<Cell>() { secondPlayerPosition };
            bool secondPlayerWayExists = WayExists(secondPlayerPosition, target, ref visitedCells);

            return firstPlayerWayExists && secondPlayerWayExists;
        }

        public List<Cell> GetPassages(Cell from) => cells[from.X, from.Y];

        public const int fieldSize = 9;
        private List<Cell>[,] cells;
        private List<Wall> wallsList;
    }
}
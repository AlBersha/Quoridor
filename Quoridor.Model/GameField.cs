using System;
using System.Collections.Generic;
using System.Linq;

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
            var result = new List<Cell>();

            if (from.X > 0) result.Add(new Cell(from.X - 1, from.Y));
            if (from.Y > 0) result.Add(new Cell(from.X, from.Y - 1));
            if (from.X < fieldSize - 1) result.Add(new Cell(from.X + 1, from.Y));
            if (from.Y < fieldSize - 1) result.Add(new Cell(from.X, from.Y + 1));

            return result;
        }

        private bool WallExists(Cell from, Vector2Int direction)
        {
            var possibleWallCell = from + direction;

            return !cells[from.X, from.Y].Contains(possibleWallCell);
        }

        private bool IsInFieldBorders(Cell cell)
        {
            return cell.X is >= 0 and <= fieldSize - 1 && cell.Y is >= 0 and <= fieldSize - 1;
        }

        private List<Cell> GetPossibleDiagonalSpecialMoves(Cell secondPlayerPosition, Vector2Int direction)
        {
            var result = new List<Cell>();
            Func<Cell, Vector2Int, bool> moveExists =
                (@from, dir) => (!WallExists(from, dir) && IsInFieldBorders(from + dir));

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

        private List<Cell> ReplaceOnPossibleSpecialMoves(List<Cell> availableMoves, Player firstPlayer, Player secondPlayer)
        {
            var result = availableMoves;

            var isSpecialMovePossible = availableMoves.Contains(secondPlayer.Position);
            if (isSpecialMovePossible)
            {
                result.Remove(secondPlayer.Position);
                var direction = secondPlayer.Position - firstPlayer.Position;

                if (WallExists(secondPlayer.Position, direction))
                    result.AddRange(GetPossibleDiagonalSpecialMoves(secondPlayer.Position, direction));
                else
                    result.Add(secondPlayer.Position + direction);
            }

            return result;
        }

        public List<Cell> GeneratePossibleMoves(Player firstPlayer, Player secondPlayer)
        {
            var result = cells[firstPlayer.Position.X, firstPlayer.Position.Y];
            result = ReplaceOnPossibleSpecialMoves(result, firstPlayer, secondPlayer);

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

        public bool AddWall(Wall wall, Player firstPlayer, Player secondPlayer, Dictionary<string, List<Cell>> targets)
        {
            if (!CheckWallConsistency(wall)) return false;
            
            RemovePassages(wall);

            var firstNearWallExists = wallsList.Any(wallElement => wallElement == (wall + (wall.isVertical ? Vector2Int.UnaryUp : Vector2Int.UnaryLeft)));
            var secondNearWallExists = wallsList.Any(wallElement => wallElement == (wall + (wall.isVertical ? Vector2Int.UnaryDown : Vector2Int.UnaryRight)));
            var intersectingWallExists = wallsList.Any(wallElement => wallElement == wall.Reverse());

            if (wallsList.Contains(wall)   // the wall already exists
                || firstNearWallExists    // a wall overlaps the new one
                || secondNearWallExists   // ~
                || intersectingWallExists // ~
                || !WinningWaysExist(firstPlayer, secondPlayer, targets)) // the wall blocks someone
            {
                AddPassages(wall);
                return false;
            }

            wallsList.Add(wall);
            firstPlayer.WallsLeft--;
            return true;
        }

        private bool CheckWallConsistency(Wall wall)
        {
            return !(from cell in wall.cells let t = wall.cells.Where(c => Math.Abs(cell.X - c.X) == 1).ToList() let m = wall.cells.Where(c => Math.Abs(cell.Y - c.Y) == 1).ToList() let q = wall.cells.Exists(c => Math.Abs(cell.X - c.X) == 1 && Math.Abs(cell.Y - c.Y) == 1) where t.Count != 2 || m.Count != 2 && !q select t).Any();
        }

        private bool WayExists(Cell from, List<Cell> to, ref List<Cell> visitedCells)
        {
            foreach (var cell in cells[from.X, from.Y])
            {
                if (!visitedCells.Contains(cell))
                {
                    if (to.Contains(cell)) return true;
                    visitedCells.Add(cell);
                    if (WayExists(cell, to, ref visitedCells)) return true;
                }
            }
            return false;
        }

        private bool WinningWaysExist(Player firstPlayer, Player secondPlayer, Dictionary<string, List<Cell>> targets)
        {
            var visitedCells = new List<Cell>() { firstPlayer.Position };
            var firstPlayerWayExists = WayExists(firstPlayer.Position, targets[firstPlayer.Name], ref visitedCells);

            visitedCells = new List<Cell>() { secondPlayer.Position };
            var secondPlayerWayExists = WayExists(secondPlayer.Position, targets[firstPlayer.Name], ref visitedCells);

            return firstPlayerWayExists && secondPlayerWayExists;
        }

        public List<Cell> GetPassages(Cell from) => cells[from.X, from.Y];

        public const int fieldSize = 9;
        private List<Cell>[,] cells;
        private List<Wall> wallsList;
        public List<Wall> Walls => wallsList;
    }
}
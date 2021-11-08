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

        public GameField(GameField from)
        {
            cells = new List<Vector2Int>[fieldSize, fieldSize];

            for (int columnIndex = 0; columnIndex < fieldSize; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < fieldSize; rowIndex++)
                {
                    cells[columnIndex, rowIndex] = new List<Vector2Int>(from.cells[columnIndex, rowIndex]);
                }
            }

            wallsList = new List<Wall>(from.wallsList);
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

        public bool WallExists(Cell from, bool bottomToTopMovement)
        {
            return WallExists(from, !bottomToTopMovement ? Vector2Int.UnaryUp : Vector2Int.UnaryDown);
        }

        private bool WallExists(Cell from, Vector2Int direction)
        {
            var possibleWallCell = from + direction;

            return !cells[from.X, from.Y].Contains(possibleWallCell);
        }

        public bool IsInFieldBorders(Cell cell)
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

        private List<Cell> ReplaceOnPossibleSpecialMoves(List<Cell> availableMoves, Cell firstPlayer, Cell secondPlayer)
        {
            var result = new List<Cell>(availableMoves);

            var isSpecialMovePossible = availableMoves.Contains(secondPlayer);
            if (isSpecialMovePossible)
            {
                result.Remove(secondPlayer);
                var direction = secondPlayer - firstPlayer;

                if (WallExists(secondPlayer, direction))
                    result.AddRange(GetPossibleDiagonalSpecialMoves(secondPlayer, direction));
                else
                    result.Add(secondPlayer + direction);
            }

            return result.Where(x => IsInsideField(x)).ToList();
        }

        private bool IsInsideField(Cell cell)
        {
            return cell.X < GameField.fieldSize && cell.X >= 0 &&
                cell.Y < GameField.fieldSize && cell.Y >= 0;
        }

        public List<Cell> GeneratePossibleMoves(Cell firstPlayer, Cell secondPlayer)
        {
            var result = new List<Cell>(cells[firstPlayer.X, firstPlayer.Y]);
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

            var duplicateWallExists = wallsList.Any(wallElement => wallElement == wall);
            var firstNearWallExists = wallsList.Any(wallElement => wallElement == (wall + (wall.isVertical ? Vector2Int.UnaryUp : Vector2Int.UnaryLeft)));
            var secondNearWallExists = wallsList.Any(wallElement => wallElement == (wall + (wall.isVertical ? Vector2Int.UnaryDown : Vector2Int.UnaryRight)));
            var intersectingWallExists = wallsList.Any(wallElement => wallElement == wall.Reverse());

            if (duplicateWallExists       // the wall already exists
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

        private bool WayExists(Cell from, List<Cell> to, Cell secondPlayer)
        {
            Dictionary<Cell, bool> visited = new Dictionary<Cell, bool>();
            for (int indexX = 0; indexX < GameField.fieldSize; indexX++)
                for (int indexY = 0; indexY < GameField.fieldSize; indexY++)
                    visited[new Cell(indexX, indexY)] = false;
            Queue<Cell> yetToVisit = new Queue<Cell>();

            Cell currentCell;
            yetToVisit.Enqueue(from);
            visited[from] = true;

            while (yetToVisit.Count() > 0)
            {
                currentCell = yetToVisit.Dequeue();
                if (to.Contains(currentCell))
                {
                    return true;
                }
                foreach (var childCell in GeneratePossibleMoves(currentCell, secondPlayer))
                {
                    if (!visited[childCell])
                    {
                        yetToVisit.Enqueue(childCell);
                        visited[childCell] = true;
                    }
                }
            }

            return false;
        }

        private bool WinningWaysExist(Player firstPlayer, Player secondPlayer, Dictionary<string, List<Cell>> targets)
        {
            var firstPlayerWayExists = WayExists(firstPlayer.Position, targets[firstPlayer.Name], secondPlayer.Position);
            var secondPlayerWayExists = WayExists(secondPlayer.Position, targets[secondPlayer.Name], secondPlayer.Position);

            return firstPlayerWayExists && secondPlayerWayExists;
        }

        public List<Cell> GetPassages(Cell from) => cells[from.X, from.Y];

        public const int fieldSize = 9;
        private List<Cell>[,] cells;
        private List<Wall> wallsList;
        public List<Wall> Walls => wallsList;
    }
}
using System.Collections.Generic;

namespace Quoridor.Model
{
    public class MinimaxTree
    {
        class Node
        {
            public Node(GameField gameField, Player player, Player enemy, List<Cell> targets, int depth, bool recalculatePaths, int bestPathLengthPlayer, List<Cell> bestPathPlayer, int bestPathLengthEnemy, List<Cell> bestPathEnemy)
            {
                this.gameField = gameField;
                this.player = player;
                this.enemy = enemy;
                this.depth = depth;
                this.targets = targets;
                this.isPlayersTurn = depth % 2 == 0;

                if (depth == 0 || recalculatePaths)
                    GenerateBestPaths();
                else
                {
                    this.bestPathLengthPlayer = bestPathLengthPlayer;
                    this.bestPathPlayer = bestPathPlayer;

                    this.bestPathLengthEnemy = bestPathLengthEnemy;
                    this.bestPathEnemy = bestPathEnemy;
                }
                
                this.value = GetCurrentEvaluation(gameField, player, enemy);

                if (depth != maxDepth && !targets.Contains(player.Position) && !targets.Contains(enemy.Position))
                    this.children = GenerateChildren();
                else
                    this.children = new List<Node>();
            }

            private List<Node> PlaceWallsCloseToCell(Cell cell, bool vertically)
            {
                if (vertically ? cell.Y <= ta)
                {

                }

                
            }
            
            private List<Node> PlaceWallsCloseToTargets(bool vertically)
            {
                return new List<Node>();
            }

            private List<Node> GenerateVerticalWallsPlacingChildren()
            {
                List<Node> resulting_children = new List<Node>();

                resulting_children.InsertRange(0, PlaceWallsCloseToCell(isPlayersTurn ? player.Position : enemy.Position, true));
                resulting_children.InsertRange(0, PlaceWallsCloseToTargets(true));

                return resulting_children;
            }
            
            private List<Node> GenerateHorizontalWallsPlacingChildren()
            {
                List<Node> resulting_children = new List<Node>();

                resulting_children.InsertRange(0, PlaceWallsCloseToCell(isPlayersTurn ? player.Position : enemy.Position, false));
                resulting_children.InsertRange(0, PlaceWallsCloseToTargets(false));

                return resulting_children;
            }

            private void GenerateBestPaths()
            {
                List<Cell> visitedCells = new List<Cell>();
                (this.bestPathLengthPlayer, this.bestPathPlayer) = GetBestPath(player.Position, targets, 1, ref visitedCells, new List<Cell>());

                visitedCells.Clear();
                (this.bestPathLengthEnemy, this.bestPathEnemy) = GetBestPath(enemy.Position, targets, 1, ref visitedCells, new List<Cell>());
            }

            private List<Node> GenerateChildren()
            {
                List<Node> children = new List<Node>();

                if (IsMovingTheBestChoice())
                {
                    Player nextPlayerState = player;
                    nextPlayerState.Position = bestPathPlayer[bestPathPlayer.FindIndex(cell => cell == player.Position) + 1];
                    children.Add(new Node(gameField, nextPlayerState, enemy, targets, depth + 1, false, bestPathLengthPlayer, bestPathPlayer, bestPathLengthEnemy, bestPathEnemy));
                }
                else if (IsVerticalWallPlacingTheBestChoice())
                    children.InsertRange(0, GenerateVerticalWallsPlacingChildren());
                else
                    children.InsertRange(0, GenerateHorizontalWallsPlacingChildren());

                return children;
            }

            private float GetCurrentEvaluation(GameField gameField, Player player, Player enemy)
            {
                return bestPathLengthEnemy / bestPathLengthPlayer;
            }

            private bool IsVerticalWallInTheWay(int Y1, int Y2, int X)
            {
                if (Y1 == Y2) return false;

                if (gameField.GetPassages(new Cell(X, Y1)).Contains(new Cell(X, Y1 + 1)))
                    return IsVerticalWallInTheWay(Y1 + 1, Y2, X);
                else
                    return true;
            }

            private bool IsHorizontalWallInTheWay(int X1, int X2, int Y)
            {
                if (X1 == X2) return false;

                if (gameField.GetPassages(new Cell(X1, Y)).Contains(new Cell(X1 + 1, Y)))
                    return IsHorizontalWallInTheWay(X1 + 1, X2, Y);
                else
                    return true;
            }

            private bool IsMovingTheBestChoice()
            {
                return bestPathLengthPlayer < bestPathLengthEnemy;
            }

            private (int, List<Cell>) GetBestPath(Cell from, List<Cell> to, int length, ref List<Cell> visitedCells, List<Cell> path)
            {
                foreach (var cell in gameField.GetPassages(from))
                    if (to.Contains(cell))
                        return (length, path);

                int shortest_length = 100;
                List<Cell> shortest_path = new List<Cell>();

                foreach (var cell in gameField.GetPassages(from))
                {
                    if (!visitedCells.Contains(cell))
                    {
                        visitedCells.Add(cell);
                        (int found_length, List<Cell> found_path) = GetBestPath(cell, to, length + 1, ref visitedCells, path);
                        if (found_length < shortest_length)
                        {
                            shortest_length = found_length;
                            shortest_path = found_path;
                        }
                    }
                }

                return (shortest_length, shortest_path);
            }

            private bool IsVerticalWallPlacingTheBestChoice()
            {
                return targets.Exists(target => (
                    (enemy.Position.Y >= target.Y - GameField.fieldSize / 4) &&
                    (enemy.Position.Y <= target.Y + GameField.fieldSize / 4)
                ));
            }

            GameField gameField;
            
            Player player;
            Player enemy;

            float value;

            int depth;
            bool isPlayersTurn;
            static int maxDepth = 5;

            int bestPathLengthEnemy;
            List<Cell> bestPathEnemy;

            int bestPathLengthPlayer;
            List<Cell> bestPathPlayer;

            private List<Node> children;
            private List<Cell> targets;
        }

        Node root;
        int depth = 5;

        int currentLevel = 0;

    }
}

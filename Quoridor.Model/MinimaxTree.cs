using System.Collections.Generic;

namespace Quoridor.Model
{
    public class MinimaxTree
    {
        class Node
        {
            public Node(GameField gameField, Player player, Player enemy, List<Cell> targets, int currentDepth, bool recalculatePaths, int bestPathLengthPlayer, List<Cell> bestPathPlayer, int bestPathLengthEnemy, List<Cell> bestPathEnemy)
            {
                this.gameField = gameField;
                this.player = player;
                this.enemy = enemy;
                this.depth = currentDepth;
                this.targets = targets;

                if (currentDepth == 0 || recalculatePaths)
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
                {

                }
                else
                {

                }

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
                return targets.Exists(target => target.X == enemy.Position.X);
            }

            GameField gameField;
            Player player;
            Player enemy;
            float value;
            int depth;
            
            int bestPathLengthEnemy;
            List<Cell> bestPathEnemy;

            int bestPathLengthPlayer;
            List<Cell> bestPathPlayer;

            static int maxDepth = 5;
            private List<Node> children;
            private List<Cell> targets;
        }

        Node root;
        int depth = 5;

        int currentLevel = 0;

    }
}

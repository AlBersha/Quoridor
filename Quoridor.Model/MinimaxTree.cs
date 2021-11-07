using System;
using System.Collections.Generic;
using System.Linq;

namespace Quoridor.Model
{
    public class MinimaxTree
    {
        class Node
        {
            public Node(GameField gameField, Player player, Player enemy, Dictionary<string, List<Cell>> targets, int depth, bool recalculatePaths, int bestPathLengthPlayer, List<Cell> bestPathPlayer, int bestPathLengthEnemy, List<Cell> bestPathEnemy, Quoridor.GameAction gameAction)
            {
                this.gameField = gameField;
                this.player = player;
                this.enemy = enemy;
                this.depth = depth;
                this.targets = targets;
                this.gameAction = gameAction;

                if (depth == 0 || recalculatePaths)
                    GenerateBestPaths();
                else
                {
                    this.bestPathLengthPlayer = bestPathLengthPlayer;
                    this.bestPathPlayer = bestPathPlayer;

                    this.bestPathLengthEnemy = bestPathLengthEnemy;
                    this.bestPathEnemy = bestPathEnemy;
                }

                if (depth != maxDepth && bestPathLengthPlayer != 0 && bestPathLengthEnemy != 0)
                {
                    children = GenerateChildren();

                    isLeaf = children.Count() == 0;
                    if (isLeaf)
                        value = GetCurrentEvaluation();
                }
                else
                {
                    children = new List<Node>();
                    isLeaf = true;
                    value = GetCurrentEvaluation();
                }
            }

            private float GetCurrentEvaluation()
            {
                if (depth % 2 != 0 && bestPathLengthEnemy == 0)
                    return -100 + this.depth;
                else if (depth % 2 == 0 && bestPathLengthEnemy == 0)
                    return 100 - this.depth;
                else
                    return bestPathLengthEnemy / bestPathLengthPlayer;
            }

            private void TryAddingWallPlacingNode(int leftXPosition, int bottomYPosition, int topYPosition, bool isVerticalWall, ref GameField changedGameField, ref List<Node> children)
            {
                Wall wall = new Wall(
                        new Cell(leftXPosition, topYPosition),
                        new Cell(leftXPosition, bottomYPosition),
                        new Cell(leftXPosition + 1, topYPosition),
                        new Cell(leftXPosition + 1, bottomYPosition),
                        isVerticalWall);

                if (changedGameField.AddWall(wall, player, enemy, targets))
                    children.Add(new Node(changedGameField, enemy, player, targets, depth + 1, true, bestPathLengthEnemy, bestPathEnemy, bestPathLengthPlayer, bestPathPlayer, new Quoridor.GameAction(Quoridor.GameAction.GameActionType.WallPlacement, wall.cells)));

                changedGameField = gameField;
            }

            private List<Node> TryPlacingWallsCloseToEnemy()
            {
                // TODO add the check for the amount of walls left being 2 or more
                const bool isWallsCountAbove2 = false;
                bool bottomToTopMovement = enemy.Position.Y - targets[enemy.Name][0].Y < 0;
                bool areVerticalWallsPossible = Math.Abs(enemy.Position.Y - targets[enemy.Name][0].Y) > 1;
                bool areFarVerticalWallsPossible = Math.Abs(enemy.Position.Y - targets[enemy.Name][0].Y) > 2;

                List<Node> resulting_children = new List<Node>();
                GameField changedGameField = gameField;

                int lowerWallIndexY = enemy.Position.Y;
                int upperWallIndexY = bottomToTopMovement ? enemy.Position.Y - 1 : enemy.Position.Y + 1;

                // a vertical wall to the left of the player going "upwards"
                if (enemy.Position.X > 0 && areVerticalWallsPossible && isWallsCountAbove2)
                    TryAddingWallPlacingNode(enemy.Position.X - 1, lowerWallIndexY, upperWallIndexY, true, ref changedGameField, ref resulting_children);

                // a vertical wall to the far left of the player going "upwards"
                if (enemy.Position.X > 1 && areVerticalWallsPossible && isWallsCountAbove2)
                    TryAddingWallPlacingNode(enemy.Position.X - 2, lowerWallIndexY, upperWallIndexY, true, ref changedGameField, ref resulting_children);

                // a vertical wall to the right of the player going "upwards"
                if (enemy.Position.X < GameField.fieldSize - 1 && areVerticalWallsPossible && isWallsCountAbove2)
                    TryAddingWallPlacingNode(enemy.Position.X, lowerWallIndexY, upperWallIndexY, true, ref changedGameField, ref resulting_children);

                // a vertical wall to the far right of the player going "upwards"
                if (enemy.Position.X < GameField.fieldSize - 2 && areVerticalWallsPossible && isWallsCountAbove2)
                    TryAddingWallPlacingNode(enemy.Position.X, lowerWallIndexY, upperWallIndexY, true, ref changedGameField, ref resulting_children);

                // a horizontal wall right in front of the player going "left"
                if (enemy.Position.X < GameField.fieldSize - 1 && areVerticalWallsPossible)
                    TryAddingWallPlacingNode(enemy.Position.X - 1, lowerWallIndexY, upperWallIndexY, false, ref changedGameField, ref resulting_children);

                // a horizontal wall right in front of the player going "right"
                if (enemy.Position.X > 0 && areVerticalWallsPossible)
                    TryAddingWallPlacingNode(enemy.Position.X, lowerWallIndexY, upperWallIndexY, false, ref changedGameField, ref resulting_children);

                int farLowerWallIndexY = upperWallIndexY;
                int farUpperWallIndexY = bottomToTopMovement ? upperWallIndexY - 1 : upperWallIndexY + 1;

                // a horizontal wall in 1 cell in front of the player going "left"
                if (enemy.Position.X < GameField.fieldSize - 1 && areFarVerticalWallsPossible)
                    TryAddingWallPlacingNode(enemy.Position.X - 1, farLowerWallIndexY, farUpperWallIndexY, false, ref changedGameField, ref resulting_children);

                // a horizontal wall in 1 cell in front of the player going "right"
                if (enemy.Position.X > 0 && areFarVerticalWallsPossible)
                    TryAddingWallPlacingNode(enemy.Position.X, farLowerWallIndexY, farUpperWallIndexY, false, ref changedGameField, ref resulting_children);

                return resulting_children;
            }

            private void GenerateBestPaths()
            {
                List<Cell> visitedCells = new List<Cell>();
                (bestPathLengthPlayer, bestPathPlayer) = GetBestPath(player.Position, targets[player.Name], 0, ref visitedCells, new List<Cell>());

                visitedCells.Clear();
                (bestPathLengthEnemy, bestPathEnemy) = GetBestPath(enemy.Position, targets[enemy.Name], 0, ref visitedCells, new List<Cell>());
            }

            private List<Node> GenerateChildren()
            {
                List<Node> children = new List<Node>();

                Cell cellToGo = bestPathPlayer[bestPathPlayer.FindIndex(cell => cell == player.Position) + 1];
                if (IsMovingTheBestChoice() && gameField.GeneratePossibleMoves(player, enemy).Contains(cellToGo))
                {
                    Player nextPlayerState = player;
                    nextPlayerState.Position = cellToGo;
                    children.Add(new Node(gameField, enemy, nextPlayerState, targets, depth + 1, false, bestPathLengthEnemy, bestPathEnemy, bestPathLengthPlayer, bestPathPlayer, new Quoridor.GameAction(Quoridor.GameAction.GameActionType.Movement, new List<Cell> { cellToGo })));
                }
                else
                    children.InsertRange(0, TryPlacingWallsCloseToEnemy());

                return children;
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

            private GameField gameField;

            private Player player;
            private Player enemy;

            public float value;

            public int depth;
            static int maxDepth = 5;

            public bool isLeaf;

            private int bestPathLengthEnemy;
            private List<Cell> bestPathEnemy;

            private int bestPathLengthPlayer;
            private List<Cell> bestPathPlayer;

            public Quoridor.GameAction gameAction;

            public List<Node> children;
            private Dictionary<string, List<Cell>> targets;
        }

        public Quoridor.GameAction FindTheBestDecision(GameField gameField, Player player, Player enemy, Dictionary<string, List<Cell>> targets)
        {
            root = new Node(gameField, player, enemy, targets, 0, true, 0, new List<Cell>(), 0, new List<Cell>(), new Quoridor.GameAction(Quoridor.GameAction.GameActionType.Empty, new List<Cell>()));

            Node resultingDecision = GetTheBestNode(root);
            return resultingDecision.gameAction;
        }

        private Node GetTheBestNode(Node currentNode)
        {
            if (currentNode.isLeaf)
                return currentNode;

            bool isMinValuePreferrable = currentNode.depth % 2 == 0;

            Node bestNode = null, tempNode;
            bool isFirstIteration = true;

            foreach (Node childNode in currentNode.children)
            {
                if (isFirstIteration)
                {
                    bestNode = GetTheBestNode(childNode);
                    isFirstIteration = false;
                    continue;
                }

                tempNode = GetTheBestNode(childNode);
                if (isMinValuePreferrable ? tempNode.value < bestNode.value : tempNode.value > bestNode.value)
                    bestNode = tempNode;
            }

            return bestNode;
        }

        Node root;
    }
}

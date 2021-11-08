using System;
using System.Collections.Generic;
using System.Linq;

namespace Quoridor.Model
{
    public class MinimaxTree
    {
        class Node
        {
            public Node(GameField gameField, Player player, Player enemy, Dictionary<string, List<Cell>> targets, int depth, Quoridor.GameAction gameAction, bool isLeaf = false)
            {
                this.gameField = gameField;
                this.player = player;
                this.enemy = enemy;
                this.depth = depth;
                this.targets = targets;
                this.gameAction = gameAction;
                this.isLeaf = isLeaf;

                GenerateBestPaths();

                if (this.depth != maxDepth && this.bestPathLengthPlayer != 0 && this.bestPathLengthEnemy != 0 && !isLeaf)
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
                if (depth % 2 == 0 && bestPathLengthPlayer == 0)
                    return -100 + this.depth;
                else if ((depth % 2 != 0 && (bestPathLengthPlayer == 0 || bestPathLengthEnemy == 0))
                    || (depth % 2 == 0 && bestPathLengthEnemy == 0))
                    return 100 - this.depth;
                else
                    return bestPathLengthPlayer / bestPathLengthEnemy;
            }

            private bool TryAddingWallPlacingNode(int leftXPosition, int bottomYPosition, int topYPosition, bool isVerticalWall, ref List<Node> children)
            {
                GameField changedGameField = new GameField(gameField);
                Wall wall = new Wall(
                        new Cell(leftXPosition, topYPosition),
                        new Cell(leftXPosition, bottomYPosition),
                        new Cell(leftXPosition + 1, topYPosition),
                        new Cell(leftXPosition + 1, bottomYPosition),
                        isVerticalWall);

                Player changedPlayer = new Player(player);

                if (changedGameField.AddWall(wall, changedPlayer, enemy, targets))
                {
                    children.Add(new Node(changedGameField, enemy, changedPlayer, targets, depth + 1, new Quoridor.GameAction(Quoridor.GameAction.GameActionType.PlaceWall, wall.cells, wall.isVertical)));
                    return true;
                }
                return false;
            }

            private void TryAddingHorizontalWalls(int lowerWallIndexY, int upperWallIndexY, ref List<Node> resulting_children, bool topToBottomMovement)
            {
                int distanceToTargets = Math.Abs(enemy.Position.Y - targets[enemy.Name][0].Y);
                bool areVerticalWallsPossible = distanceToTargets >= 1;
                if (!areVerticalWallsPossible)
                    return;

                // a horizontal wall right in front of the enemy going "left"
                if (enemy.Position.X > 0)
                    TryAddingWallPlacingNode(enemy.Position.X - 1, lowerWallIndexY, upperWallIndexY, false, ref resulting_children);

                // a horizontal wall right in front of the enemy going "right"
                if (enemy.Position.X < GameField.fieldSize - 1)
                    TryAddingWallPlacingNode(enemy.Position.X, lowerWallIndexY, upperWallIndexY, false, ref resulting_children);
            }

            private void TryAddingVerticalWalls(int lowerWallIndexY, int upperWallIndexY, ref List<Node> resulting_children, bool topToBottomMovement)
            {
                bool areVerticalWallsPossible = Math.Abs(enemy.Position.Y - targets[enemy.Name][0].Y) > 1;
                bool isWallInFrontOfEnemy = gameField.WallExists(enemy.Position, !topToBottomMovement);
                if (!areVerticalWallsPossible || !isWallInFrontOfEnemy || enemy.Position.Y == 0 || enemy.Position.Y == 8)
                    return;

                // a vertical wall to the left of the enemy going "backwards"
                if (enemy.Position.X > 0)
                    TryAddingWallPlacingNode(enemy.Position.X - 1, lowerWallIndexY, upperWallIndexY, true, ref resulting_children);

                // a vertical wall to the right of the enemy going "backwards"
                if (enemy.Position.X < GameField.fieldSize - 1)
                    TryAddingWallPlacingNode(enemy.Position.X, lowerWallIndexY, upperWallIndexY, true, ref resulting_children);
            }

            private List<Node> TryPlacingWallsCloseToEnemy()
            {
                bool isWallsCountAbove6 = player.WallsLeft > 6;
                bool topToBottomEnemyMovement = enemy.Position.Y - targets[enemy.Name][0].Y < 0;

                List<Node> resulting_children = new List<Node>();

                if (isWallsCountAbove6)
                    TryAddingVerticalWalls(topToBottomEnemyMovement ? enemy.Position.Y - 1 : enemy.Position.Y + 1, enemy.Position.Y, ref resulting_children, topToBottomEnemyMovement);

                TryAddingHorizontalWalls(enemy.Position.Y, !topToBottomEnemyMovement ? enemy.Position.Y - 1 : enemy.Position.Y + 1, ref resulting_children, topToBottomEnemyMovement);

                return resulting_children;
            }

            private List<Cell> GetBestPath(Cell from, List<Cell> to)
            {
                List<Cell> path = new List<Cell>();

                if (to.Contains(from))
                    return path;

                Dictionary<Cell, bool> visited = new Dictionary<Cell, bool>();
                for (int indexX = 0; indexX < GameField.fieldSize; indexX++)
                    for (int indexY = 0; indexY < GameField.fieldSize; indexY++)
                        visited[new Cell(indexX, indexY)] = false;

                Queue<Cell> yetToVisit = new Queue<Cell>();
                Dictionary<Cell, Cell> parents = new Dictionary<Cell, Cell>();

                Cell currentCell;
                yetToVisit.Enqueue(from);
                visited[from] = true;

                while (yetToVisit.Count() > 0)
                {
                    currentCell = yetToVisit.Dequeue();
                    if (to.Contains(currentCell))
                    {
                        Cell tempCell = currentCell;
                        path.Add(tempCell);

                        while (parents[tempCell] != from)
                        {
                            tempCell = parents[tempCell];
                            path.Add(tempCell);
                        }
                        break;
                    }
                    foreach (var childCell in gameField.GeneratePossibleMoves(currentCell, enemy.Position))
                    {
                        if (!visited[childCell])
                        {
                            parents[childCell] = currentCell;
                            yetToVisit.Enqueue(childCell);
                            visited[childCell] = true;
                        }
                    }
                }

                path.Reverse();
                return path;
            }

            private void GenerateBestPaths()
            {
                bestPathPlayer = GetBestPath(player.Position, targets[player.Name]);
                bestPathLengthPlayer = bestPathPlayer.Count();

                bestPathEnemy = GetBestPath(enemy.Position, targets[enemy.Name]);
                bestPathLengthEnemy = bestPathEnemy.Count();
            }

            private List<Node> GenerateChildren()
            {
                List<Node> children = new List<Node>();

                bool isWinningNodeFound = bestPathPlayer.Count() == 1;

                Cell cellToGo = bestPathPlayer.Contains(player.Position)
                    ? bestPathPlayer[bestPathPlayer.FindIndex(cell => cell == player.Position) + 1]
                    : bestPathPlayer[0];

                if (gameField.GeneratePossibleMoves(player.Position, enemy.Position).Contains(cellToGo))
                {
                    Player nextPlayerState = new Player(player);
                    nextPlayerState.Position = cellToGo;
                    Quoridor.GameAction newGameAction = new Quoridor.GameAction(gameField.GetPassages(player.Position).Contains(cellToGo) ? Quoridor.GameAction.GameActionType.Move : Quoridor.GameAction.GameActionType.Jump, new List<Cell> { cellToGo });

                    children.Add(new Node(gameField, enemy, nextPlayerState, targets, depth + 1, newGameAction, isWinningNodeFound));
                }

                if (player.WallsLeft > 0 && bestPathLengthPlayer < bestPathLengthEnemy + 2 && depth % 2 == 0)
                    children.InsertRange(0, TryPlacingWallsCloseToEnemy());

                return children;
            }

            private GameField gameField;

            private Player player;
            private Player enemy;

            public float value;

            public int depth;
            static int maxDepth = 27;

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
            root = new Node(new GameField(gameField), new Player(player), new Player(enemy), targets, 0, new Quoridor.GameAction(Quoridor.GameAction.GameActionType.Empty, new List<Cell>()));

            (float nodeValue, Node bestNode) = GetTheBestNodeAndValue(root, true);
            return bestNode.gameAction;
        }

        (float, Node) GetTheBestNodeAndValue(Node node, bool isMinValuePreferrable)
        {
            if (node.isLeaf)
                return (node.value, node);

            Node bestNode = null, tempNode;
            float bestNodeValue = 0, tempNodeValue = 0;
            bool isFirstIteration = true;

            foreach (Node childNode in node.children)
            {
                if (isFirstIteration)
                {
                    bestNode = childNode;
                    (bestNodeValue, _) = GetTheBestNodeAndValue(bestNode, !isMinValuePreferrable);
                    isFirstIteration = false;
                    continue;
                }

                tempNode = childNode;
                (tempNodeValue, _) = GetTheBestNodeAndValue(tempNode, !isMinValuePreferrable);
                if (isMinValuePreferrable ? tempNodeValue < bestNodeValue : tempNodeValue > bestNodeValue)
                {
                    bestNode = tempNode;
                    bestNodeValue = tempNodeValue;
                }
            }

            return (bestNodeValue, bestNode);
        }

        Node root;
    }
}

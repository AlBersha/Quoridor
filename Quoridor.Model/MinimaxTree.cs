using System.Collections.Generic;

namespace Quoridor.Model
{
    public class MinimaxTree
    {
        class Node
        {
            public Node(GameField gameField, Player player, Player enemy, List<Cell> targets, int currentDepth)
            {
                this.gameField = gameField;
                this.player = player;
                this.enemy = enemy;
                this.depth = currentDepth;
                this.targets = targets;
                this.value = GetCurrentEvaluation(gameField, player, enemy);
                if (depth != maxDepth)
                    this.children = GenerateChildren();
                else
                    this.children = new List<Node>();
            }

            private List<Node> GenerateChildren()
            {
                return new List<Node>();
            }

            private int GetCurrentEvaluation(GameField gameField, Player player, Player enemy)
            {
                return 0;
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
                return false;
            }

            private bool IsVerticalWallPlacingTheBestChoice()
            {
                return targets.Exists(target => target.X == enemy.Position.X);
            }

            private bool IsHorizontalWallPlacingTheBestChoice()
            {
                return targets.Exists(target => target.Y == enemy.Position.Y);
            }

            GameField gameField;
            Player player;
            Player enemy;
            int value;
            int depth;
            static int maxDepth = 5;
            private List<Node> children;
            private List<Cell> targets;
        }

        Node root;
        int depth = 5;

        int currentLevel = 0;

    }
}

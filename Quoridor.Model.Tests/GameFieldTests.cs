using NUnit.Framework;
using Quoridor.Model;
using System.Collections.Generic;

namespace Quoridor.Model.Tests
{
    class GameFieldTests
    {
        private GameField gameField;

        [SetUp]
        public void Setup()
        {
            gameField = new GameField();
        }

        [Test]
        public void GameFieldCellsAreInitializedCorrectly_cornerCases()
        {
            var passages = gameField.GetPassages(new Cell(0, 0));
            Assert.IsTrue(passages.Count == 2, "upper left case");
            Assert.IsTrue(passages.Contains(new Cell(1, 0)), "upper left case - right");
            Assert.IsTrue(passages.Contains(new Cell(0, 1)), "upper left case - bottom");

            passages = gameField.GetPassages(new Cell(0, GameField.fieldSize - 1));
            Assert.IsTrue(passages.Count == 2, "lower left case");
            Assert.IsTrue(passages.Contains(new Cell(0, GameField.fieldSize - 2)), "lower left case - top");
            Assert.IsTrue(passages.Contains(new Cell(1, GameField.fieldSize - 1)), "lower left case - right");

            passages = gameField.GetPassages(new Cell(GameField.fieldSize - 1, 0));
            Assert.IsTrue(passages.Count == 2, "upper right case");
            Assert.IsTrue(passages.Contains(new Cell(GameField.fieldSize - 2, 0)), "upper right case - left");
            Assert.IsTrue(passages.Contains(new Cell(GameField.fieldSize - 1, 1)), "upper right case - bottom");

            passages = gameField.GetPassages(new Cell(GameField.fieldSize - 1, GameField.fieldSize - 1));
            Assert.IsTrue(passages.Count == 2, "lower right case");
            Assert.IsTrue(passages.Contains(new Cell(GameField.fieldSize - 2, GameField.fieldSize - 1)), "lower right case - left");
            Assert.IsTrue(passages.Contains(new Cell(GameField.fieldSize - 1, GameField.fieldSize - 2)), "lower right case - top");
        }

        [Test]
        public void GameFieldCellsAreInitializedCorrectly_borderCases()
        {
            var passages = gameField.GetPassages(new Cell(GameField.fieldSize / 2, 0));
            Assert.IsTrue(passages.Count == 3, "upper case");
            Assert.IsTrue(passages.Contains(new Cell(GameField.fieldSize / 2 - 1, 0)), "upper case - left");
            Assert.IsTrue(passages.Contains(new Cell(GameField.fieldSize / 2 + 1, 0)), "upper case - right");
            Assert.IsTrue(passages.Contains(new Cell(GameField.fieldSize / 2, 1)), "upper case - bottom");
            
            passages = gameField.GetPassages(new Cell(GameField.fieldSize / 2, GameField.fieldSize - 1));
            Assert.IsTrue(passages.Count == 3, "lower case");
            Assert.IsTrue(passages.Contains(new Cell(GameField.fieldSize / 2 - 1, GameField.fieldSize - 1)), "lower case - left");
            Assert.IsTrue(passages.Contains(new Cell(GameField.fieldSize / 2 + 1, GameField.fieldSize - 1)), "lower case - right");
            Assert.IsTrue(passages.Contains(new Cell(GameField.fieldSize / 2, GameField.fieldSize - 2)), "lower case - top");
            
            passages = gameField.GetPassages(new Cell(0, GameField.fieldSize / 2));
            Assert.IsTrue(passages.Count == 3, "left case");
            Assert.IsTrue(passages.Contains(new Cell(0, GameField.fieldSize / 2 + 1)), "left case - top");
            Assert.IsTrue(passages.Contains(new Cell(0, GameField.fieldSize / 2 - 1)), "left case - bottom");
            Assert.IsTrue(passages.Contains(new Cell(1, GameField.fieldSize / 2)), "left case - right");
            
            passages = gameField.GetPassages(new Cell(GameField.fieldSize - 1, GameField.fieldSize / 2));
            Assert.IsTrue(passages.Count == 3, "right case");
            Assert.IsTrue(passages.Contains(new Cell(GameField.fieldSize - 1, GameField.fieldSize / 2 + 1)), "right case - top");
            Assert.IsTrue(passages.Contains(new Cell(GameField.fieldSize - 1, GameField.fieldSize / 2 - 1)), "right case - bottom");
            Assert.IsTrue(passages.Contains(new Cell(GameField.fieldSize - 2, GameField.fieldSize / 2)), "right case - left");
        }

        [Test]
        public void GameFieldPossibleMovesAreGeneratedCorrectly()
        {
            var firstPlayerPosition = new Player("A", new Cell(1, 2));
            var secondPlayerPosition = new Player("B", new Cell(2, 2));
            var targets = new Dictionary<string, List<Cell>>()
            {
                {
                    "A",
                    new List<Cell>
                        {new(0, 0), new(1, 0), new(2, 0), new(3, 0), new(4, 0), new(5, 0), new(6, 0), new(7, 0), new(8, 0)}
                },
                {
                    "B",
                    new List<Cell>
                        {new(0, 8), new(1, 8), new(2, 8), new(3, 8), new(4, 8), new(5, 8), new(6, 8), new(7, 8), new(8, 8)}
                }
            };

            gameField.AddWall(new Wall(
                new Cell(1, 1),
                new Cell(2, 2),
                new Cell(1, 2),
                new Cell(2, 1),
                false
                ), firstPlayerPosition, secondPlayerPosition, targets);

            gameField.AddWall(new Wall(
                new Cell(2, 2),
                new Cell(3, 3),
                new Cell(2, 3),
                new Cell(3, 2),
                true
                ), firstPlayerPosition, secondPlayerPosition, targets);

            List<Cell> possibleMoves = gameField.GeneratePossibleMoves(firstPlayerPosition.Position, secondPlayerPosition.Position);
            Assert.IsTrue(possibleMoves.Count == 3);
            Assert.IsTrue(possibleMoves.Contains(new Cell(0, 2)));
            Assert.IsTrue(possibleMoves.Contains(new Cell(1, 3)));
            Assert.IsTrue(possibleMoves.Contains(new Cell(2, 3)));

            possibleMoves = gameField.GeneratePossibleMoves(secondPlayerPosition.Position, firstPlayerPosition.Position);
            Assert.IsTrue(possibleMoves.Count == 2);
            Assert.IsTrue(possibleMoves.Contains(new Cell(0, 2)));
            Assert.IsTrue(possibleMoves.Contains(new Cell(2, 3)));
        }

        [Test]
        public void AddingBlockingWallsIsNotPossible()
        {
            var firstPlayerPosition = new Player("A", new Cell(0, 0));
            var secondPlayerPosition = new Player("B", new Cell(2, 2));
            var targetPosition = new Dictionary<string, List<Cell>>()
            {
                {
                    "A",
                    new List<Cell>
                        {new(0, 8), new(1, 8), new(2, 8), new(3, 8), new(4, 8), new(5, 8), new(6, 8), new(7, 8), new(8, 8)}
                },
                {
                    "B",
                    new List<Cell>
                        {new(0, 8), new(1, 8), new(2, 8), new(3, 8), new(4, 8), new(5, 8), new(6, 8), new(7, 8), new(8, 8)}
                }
            };

            var firstWallPlaced = gameField.AddWall(new Wall(
                new Cell(1, 0),
                new Cell(1, 1),
                new Cell(2, 0),
                new Cell(2, 1),
                false
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);
            Assert.IsTrue(firstWallPlaced);

            var secondWallPlaced = gameField.AddWall(new Wall(
                new Cell(0, 1),
                new Cell(0, 2),
                new Cell(1, 1),
                new Cell(1, 2),
                true
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);
            Assert.IsFalse(secondWallPlaced);
        }

        [Test]
        public void AddingCrossOverlappingWallsIsNotPossible()
        {
            var firstPlayerPosition = new Player("A", new Cell(1, 2));
            var secondPlayerPosition = new Player("B", new Cell(2, 2));
            var targetPosition = new Dictionary<string, List<Cell>>()
            {
                {
                    "A",
                    new List<Cell>
                        {new(0, 0), new(1, 0), new(2, 0), new(3, 0), new(4, 0), new(5, 0), new(6, 0), new(7, 0), new(8, 0)}
                },
                {
                    "B",
                    new List<Cell>
                        {new(0, 8), new(1, 8), new(2, 8), new(3, 8), new(4, 8), new(5, 8), new(6, 8), new(7, 8), new(8, 8)}
                }
            };

            var firstWallPlaced = gameField.AddWall(new Wall(
                new Cell(7, 6),
                new Cell(7, 7),
                new Cell(8, 6),
                new Cell(8, 7),
                false
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);
            Assert.IsTrue(firstWallPlaced);

            var secondWallPlaced = gameField.AddWall(new Wall(
                new Cell(7, 6),
                new Cell(7, 7),
                new Cell(8, 6),
                new Cell(8, 7),
                true
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);
            Assert.IsFalse(secondWallPlaced);
        }
        
        [Test]
        public void AddingDublicateWallsIsNotPossible()
        {
            var firstPlayerPosition = new Player("A", new Cell(1, 2));
            var secondPlayerPosition = new Player("B", new Cell(2, 2));
            var targetPosition = new Dictionary<string, List<Cell>>()
            {
                {
                    "A",
                    new List<Cell>
                        {new(0, 0), new(1, 0), new(2, 0), new(3, 0), new(4, 0), new(5, 0), new(6, 0), new(7, 0), new(8, 0)}
                },
                {
                    "B",
                    new List<Cell>
                        {new(0, 8), new(1, 8), new(2, 8), new(3, 8), new(4, 8), new(5, 8), new(6, 8), new(7, 8), new(8, 8)}
                }
            };

            var firstWallPlaced = gameField.AddWall(new Wall(
                new Cell(7, 6),
                new Cell(7, 7),
                new Cell(8, 6),
                new Cell(8, 7),
                false
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);
            Assert.IsTrue(firstWallPlaced);

            var secondWallPlaced = gameField.AddWall(new Wall(
                new Cell(7, 6),
                new Cell(7, 7),
                new Cell(8, 6),
                new Cell(8, 7),
                false
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);
            Assert.IsFalse(secondWallPlaced);
        }

        [Test]
        public void AddingSlightlyOverlappingWallsIsNotPossible_Vertically()
        {
            var firstPlayerPosition = new Player("A", new Cell(1, 2));
            var secondPlayerPosition = new Player("B", new Cell(2, 2));
            var targetPosition = new Dictionary<string, List<Cell>>()
            {
                {
                    "A",
                    new List<Cell>
                        {new(0, 0), new(1, 0), new(2, 0), new(3, 0), new(4, 0), new(5, 0), new(6, 0), new(7, 0), new(8, 0)}
                },
                {
                    "B",
                    new List<Cell>
                        {new(0, 8), new(1, 8), new(2, 8), new(3, 8), new(4, 8), new(5, 8), new(6, 8), new(7, 8), new(8, 8)}
                }
            };

            var firstWallPlaced = gameField.AddWall(new Wall(
                new Cell(7, 5),
                new Cell(7, 6),
                new Cell(8, 5),
                new Cell(8, 6),
                true
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);
            Assert.IsTrue(firstWallPlaced);

            var secondWallPlaced = gameField.AddWall(new Wall(
                new Cell(7, 6),
                new Cell(7, 7),
                new Cell(8, 6),
                new Cell(8, 7),
                true
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);
            Assert.IsFalse(secondWallPlaced);

            var thirdWallPlaced = gameField.AddWall(new Wall(
                new Cell(7, 4),
                new Cell(7, 5),
                new Cell(8, 4),
                new Cell(8, 5),
                true
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);
            Assert.IsFalse(thirdWallPlaced);
        }

        [Test]
        public void AddingSlightlyOverlappingWallsIsNotPossible_Horizontally()
        {
            var firstPlayerPosition = new Player("A", new Cell(1, 2));
            var secondPlayerPosition = new Player("B", new Cell(2, 2));
            var targetPosition = new Dictionary<string, List<Cell>>()
            {
                {
                    "A",
                    new List<Cell>
                        {new(0, 0), new(1, 0), new(2, 0), new(3, 0), new(4, 0), new(5, 0), new(6, 0), new(7, 0), new(8, 0)}
                },
                {
                    "B",
                    new List<Cell>
                        {new(0, 8), new(1, 8), new(2, 8), new(3, 8), new(4, 8), new(5, 8), new(6, 8), new(7, 8), new(8, 8)}
                }
            };

            var firstWallPlaced = gameField.AddWall(new Wall(
                new Cell(6, 6),
                new Cell(6, 7),
                new Cell(7, 6),
                new Cell(7, 7),
                false
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);
            Assert.IsTrue(firstWallPlaced);

            var secondWallPlaced = gameField.AddWall(new Wall(
                new Cell(5, 6),
                new Cell(5, 7),
                new Cell(6, 6),
                new Cell(6, 7),
                false
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);
            Assert.IsFalse(secondWallPlaced);

            var thirdWallPlaced = gameField.AddWall(new Wall(
                new Cell(7, 6),
                new Cell(7, 7),
                new Cell(8, 6),
                new Cell(8, 7),
                false
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);
            Assert.IsFalse(thirdWallPlaced);
        }
    }
}
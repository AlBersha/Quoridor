using NUnit.Framework;
using Quoridor.Model;

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
            var firstPlayerPosition = new Cell(1, 2);
            var secondPlayerPosition = new Cell(2, 2);
            var targetPosition = new Cell(8, 8);

            gameField.AddWall(new Wall(
                new Cell(2, 1),
                new Cell(3, 2),
                new Cell(2, 2),
                new Cell(3, 1),
                false
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);

            gameField.AddWall(new Wall(
                new Cell(3, 2),
                new Cell(4, 3),
                new Cell(3, 3),
                new Cell(4, 2),
                true
                ), firstPlayerPosition, secondPlayerPosition, targetPosition);

            var possibleMoves = gameField.GeneratePossibleMoves(firstPlayerPosition, secondPlayerPosition);
            Assert.Fail();
        }
    }
}
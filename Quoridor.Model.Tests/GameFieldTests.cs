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
        public void GameFieldIsInitialized()
        {
            Assert.Fail();
        }
    }
}
using NUnit.Framework;
using Quoridor.Model;

namespace Quoridor.Model.Tests
{
    public class QuoridorTests
    {
        private Quoridor game;

        private Player playerA;
        private Player playerB;
        private Cell target;

        [SetUp]
        public void Setup()
        {
            playerA = new Player("A", System.Numerics.Vector2.One);
            playerB = new Player("B", System.Numerics.Vector2.Zero);
            target = new Cell(8, 8);

            game = new Quoridor(playerA, playerB, target);
        }

        [Test]
        public void PlayerAssignedCorrectlyAfterInitialization()
        {
            Assert.IsTrue(game.CurrentPlayer == playerA);
        }

        [Test]
        public void PlayerSidesSwitchedCorrectly()
        {
            game.SwitchSides();
            Assert.IsTrue(game.CurrentPlayer == playerB);
        }
    }
}
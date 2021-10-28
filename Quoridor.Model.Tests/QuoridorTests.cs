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

        // [SetUp]
        // public void Setup()
        // {
        //     playerA = new Player("A", new Cell(1,1));
        //     playerB = new Player("B", new Cell(0, 0));
        //     target = new Cell(8, 8);
        //
        //     game = new Quoridor(playerA, playerB, target);
        // }
        //
        // [Test]
        // public void PlayerAssignedCorrectlyAfterInitialization()
        // {
        //     Assert.IsTrue(game.CurrentPlayer == playerA);
        // }
        //
        // [Test]
        // public void PlayerSidesSwitchedCorrectly()
        // {
        //     game.SwitchSides();
        //     Assert.IsTrue(game.CurrentPlayer == playerB);
        // }
    }
}
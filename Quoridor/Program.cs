using System.Collections.Generic;
using Quoridor.Input;
using Quoridor.Model;
using Quoridor.View;

namespace Quoridor
{
    static class Program
    {
        static void Main(string[] args)
        {
            var targets = new Dictionary<string, List<Cell>>()
            {
                {
                    "A",
                    new List<Cell>
                        {new(0, 8), new(1, 8), new(2, 8), new(3, 8), new(4, 8), new(5, 8), new(6, 8), new(7, 8), new(8, 8)}
                },
                {
                    "B",
                    new List<Cell>
                        {new(0, 0), new(1, 0), new(2, 0), new(3, 0), new(4, 0), new(5, 0), new(6, 0), new(7, 0), new(8, 0)}
                }
            };

            var quoridor = new QuoridorEvents(new Player("A", new Cell(4, 0)), new Player("B", new Cell(4, 8)), targets);
            var output = new ConsoleView();
            output.SetUpGame(quoridor, 9, 9);

            var input = new ConsoleInput();
            input.ProcessInput(quoridor);

            //Player playerA = new Player("A", new Cell(4, 0));
            //Player playerB = new Player("B", new Cell(4, 8));

            //GameField gameField = new GameField();

            //MinimaxTree minimaxTree = new MinimaxTree();
            //var gameAction = minimaxTree.FindTheBestDecision(gameField, playerA, playerB, targets);
        }
    }
}

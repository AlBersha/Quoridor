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
                        {new(8, 0), new(8, 1), new(8, 2), new(8, 3), new(8, 4), new(8, 5), new(8, 6), new(8, 7), new(8, 8)}
                },
                {
                    "B",
                    new List<Cell>
                        {new(0, 0), new(0, 1), new(0, 2), new(0, 3), new(0, 4), new(0, 5), new(0, 6), new(0, 7), new(0, 8)}
                }
            };
            
            var quoridor = new QuoridorEvents(new Player("A", new Cell(0, 4)), new Player("B", new Cell(8, 4)), targets);
            var output = new ConsoleView();
            output.SetUpGame(quoridor, 9, 9);
            
            var input = new ConsoleInput();
            input.ProcessInput(quoridor);
        }
    }
}

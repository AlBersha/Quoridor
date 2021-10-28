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
            // var playersPosition = new List<Player> {new ("A", new Cell(0, 4)), new ("B", new Cell(8, 4))};
            // var walls = new List<(Vector2, Vector2)> { (new Vector2(0, 0), new Vector2(0, 1)), (new Vector2(0, 2), new Vector2(0, 3)), 
                                                    // (new Vector2(0, 2), new Vector2(1, 2)), (new Vector2(2, 2), new Vector2(3, 2))};

            var walls = new List<Wall>();
            walls.Add(new Wall(new Cell(0, 0), new Cell(0, 1), new Cell(1, 0), new Cell(1, 1), true));
            walls.Add(new Wall(new Cell(0, 0), new Cell(0, 1), new Cell(1, 0), new Cell(1, 1), false));
            
            var targets = new Dictionary<Player, List<Cell>>()
            {
                {
                    new Player("A", new Cell(0, 4)),
                    new List<Cell>
                        {new(8, 0), new(8, 1), new(8, 2), new(8, 3), new(8, 4), new(8, 5), new(8, 6), new(8, 7), new(8, 8)}
                },
                {
                    new Player("B", new Cell(8, 4)),
                    new List<Cell>
                        {new(0, 0), new(0, 1), new(0, 2), new(0, 3), new(0, 4), new(0, 5), new(0, 6), new(0, 7), new(0, 8)}
                }
            };
            var quoridor = new QuoridorEvents(new Player("A", new Cell(0, 4)), new Player("B", new Cell(8, 4)), targets);
            var output = new ConsoleView(quoridor);
            // output.PrintGameField(playersPosition, walls);
            
            var input = new ConsoleInput();
            input.ProcessInput(quoridor);
        }
    }
}

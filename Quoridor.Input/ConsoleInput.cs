using System;
using System.Numerics;

namespace Quoridor.Input
{
    public class ConsoleInput
    {
        public void ProcessInput()
        {
            Console.WriteLine("Print START to play\n>");

            while (true)
            {
                var command = Console.ReadLine();
                var split = command.Split(' ', ',');

                switch (split[0].ToLower())
                {
                    case "start":
                        //todo start the game
                        break;
                    case "wall":
                        var wall1 = new Vector2(int.Parse(split[1]), int.Parse(split[2]));
                        var wall2 = new Vector2(int.Parse(split[3]), int.Parse(split[4]));
                        
                        //todo process wall building
                        break;
                    case "move":
                        var targetCell = new Vector2(int.Parse(split[1]), int.Parse(split[2]));
                        //todo process the move ;)
                        break;
                    case "exit":
                        Console.WriteLine("Thanks for playing wonderful Quoridor!");
                        break;
                }
            }
        }
    }
}
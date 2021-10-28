using System;
using System.Collections.Generic;
using Quoridor.Model;

namespace Quoridor.Input
{
    public class ConsoleInput
    {
        public void ProcessInput(QuoridorEvents game)
        {
            while (true)
            {
                Console.WriteLine("Print START and amount of players (2 or 4) to play game\n");
                var command = Console.ReadLine();
                var split = command.Split(' ', ',', '(', ')');

                switch (split[0].ToLower())
                {
                    case "start":
                        //todo start the game
                        game.StartGame();
                        Console.Out.WriteLine("The game has begun. Build a wall \nor make a move");
                        break;
                    case "wall":
                        try
                        {
                            var wall = new Wall(new Cell(int.Parse(split[1]), int.Parse(split[2])),
                                new Cell(int.Parse(split[3]), int.Parse(split[4])),
                                new Cell(int.Parse(split[5]), int.Parse(split[6])),
                                new Cell(int.Parse(split[7]), int.Parse(split[8])), bool.Parse(split[9].ToLower()));
                            
                            if (!game.TryAddingWall(wall))
                            {
                                Console.Out.WriteLine("You've entered either incorrect command or the place is already taken. Try different input!");
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("You have entered not enough data to build the wall. Try again please!");
                        }
                        //todo process wall building
                        break;
                    case "move":
                        var moveTo = new Cell(int.Parse(split[1]), int.Parse(split[2]));
                        
                        game.MovePlayer(moveTo);
                        //todo process the move ;)
                        break;
                    case "help":
                        break;
                    case "exit":
                        Console.WriteLine("Thanks for playing wonderful Quoridor!");
                        break;
                }
            }
        }
    }
}
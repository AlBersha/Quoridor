﻿using System;
using System.Text.RegularExpressions;
using Quoridor.Model;

namespace Quoridor.Input
{
    public class ConsoleInput
    {
        public void ProcessInput(QuoridorEvents game)
        {
            Console.WriteLine("Print START to play game");
            var continuePlay = true;
            while (continuePlay)
            {
                var command = Console.ReadLine();
                command = Regex.Replace(command ?? string.Empty, @"\s+", " ");
                var split = command.Split(' ', ',', '(', ')', ';');

                switch (split[0].ToLower())
                {
                    case "start":
                        game.StartGame();
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
                        break;
                    case "move":
                        try
                        {
                            var moveTo = new Cell(int.Parse(split[1]), int.Parse(split[2]));
                            game.MovePlayer(moveTo);

                        }
                        catch (Exception e)
                        {
                            Console.Out.WriteLine("You've entered not enough info to make move. Try again please!");
                        }
                        break;
                    case "help":
                        break;
                    case "exit":
                        Console.WriteLine("Thanks for playing wonderful Quoridor!");
                        continuePlay = false;
                        break;
                }
            }
        }
    }
}
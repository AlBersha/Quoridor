using System;
using System.Collections.Generic;
using System.Linq;
using Quoridor.Model;

namespace Quoridor.View
{
    public class ConsoleView: IConsoleView
    {
        public int FieldWidth { get; set; }
        public int FieldHeight { get; set; }
        
        public void SetUpGame(QuoridorEvents game, int fieldWidth, int fieldHeight)
        {
            FieldWidth = fieldWidth;
            FieldHeight = fieldHeight;
            
            game.FieldUpdated += PrintGameField;
            game.GameStarted += GameStarted;
            game.PlayerWon += PlayerWon;
            game.WrongActivity += OnWrongActivity;
            game.HelpRequest += OnHelpRequest;
            game.CurrentPlayerRequest += OnCurrentPlayerRequest;
        }

        public void PrintGameField(List<Player> playersPosition, List<Wall> walls)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Out.WriteLine($"\nWalls left: {playersPosition[1].WallsLeft}");
            Console.ForegroundColor = ConsoleColor.White;
            for (var xIndex = 0; xIndex < FieldWidth; xIndex++)
            {
                Console.Write(" ———");
            }
            Console.WriteLine();
            for (var yIndex = 0; yIndex < FieldHeight; yIndex++) 
            {
                Console.Write("|");
                for (var xIndex = 0; xIndex < FieldWidth; xIndex++)
                {
                    var player = playersPosition.Find(player => player.Position.X == xIndex && player.Position.Y == yIndex);
                    if (player != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Out.Write($" {player.Name} ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Out.Write($"{xIndex},{yIndex}");
                    }

                    var wallExists = walls.Any(wall => wall.isVertical && wall.cells.Exists(cell => cell.Y == yIndex && cell.X == xIndex) && wall.cells.Exists(cell => cell.Y == yIndex && cell.X == xIndex + 1));
                    if (wallExists)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("║");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write("|");
                    }
                }
                Console.Out.WriteLine();

                for (var xIndex = 0; xIndex < FieldWidth; xIndex++)
                {
                    var wallExists = walls.Any(wall => !wall.isVertical && wall.cells.Exists(cell => cell.Y == yIndex && cell.X == xIndex) && wall.cells.Exists(cell => cell.Y == yIndex+1 && cell.X == xIndex));
                    if (wallExists)
                    {
                        Console.ForegroundColor = ConsoleColor.Green; 
                        Console.Write(" ═══"); 
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Out.Write(" ———");
                    }
                }
                Console.Out.WriteLine();
            }
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Out.WriteLine($"Walls left: {playersPosition[0].WallsLeft}\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void PlayerWon(Player player)
        {
            Console.Out.WriteLine($"Game is over! Player {player.Name} won!\n\nType EXIT to end game, type START to continue playing");
        }

        public void GameStarted(List<Player> playersPosition, List<Wall> walls)
        {
            Console.Out.WriteLine("The game has begun. Build a wall or make a move");
            PrintGameField(playersPosition, walls);
        }

        public void OnWrongActivity(bool isMove, Player player)
        {
            Console.Out.Write($"Wrong activity for player {player.Name}.");
            Console.Out.Write(
                isMove ? " The cell is either busy or unreachable.\n" : " The position of wall is either unacceptable or incorrect. \nOr you have built as many walls as possible. Pay attention to wall counter\n");
        }

        public void OnHelpRequest()
        {
            Console.Out.WriteLine("To build the wall print \"WALL x1 y1 x2 y2 x3 y3 x4 y4 TRUE (if your wall is vertical) / FALSE (if a wall is horizontal)\nwhere x, y - cell coordinates" +
                                  "\nTo make a move print MOVE x y" +
                                  "\nTo suggest or comment write to @Necessity or @vermi4elli\n");
        }

        private void OnCurrentPlayerRequest(Player player)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Out.WriteLine(player is null ? "Start game first": $"{player.Name}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        
    }
}
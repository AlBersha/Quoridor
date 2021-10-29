using System;
using System.Collections.Generic;
using System.Linq;
using Quoridor.Model;

namespace Quoridor.View
{
    public class ConsoleView
    {
        private int FieldWidth { get; set; }
        private int FieldHeight { get; set; }
        
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

        private void PrintGameField(List<Player> playersPosition, List<Wall> walls)
        {
            for (var i = 0; i < FieldWidth; i++)
            {
                Console.Write(" ———");
            }
            Console.WriteLine();
            for (var i = 0; i < FieldHeight; i++) 
            {
                Console.Write("|");
                for (var j = 0; j < FieldWidth; j++)
                {
                    var player = playersPosition.Find(player => player.Position.X == i && player.Position.Y == j);
                    if (player != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Out.Write($" {player.Name} ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Out.Write($"{i},{j}");
                    }

                    var wallExists = walls.Any(wall => wall.isVertical && wall.cells.Exists(cell => cell.X == i && cell.Y == j) && wall.cells.Exists(cell => cell.X == i && cell.Y == j + 1));
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

                for (var j = 0; j < FieldWidth; j++)
                {
                    var wallExists = walls.Any(wall => !wall.isVertical && wall.cells.Exists(cell => cell.X == i && cell.Y == j) && wall.cells.Exists(cell => cell.X == i+1 && cell.Y == j));
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
        }
        
        private void PlayerWon(Player player)
        {
            Console.Out.WriteLine($"Game is over! Player {player.Name} won!\n\nType EXIT to end game, type START to continue playing");
        }
        
        private void GameStarted(List<Player> playersPosition, List<Wall> walls)
        {
            Console.Out.WriteLine("The game has begun. Build a wall or make a move");
            PrintGameField(playersPosition, walls);
        }

        private void OnWrongActivity(bool isMove, Player player)
        {
            Console.Out.Write($"Wrong activity for player {player.Name}.");
            Console.Out.Write(
                isMove ? " The cell is either busy or unreachable.\n" : " The position of wall is either unacceptable or incorrect.\n");
        }

        private void OnHelpRequest()
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
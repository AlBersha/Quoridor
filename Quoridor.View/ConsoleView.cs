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
        // private QuoridorEvents game;
        
        public void SetUpGame(QuoridorEvents game, int fieldWidth, int fieldHeight)
        {
            FieldWidth = fieldWidth;
            FieldHeight = fieldHeight;
            
            // this.game = game;
            game.FieldUpdated += PrintGameField;
            game.GameStarted += GameStarted;
            game.PlayerWon += PlayerWon;
            game.WrongActivity += OnWrongActivity;
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
                    Console.Out.Write(player != null ? $" {player.Name} " : $"{i},{j}");

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

        private void OnWrongActivity(Player player)
        {
            Console.Out.WriteLine($"Wrong activity data for player {player.Name}. The cell is either busy or unreachable");
        }
        
        
    }
}
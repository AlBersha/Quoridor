using System;
using System.Collections.Generic;
using System.Linq;
using Quoridor.Model;

namespace Quoridor.View
{
    public class ConsoleViewV2: IConsoleView
    {
        public int FieldWidth { get; set; }
        public int FieldHeight { get; set; }

        public void SetUpGame(QuoridorEvents game, int fieldWidth, int fieldHeight)
        {
            throw new NotImplementedException();
        }

        public ConsoleViewV2(int width, int height)
        {
            FieldWidth = width;
            FieldHeight = height;
        }

        public void PrintGameField(List<Player> playersPosition, List<Wall> walls)
        {
            var A = 65;
            Console.Out.Write("   ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            for (var i = 0; i < FieldWidth; i++)
            {
                Console.Write($"  {(char)A} ");
                A++;
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine();
            Console.Out.Write("   ");
            for (var i = 0; i < FieldWidth; i++)
            {
                Console.Write(" ———");
            }
            Console.WriteLine();
            for (var i = 0; i < FieldHeight + 1; i++) 
            {
                if (i == FieldHeight)
                {
                    var S = 83;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Out.Write("    ");
                    for (var j = 1; j < FieldWidth; j++)
                    {
                        Console.Out.Write($"   {(char)S}");
                        S++;
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Out.Write($" {i} ");
                Console.ForegroundColor = ConsoleColor.White;
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
                        Console.Out.Write($"   ");
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

                Console.Out.Write("   ");
                for (var j = 0; j < FieldWidth + 1; j++)
                {
                    if (j == FieldWidth)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Out.Write($"  {i+1}");
                        Console.ForegroundColor = ConsoleColor.White;

                        break;
                    }
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
        
        public void PlayerWon(Player player)
        {
            throw new NotImplementedException();
        }

        public void GameStarted(List<Player> playersPosition, List<Wall> walls)
        {
            throw new NotImplementedException();
        }

        public void OnHelpRequest()
        {
            throw new NotImplementedException();
        }

        public void OnWrongActivity(bool isMove, Player player)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Quoridor.Model;

namespace Quoridor.View
{
    public class ConsoleView
    {
        private int FieldWidth { get; set; }
        private int FieldHeight { get; set; }

        public ConsoleView()
        {
            FieldWidth = 9;
            FieldHeight = 9;
        }
        public ConsoleView(int width, int height)
        {
            FieldWidth = width;
            FieldHeight = height;
        }

        public void PrintGameField(List<Player> playersPosition, List<(Vector2, Vector2)> walls)
        {
            var playersInRow = new List<Player>();
            var verticalWalls = new List<(Vector2, Vector2)>();

            for (var i = 0; i < FieldWidth; i++)
            {
                Console.Write(" ———");
            }
            Console.WriteLine();
            for (var i = 0; i < FieldHeight; i++) 
            { 
                playersInRow.AddRange(playersPosition.Where(point => point.Position.X == i));
                verticalWalls.AddRange(walls.Where(wall => wall.Item1.X == i && wall.Item2.X == i));
                PrintRow(playersInRow, verticalWalls); 
                PrintGrooves(i, walls);
                
                playersInRow.Clear();
                verticalWalls.Clear();
            }
        }

        private void PrintGrooves(int index, List<(Vector2, Vector2)> walls)
        {
            var horizontalWalls = new List<(Vector2, Vector2)>(); 
            for (var i = 0; i < FieldWidth; i++)
            {
                if (walls.Count != 0 && walls.Exists(wall => wall.Item1.X == index))
                {
                    horizontalWalls.AddRange(walls.Where(wall => wall.Item1.X == index));
                }

                if (horizontalWalls.Count != 0 && horizontalWalls.Exists(wall => wall.Item1.Y == i && wall.Item2.Y == i))
                {
                    Console.ForegroundColor = ConsoleColor.Green; 
                    Console.Write(" ═══"); 
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(" ———");
                }
            }
            Console.WriteLine();
        }
        
        private void PrintRow(List<Player> playersInRow, List<(Vector2, Vector2)> wallsInRow)
        {
            Console.Write("|");
            for (var j = 0; j < FieldWidth; j++)
            {
                Console.Write(playersInRow.Count != 0 && playersInRow.Exists(player => player.Position.Y == j)
                    ? $" {playersInRow.Find(player => player.Position.Y == j).Name} "
                    : "   ");
                
                if (wallsInRow.Count != 0 && wallsInRow.Exists(wall => wall.Item1.Y == j))
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
            Console.WriteLine();
        }
         
    }
}
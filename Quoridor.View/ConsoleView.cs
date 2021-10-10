using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Quoridor.Model;

namespace Quoridor.View
{
    public class ConsoleView
    {
        private int FieldWidth { get; set; } = 9;
        private int FieldHeight { get; set; } = 9;
        private List<Player> playersPosition { get; set; }

        public ConsoleView()
        { 
            playersPosition = new List<Player> {new ("A", new Point(0, 4)), new ("B", new Point(8, 4))};
        }

        public ConsoleView(int width, int height)
        {
            FieldWidth = width;
            FieldHeight = height;
        }

        public void PrintGameField()
        {
            var playersInRow = new List<Player>(); 
            Console.WriteLine(" ——— ——— ——— ——— ——— ——— ——— ——— ——— ");
            for (var i = 0; i < FieldHeight; i++) 
            { 
                playersInRow.AddRange(playersPosition.Where(point => point.Position.X == i)); 
                PrintRow(playersInRow); 
                Console.WriteLine(" ——— ——— ——— ——— ——— ——— ——— ——— ——— ");
                playersInRow.Clear();
            }
        }

        private void PrintRow(List<Player> playersInRow)
        {
            Console.Write("|");
            for (var j = 0; j < FieldWidth; j++)
            {
                if (playersInRow.Count != 0)
                {
                    foreach (var player in playersInRow)
                    {
                        Console.Write(player.Position.Y == j ? $" {player.Name} |" : "   |");
                    }
                }
                else
                {
                    // Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("   |");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            Console.WriteLine();
        }
         
    }
}
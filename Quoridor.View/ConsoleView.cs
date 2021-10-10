using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Quoridor.View
{
    public class ConsoleView
    {
        private int FieldWidth { get; set; } = 9;
        private int FieldHeight { get; set; } = 9;
        private List<Point> playersPosition { get; set; }

        public ConsoleView()
        { 
            playersPosition = new List<Point>{new (0, 4), new (8, 4)};
        }

        public ConsoleView(int width, int height)
        {
            FieldWidth = width;
            FieldHeight = height;
        }

        public void PrintGameField()
        {
            var playersInRow = new List<Point>(); 
            for (var i = 0; i < FieldHeight; i++) 
            { 
                Console.WriteLine(" ———  ———  ———  ———  ———  ———  ———  ———  ——— "); 
                playersInRow.AddRange(playersPosition.Where(point => point.X == i)); 
                PrintRow(playersInRow); 
                Console.WriteLine(" ———  ———  ———  ———  ———  ———  ———  ———  ——— "); 
                playersInRow.Clear();
            }
        }

        private void PrintRow(List<Point> playersInRow)
        {
            var IsCellEmpty = true;
            for (var j = 0; j < FieldWidth; j++)
            {
             if (playersInRow.Any(player => player.Y == j))
             {
                 IsCellEmpty = false;
             }
             if (playersInRow.Count != 0 && !IsCellEmpty)
             {
                 Console.Write("|***|");
             }
             else
             {
                 Console.Write("|   |");
             }

             IsCellEmpty = true;
            }
            Console.WriteLine();
        }
         
    }
}
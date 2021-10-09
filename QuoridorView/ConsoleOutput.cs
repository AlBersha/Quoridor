using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace QuoridorConsoleView
{
    public class ConsoleOutput
    {
        private int FieldWidth { get; set; } = 9;
        private int FieldHeight { get; set; } = 9;
        private List<Point> playersPosition { get; set; }

        public ConsoleOutput()
        {
            playersPosition = new List<Point>{new Point(0, 4), new Point(8, 4)};
        }

        public ConsoleOutput(int width, int height)
        {
            FieldWidth = width;
            FieldHeight = height;
        }
        
        public void PrintGameField()
        {
            //get position of players
            //??

            var IsRowEmpty = true;
            for (var i = 0; i < FieldHeight; i++)
            {
                Console.WriteLine(" _______  ______  ______  ______  ______  ______  ______  ______  ______  ");
                if (playersPosition.Any(point => i == point.X))
                {
                    IsRowEmpty = false;
                }
                
                PrintRow(IsRowEmpty);
                PrintRow(IsRowEmpty);

                Console.WriteLine(" _______  ______  ______  ______  ______  ______  ______  ______  ______  ");
                IsRowEmpty = true;
            }
        }

        private void PrintRow(bool IsRowEmpty)
        {
            var IsCellEmpty = true;
            for (var j = 0; j < FieldWidth; j++)
            {
                    
                foreach (var point in playersPosition.Where(point => !IsRowEmpty && j == point.Y))
                {
                    IsCellEmpty = false;
                }

                if (!IsCellEmpty && !IsRowEmpty)
                {
                    Console.Write("|------|");
                }
                else
                {
                    Console.Write("|\t|");
                }

                IsCellEmpty = true;
            }
            Console.WriteLine();
        }
    }
}
using System;
using Microsoft.VisualBasic;
using Quoridor.View;

namespace Quoridor
{
    class Program
    {
        static void Main(string[] args)
        {
            var output = new ConsoleView();
            output.PrintGameField();
            
            // Console.OutputEncoding = System.Text.Encoding.UTF8;
            // for (var i = 0; i <= 1000; i++) {
            //     Console.Write(Strings.ChrW(i) + " ");
            //     if (i % 50 == 0) { // break every 50 chars
            //         Console.WriteLine();
            //     }
            // }
        }
    }
}

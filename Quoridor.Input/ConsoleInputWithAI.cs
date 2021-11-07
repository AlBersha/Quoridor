using System;
using System.Text.RegularExpressions;
using Quoridor.Model;

namespace Quoridor.Input
{
    public class ConsoleInputWithAI: IConsoleInput
    {
        public void ProcessInput(QuoridorEvents game)
        {
            Console.ForegroundColor = ConsoleColor.White;
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
                        
                        break;
                    case "white":
                    case "black":
                        
                        break;
                    case "move":
                        
                        break;
                    case "jump":
                        
                        break;
                    case "wall":
                        
                        break;
                    case "h":
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
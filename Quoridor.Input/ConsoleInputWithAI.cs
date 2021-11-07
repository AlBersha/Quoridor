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
                    case "white":
                        game.StartAIGame();
                        game.MakeBotMove();
                        break;
                    case "black":
                        game.StartAIGame();
                        break;
                    case "move":
                    case "jump":
                        var moveTo = ParseMoveArgument(split[1]);
                        game.MovePlayer(moveTo);
                        break;
                    case "wall":
                        var wall = ParseWallPlacingArgument(split[1]);
                        game.TryAddingWall(wall);
                        break;
                }
            }
        }

        private int GetMoveIndexFromLetter(char letter)
        {
            return ((int)letter) - 65;
        }
        
        private (int, int) GetWallMinMaxIndicesFromLetter(char letter)
        {
            return GetMinMaxIndicesFromNumber(((int)letter) - 82);
        }
        
        private (int, int) GetMinMaxIndicesFromNumber(int number)
        {
            return (number - 1, number);
        }

        private Cell ParseMoveArgument(string moveArgument)
        {
            int xIndex = GetMoveIndexFromLetter(moveArgument[0]);
            int yIndex = int.Parse(moveArgument[1].ToString()) - 1;

            return new Cell(xIndex, yIndex);
        }

        private Wall ParseWallPlacingArgument(string wallArgument)
        {
            (int minXIndex, int maxXIndex) = GetWallMinMaxIndicesFromLetter(wallArgument[0]);
            (int minYIndex, int maxYIndex) = GetMinMaxIndicesFromNumber(int.Parse(wallArgument[1].ToString()));
            bool isVertical = wallArgument[2] == 'h';

            return new Wall(
                new Cell(minXIndex, minYIndex),
                new Cell(minXIndex, maxYIndex),
                new Cell(maxXIndex, minYIndex),
                new Cell(maxXIndex, maxYIndex),
                isVertical
                );
        }
    }
}
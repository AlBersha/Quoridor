using System;
using System.Collections.Generic;
using System.Linq;
using Quoridor.Model;

namespace Quoridor.View
{
    public class ConsoleViewV2 : IConsoleView
    {
        public int FieldWidth { get; set; }
        public int FieldHeight { get; set; }

        public void SetUpGame(QuoridorEvents game, int fieldWidth, int fieldHeight)
        {
            FieldWidth = fieldWidth;
            FieldHeight = fieldHeight;

            game.AIMoveFinished += PrintMessage;
        }

        public void PrintMessage(string message)
        {
            Console.Out.WriteLine(message);
        }

        public void PrintGameField(List<Player> playersPosition, List<Wall> walls)
        {
            return;
        }

        public void PlayerWon(Player player)
        {
            return;
        }

        public void GameStarted(List<Player> playersPosition, List<Wall> walls)
        {
            return;
        }

        public void OnHelpRequest()
        {
            return;
        }

        public void OnWrongActivity(bool isMove, Player player)
        {
            string message = $"Wrong activity for player {player.Name}."
                + (isMove ? " The cell is either busy or unreachable.\n" : " The position of wall is either unacceptable or incorrect. \nOr you have built as many walls as possible. Pay attention to wall counter\n");
            throw new Exception(message);
        }
    }
}
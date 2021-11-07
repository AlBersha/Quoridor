using System.Collections.Generic;
using Quoridor.Model;

namespace Quoridor.View
{
    public interface IConsoleView
    {
        int FieldWidth { get; set; }
        int FieldHeight { get; set; }
        void SetUpGame(QuoridorEvents game, int fieldWidth, int fieldHeight);
        void PrintGameField(List<Player> playersPosition, List<Wall> walls);
        void PlayerWon(Player player);
        void GameStarted(List<Player> playersPosition, List<Wall> walls);
        void OnHelpRequest();
        void OnWrongActivity(bool isMove, Player player);

    }
}
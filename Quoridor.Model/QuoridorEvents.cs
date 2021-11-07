using System;
using System.Collections.Generic;

namespace Quoridor.Model
{
    public class QuoridorEvents: Quoridor
    {
        public event Action<List<Player>, List<Wall>> GameStarted;
        public event Action<List<Player>, List<Wall>> FieldUpdated;
        public event Action<bool, Player> WrongActivity; 
        public event Action<Player> PlayerWon;
        public event Action HelpRequest;
        public event Action<Player> CurrentPlayerRequest;

        public QuoridorEvents(Player firstPlayer, Player secondPlayer, Dictionary<String, List<Cell>> targets) : 
            base(firstPlayer, secondPlayer, targets)
        {
        }

        public override void StartGame()
        {
            base.StartGame();
            GameStarted?.Invoke(new List<Player>{CurrentPlayer, NextPlayer}, gameField.Walls);
        }

        public override bool MovePlayer(Cell to)
        {
            var result = base.MovePlayer(to);
            if (result)
            {
                if (Winner is not null)
                {
                    PlayerWon?.Invoke(Winner);
                }
                else
                {
                    FieldUpdated?.Invoke(new List<Player>{CurrentPlayer, NextPlayer}, gameField.Walls);
                }
            }
            else
            {
                WrongActivity?.Invoke(true, CurrentPlayer);
            }
            return result;
        }

        public override bool TryAddingWall(Wall wall)
        {
            if (CurrentPlayer.WallsLeft < 1)
            {
                WrongActivity?.Invoke(false, CurrentPlayer);
                return false;
            }
            if (base.TryAddingWall(wall))
            {
                FieldUpdated?.Invoke(new List<Player>{CurrentPlayer, NextPlayer}, gameField.Walls);
                if (Winner is not null)
                {
                    PlayerWon?.Invoke(Winner);
                }
                return true;
            }

            WrongActivity?.Invoke(false, CurrentPlayer);
            return false;
        }

        public void GetHelp()
        {
            HelpRequest?.Invoke();
        }

        public void GetCurrentPlayer()
        {
            CurrentPlayerRequest?.Invoke(CurrentPlayer);
        }
        
        
    }
}
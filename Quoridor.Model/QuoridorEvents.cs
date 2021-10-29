using System;
using System.Collections.Generic;

namespace Quoridor.Model
{
    public class QuoridorEvents: Quoridor
    {
        public event Action<List<Player>, List<Wall>> GameStarted;
        public event Action<List<Player>, List<Wall>> FieldUpdated;
        public event Action<Player> WrongActivity; 
        public event Action<Player> PlayerWon;

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
                WrongActivity?.Invoke(CurrentPlayer);
            }
            return result;
        }

        public override bool TryAddingWall(Wall wall)
        {
            var result = base.TryAddingWall(wall);
            if (result)
            {
                FieldUpdated?.Invoke(new List<Player>{CurrentPlayer, NextPlayer}, gameField.Walls);
                if (Winner is not null)
                {
                    PlayerWon?.Invoke(Winner);
                }
            }
            else
            {
                WrongActivity?.Invoke(CurrentPlayer);
            }
            return result;
        }
        
    }
}
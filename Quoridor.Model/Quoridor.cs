using System.Collections.Generic;

namespace Quoridor.Model
{
    public class Quoridor
    {
        private readonly Player firstPlayer;

        private readonly Player secondPlayer;
        
        private readonly Cell target;

        private GameField gameField;

        public Player CurrentPlayer { get; private set; }
        public Player NextPlayer { get; private set; }

        public Player Winner { get; private set; }

        public bool IsEnded { get; private set; }

        public Quoridor(Player firstPlayer, Player secondPlayer, Cell target)
        {
            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;
            this.target = target;

            SetFirstPlayerActive();
        }

        private void SetFirstPlayerActive()
        {
            CurrentPlayer = firstPlayer;
            NextPlayer = secondPlayer;
        }

        public void SwitchSides()
        {
            CurrentPlayer = NextPlayer;
            NextPlayer = NextPlayer == firstPlayer ? secondPlayer : firstPlayer;
        }

        public List<Cell> GetPlayerMoves()
        {
            return gameField.GeneratePossibleMoves(CurrentPlayer.Position, NextPlayer.Position);
        }

        public bool MovePlayer(Cell to)
        {
            if (GetPlayerMoves().Contains(to))
            {
                CurrentPlayer.Position = to;
                return true;
            }
            return false;
        }

        public bool TryAddingWall(Wall wall)
        {
            return gameField.AddWall(wall, CurrentPlayer.Position, NextPlayer.Position, target);
        }

        public bool IsVictoryAchieved()
        {
            return CurrentPlayer.Position == target;
        }
    }
}

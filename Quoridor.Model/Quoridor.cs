using System.Collections.Generic;

namespace Quoridor.Model
{
    public class Quoridor
    {
        private readonly Player firstPlayer;

        private readonly Player secondPlayer;
        
        private readonly Dictionary<string, List<Cell>> targets;

        protected GameField gameField { get; private set; }

        protected Player CurrentPlayer { get; private set; }
        protected Player NextPlayer { get; private set; }

        public Player Winner { get; private set; }

        public bool IsEnded { get; private set; }

        protected Quoridor(Player firstPlayer, Player secondPlayer, Dictionary<string, List<Cell>> targets)
        {
            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;
            this.targets = targets;
        }
        
        public virtual void StartGame()
        {         
            SetFirstPlayerActive();
            gameField = new GameField();

            Winner = null;
            IsEnded = false;
        }

        private void SetFirstPlayerActive()
        {
            ResetPlayersPosition();
            CurrentPlayer = firstPlayer;
            NextPlayer = secondPlayer;
        }

        private void ResetPlayersPosition()
        {
            firstPlayer.Position = new Cell(0, 4);
            secondPlayer.Position = new Cell(8, 4);
        }

        private void SwitchSides()
        {
            CurrentPlayer = NextPlayer;
            NextPlayer = NextPlayer == firstPlayer ? secondPlayer : firstPlayer;
        }

        private List<Cell> GetPlayerMoves()
        {
            return gameField.GeneratePossibleMoves(CurrentPlayer, NextPlayer);
        }

        public virtual bool MovePlayer(Cell to)
        {
            if (GetPlayerMoves().Contains(to))
            {
                CurrentPlayer.Position = to;
                if (IsVictoryAchieved())
                {
                    Winner = CurrentPlayer;
                    IsEnded = true;
                    return true;
                }
                SwitchSides();
                return true;
            }
            return false;
        }

        public virtual bool TryAddingWall(Wall wall)
        {
            if (!gameField.AddWall(wall, CurrentPlayer, NextPlayer, targets)) return false;
            SwitchSides();
            return true;
        }

        private bool IsVictoryAchieved()
        {
            return targets[CurrentPlayer.Name]
                .Exists(cell => cell.X == CurrentPlayer.Position.X && cell.Y == CurrentPlayer.Position.Y);
        }
    }
}

using System.Collections.Generic;

namespace Quoridor.Model
{
    public class Quoridor
    {
        private readonly Player firstPlayer;

        private readonly Player secondPlayer;
        
        private readonly Dictionary<Player, List<Cell>> targets;

        private GameField gameField;

        public Player CurrentPlayer { get; private set; }
        public Player NextPlayer { get; private set; }

        public Player Winner { get; private set; }

        public bool IsEnded { get; private set; }

        public Quoridor(Player firstPlayer, Player secondPlayer, Dictionary<Player, List<Cell>> targets)
        {
            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;
            this.targets = targets;
        
            SetFirstPlayerActive();
            gameField = new GameField();
        }
        
        public void StartGame()
        {         
            SetFirstPlayerActive();
            gameField = new GameField();
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
            return gameField.GeneratePossibleMoves(CurrentPlayer, NextPlayer);
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
            return gameField.AddWall(wall, CurrentPlayer, NextPlayer, targets);
        }

        public bool IsVictoryAchieved()
        {
            return targets[CurrentPlayer]
                .Exists(cell => cell.X == CurrentPlayer.Position.X && cell.Y == CurrentPlayer.Position.Y);
        }
    }
}

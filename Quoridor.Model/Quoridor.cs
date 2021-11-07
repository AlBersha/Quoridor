using System.Collections.Generic;

namespace Quoridor.Model
{
    public class Quoridor
    {
        public class GameAction
        {
            public enum GameActionType
            {
                Movement,
                WallPlacement,
                Empty
            }

            public GameAction(GameActionType action, List<Cell> cells)
            {
                this.actionType = action;
                this.cells = cells;
            }

            public GameActionType actionType;
            public List<Cell> cells;
            public bool isVertical = false;
        }

        private readonly Player firstPlayer;

        private readonly Player secondPlayer;
        
        private readonly Dictionary<string, List<Cell>> targets;

        protected GameField gameField { get; private set; }

        protected Player CurrentPlayer { get; private set; }
        protected Player NextPlayer { get; private set; }
        private MinimaxTree minimaxTree;

        public Player Winner { get; private set; }

        public bool IsEnded { get; private set; }

        protected Quoridor(Player firstPlayer, Player secondPlayer, Dictionary<string, List<Cell>> targets)
        {
            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;
            this.targets = targets;
            this.minimaxTree = new MinimaxTree();
        }
        
        public virtual void StartGame()
        {         
            SetFirstPlayerActive();
            ResetWallsCounter();
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
            firstPlayer.Position = new Cell(4, 8);
            secondPlayer.Position = new Cell(4, 0);
        }

        private void ResetWallsCounter()
        {
            firstPlayer.WallsLeft = 10;
            secondPlayer.WallsLeft = 10;
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

        public virtual string MakeBotMove()
        {
            var action = minimaxTree.FindTheBestDecision(gameField, CurrentPlayer, NextPlayer, targets);

            if (action.actionType == GameAction.GameActionType.Movement)
                MovePlayer(action.cells[0]);
            else
                TryAddingWall(new Wall(action.cells[0], action.cells[1], action.cells[2], action.cells[3], action.isVertical));

            return GetMessage(action);
        }

        private string GetMessage(GameAction action)
        {
            string message = "";
            if (action.actionType == GameAction.GameActionType.Movement)
            {
                message += "move ";
                message += (char)(action.cells[0].X + 65);
                message += (action.cells[0].Y + 1);
            }
            else
            {
                message += "wall ";
                List<Cell> cells = action.cells;
                cells.Sort();
                message += (cells[3].X + 82);
                message += (cells[3].Y);
            }

            return message;
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

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
    }
}

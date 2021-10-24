namespace Quoridor.Model
{
    public class GameAction
    {
        public enum GameActionType
        {
            Movement,
            WallPlacement
        }

        public GameAction(GameActionType action)
        {
            this.actionType = action;
        }

        GameActionType actionType { get; }
    }

    public class MoveAction : GameAction
    {
        public MoveAction(GameActionType action)
            : base(action) { }
    }

    public class WallPlacementAction : GameAction
    {
        public WallPlacementAction(GameActionType action)
            : base(action) { }
    }
}

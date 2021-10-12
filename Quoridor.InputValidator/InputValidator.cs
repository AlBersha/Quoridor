using System.Numerics;

namespace Quoridor.InputValidator
{
    public enum ActionType
    {
        Move,
        PlaceWall
    }

    public interface Input {}

    public class GameAction : Input
    {
        public GameAction(ActionType action)
        {
            this.actionType = action;
        }

        ActionType actionType { get; }
    }

    public class MoveAction : GameAction
    {
        public MoveAction(ActionType action)
            : base(action) { }
    }
    
    public class WallPlacementAction : GameAction
    {
        public WallPlacementAction(ActionType action)
            : base(action) { }
    }

    public class GlobalAction : Input
    {

    }

    public class InputValidator
    {
        public bool ValidateInput()
        {
            return false;
        }
    }
}

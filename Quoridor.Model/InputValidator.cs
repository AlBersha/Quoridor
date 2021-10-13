namespace Quoridor.Model
{
    class InputValidator
    {
        public bool ValidatePlayerInputGameAction(GameAction gameAction, GameAction.GameActionType gameActionType, Player player)
        {
            switch (gameActionType)
            {
                case GameAction.GameActionType.Movement:
                    return ValidateMoveAction(gameAction, player);
                case GameAction.GameActionType.WallPlacement:
                    return ValidateWallPlacementAction(gameAction, player);
            }

            return false;
        }

        public bool ValidateMoveAction(GameAction gameAction, Player player)
        {

            return false;
        }

        public bool ValidateWallPlacementAction(GameAction gameAction, Player player)
        {

            return false;
        }
    }
}

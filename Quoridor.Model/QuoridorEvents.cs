using System;
using System.Collections.Generic;

namespace Quoridor.Model
{
    public class QuoridorEvents: Quoridor
    {
        public event Action<Cell[,]> GameStarted;
        public event Action<Cell[,]> FieldUpdated;
        public event Action<Player> PlayerWon;

        public QuoridorEvents(Player firstPlayer, Player secondPlayer, Dictionary<Player, List<Cell>> targets) : base(firstPlayer, secondPlayer, targets)
        {
        }
        
        
    }
}
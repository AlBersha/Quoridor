using System.Numerics;

namespace Quoridor.Model
{
    public class Player
    {
        public string Name { get; private set; }
        public Cell Position { get; set; }
        public int WallsLeft { get; set; }

        public Player(string name, Cell position)
        {
            Name = name;
            Position = position;
        }

        public Player(Player from)
        {
            Name = from.Name;
            Position = new Cell(from.Position);
            WallsLeft = from.WallsLeft;
        }
    }
}
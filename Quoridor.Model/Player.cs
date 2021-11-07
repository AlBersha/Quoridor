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
    }
}
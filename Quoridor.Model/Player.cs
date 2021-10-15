using System.Numerics;

namespace Quoridor.Model
{
    public class Player
    {
        public string Name { get; private set; }
        public Vector2 Position { get; private set; }

        public Player(string name, Vector2 position)
        {
            Name = name;
            Position = position;
        }
    }
}
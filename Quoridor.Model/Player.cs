using System.Numerics;

namespace Quoridor.Model
{
    public class Player
    {
        public string Name { get; set; }
        public Vector2 Position { get; set; }

        public Player(string name, Vector2 position)
        {
            Name = name;
            Position = position;
        }
    }
}
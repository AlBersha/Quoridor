using System.Drawing;

namespace Quoridor.Model
{
    public class Player
    {
        public string Name { get; set; }
        public Point Position { get; set; }

        public Player(string name, Point position)
        {
            Name = name;
            Position = position;
        }
    }
}
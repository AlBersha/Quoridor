namespace Quoridor.Model
{
    public struct Cell
    {
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public static Cell operator +(Cell l, Cell r)
        {
            return new Cell(l.X + r.X, l.Y + r.Y);
        }
        
        public static Cell operator -(Cell l, Cell r)
        {
            return new Cell(l.X - r.X, l.Y - r.Y);
        }
        
        public static bool operator ==(Cell l, Cell r)
        {
            return l.X == r.X && l.Y == r.Y;
        }
        
        public static bool operator !=(Cell l, Cell r)
        {
            return l.X != r.X || l.Y != r.Y;
        }
        
        public static bool operator <(Cell l, Cell r)
        {
            return l.X < r.X && l.Y < r.Y;
        }

        public static bool operator >(Cell l, Cell r)
        {
            return l.X > r.X && l.Y > r.Y;
        }

        public static Cell UnaryRight => new Cell(1, 0);
        public static Cell UnaryLeft => new Cell(-1, 0);
        public static Cell UnaryUp => new Cell(0, -1);
        public static Cell UnaryDown => new Cell(0, 1);
    }
}

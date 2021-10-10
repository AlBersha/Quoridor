using Quoridor.View;

namespace Quoridor
{
    class Program
    {
        static void Main(string[] args)
        {
            var output = new ConsoleView();
            output.PrintGameField();
        }
    }
}

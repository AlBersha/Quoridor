using Quoridor.Model;

namespace Quoridor.Input
{
    public interface IConsoleInput
    {
        void ProcessInput(QuoridorEvents game);
    }
}
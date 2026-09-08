
namespace Sakemottekoi.Maingame
{
    public abstract class GameSystem
    {
        public abstract string Id { get; }

        public abstract OrderRule OrderRule { get; }
        public abstract bool IsOneshot { get; }

        public abstract void Execute();
    }
}
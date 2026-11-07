using Cysharp.Threading.Tasks;
namespace Sakemottekoi.MainGame
{
    public abstract class GameSystemBase
    {
        public abstract string Id { get; }

        public abstract OrderRule OrderRule { get; }
        public abstract bool IsOneShot { get; }
    }

    public abstract class GameSystem: GameSystemBase
    {
        public abstract UniTask ExecuteAsync();
    }

    public abstract class GameSystem<T>: GameSystemBase
    {
        public abstract UniTask ExecuteAsync(T arg);
    }
}
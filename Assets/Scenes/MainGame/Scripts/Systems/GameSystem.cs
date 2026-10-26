using System;
using System.Collections.Generic;

namespace Sakemottekoi.MainGame
{
    public abstract class GameSystemBase
    {
        public abstract string Id { get; }

        public abstract OrderRule OrderRule { get; }
        public abstract bool IsOneShot { get; }

        // 演出生成用のファクトリ関数
        protected Func<IEnumerable<VisualEvent>> visualFactory;

        protected void RegisterVisuals()
        {
            if (visualFactory == null) return;

            foreach (var visual in visualFactory())
            {
                VisualQueue.Enqueue(visual);
            }
        }
    }

    public abstract class GameSystem: GameSystemBase
    {
        public abstract void Execute();
    }

    public abstract class GameSystem<T>: GameSystemBase
    {
        public abstract void Execute(T arg);
    }
}
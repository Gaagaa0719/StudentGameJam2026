
using Assets.Scenes.MainGame;
using System;
using System.Collections.Generic;

namespace Sakemottekoi.Maingame
{
    public abstract class GameSystem
    {
        public abstract string Id { get; }

        public abstract OrderRule OrderRule { get; }
        public abstract bool IsOneShot { get; }

        // 演出生成用のファクトリ関数
        protected Func<IEnumerable<VisualEvent>> visualFactory;

        public abstract void Execute();

        protected void RegisterVisuals()
        {
            if (visualFactory == null) return;

            foreach (var visual in visualFactory())
            {
                VisualQueue.Enqueue(visual);
            }
        }
    }
}
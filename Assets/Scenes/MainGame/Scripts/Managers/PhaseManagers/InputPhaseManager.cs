using System;
using Cysharp.Threading.Tasks;

namespace Sakemottekoi.MainGame
{
    // 返り値を考慮しないバージョン
    public abstract class InputPhaseManager<T> : SingletonObject<T> where T : InputPhaseManager<T>
    {
        private UniTaskCompletionSource pending;

        public UniTask StartPhaseAsync()
        {
            if (pending != null)
                throw new InvalidOperationException($"{typeof(T).Name} is already active.");

            pending = new UniTaskCompletionSource();

            TableBell.OnClicked += EndPhase;
            OnPhaseStart();

            return pending.Task;
        }

        protected virtual void EndPhase()
        {
            if (pending == null)
                throw new InvalidOperationException($"{typeof(T).Name} is inactive.");

            TableBell.OnClicked -= EndPhase;
            var completion = pending;
            pending = null;

            completion.TrySetResult();
        }
        protected virtual void OnPhaseStart() { }
    }

    // 返り値を考慮するバージョン
    public abstract class InputPhaseManager<T, TResult> : SingletonObject<T> where T : InputPhaseManager<T, TResult>
    {
        private UniTaskCompletionSource<TResult> pending;

        public UniTask<TResult> StartPhaseAsync()
        {
            if (pending != null)
                throw new InvalidOperationException($"{typeof(T).Name} is already active.");

            var completion = new UniTaskCompletionSource<TResult>();
            pending = completion;
            TableBell.OnClicked += OnTryComplete;

            OnPhaseStart();

            return completion.Task;
        }


        protected void CompletePhase(TResult result)
        {
            if (pending == null)
                throw new InvalidOperationException($"{typeof(T).Name} is inactive.");

            TableBell.OnClicked -= OnTryComplete;
            var completion = pending;
            pending = null;

            completion.TrySetResult(result);
        }

        /// <summary>
        /// フェーズ開始時に実行される関数
        /// </summary>
        protected virtual void OnPhaseStart() { }
        
        /// <summary>
        /// フェーズの完了をリクエストされた時に実行される関数
        /// </summary>
        protected abstract void OnTryComplete();
    }
}

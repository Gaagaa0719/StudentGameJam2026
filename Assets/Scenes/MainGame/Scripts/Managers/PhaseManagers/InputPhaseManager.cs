using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

namespace Sakemottekoi.MainGame
{
    // 返り値を考慮しないバージョン
    public abstract class InputPhaseManager<T> : SingletonObject<T> where T : InputPhaseManager<T>
    {
        private UniTaskCompletionSource pending;

        public async UniTask StartPhaseAsync(CancellationToken token)
        {
            if (pending != null)
                throw new InvalidOperationException($"{typeof(T).Name} is already active.");

            var completion = new UniTaskCompletionSource();
            pending = completion;

            TableBell.OnClicked += EndPhase;

            CancellationTokenRegistration registration = default;

            try
            {
                registration = token.Register(() =>
                {
                    if (pending == completion)
                        completion.TrySetCanceled();
                });

                token.ThrowIfCancellationRequested();

                OnPhaseStart();

                await completion.Task;
            }
            finally
            {
                TableBell.OnClicked -= EndPhase;
                registration.Dispose();

                if(pending == completion)
                    pending = null;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            TableBell.OnClicked -= EndPhase;
        }

        protected virtual void EndPhase()
        {
            if (pending == null)
                throw new InvalidOperationException($"{typeof(T).Name} is inactive.");

            pending.TrySetResult();
        }

        protected virtual void OnPhaseStart() { }
    }

    // 返り値を考慮するバージョン
    public abstract class InputPhaseManager<T, TResult> : SingletonObject<T> where T : InputPhaseManager<T, TResult>
    {
        private UniTaskCompletionSource<TResult> pending;
        protected override void OnDestroy()
        {
            base.OnDestroy();
            TableBell.OnClicked -= OnTryComplete;
        }

        public async UniTask<TResult> StartPhaseAsync(CancellationToken token)
        {
            if (pending != null)
                throw new InvalidOperationException($"{typeof(T).Name} is already active.");

            var completion = new UniTaskCompletionSource<TResult>();
            pending = completion;
            TableBell.OnClicked += OnTryComplete;

            CancellationTokenRegistration registration = default;

            try
            {
                registration = token.Register(() =>{
                    if(pending == completion)
                        completion.TrySetCanceled();
                });

                token.ThrowIfCancellationRequested();

                OnPhaseStart();

                return await completion.Task;
            }
            finally
            {
                registration.Dispose();
                TableBell.OnClicked -= OnTryComplete;

                if (pending == completion)
                    pending = null;
            }
        }


        protected void CompletePhase(TResult result)
        {
            if (pending == null)
                throw new InvalidOperationException($"{typeof(T).Name} is inactive.");

            pending.TrySetResult(result);
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

using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Sakemottekoi.MainGame
{
    public static class GlassSelection
    {
        private static UniTaskCompletionSource<AlcoholGlass> pending;

        public static UniTask<AlcoholGlass> SelectAsync()
        {
            if (pending != null)
                throw new InvalidOperationException(
                    "Selection already active");

            pending = new UniTaskCompletionSource<AlcoholGlass>();
            AlcoholGlass.OnClick += HandleClicked;

            return pending.Task;
        }

        private static void HandleClicked(AlcoholGlass glass)
        {
            AlcoholGlass.OnClick -= HandleClicked;

            var completion = pending;
            pending = null;

            completion?.TrySetResult(glass);
        }

        public static void Cancel()
        {
            AlcoholGlass.OnClick -= HandleClicked;

            var completion = pending;
            pending = null;

            completion?.TrySetCanceled();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            AlcoholGlass.OnClick -= HandleClicked;
            pending = null;
        }
    }
}

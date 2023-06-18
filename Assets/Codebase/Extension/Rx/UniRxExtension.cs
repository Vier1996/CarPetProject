using System;
using UniRx;
using UnityEngine;

namespace Codebase.Extension.Rx
{
    public static class UniRxExtension
    {
        public static IDisposable RxInvoke(this GameObject caller, float startDelay, Action onInvoke) =>
            Observable.Timer(TimeSpan.FromSeconds(startDelay))
                .Take(1)
                .Subscribe(_ => onInvoke?.Invoke())
                .AddTo(caller);
        
        public static IDisposable RxRepeat(this GameObject caller, float startDelay, float repeatingDelay, Action onInvoke) =>
            Observable.Timer(TimeSpan.FromSeconds(startDelay), TimeSpan.FromSeconds(repeatingDelay))
                .Subscribe(_ => onInvoke?.Invoke())
                .AddTo(caller);
    }
}

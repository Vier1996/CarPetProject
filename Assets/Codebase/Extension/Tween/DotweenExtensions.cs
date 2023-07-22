using System;
using DG.Tweening;
using UnityEngine;

namespace Codebase.Extension.Tween
{
    public static class DotweenExtensions
    {
        public static void KillTween(this Component component)
        {
            component.DOPause();
            component.DOKill();
        }
    }
}
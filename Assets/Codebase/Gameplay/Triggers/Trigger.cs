using System;
using Codebase.Gameplay.Colliders;
using UnityEngine;

namespace Codebase.Gameplay.Triggers
{
    public abstract class Trigger : MonoBehaviour
    {
        public event Action<WrappedCollider> OnEnter;
        public event Action<WrappedCollider> OnStay;
        public event Action<WrappedCollider> OnExit;
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out WrappedCollider wrappedCollider)) 
                TriggerEnterEvent(wrappedCollider);
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out WrappedCollider wrappedCollider)) 
                TriggerStayEvent(wrappedCollider);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out WrappedCollider wrappedCollider)) 
                TriggerExitEvent(wrappedCollider);
        }
        
        protected void TriggerEnterEvent(WrappedCollider wrappedCollider) => OnEnter?.Invoke(wrappedCollider);
        protected void TriggerStayEvent(WrappedCollider wrappedCollider) => OnStay?.Invoke(wrappedCollider);
        protected void TriggerExitEvent(WrappedCollider wrappedCollider) => OnExit?.Invoke(wrappedCollider);
    }
}
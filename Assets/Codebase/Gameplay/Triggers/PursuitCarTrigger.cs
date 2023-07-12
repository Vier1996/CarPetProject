using Codebase.Gameplay.Colliders;
using UnityEngine;

namespace Codebase.Gameplay.Triggers
{
    public class PursuitCarTrigger : Trigger
    {
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out WrappedCollider wrappedCollider))
            {
                if(wrappedCollider.GetType() == ColliderType.PURSUIT_CAR)
                    TriggerEnterEvent(wrappedCollider);
            }
        }
        
        protected override void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out WrappedCollider wrappedCollider))
            {
                if(wrappedCollider.GetType() == ColliderType.PURSUIT_CAR)
                    TriggerExitEvent(wrappedCollider);
            }
        }
    }
}

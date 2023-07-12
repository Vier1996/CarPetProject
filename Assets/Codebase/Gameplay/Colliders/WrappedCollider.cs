using UnityEngine;

namespace Codebase.Gameplay.Colliders
{
    public class WrappedCollider : MonoBehaviour
    {
        [SerializeField] private ColliderType _colliderType;

        public new ColliderType GetType() => _colliderType;
    }
}

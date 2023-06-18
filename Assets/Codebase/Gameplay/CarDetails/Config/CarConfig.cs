using Sirenix.OdinInspector;
using UnityEngine;

namespace Codebase.Gameplay.CarDetails.Config
{
    [CreateAssetMenu(fileName = "CarConfig", menuName = "App/Car/CarConfig")]
    public class CarConfig : ScriptableObject
    {
        public CarType CarType;

        [BoxGroup("SPEED"), Range(0, 190)] public int MaxForwardSpeed = 90;
        [BoxGroup("SPEED"), Range(0, 190)] public int MaxReverseSpeed = 45;

        [BoxGroup("ACCELERATION"), Range(1, 10)] public int AccelerationMultiplier = 2;
        
        [BoxGroup("STEERING")] public int MaxSteeringAngle = 27;
        [BoxGroup("STEERING"), Range(0.1f, 1f)] public float SteeringSpeed = 0.5f;
        
        [BoxGroup("BRAKE"), Range(100, 600)] public int BrakeForce = 350;

        [BoxGroup("DECELERATION"), Range(1, 10)] public int DecelerationMultiplier = 2;
        
        [BoxGroup("HAND_BRAKE"), Range(1, 10)] public int HandbrakeDriftMultiplier = 5;
    }
}
using System;
using UniRx;
using UnityEngine;

namespace Codebase.Gameplay.CarDetails.ControlType
{
    public class SeekerAI : IDisposable
    {
        private Car _ownCar;
        private Car _targetCar;
        private IDisposable _pursuitDisposable;

        private Vector3 _direction;
        private Vector3 _cross;
        private Quaternion _rotation;
        private float _angle = 0;
        
        public SeekerAI(Car ownCar, Car targetCar)
        {
            _ownCar = ownCar;
            _targetCar = targetCar;

            _pursuitDisposable = Observable.EveryUpdate().Subscribe(OnTick);
        }

        private void OnTick(long tick)
        {
            CalculateAngle();
            
            if (_angle >= 0)
            {
                if (_angle >= 10) 
                    _ownCar.TurnRight();
            }
            else
            {
                if (_angle <= -10) 
                    _ownCar.TurnLeft();
            }
            
            _ownCar.MoveUp();
        }

        private void CalculateAngle()
        {
            _direction = _targetCar.Transform.position - _ownCar.Transform.position;
            _rotation = Quaternion.LookRotation(_direction);
            _angle = Quaternion.Angle(_ownCar.Transform.rotation, _rotation);
            _cross = Vector3.Cross(_ownCar.Transform.forward, _direction);
            _angle *= Mathf.Sign(_cross.y);
        }
        
        public void Dispose()
        {
            _pursuitDisposable?.Dispose();
        }
    }
}
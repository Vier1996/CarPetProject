using System;
using Codebase.Extension.Rx;
using Codebase.Gameplay.CarDetails.Config;
using UnityEngine;

namespace Codebase.Gameplay.CarDetails.ControlType
{
  public class ManualCarController : MonoBehaviour
  {
    [SerializeField] private Car _targetCar;
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.W)) _targetCar.MoveUp();
        if (Input.GetKey(KeyCode.S)) _targetCar.MoveDown();
        if (Input.GetKey(KeyCode.A)) _targetCar.TurnLeft();
        if (Input.GetKey(KeyCode.D)) _targetCar.TurnRight();
        if (Input.GetKey(KeyCode.Space)) _targetCar.UseHandbrake();
        if (Input.GetKeyUp(KeyCode.Space)) _targetCar.RecoverTraction();
        if ((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))) _targetCar.ThrottleOff();
        if ((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) && !Input.GetKey(KeyCode.Space)) _targetCar.Decelerate();
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) _targetCar.ResetSteeringAngle();
    }
  }
}

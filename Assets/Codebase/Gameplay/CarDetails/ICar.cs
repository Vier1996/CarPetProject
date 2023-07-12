namespace Codebase.Gameplay.CarDetails
{
    public interface ICar
    {
        public void MoveUp();
        public void MoveDown();
        public void TurnRight();
        public void TurnLeft();
        public void UseHandbrake();
        public void RecoverTraction();
        public void ResetSteeringAngle();
        public void Decelerate();
        public void ThrottleOff();
        public void SwitchOnCar();
        public void SwitchOffCar();
    }
}
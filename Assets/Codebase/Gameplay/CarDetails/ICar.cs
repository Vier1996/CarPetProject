namespace Codebase.Gameplay.CarDetails
{
    public interface ICar
    {
        public void MoveUp();
        public void MoveDown();
        public void TurnRight();
        public void TurnLeft();
        public void UseHandbrake();
    }
}
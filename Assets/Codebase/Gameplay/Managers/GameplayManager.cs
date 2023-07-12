using System.Collections.Generic;
using Codebase.Gameplay.Colliders;
using Codebase.Gameplay.Entities;
using Codebase.UI.PursuitViews;
using UnityEngine;

namespace Codebase.Gameplay.Managers
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private PursuitView _pursuitView;

        private Player _player;
        private List<WrappedCollider> pursuitCarColliders = new List<WrappedCollider>();

        public void Init(Player player)
        {
            _player = player;
            
            _pursuitView.OnPlayerCarStopped += OnCarStopped;
            
            _player.GetPursuitTrigger().OnEnter += AddPursuitTrigger;
            _player.GetPursuitTrigger().OnExit += RemoveAddPursuitTrigger;
        }

        private void AddPursuitTrigger(WrappedCollider wrappedCollider)
        {
            if (!pursuitCarColliders.Contains(wrappedCollider))
            {
                pursuitCarColliders.Add(wrappedCollider);
                
                CheckPursuitState();
            }
        }
        
        private void RemoveAddPursuitTrigger(WrappedCollider wrappedCollider)
        {
            if(pursuitCarColliders.Contains(wrappedCollider))
            {
                pursuitCarColliders.Remove(wrappedCollider);
                
                CheckPursuitState();
            }
        }

        private void CheckPursuitState()
        {
            if (pursuitCarColliders.Count > 0) 
                _pursuitView.EnablePursuitView();
            else 
                _pursuitView.DisablePursuitView();
        }
        
        private void OnCarStopped()
        {
            _player.GetPlayerCar().SwitchOffCar();
        }

        private void OnDestroy()
        {
            _pursuitView.OnPlayerCarStopped -= OnCarStopped;

            _player.GetPursuitTrigger().OnEnter -= AddPursuitTrigger;
            _player.GetPursuitTrigger().OnExit -= RemoveAddPursuitTrigger;
        }
    }
}

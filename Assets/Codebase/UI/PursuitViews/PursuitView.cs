using System;
using TMPro;
using UnityEngine;

namespace Codebase.UI.PursuitViews
{
    public class PursuitView : MonoBehaviour
    {
        public event Action OnPlayerCarStopped;
        
        [SerializeField] private TextMeshProUGUI _pursuitText;

        public void EnablePursuitView()
        {
            
        }

        public void DisablePursuitView()
        {
            
        }
    }
}

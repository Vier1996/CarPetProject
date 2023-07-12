using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Codebase.UI.PursuitViews
{
    public class PursuitView : MonoBehaviour
    {
        public event Action OnPlayerCarStopped;
        
        [SerializeField] private TextMeshProUGUI _pursuitText;

        private Coroutine _pursuitCoroutine;
        private WaitForSeconds _coroutineDelay;
        
        private bool _isEnabled = false;
        
        private void Start()
        {
            _coroutineDelay = new WaitForSeconds(1f);
        }

        [Button]
        public void EnablePursuitView()
        {
            if(_isEnabled) return;
            
            _pursuitText.gameObject.SetActive(true);
            
            if(_pursuitCoroutine != null)
                StopCoroutine(_pursuitCoroutine);

            _pursuitCoroutine = StartCoroutine(PursuitEnumerator());
            
            _isEnabled = true;
        }

        [Button]
        public void DisablePursuitView()
        {
            if(!_isEnabled) return;

            if(_pursuitCoroutine != null)
                StopCoroutine(_pursuitCoroutine);
            
            _pursuitText.gameObject.SetActive(false);

            _isEnabled = false;
        }

        private IEnumerator PursuitEnumerator()
        {
            int second = 3;

            _pursuitText.text = second.ToString();
            
            while (true)
            {
                yield return _coroutineDelay;

                if (second <= 0)
                    break;

                _pursuitText.text = (--second).ToString();
            }
            
            _pursuitText.text = "Stopped!!!";

            OnPlayerCarStopped?.Invoke();
        }
    }
}

using Codebase.Extension.Tween;
using DG.Tweening;
using UnityEngine;

namespace Codebase.Gameplay.Scenes
{
    public class GarageMenu : MonoBehaviour
    {
        [SerializeField] protected GarageMenuType _menuType;
        
        [Range(0f, 2f), SerializeField] protected float _showAnimateDuration = 1f;
        [Range(0f, 2f), SerializeField] protected float _hideAnimateDuration = 1f;

        protected GarageSceneUiHandler.CallMenuDelegate _callMenuDelegate;
        
        private CanvasGroup _menuCanvasGroup;
        
        public virtual void Init(GarageSceneUiHandler.CallMenuDelegate callMenuDelegate)
        {
            _callMenuDelegate = callMenuDelegate;
            _menuCanvasGroup = GetComponent<CanvasGroup>();
        }
        
        public virtual void ShowMenu(bool instant = false)
        {
            if (instant)
            {
                _menuCanvasGroup.gameObject.SetActive(true);
                _menuCanvasGroup.alpha = 1;
                return;
            }
            
            _menuCanvasGroup.KillTween();
            _menuCanvasGroup.gameObject.SetActive(true);
            _menuCanvasGroup.DOFade(1, _showAnimateDuration);
        }

        public virtual void HideMenu(bool instant = false)
        {
            if (instant)
            {
                _menuCanvasGroup.alpha = 0;
                _menuCanvasGroup.gameObject.SetActive(false);
                return;
            }

            _menuCanvasGroup.KillTween();
            _menuCanvasGroup
                .DOFade(0, _hideAnimateDuration)
                .OnComplete(() => _menuCanvasGroup.gameObject.SetActive(false));
        }
    }
}
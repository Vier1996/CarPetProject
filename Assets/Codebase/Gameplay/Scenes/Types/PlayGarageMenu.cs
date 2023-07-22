using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Codebase.Gameplay.Scenes.Types
{
    public class PlayGarageMenu : GarageMenu
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _storeButton;
        [SerializeField] private Button _upgradeButton;

        private Vector3 _playButtonDefaultPosition;
        private Vector3 _storeButtonDefaultPosition;
        private Vector3 _upgradeButtonDefaultPosition;
        
        public override void Init(GarageSceneUiHandler.CallMenuDelegate callMenuDelegate)
        {
            base.Init(callMenuDelegate);
            
            _playButtonDefaultPosition = _playButton.transform.position;
            _storeButtonDefaultPosition = _storeButton.transform.position;
            _upgradeButtonDefaultPosition = _upgradeButton.transform.position;
            
            _storeButton.onClick.AddListener(CallStoreMenuCallback);
            _upgradeButton.onClick.AddListener(CallUpgradeMenuCallback);
        }

        private void CallStoreMenuCallback()
        {
            _callMenuDelegate?.Invoke(GarageMenuType.STORE);
        }

        private void CallUpgradeMenuCallback()
        {
            _callMenuDelegate?.Invoke(GarageMenuType.UPGRADE);
        }
        
        public override void ShowMenu(bool instant = false)
        {
            if (instant)
            {
                base.ShowMenu(true);
                return;
            }
            
            _storeButton.transform.DOMoveX(_storeButtonDefaultPosition.x + 50, _hideAnimateDuration);
            _upgradeButton.transform.DOMoveX(_upgradeButtonDefaultPosition.x - 50, _hideAnimateDuration);
            _playButton.transform.DOMoveY(_playButtonDefaultPosition.y - 250, _hideAnimateDuration);
            
            base.ShowMenu(false);
        }

        public override void HideMenu(bool instant = false)
        {
            _storeButton.transform.DOMoveX(_storeButtonDefaultPosition.x + 50, _hideAnimateDuration);
            _upgradeButton.transform.DOMoveX(_upgradeButtonDefaultPosition.x - 50, _hideAnimateDuration);
            _playButton.transform.DOMoveY(_playButtonDefaultPosition.y - 250, _hideAnimateDuration);
            
            base.HideMenu(instant);
        }

        private void OnDestroy()
        {
            _storeButton.onClick.RemoveAllListeners();
            _upgradeButton.onClick.RemoveAllListeners();
        }
    }
}

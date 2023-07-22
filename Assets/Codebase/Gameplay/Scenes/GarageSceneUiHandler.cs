using System;
using Codebase.Gameplay.Scenes.Types;
using UnityEngine;

namespace Codebase.Gameplay.Scenes
{
    public class GarageSceneUiHandler : MonoBehaviour
    {
        public delegate void CallMenuDelegate(GarageMenuType menuType);
        
        [SerializeField] private GarageMenu _playGarageMenu;
        [SerializeField] private GarageMenu _storeGarageMenu;
        [SerializeField] private GarageMenu _upgradeGarageMenu;

        private GarageMenu _currentMenu;
        private CallMenuDelegate _callMenuDelegate;
        
        private void Start()
        {
            _callMenuDelegate = CallAnotherMenu;
            
            _playGarageMenu.Init(_callMenuDelegate);
            _playGarageMenu.ShowMenu(true);

            _storeGarageMenu.Init(_callMenuDelegate);
            _storeGarageMenu.HideMenu(true);

            _upgradeGarageMenu.Init(_callMenuDelegate);
            _upgradeGarageMenu.HideMenu(true);

            _currentMenu = _playGarageMenu;
        }

        private void CallAnotherMenu(GarageMenuType menuType)
        {
            _currentMenu.HideMenu();
            
            _currentMenu = GetActualGarageMenu(menuType);
            
            _currentMenu.ShowMenu();
        }

        private GarageMenu GetActualGarageMenu(GarageMenuType menuType)
        {
            switch (menuType)
            {
                case GarageMenuType.PLAY: return _playGarageMenu;
                case GarageMenuType.STORE: return _storeGarageMenu;
                case GarageMenuType.UPGRADE: return _upgradeGarageMenu;
            }

            throw new ArgumentException($"Can not get instance with type:[{menuType}]");
        }
    }
}

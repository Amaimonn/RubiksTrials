using UnityEngine;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.Configs;

namespace Assets._Game.Scripts.MVVM.Models
{
    public class TabbedMenuModel : IModel
    {
        public ITabData[] Tabs => _tabs;
        public Observable<int> CurrentTab => _currentTab;
        public Observable<int> TabToClose => _tabToClose;
        public Observable<bool> IsActive => _isActive;
        public Observable<Unit> OnOpenMenu => _onOpenMenu;

        protected ReactiveProperty<int> _currentTab = new();
        protected Subject<int> _tabToClose = new();
        protected ReactiveProperty<bool> _isActive = new();
        protected Subject<Unit> _onOpenMenu = new();
        protected ITabData[] _tabs;

        public TabbedMenuModel(ITabData[] tabs)
        {
            _tabs = tabs;
        }

        public void SelectTab(int tabIndex)
        {
            _tabToClose.OnNext(_currentTab.Value);
            _currentTab.Value = tabIndex;
        }

        public void OpenMenu()
        {
            _isActive.Value = true;
        }

        public void CloseMenu()
        {
            _isActive.Value = false;
        }
    }
}
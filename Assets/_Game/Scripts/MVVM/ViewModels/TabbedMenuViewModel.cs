using System;
using UnityEngine;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.Models;
using Assets._Game.Scripts.Configs;

namespace Assets._Game.Scripts.MVVM.ViewModels
{
    public class TabbedMenuViewModel : ViewModel<TabbedMenuModel>
    {
        public override Type ViewModelType => typeof(TabbedMenuViewModel);
        public ITabData[] Tabs => _tabs;
        public Observable<int> CurrentTab => _currentTab;
        public Observable<int> TabToClose => _tabToClose;
        public Observable<bool> OnActive => _onActive;

        protected ReactiveProperty<int> _currentTab = new();
        protected Subject<int> _tabToClose = new();
        protected ReactiveProperty<bool> _onActive = new();
        protected CompositeDisposable _disposables;
        protected ITabData[] _tabs;

        public TabbedMenuViewModel(TabbedMenuModel model) : base(model)
        {
        }

        protected override void OnBind(TabbedMenuModel model)
        {
            _tabs = model.Tabs;
            _disposables = new()
            {
                model.CurrentTab.Subscribe(x => _currentTab.Value = x),
                model.TabToClose.Subscribe(x => _tabToClose.OnNext(x)),
                model.IsActive.Subscribe((x) => {
                    _onActive.OnNext(x);
                })
            };
        }

        public void OnTabClicked(int tabIndex)
        {
            if (_currentTab.Value != tabIndex)
            {
                _model.SelectTab(tabIndex);
            }
        }

        public void CloseMenu()
        {
            _model.CloseMenu();
        }

        public override void Dispose()
        {
            // очищать только при выходе с уровня
            _disposables.Dispose();
        }
    }
}
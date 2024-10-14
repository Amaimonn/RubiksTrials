using UnityEngine.UIElements;
using UnityEngine;
using R3;
using Assets._Game.Scripts.MVVM.Abstractions;
using Assets._Game.Scripts.MVVM.ViewModels;

public class TabbedMenuView : BaseScreen<TabbedMenuViewModel>
{
    protected ScrollView _scrollView;
    private const string _tabClassName = "tab";
    private const string _currentlySelectedTabClassName = "tutorial-tab--selected";
    private const string _unselectedContentClassName = "tutorial-content--unselected";
    private const string _scrollViewClassName = "tutorial__scrollview";
    protected const string _tabLabelsContainerClassName = "tabs-scroll-view";
    protected const string _exitButtonClassName = "tutorial-exit__button";

    [SerializeField] protected UIDocument _uiDocument;
    protected VisualElement _root;
    protected Button _exitButton;
    protected Label[] _tabLabels;
    protected VisualElement[] _tabContents;
    protected CompositeDisposable _disposables;

#region BaseScreen implementation
    public override void Show()
    {
        // gameObject.SetActive(true);
        _root.style.display = DisplayStyle.Flex;
    }

    public override void Close()
    {
        // gameObject.SetActive(false);
        _root.style.display = DisplayStyle.None;
        Debug.Log("Tutor closed");
    }

    protected override void OnBind(TabbedMenuViewModel viewModel)
    {
        _exitButton = _root.Q<Button>(className: _exitButtonClassName);
        _exitButton.clicked += OnExitButtonClicked;
        _scrollView = _root.Q<ScrollView>(className: _scrollViewClassName);

        _tabLabels = new Label[viewModel.Tabs.Length];
        _tabContents = new VisualElement[viewModel.Tabs.Length];

        VisualElement contentContainer = _scrollView.contentContainer;
        VisualElement tabLabelsContainer = _root.Q<VisualElement>(className: _tabLabelsContainerClassName);
        contentContainer.Clear();
        tabLabelsContainer.Clear();

        for (var tabIndex = 0;  tabIndex < viewModel.Tabs.Length; tabIndex++)
        {
            var tabLabel = viewModel.Tabs[tabIndex].TabLabel;
            tabLabel.userData = tabIndex;
            tabLabel.RegisterCallback<ClickEvent>(TabOnClick);
            _tabLabels[tabIndex] = tabLabel;
            tabLabelsContainer.Add(tabLabel);
            
            var tabContent = viewModel.Tabs[tabIndex].TabContent;
            tabContent.userData = tabIndex;
            _tabContents[tabIndex] = tabContent;
            tabContent.AddToClassList(_unselectedContentClassName);
            contentContainer.Add(tabContent);
        }
        RegisterTabCallbacks();

        _disposables = new()
        {
            viewModel.TabToClose.Where(x => x > -1).Subscribe(UnselectTab),
            viewModel.CurrentTab.Subscribe(SelectTab),
            viewModel.OnActive.Subscribe(SetViewActive)
        };

        // //3. Связываем шаблон элементов списка с самим списком
        // itemsListView.makeItem = () =>
        // {
        //     return itemsListTemplate.Instantiate();
        // };

        // //4. Связываем данные с отображаемым списком
        // itemsListView.bindItem = (_item, _index) =>
        // {
        //     //Связываем список товаров по индексу
        //     var item = items[_index];
        //     //Получаем доступ к визуальным элементам шаблона по именам, которые мы указали в шаблоне
        //     _item.Q<Label>("name").text = item.Name;
        //     _item.Q<Label>("price").text = $"{item.Price} руб";
        //     //В данном случае картинка товара должна лежать в папке Resources и иметь название такое же, как название товара
        //     _item.Q<VisualElement>("image").style.backgroundImage = Resources.Load<Texture2D>(item.Name);
        // };

        // //Здесь все стандартно
        // itemsListView.itemsSource = items;


    }
#endregion

    public void SetViewActive(bool isActive)
    {
        if (isActive)
        {
            Show();
        }
        else
        {
            Close();
        }
    }

    public void RegisterTabCallbacks()
    {
        UQueryBuilder<Label> tabs = GetAllTabs();
        tabs.ForEach((Label tab) => {
            tab.RegisterCallback<ClickEvent>(TabOnClick);
        });
    }

#region MonoBehaviour
    private void Awake()
    {
        _root = _uiDocument.rootVisualElement.Q<VisualElement>(className: "tutorial__body");
    }
#endregion

    private void OnExitButtonClicked()
    {
        _viewModel.CloseMenu();
    }

    private void TabOnClick(ClickEvent evt)
    {
        Label clickedTab = evt.currentTarget as Label;
        _viewModel.OnTabClicked((int)clickedTab.userData);
    }

    private UQueryBuilder<Label> GetAllTabs()
    {
        return _root.Query<Label>(className: _tabClassName);
    }

    private void SelectTab(int tabIndex)
    {
        _tabLabels[tabIndex].AddToClassList(_currentlySelectedTabClassName);
        _tabContents[tabIndex].RemoveFromClassList(_unselectedContentClassName);
        _scrollView.ScrollTo(_scrollView.ElementAt(0));
    }

    private void UnselectTab(int tabIndex)
    {
        _tabLabels[tabIndex].RemoveFromClassList(_currentlySelectedTabClassName);
        _tabContents[tabIndex].AddToClassList(_unselectedContentClassName);
    }

    public override void Dispose()
    {
        _disposables.Dispose();
        _exitButton.clicked -= OnExitButtonClicked;
    }
}

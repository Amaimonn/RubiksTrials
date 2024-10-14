using Assets._Game.Scripts.Configs;
using UnityEngine;

[CreateAssetMenu(fileName = "TabSO", menuName = "ScriptableObjects/Tabbed Menu")]
public class TabbedMenuSO : ScriptableObject, ITabbedMenuData
{
    public Assets._Game.Scripts.Configs.ITabData[] TabsData => _tabsData;

    [SerializeField] private TabSO[] _tabsData;
}

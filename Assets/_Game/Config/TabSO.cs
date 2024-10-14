using System.Collections.Generic;
using System.Xml.Schema;
using Assets._Game.Scripts.Configs;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "TabSO", menuName = "ScriptableObjects/Tab")]
public class TabSO : ScriptableObject, ITabData
{
    public Label TabLabel => GetLabel();
    public VisualElement TabContent => GetContent();

    [SerializeField] private string _tabLabelText;
    [SerializeField] private VisualTreeAsset _tabContent;
    // [TextArea(3, 10)]
    // [SerializeField] private string[] _tabContent;

    private Label GetLabel()
    {
        var label = new Label
        {
            text = _tabLabelText
        };

        return label;
    }

    private VisualElement GetContent()
    {
        // var content = new VisualElement();
        // var index = 0;
        // foreach (var contentText in _tabContent)
        // {
        //     var label = new Label
        //     {
        //         text = _tabContent[index]
        //     };
        //     index++;
        //     content.Add(label);
        // }
        return _tabContent.Instantiate();
    }
}

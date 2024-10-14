using UnityEngine.UIElements;

namespace Assets._Game.Scripts.Configs
{
    public interface ITabData
    {
        public Label TabLabel { get; }
        public VisualElement TabContent { get; } 
    }
}

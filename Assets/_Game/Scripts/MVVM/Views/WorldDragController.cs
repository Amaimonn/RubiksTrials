using UnityEngine;

namespace Assets._Game.Scripts.MVVM.Views
{
    public class WorldDragController : MonoBehaviour
    {
        [Header("Controls")]
        public GameObject Panel;
        public GameObject UpArrow;
        public GameObject DownArrow;
        public GameObject RightArrow;
        public GameObject LeftArrow;
        public Collider CancelZone;
    }
}
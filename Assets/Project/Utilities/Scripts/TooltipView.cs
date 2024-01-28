using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    [RequireComponent(typeof(RectTransform))]
    public class TooltipView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        public RectTransform RectTransform { get; private set; }
        private Camera _camera;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            _camera = Camera.main;
        }

        public void Align()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform); 
            Vector3[] corners = new Vector3[4];
            RectTransform.GetWorldCorners(corners);

            Vector2 screenMax = Screen.safeArea.max;
            Vector2 screenMin = Screen.safeArea.min;
            Vector2 rectMax = Vector2.zero;
            Vector2 rectMin = Vector2.zero;
            foreach (Vector3 corner in corners)
            {
                if (corner.x > rectMax.x) rectMax.x = corner.x;
                if (corner.x < rectMin.x) rectMin.x = corner.x;
                if (corner.y > rectMax.y) rectMax.y = corner.y;
                if (corner.y < rectMin.y) rectMin.y = corner.y;
            }

            Vector2 overMax = new Vector2(Mathf.Max(rectMax.x - screenMax.x, 0), Mathf.Max(rectMax.y - screenMax.y, 0));
            Vector2 underMin = new Vector2(Mathf.Max(screenMin.x - rectMin.x, 0), Mathf.Max(screenMin.y - rectMin.y, 0));
            if (overMax.x > 0) transform.position += Vector3.left * overMax.x;
            if (overMax.y > 0) transform.position += Vector3.down * overMax.y;
            if (underMin.x > 0) transform.position += Vector3.right * underMin.x;
            if (underMin.y > 0) transform.position += Vector3.up * underMin.y;
        }
        
        public string Text
        {
            get => _text.text;
            set => _text.text = value;
        }

        public void ShowTooltip(string text, Vector3 position)
        {
            transform.position = position;
            Text = text;
            Align();
        }
    }
}
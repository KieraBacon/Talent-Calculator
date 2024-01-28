using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities;

namespace SkillSystem.UI
{
    public class TalentNodeView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Color _unavailableColor;
        [SerializeField] private Color _availableColor;
        [SerializeField] private Color _fullyTakenColor;
        private Color[] _colors;
        private Color[] Colors => _colors ??= new[] { _unavailableColor, _availableColor, _fullyTakenColor };
        [SerializeField] private Image _borderObject;
        [SerializeField] private Image _highlightObject;
        [SerializeField] private Image _iconObject;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _grayscaleMaterial;
        [SerializeField] private TMP_Text _labelObject;
        [SerializeField] private TMP_Text _counterLabelObject;
        [SerializeField] private RectTransform _counterObject;
        private TooltipView _tooltipView;
        private bool _tooltipActive = false;
        private Vector3 TooltipPosition => transform.position + Vector3.one * 50;
        private bool _isHovered = false;
        
        private TalentNodeInstance _talentNodeInstance;
        public TalentNodeInstance TalentNodeInstance
        {
            get => _talentNodeInstance;
            set
            {
                if (_talentNodeInstance == value) return;
                if (_talentNodeInstance != null)
                    _talentNodeInstance.OnInvestmentChanged -= HandleTalentInvestmentChange;
                
                _talentNodeInstance = value;
                if (_talentNodeInstance != null)
                    _talentNodeInstance.OnInvestmentChanged += HandleTalentInvestmentChange;
            }
        }

        private void Start()
        {
            _isHovered = false;
            UpdateInformation();
        }

        private void HandleTalentInvestmentChange(TalentNodeInstance talentNodeInstance)
        {
            UpdateInformation();
        }

        private void UpdateInformation()
        {
            SetHighlight();
            SetSprite();
            SetLabels();
            SetColours();
            SetVisibility();
        }

        private void SetHighlight()
        {
            _highlightObject.gameObject.SetActive(_isHovered);
        }

        private void SetLabels()
        {
            bool isValid = TalentNodeInstance.IsValid();
            _labelObject.text = isValid ? _iconObject.sprite == null ? TalentNodeInstance.DisplayRankTalent.Name : "" : ""; 
            _counterLabelObject.text = isValid ? $"{TalentNodeInstance.Investment}/{TalentNodeInstance.MaxInvestment}" : "";
        }

        private void SetSprite()
        {
            bool isValid = TalentNodeInstance.IsValid();
            _iconObject.sprite = isValid ? TalentNodeInstance.DisplayRankTalent.Sprite : null;
            _iconObject.material = isValid ? TalentNodeInstance.AvailabilityState != TalentNodeInstance.AvailabilityStates.Unavailable ? _defaultMaterial : _grayscaleMaterial : null;
        }
        
        private void SetColours()
        {
            Color color = Color.clear;
            if (TalentNodeInstance != null) color = Colors[(int)TalentNodeInstance.AvailabilityState];
            _borderObject.color = color;
            _labelObject.color = color;
        }

        private void SetVisibility()
        {
            bool isValid = TalentNodeInstance.IsValid();
            _labelObject.gameObject.SetActive(isValid);
            _counterObject.gameObject.SetActive(isValid);
            _counterLabelObject.gameObject.SetActive(isValid);
            _iconObject.gameObject.SetActive(isValid);
            _borderObject.gameObject.SetActive(isValid);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Increment(eventData.button switch
            {
                PointerEventData.InputButton.Left => 1,
                PointerEventData.InputButton.Right => -1,
                _ => 0
            });
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovered = true;
            _highlightObject.gameObject.SetActive(_isHovered);
            ReleaseTooltip();
            ShowTooltip();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
            _highlightObject.gameObject.SetActive(_isHovered);
            ReleaseTooltip();
        }

        public void Increment(int increment)
        {
            if (increment == 0) return; 
            TalentNodeInstance.Increment(increment);
            ShowTooltip();
        }

        public void ReleaseTooltip()
        {
            if (!_tooltipActive || _tooltipView == null) return;
            TooltipManager.Instance.ReleaseTooltip(_tooltipView);
            _tooltipActive = false;
        }

        public void ShowTooltip()
        {
            if (_tooltipActive)
            {
                _tooltipView.ShowTooltip(TalentNodeInstance.Tooltip, TooltipPosition);
            }
            else
            {
                _tooltipView = TooltipManager.Instance.ShowTooltip(TalentNodeInstance.Tooltip, TooltipPosition);
                _tooltipActive = true;
            }
        }
    }
}
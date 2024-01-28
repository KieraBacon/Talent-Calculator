using System;
using UnityEngine;

namespace Utilities
{
    public class TooltipManager
    {
        private static readonly Lazy<TooltipManager> _lazy = new Lazy<TooltipManager>(() => new TooltipManager());
        public static TooltipManager Instance =>
            _lazy.Value;
        private Canvas _tooltipCanvas;

        public Canvas TooltipCanvas
        {
            get
            {
                if (_tooltipCanvas != null) return _tooltipCanvas;
                
                Pool<Canvas> canvasPool = PoolManager.Instance.Get<Canvas>("Canvas");
                Canvas canvas = canvasPool.Get();
                canvas.sortingOrder = 1;
                _tooltipCanvas = canvas;

                return _tooltipCanvas;
            }
        }

        public TooltipView ShowTooltip(string text, Vector3 position)
        {
            Pool<TooltipView> tooltipPool = PoolManager.Instance.Get<TooltipView>("Tooltip View");
            TooltipView tooltipView = tooltipPool.Get(TooltipCanvas.transform);
            tooltipView.ShowTooltip(text, position);
            return tooltipView;
        }

        public void ReleaseTooltip(TooltipView tooltipView)
        {
            if (tooltipView == null) return;
            Pool<TooltipView> tooltipPool = PoolManager.Instance.Get<TooltipView>("Tooltip View");
            tooltipPool.Release(tooltipView);
        }
    }
}
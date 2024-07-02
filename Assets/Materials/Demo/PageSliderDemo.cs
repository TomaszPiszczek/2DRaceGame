#region Includes
using System;

using UnityEngine;
#endregion

namespace TS.PageSlider.Demo
{
    public class PageSliderDemo : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private PageSlider _slider;

        [Header("Configuration")]
        [SerializeField] private SliderItem[] _items;

        #endregion

        private void Start()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                Debug.Log(i);
                var canvasInstance = Instantiate(_items[i].CanvasPrefab);
                _slider.AddPage((RectTransform)canvasInstance.transform);
            }
        }
    }

    [Serializable]
    public class SliderItem
    {
        [SerializeField] private GameObject _canvasPrefab;

        public GameObject CanvasPrefab { get { return _canvasPrefab; } }
    }
}
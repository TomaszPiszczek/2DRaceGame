using System.Collections;
using System.Collections.Generic;
using TS.PageSlider;
using UnityEngine;

public class PageScript : MonoBehaviour
{
  public PageSlider _pageSlider;
  public PageView _pageView;

  void Start() {
    for (int i = 0; i < 3; i++)
    {
        var page = Instantiate(_pageView);
       
        _pageSlider.AddPage((RectTransform)page.transform);
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UI.Pagination;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabButton selectedTab;

    public List<GameObject> objectsToSwap;

    public void Subscribe(TabButton tabButton){
        if(tabButtons == null){
            tabButtons = new List<TabButton>{};
        }
        tabButtons.Add(tabButton);
    }


    public void OnTabEnter(TabButton button)
    {

        ResetTabs();
        if(selectedTab == null || selectedTab != button)
        {
        button.background.sprite = tabHover;

        }
    }

     public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        foreach (TabButton tab in tabButtons)
        {
            if (tab == selectedTab)
                tab.SetSelectedState();
            else
                tab.SetIdleState();
        }

        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            objectsToSwap[i].SetActive(i == index);
        }
    }




    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if(selectedTab !=null && button == selectedTab ){continue;}
            button.background.sprite=tabIdle;
        }
    }

}

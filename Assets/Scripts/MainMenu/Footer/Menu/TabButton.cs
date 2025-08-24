using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler
{
    public TabGroup tabGroup;
    public Image background;

    public Sprite idleSprite;
    public Sprite selectedSprite;

    [Header("Size Settings")]
    public float normalWidth = 100f;
    public float selectedWidth = 130f;
    public float normalHeight = 50f;
    public float selectedHeight = 65f;
    public float resizeSpeed = 10f;

    private LayoutElement layoutElement;
    private Coroutine resizeCoroutine;

    void Start()
    {
        background = GetComponent<Image>();
        layoutElement = GetComponent<LayoutElement>();
        
        // Add LayoutElement if it doesn't exist
        if (layoutElement == null)
        {
            layoutElement = gameObject.AddComponent<LayoutElement>();
        }

        // Set initial size
        layoutElement.preferredWidth = normalWidth;
        layoutElement.preferredHeight = normalHeight;

        if (tabGroup == null)
        {
            Debug.LogError($"[{name}] TabGroup is not assigned!");
            return;
        }

        tabGroup.Subscribe(this);

        if (idleSprite != null)
        {
            background.sprite = idleSprite;
            Debug.Log($"[{name}] Idle sprite assigned at Start(): {idleSprite.name}");
        }
        else
        {
            Debug.LogWarning($"[{name}] Idle sprite is null at Start()");
        }
    }

    public void SetIdleState()
    {
        if (idleSprite != null)
        {
            background.sprite = idleSprite;
            Debug.Log($"[{name}] SetIdleState() -> {idleSprite.name}");
        }
        
        // Resize to normal (instant version - uncomment if you don't want animation)
        // layoutElement.preferredWidth = normalWidth;
        // layoutElement.preferredHeight = normalHeight;
        
        // Animated resize
        if (resizeCoroutine != null)
        {
            StopCoroutine(resizeCoroutine);
        }
        resizeCoroutine = StartCoroutine(ResizeTab(normalWidth, normalHeight));
    }

    public void SetSelectedState()
    {
        if (selectedSprite != null)
        {
            background.sprite = selectedSprite;
            Debug.Log($"[{name}] SetSelectedState() -> {selectedSprite.name}");
        }
        
        // Resize to selected size (instant version - uncomment if you don't want animation)
        // layoutElement.preferredWidth = selectedWidth;
        // layoutElement.preferredHeight = selectedHeight;
        
        // Animated resize
        if (resizeCoroutine != null)
        {
            StopCoroutine(resizeCoroutine);
        }
        resizeCoroutine = StartCoroutine(ResizeTab(selectedWidth, selectedHeight));
    }

    private IEnumerator ResizeTab(float targetWidth, float targetHeight)
    {
        float startWidth = layoutElement.preferredWidth;
        float startHeight = layoutElement.preferredHeight;
        float elapsedTime = 0f;
        float duration = 0.2f; // Animation duration in seconds

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            // Use smooth step for smoother animation
            t = t * t * (3f - 2f * t);
            
            layoutElement.preferredWidth = Mathf.Lerp(startWidth, targetWidth, t);
            layoutElement.preferredHeight = Mathf.Lerp(startHeight, targetHeight, t);
            
            yield return null;
        }

        // Ensure we reach exact target values
        layoutElement.preferredWidth = targetWidth;
        layoutElement.preferredHeight = targetHeight;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"[{name}] OnPointerClick");
        tabGroup.OnTabSelected(this);
    }
}
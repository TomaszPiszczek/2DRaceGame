using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;

public class ShopPopulatorPages : MonoBehaviour
{
    [Header("SSS + Layout")]
    public SimpleScrollSnap scrollSnap;   // przypnij SimpleScrollSnap z Main
    public RectTransform viewport;        // przypnij ViewPort
    public RectTransform content;         // przypnij Content

    [Header("Prefabs")]
    public RectTransform pagePrefab;      // przypnij Page.prefab
    public CarCardUI cardPrefab;          // przypnij Card.prefab

    [Header("Page setup")]
    public int cardsPerPage = 4;          // 4 karty/strona
    public int columns = 4;               // 4 kolumny, 1 rząd
    public int rows = 1;
    public Vector2 spacing = new Vector2(20f, 0f);
    public Vector4 padding = new Vector4(20, 20, 20, 20); // L, T, R, B

    private readonly List<GameObject> spawned = new();

    private void OnEnable()
    {
        // Check if components are valid before populating
        if (ValidateComponents())
        {
            // domyślnie tier 0 — zmień jak potrzebujesz
            Populate(0);
        }
    }

    private bool ValidateComponents()
    {
        if (scrollSnap == null)
        {
            Debug.LogError("ScrollSnap is null!");
            return false;
        }
        if (viewport == null)
        {
            Debug.LogError("Viewport is null!");
            return false;
        }
        if (content == null)
        {
            Debug.LogError("Content is null!");
            return false;
        }
        if (pagePrefab == null)
        {
            Debug.LogError("PagePrefab is null!");
            return false;
        }
        if (cardPrefab == null)
        {
            Debug.LogError("CardPrefab is null!");
            return false;
        }
        return true;
    }

    public void Populate(int tier)
    {
        // Validate components before proceeding
        if (!ValidateComponents())
        {
            Debug.LogError("Cannot populate - missing components!");
            return;
        }

        ClearSpawned();

        List<Car> cars = CarShopList.GetCarsByTier(tier);
        int i = 0;
        while (i < cars.Count)
        {
            RectTransform page = CreatePage();
            
            // Check if page creation failed
            if (page == null)
            {
                Debug.LogError("Failed to create page!");
                break;
            }

            float innerW = viewport.rect.width  - padding.x - padding.z;
            float innerH = viewport.rect.height - padding.y - padding.w;

            float cellW = (innerW - spacing.x * (columns - 1)) / columns;
            float cellH = rows > 1
                ? (innerH - spacing.y * (rows - 1)) / rows
                : innerH;

            for (int j = 0; j < cardsPerPage && i < cars.Count; j++, i++)
            {
                var card = Instantiate(cardPrefab, page);
                card.SetData(cars[i]);

                // Compute row/col
                int col = j % columns;
                int row = j / columns;

                // Position
                float x = padding.x + col * (cellW + spacing.x);
                float y = -(padding.y + row * (cellH + spacing.y));

                var rt = card.GetComponent<RectTransform>();
                rt.anchorMin = rt.anchorMax = new Vector2(0, 1); // top-left anchoring
                rt.pivot = new Vector2(0, 1);
                rt.sizeDelta = new Vector2(cellW, cellH);
                rt.anchoredPosition = new Vector2(x, y);

                spawned.Add(card.gameObject);
            }
        }

        StartCoroutine(RebuildScrollSnapNextFrame());
    }


    private RectTransform CreatePage()
    {
        // Check if content is still valid
        if (content == null)
        {
            Debug.LogError("Content RectTransform is null - cannot create page!");
            return null;
        }
        
        if (pagePrefab == null)
        {
            Debug.LogError("PagePrefab is null - cannot create page!");
            return null;
        }

        RectTransform page = Instantiate(pagePrefab, content);

        // Strona = rozmiar ViewPorta
        page.anchorMin = new Vector2(0.5f, 0.5f);
        page.anchorMax = new Vector2(0.5f, 0.5f);
        page.pivot    = new Vector2(0.5f, 0.5f);
        page.sizeDelta = viewport.rect.size;

        var le = page.GetComponent<LayoutElement>() ?? page.gameObject.AddComponent<LayoutElement>();
        le.preferredWidth  = viewport.rect.width;
        le.preferredHeight = viewport.rect.height;

        spawned.Add(page.gameObject);
        return page;
    }

    private void ConfigureGridForViewport(GridLayoutGroup grid)
    {
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.spacing = spacing;
        grid.childAlignment = TextAnchor.MiddleCenter;

        float innerW = viewport.rect.width  - padding.x - padding.z; // L, R
        float innerH = viewport.rect.height - padding.y - padding.w; // T, B

        float cellW = (innerW - spacing.x * (columns - 1)) / columns;
        float cellH = rows > 1
            ? (innerH - spacing.y * (rows - 1)) / rows
            : innerH; // 1 rząd = pełna wysokość

        grid.cellSize = new Vector2(cellW, cellH);
        grid.padding = new RectOffset(
            Mathf.RoundToInt(padding.x), // Left
            Mathf.RoundToInt(padding.z), // Right
            Mathf.RoundToInt(padding.y), // Top
            Mathf.RoundToInt(padding.w)  // Bottom
        );
    }

    private IEnumerator RebuildScrollSnapNextFrame()
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        if (scrollSnap != null)
        {
            scrollSnap.enabled = false;
            yield return null; // 1 klatka
            scrollSnap.enabled = true;

            try { if (scrollSnap.NumberOfPanels > 0) scrollSnap.GoToPanel(0); } catch { }
        }
    }

    private void ClearSpawned()
    {
        foreach (var go in spawned) if (go) Destroy(go);
        spawned.Clear();

        // Only clear content children if content is still valid
        if (content != null)
        {
            for (int i = content.childCount - 1; i >= 0; i--)
            {
                if (content.GetChild(i) != null)
                    Destroy(content.GetChild(i).gameObject);
            }
        }
    }

    // opcjonalnie: pod guziki do zakładek
    public void ShowATier() 
    { 
        if (ValidateComponents()) 
            Populate(0); 
    }
    public void ShowBTier() 
    { 
        if (ValidateComponents()) 
            Populate(1); 
    }
    public void ShowCTier() 
    { 
        if (ValidateComponents()) 
            Populate(2); 
    }
    public void ShowDTier() 
    { 
        if (ValidateComponents()) 
            Populate(3); 
    }
    public void ShowETier() 
    { 
        if (ValidateComponents()) 
            Populate(4); 
    }
}
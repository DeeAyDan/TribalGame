using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform originalParent;
    [HideInInspector] public int originalSiblingIndex;

    private Canvas rootCanvas;
    private Transform dragLayer;
    private CanvasGroup canvasGroup;
    private RectTransform rect;
    private Vector2 originalSize;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        rootCanvas = GetComponentInParent<Canvas>();

        originalSize = rect.sizeDelta;

        if (rootCanvas != null)
        {
            var layer = rootCanvas.transform.Find("DragLayer");
            dragLayer = layer != null ? layer : rootCanvas.transform;
        }
        else
        {
            dragLayer = transform.parent;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        transform.SetParent(dragLayer, true);
        canvasGroup.blocksRaycasts = false;

        canvasGroup.alpha = 0.7f;
        rect.sizeDelta = originalSize * 1.1f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        rect.sizeDelta = originalSize;

        if (transform.parent == dragLayer)
        {
            transform.SetParent(originalParent, false);
            transform.SetSiblingIndex(originalSiblingIndex);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Pickable : MonoBehaviour
{
    private SpriteRenderer myRenderer;
    private PickableData myData;

    private PolygonCollider2D myCollider;

    private bool pickableUsed;

    private void Awake()
    {
        TryGetComponent(out myRenderer);
    }

    private void Start()
    {
        GameManager.Instance.onPickableUsed += PickableUsed;
    }

    private void PickableUsed(PickableData pick)
    {
        if (pick == myData)
        {
            OnUsed();
        }
    }

    void OnMouseUpAsButton()
    {
        if (CanvasController.Instance.isCanvasOpened) return;
        OnPick();
    }

    public void OnPick()
    {
        GameManager.Instance.OnPickablePicked(myData);
    }

    public void OnUsed()
    {
        pickableUsed = true;
        Destroy(myCollider);
        myRenderer.enabled = false;
    }

    public void Show()
    {
        if (pickableUsed) return;
        myRenderer.enabled = true;
        myCollider.enabled = true;
    }

    public void Hide()
    {
        if (pickableUsed) return;
        myRenderer.enabled = false;
        myCollider.enabled = false;
    }
    
    public void NewPickable(PickableData pickableData)
    {
        myRenderer.enabled = false;
        pickableUsed = false;
        myData = pickableData;
        myRenderer.sprite = pickableData.storedSprite;
        myCollider = gameObject.AddComponent<PolygonCollider2D>();
        myCollider.isTrigger = true;
        myCollider.enabled = false;
    }
}

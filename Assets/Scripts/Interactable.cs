using UnityEngine;
[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    
    void OnMouseUpAsButton()
    {
        if (CanvasController.Instance.isCanvasOpened) return;
        OnClick();
    }

    public abstract void OnClick();
    
}

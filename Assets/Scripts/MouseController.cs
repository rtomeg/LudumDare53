using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    private Texture2D cursorTexture;
    private Physics2DRaycaster physics2DRaycaster;
    [SerializeField] private LayerMask layerMask;

    private bool isOverInteractable;

    private Camera mainCamera;
    private CanvasController canvasController;

    private void Start()
    {
        cursorTexture = (Texture2D)Resources.Load("manita");
        mainCamera = Camera.main;
        canvasController = CanvasController.Instance;
    }

    void Update()
    {
        if (canvasController.isCanvasOpened) return;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D collider = Physics2D.OverlapPoint(mousePosition, layerMask);

        if (collider != null)
        {
            if (!isOverInteractable)
            {
                Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f), CursorMode.Auto);
                isOverInteractable = true;
            }
        }
        else
        {
            if (isOverInteractable)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                isOverInteractable = false;
            }
        }
    }
}

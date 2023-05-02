public class OpenLid : Interactable
{
    private Box box;

    public void Start()
    {
        box = GetComponentInParent<Box>();
    }

    public override void OnClick()
    {
        box.CloseBox(true);
    }
}

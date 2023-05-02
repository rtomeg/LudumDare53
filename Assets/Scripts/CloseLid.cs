using UnityEngine;

public class CloseLid : Interactable
{
    private Box box;
    
    public void Start()
    {
        box = GetComponentInParent<Box>();
    }
    
    public override void OnClick()
    {
        box.OpenBox(true);
    }
}

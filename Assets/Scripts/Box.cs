using UnityEngine;

public class Box : Interactable
{
    private bool isOpen = false;

    [SerializeField] private OpenLid openedLid;
    [SerializeField] private CloseLid closedLid;

    [SerializeField] private Pickable myPickable;

    public int boxNumber;
    private PickableData pickableData;

    private void Start()
    {
        GameManager.Instance.onStageEntered += NewPickableComing;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onStageEntered -= NewPickableComing;

    }

    private void NewPickableComing(StageData stageData)
    {
        foreach (var pick in stageData.objects)
        {
            if(pick.boxNumber == boxNumber)
            {
                pickableData = pick;
                StorePickable(pick);
            }
        }
    }

    public void CloseBox(bool isUser)
    {
        if (!isOpen) return;
        openedLid.gameObject.SetActive(false);
        closedLid.gameObject.SetActive(true);
        myPickable.Hide();
        isOpen = false;
    }

    public void OpenBox(bool isUser)
    {
        if (isOpen) return;
        openedLid.gameObject.SetActive(true);
        closedLid.gameObject.SetActive(false);
        myPickable.Show();
        isOpen = true;
    }

    public void StorePickable(PickableData newPickable)
    {
        CloseBox(false);
        myPickable.NewPickable(pickableData);
    }
    
    public override void OnClick()
    {
        
    }
}

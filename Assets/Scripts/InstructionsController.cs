using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class InstructionsController : MonoBehaviour
{
    public static InstructionsController Instance { get; private set; }

    [SerializeField] private RectTransform startPosition;
    [SerializeField] private RectTransform hiddenPosition;
    [SerializeField] private RectTransform visiblePosition;

    private Tweener movementTween;

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI[] ingredients;
    [SerializeField] private TextMeshProUGUI extraInstructions;

    private StageData currentStageData;

    private void Start()
    {
        movementTween = transform.DOMove(startPosition.position, 0.5f).Pause().SetAutoKill(false);
        GameManager.Instance.onPickableUsed += OnPickableUsed;
    }

    private void OnPickableUsed(PickableData pickableData)
    {
        for (var i = 0; i < currentStageData.objects.Count; i++)
        {
            if(currentStageData.objects[i] == pickableData)
            {
                ingredients[i].SetText($"<s>{ingredients[i].text}</s>");
            }
        }
    }

    public void SnapToStartPosition(bool animate)
    {
        TweenToPosition(startPosition.position, animate);
    }
    public void SnapToHiddenPosition(bool animate)
    {
        TweenToPosition(hiddenPosition.position, animate);
    }

    public void SnapToVisiblePosition(bool animate)
    {
        TweenToPosition(visiblePosition.position, animate);
    }

    public void TweenToPosition(Vector3 position, bool animate)
    {
        
        movementTween.Pause();
        
        if (!animate)
        {
            transform.position = position;
        }
        
        else
        {
            movementTween.ChangeEndValue(position, true);
            movementTween.Restart();
            movementTween.Play();
        }
    }

    public void UpdateInstructions(StageData newStageData)
    {
        title.SetText(newStageData.title);
        extraInstructions.SetText(newStageData.instructions);

        currentStageData = newStageData;
        for (var i = 0; i < ingredients.Length; i++)
        {
            ingredients[i].SetText(newStageData.objects[i].instructionInList);
        }
    }
}

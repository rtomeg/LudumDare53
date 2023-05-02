using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public Action<StageData> onStageEntered;
    public Action<PickableData> onPickableUsed;
    public Action onPickablePicked;

    public Stage currentStage = Stage.FirstLove;
    
    [SerializeField] private Sprite gameBackground;
    [SerializeField] private SpriteRenderer backgroundRenderer;

    [SerializeField] private Transform boxesParent;
    [SerializeField] private GameObject cauldron;
    [SerializeField] private ParticleSystem bubbles;

    public List<StageData> stageDatas;

    [SerializeField] private TextMeshProUGUI narratorText;

    private string[] narratorMessages = new[]
    {
        "March 10, 1995\n\nThe Enchantment worked.\nBut it didn't last.",
        "March 11, 1995\n\nI'm sorry.\nI'm sorry.\nI'm sorry.\nI'll fix it.",
        "May 10 1995\n\nWho is this?\nIt's not you, right?",
        "He'll never have me.\nNever."
    };
    
    public enum Stage
    {
        FirstLove,
        ExBack,
        Resurrect,
        TrapDemon,
        DemonFinal
    };

    [SerializeField] private CanvasGroup fadeCanvas;

    private int currentStageUsedItems;

    private void Start()
    {
        Application.targetFrameRate = 30;
    }

    public void OnNewGameClicked()
    {
        OpenLetter();
    }

    private void OpenLetter()
    {
        CanvasController.Instance.ShowContract();
    }

    public void ContractSigned()
    {            
        CanvasController.Instance.HideContract();
        backgroundRenderer.sprite = gameBackground;
        boxesParent.position = Vector3.zero;
        cauldron.SetActive(true);
        StartStage(currentStage);
    }

    private void StartStage(Stage stage)
    {
        StageData newStageData = stageDatas.Find(x => x.stage == stage);
        onStageEntered?.Invoke(newStageData);
        
        CanvasController.Instance.UpdateInstructions(newStageData);
        CanvasController.Instance.ShowInstructions(true);
    }

    private void UpdateStage(PickableData data)
    {
        currentStageUsedItems++;

        if (currentStage == Stage.DemonFinal && currentStageUsedItems == 3)
        {
            fadeCanvas.DOFade(1f, 2f).OnComplete(() => CanvasController.Instance.ShowThanksMessage());
            return;
        }

        if (currentStageUsedItems == 3)
        {
            Sequence newLevel = DOTween.Sequence();
            
            newLevel.Append(fadeCanvas.DOFade(1f, 1f));
            newLevel.AppendCallback(() => fadeCanvas.blocksRaycasts = true);
            newLevel.AppendCallback(() => PrepareNewLevel());
            newLevel.Append(narratorText.DOText(narratorMessages[(int)currentStage],3));
            newLevel.AppendInterval(4);
            newLevel.Append(fadeCanvas.DOFade(0f, 1f));
            newLevel.AppendCallback(() => narratorText.SetText(""));
            newLevel.AppendCallback(() => fadeCanvas.blocksRaycasts = false);
            newLevel.Play();
        }
    }

    private void PrepareNewLevel()
    {
        currentStage++;
        StartStage(currentStage);
        currentStageUsedItems = 0;
    }

    public void OnPickablePicked(PickableData myData)
    {
        onPickablePicked?.Invoke();
        CanvasController.Instance.DisplayPickable(myData);
    }

    public void OnPickableUsed(PickableData displayedPickableData)
    {
        StartCoroutine(CheckSpecials(displayedPickableData));
    }

    private IEnumerator CheckSpecials(PickableData displayedPickableData)
    {
        if (displayedPickableData.name == "Alicate")
        {
            AudioManager.Instance.PlayMuela();
            CanvasController.Instance.ShowMolar();
            yield return new WaitForSeconds(3);
        }

        if (displayedPickableData.name == "Spoon")
        {
            AudioManager.Instance.PlayOjo();

            CanvasController.Instance.ShowSpoon();
            yield return new WaitForSeconds(3);
        }

        if (displayedPickableData.name == "KnifeClean")
        {
            AudioManager.Instance.PlayCuchillo();
            CanvasController.Instance.ShowFlesh();
            yield return new WaitForSeconds(3);

        }
        onPickableUsed?.Invoke(displayedPickableData);
        UpdateStage(displayedPickableData);
        bubbles.Play();
        
        yield return null;
    }
}

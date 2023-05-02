using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    [SerializeField] private PickableData displayedPickableData;
    
    [SerializeField] private Transform displayPanel;
    [SerializeField] private Image displayImage;
    [SerializeField] private RectTransform readLetterPanel;
    [SerializeField] private InstructionsController readInstructionsPanel;
    [SerializeField] private float contractLetterAnim = 1f;

    [SerializeField] private GameObject startGamePanel;
    private bool instructionsAreVisible = false;
    
    [SerializeField] private GameObject thanksMessage;

    [SerializeField] private Sprite molar;
    [SerializeField] private Sprite eyeBall;
    [SerializeField] private Sprite bloodyKnife;

    [SerializeField] private Image specialImage;
    [SerializeField] private CanvasGroup specialCanvas;
    
    public bool isCanvasOpened
    {
        get;
        private set;
    } = true;

    private void Start()
    {
        readLetterPanel.anchoredPosition = new Vector2(0, -Screen.height);
    }

    public void DisplayPickable(PickableData pickabledata)
    {
        isCanvasOpened = true;
        displayedPickableData = pickabledata;
        displayImage.sprite = pickabledata.displaySprite;
        displayPanel.gameObject.SetActive(true);
    }

    public void UsePickable()
    {
        CloseDisplayPanel();
        isCanvasOpened = false;
        GameManager.Instance.OnPickableUsed(displayedPickableData);
    }

    public void CloseDisplayPanel()
    {
        isCanvasOpened = false;
        displayPanel.gameObject.SetActive(false);
    }

    public void ShowContract()
    {
        isCanvasOpened = true;
        readLetterPanel.DOMoveY(Screen.height/2, contractLetterAnim);
    }

    public void HideContract()
    {
        isCanvasOpened = false;
        readLetterPanel.DOMoveY(-Screen.height, contractLetterAnim);
        startGamePanel.SetActive(false);
    }

    public void HideInstructionsCompletely(bool animate)
    {
        readInstructionsPanel.SnapToStartPosition(animate);
    }

    public void ShowInstructions(bool animate)
    {
        isCanvasOpened = true;
        readInstructionsPanel.SnapToVisiblePosition(animate);
        instructionsAreVisible = true;
    }

    public void HideInstructions(bool animate)
    {
        instructionsAreVisible = false;
        isCanvasOpened = false;
        readInstructionsPanel.SnapToHiddenPosition(animate);
    }

    public void OnInstructionsClick()
    {
        if (instructionsAreVisible)
        {
            HideInstructions(true);
        }
        else
        {
            ShowInstructions(true);
        }
    }

    public void UpdateInstructions(StageData currentStageData)
    {
        readInstructionsPanel.UpdateInstructions(currentStageData);
    }

    public void ShowThanksMessage()
    {
        thanksMessage.SetActive(true);
        instructionsAreVisible = true;
    }

    public void ShowMolar()
    {
        specialImage.sprite = molar;
        ShowSpecialImage();
    }

    private void ShowSpecialImage()
    {
        Sequence mySequence = DOTween.Sequence()
            .AppendCallback(() => isCanvasOpened = true)
            .AppendCallback(() => specialCanvas.blocksRaycasts = true)
            .Append(specialCanvas.DOFade(1, 0.3f))
            .Append(specialImage.DOColor(Color.red, 0.3f))
            .Append(specialImage.DOColor(Color.white, 0.3f))
            .AppendInterval(2)
            .Append(specialCanvas.DOFade(0, 1))
            .AppendCallback(() => specialCanvas.blocksRaycasts = false)
            .AppendCallback(() => isCanvasOpened = false);
        mySequence.Play();
    }

    public void ShowSpoon()
    {
        specialImage.sprite = eyeBall;
        ShowSpecialImage();
    }

    public void ShowFlesh()
    {
        specialImage.sprite = bloodyKnife;
        ShowSpecialImage();
    }
}

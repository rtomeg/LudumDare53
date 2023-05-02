using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] randomNoises;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip caldero;
    [SerializeField] private AudioClip cuchillo;
    [SerializeField] private AudioClip ojo;
    [SerializeField] private AudioClip muela;
    [SerializeField] private AudioClip tachado;
    [SerializeField] private AudioClip caja;

    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        TryGetComponent(out audioSource);

        GameManager.Instance.onPickableUsed += PlayCaldero;
        GameManager.Instance.onPickablePicked += PlayCaja;
        
    }

    public void PlayCuchillo()
    {
        audioSource.PlayOneShot(cuchillo);
    }

    public void PlayOjo()
    {
        audioSource.PlayOneShot(ojo);

    }

    public void PlayMuela()
    {
        audioSource.PlayOneShot(muela);

    }

    private void OnDestroy()
    {
        GameManager.Instance.onPickableUsed -= PlayCaldero;
        GameManager.Instance.onPickablePicked -= PlayCaja;
    }

    private void PlayCaldero(PickableData pickableData)
    {
        audioSource.PlayOneShot(caldero);
        audioSource.PlayOneShot(tachado);
    }

    private void PlayCaja()
    {
        audioSource.PlayOneShot(caja);
    }
}

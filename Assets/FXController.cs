using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FXController : MonoBehaviour
{
    private VolumeProfile volumeProfile;

    private ChromaticAberration chromaticAberration;
    private FilmGrain filmGrain;
    private LensDistortion lensDistortion;
    private Vignette vignette;

    [SerializeField] private AudioSource whiteNoise;

    [SerializeField] private int scaryLevel;

    private Ease[] easesThatLookCool = new[]
    {
        Ease.InOutElastic,
        Ease.InOutBounce,
        Ease.InElastic,
        Ease.OutElastic,
    };

    private Tweener HeartBeatTweet;

    [SerializeField] private Sprite isaacNormal;
    [SerializeField] private Sprite isaacCreepy;
    [SerializeField] private Sprite colgajoNormal;
    [SerializeField] private Sprite colgajoCreepy;
    [SerializeField] private SpriteRenderer isaacRenderer;
    [SerializeField] private SpriteRenderer colgajoRenderer;

    private void Start()
    {

        GameManager.Instance.onStageEntered += OnStageEntered;
        Volume volume;
        if (TryGetComponent(out volume))
        {
            volumeProfile = volume.profile;
        }
        else
        {
            Debug.Log("Can't find volumeprofile");
        }

        volumeProfile.TryGet(out chromaticAberration);
        volumeProfile.TryGet(out filmGrain);
        volumeProfile.TryGet(out lensDistortion);
        volumeProfile.TryGet(out vignette);

        HeartBeatTweet = DOTween.To(() => lensDistortion.intensity.value, x => lensDistortion.intensity.value = x,
            0f, 2).SetEase(Ease.InElastic).SetLoops(-1, LoopType.Yoyo).SetAutoKill(false).Pause();

        StartCoroutine(RandomScaryMoments());
        StartCoroutine(RandomSwaps());
    }

    private void OnStageEntered(StageData obj)
    {
        scaryLevel++;
        HeartBeat(scaryLevel);
        BaseFilmGrain(scaryLevel);
        BaseVignette(scaryLevel);
    }
    
    
    private IEnumerator RandomSwaps()
    {
        while (true)
        {
            if (scaryLevel < 5)
            {
                if (Random.value <= scaryLevel / 5f)
                {
                    StartCoroutine(SwapColgajo());
                }
                
                yield return new WaitForSeconds(Random.Range(5, 15));
                
                if (Random.value <= scaryLevel / 5f)
                {
                    StartCoroutine(SwapIsaac());
                }
                
                yield return new WaitForSeconds(Random.Range(5, 15));
            }
            else
            {
                yield break;
            }
        }
    }
    
    private IEnumerator RandomScaryMoments()
    {
        while (true)
        {
            if (scaryLevel < 4)
            {
                if (Random.value <= scaryLevel / 5f)
                {
                    ChromaticAberrationBurst(Random.Range(0.2f, 2f));
                }
                
                yield return new WaitForSeconds(Random.Range(5, 15));
                
                if (Random.value <= scaryLevel / 5f)
                {
                    float time = Random.Range(0.2f, 2f);
                    FilmGrainBurst(time);
                }
                
                yield return new WaitForSeconds(Random.Range(5, 15));
            }
            else
            {
                yield break;
            }
        }
    }

    private void BaseVignette(int intensity)
    {
        if (intensity >= 5)
        {
            vignette.intensity.value = 0f;
            return;
        }
        vignette.intensity.value = Mathf.Lerp(0, 0.4f, intensity / 4f);
    }
    
    private void HeartBeat(int intensity)
    {
        lensDistortion.intensity.value = 0f;
        if (intensity >= 5)
        {
            HeartBeatTweet.Pause();
            return;
        }
        HeartBeatTweet.ChangeEndValue(Mathf.Lerp(0, 0.1f, intensity/4f));
        HeartBeatTweet.Restart();
    }

    private void BaseFilmGrain(int intensity)
    {
        if (intensity >= 5)
        {
            filmGrain.intensity.value = 0f;
            return;
        }

        filmGrain.intensity.value = Mathf.Lerp(0, 0.5f, intensity / 4f);
    }

    private void ChromaticAberrationBurst(float duration = 2f, float intensity = 1f)
    {
        DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x,
            intensity, duration).SetEase(easesThatLookCool[Random.Range(0, easesThatLookCool.Length)]).SetLoops(2, LoopType.Yoyo);
    }
    
    private void ChromaticAberrationFlash(float duration = 0.5f, float intensity = 1f)
    {
        DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x,
            intensity, duration).SetEase(Ease.InElastic).SetLoops(2, LoopType.Yoyo);
    }

    private IEnumerator SwapIsaac()
    {
        isaacRenderer.sprite = isaacCreepy;
        yield return new WaitForSeconds(0.2f * scaryLevel);
        isaacRenderer.sprite = isaacNormal;
    }

    private IEnumerator SwapColgajo()
    {
        colgajoRenderer.sprite = colgajoCreepy;
        yield return new WaitForSeconds(0.2f * scaryLevel);
        colgajoRenderer.sprite = colgajoNormal;

    }

    private void FilmGrainBurst(float duration = 2f, float intensity = 1)
    {
        whiteNoise.Play();
        DOTween.To(() => filmGrain.intensity.value, x => filmGrain.intensity.value = x,
            intensity, duration).SetEase(easesThatLookCool[Random.Range(0, easesThatLookCool.Length)]).SetLoops(2, LoopType.Yoyo).OnComplete(whiteNoise.Pause);
    }
    
    private void FilmGrainFlash(float duration = 0.5f, float intensity = 1)
    {
        DOTween.To(() => filmGrain.intensity.value, x => filmGrain.intensity.value = x,
            intensity, duration).SetEase(Ease.OutElastic).SetLoops(2, LoopType.Yoyo);
    }
    
    #if UNITY_EDITOR
    
    #endif
}

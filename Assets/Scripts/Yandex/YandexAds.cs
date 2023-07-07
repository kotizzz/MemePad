using Agava.YandexGames;
using System;
using UnityEngine;

public class YandexAds : MonoBehaviour
{
    public static YandexAds Instance;

    private bool _isRewarded = false;
    public bool IsRewarded => _isRewarded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowInterstitial()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        InterstitialAd.Show(OnAdOpen, OnIterstitialAddClose);
#endif
    }

    public void ShowRewardAd(Action onRewardedCallback = null)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        VideoAd.Show(OnAdOpen, OnAdRewarded, OnAdClose);
#endif
    }

    public void OnAdOpen()
    {
        YandexSDK.Instance.IsAdRunning = true;
        Time.timeScale = 0;
        AudioListener.volume = 0;
    }

    public void OnAdClose()
    {
        YandexSDK.Instance.IsAdRunning = false;
        Time.timeScale = 1;
        AudioListener.volume = 1;
        _isRewarded = false;
    }

    public void OnAdRewarded()
    {
        _isRewarded = true;
    }

    public void OnIterstitialAddClose(bool value)
    {
        Time.timeScale = 1;
        AudioListener.volume = 1;
    }
}

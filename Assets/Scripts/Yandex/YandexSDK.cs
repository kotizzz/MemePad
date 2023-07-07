using System;
using System.Collections;
using System.Collections.Generic;
using Agava.WebUtility;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YandexSDK : MonoBehaviour
{
    private LevelLoader _levelLoader;
    private string _language;
    public bool IsAdRunning;

    public static YandexSDK Instance;

    public string CurrentLanguage => _language;

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

        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        _levelLoader = FindObjectOfType<LevelLoader>();

        _levelLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
#else
        yield return YandexGamesSdk.Initialize();

        _language = YandexGamesSdk.Environment.i18n.lang;

        InterstitialAd.Show(null, (bool _) => StartGame());
#endif
    }

    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
    }

    private void OnInBackgroundChange(bool inBackground)
    {
        if (!IsAdRunning)
            MuteAudio(inBackground);
    }

    private void MuteAudio(bool value)
    {
        Time.timeScale = value ? 0f : 1f;
        AudioListener.pause = value;
        AudioListener.volume = value ? 0f : 1f;
    }

    private void StartGame()
    {
        if (YandexGamesSdk.IsInitialized)
        {
            _levelLoader = FindObjectOfType<LevelLoader>();

            _levelLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
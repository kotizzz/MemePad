using Agava.YandexGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsButton : MonoBehaviour
{
    [SerializeField] private Image _adsImage;
    [SerializeField] private string _soundName;
    [SerializeField] private AdsButtonData _adsButtonData = new AdsButtonData();
    [SerializeField] private string _saveKey;

    private Button _adsButton;
    private int _clickCounter;
    private bool _isActive;

    private void Awake()
    {
        _adsButton = GetComponent<Button>();
        _adsButton.onClick.AddListener(ShowRewardedAdd);
        _adsButton.onClick.AddListener(PlaySound);
    }

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (YandexGamesSdk.IsInitialized)
        {
            StartCoroutine(GetData());
        }
#endif
        LoadButtonData();

        if (_adsButtonData == null)
        {
            _adsButtonData = new AdsButtonData();
        }

        if (_adsButtonData.trueAd == 0)
        {
            _adsButtonData.IsActive = true;
            SaveButtonData();
        }

        _isActive = _adsButtonData.IsActive;

        if(_isActive == true)
        {
            _adsImage.gameObject.SetActive(true);
            _clickCounter = 0;
        }
        else
        {
            _adsImage.gameObject.SetActive(false);
            _clickCounter = -1;
        }
    }

    /*private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            _adsButtonData = new AdsButtonData();

            SaveManager.Reset(_saveKey, _adsButtonData);
            SaveButtonData();
            //SaveYandex();
        }
    }*/

    private void OnDestroy()
    {
        _adsButton.onClick.RemoveListener(ShowRewardedAdd);
        _adsButton.onClick.RemoveListener(PlaySound);
    }

    private void ShowRewardedAdd()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if(_isActive == true)
        {
            _isActive = false;
            _adsImage.gameObject.SetActive(false);
            _adsButtonData.IsActive = _isActive;

            if (_adsButtonData.trueAd <= 0)
            {
                _adsButtonData.trueAd++;
            }

            SaveButtonData();
            //SaveYandex();
        }
#else
        if (_isActive == true)
        {
            YandexAds.Instance.ShowRewardAd();
            StartCoroutine(CheckRewarded());
        }
#endif
    }

    public void PlaySound()
    {
        if(_isActive == false && _clickCounter < 0)
        {
            AudioPlayer.Instance.PlaySound(_soundName);
            Debug.Log("музыка");
        }
        else
        {
            _clickCounter--;
        }
    }

    private IEnumerator CheckRewarded()
    {
        while (YandexAds.Instance.IsRewarded == false)
        {
            yield return null;
        }

        _isActive = false;
        _adsImage.gameObject.SetActive(false);
        _adsButtonData.IsActive = _isActive;

        if(_adsButtonData.trueAd <= 0)
        {
            _adsButtonData.trueAd++;
        }

        SaveButtonData();
        //SaveYandex();
    }

    private void OnDisable()
    {
        SaveButtonData();
        //SaveYandex();
    }

    public void LoadButtonData()
    {
        var data = SaveManager.Load<AdsButtonData>(_saveKey);
        _adsButtonData = data;
        Debug.Log("Я загрузил");
    }

    public void SaveButtonData()
    {
        SaveManager.Save(_saveKey, _adsButtonData);
        Debug.Log("Я сохранил");
    }

    /*public void SaveYandex()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        string jsonDataString = JsonUtility.ToJson(_adsButtonData, true);

        if (PlayerAccount.IsAuthorized)
            PlayerAccount.SetCloudSaveData(jsonDataString);
#endif
    }*/

    private IEnumerator GetData()
    {
        if (PlayerAccount.IsAuthorized)
        {
            PlayerAccount.RequestPersonalProfileDataPermission();

            string loadedString = "None";

            PlayerAccount.GetCloudSaveData((data) =>
            {
                loadedString = data;
            });

            while (loadedString == "None")
            {
                yield return null;
            }

            if (loadedString == "{}")
            {
                yield break;
            }

            _adsButtonData = JsonUtility.FromJson<AdsButtonData>(loadedString);
            SaveButtonData();
        }
    }
}

[Serializable]
public class AdsButtonData
{
    public int trueAd;
    public bool IsActive;
}

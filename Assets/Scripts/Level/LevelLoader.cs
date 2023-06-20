using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    public string _sceneName;

    private void Start() {
        LoadLevel(_sceneName);
    }

    public Slider slider;
    public TMP_Text progressText;

    public void LoadLevel (string sceneName) {
        StartCoroutine(LoadAsyncronously(sceneName));
    }

    IEnumerator LoadAsyncronously (string sceneName) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (operation.isDone == false){
            float progress = Mathf.Clamp01(operation.progress / .9f);
            
            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
}

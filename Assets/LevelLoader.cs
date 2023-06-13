using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string _sceneName;

    private void Start() {
        LoadLevel(_sceneName);
    }

    public void LoadLevel (string sceneName) {
        StartCoroutine(LoadAsyncronously(sceneName));
    }

    IEnumerator LoadAsyncronously (string sceneName) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (operation.isDone == false){
            Debug.Log(operation.progress);

            yield return null;
        }
    }
}

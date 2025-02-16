using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class AsyncLoadManager : MonoSingleton<AsyncLoadManager>
{
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Slider progressSlider;
    [SerializeField] float cacheSceneTime = 3f;


    IEnumerator LoadScene(string screenName)
    {
        yield return new WaitForSeconds(0.3f);
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(screenName);
        while(!loadScene.isDone)
        {
        
            float sliderValue = loadScene.progress / 0.9f;

            sliderValue = Mathf.Clamp01(sliderValue);

            progressSlider.value = sliderValue;

            yield return null;
        }

        yield return new WaitForSeconds(cacheSceneTime);
        loadingPanel.SetActive(false);
    }

    public void ListenToLoadSceneEvent(Component sender,object data)
    {
        if(data is string)
        {
           LoadSceneAsync((string)data);
        }
    }
    public void LoadSceneAsync(string sceneName)
    {
        loadingPanel.SetActive(true);
        StartCoroutine(LoadScene(sceneName));
    }
}

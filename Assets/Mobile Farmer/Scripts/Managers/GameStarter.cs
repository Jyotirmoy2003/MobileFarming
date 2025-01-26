using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    void Start()
    {
        LeanTween.delayedCall(1f,()=>LoadScene());
    }

    void LoadScene()
    {
        // if(PlayerPrefs.GetInt("Tutorial")>0)
        // {
        //     AsyncLoadManager.Instance.LoadSceneAsync("Main");
        // }else{
        //     AsyncLoadManager.Instance.LoadSceneAsync("Tutorial");
        // }
        AsyncLoadManager.Instance.LoadSceneAsync("Tutorial");
    }
}

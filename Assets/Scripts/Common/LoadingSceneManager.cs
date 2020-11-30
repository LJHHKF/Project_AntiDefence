using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    private GameObject gm;
    private LoadingManager loadingM;

    private Transform t_LoadingBar;
    private Image img_loadingbar;
    private Text txt_loadingTxt;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        loadingM = gm.GetComponent<LoadingManager>();

        t_LoadingBar = gameObject.transform.Find("LoadingPanel").Find("LoadingBarBG").Find("LoadingBar");
        img_loadingbar = t_LoadingBar.gameObject.GetComponent<Image>();
        txt_loadingTxt = t_LoadingBar.Find("Text").GetComponent<Text>();

        txt_loadingTxt.text = loadingM.GetLoadingString();

        //loadingM.SetLoadingImage(gameObject.GetComponent<Image>());
        //gameObject.transform.Find("Text").GetComponent<Text>().text = loadingM.GetLoadingString();

        StartCoroutine(LoadAsyncScene(loadingM.GetLoadingSceneName()));
    }

    IEnumerator LoadAsyncScene(string target)
    {
        yield return null;
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(target);
        asyncScene.allowSceneActivation = false;
        float timeCnt = 0;
        while (!asyncScene.isDone)
        {
            yield return null;
            timeCnt += Time.deltaTime;
            if (asyncScene.progress >= 0.9f)
            {
                img_loadingbar.fillAmount = Mathf.Lerp(img_loadingbar.fillAmount, 1, timeCnt);
                if (img_loadingbar.fillAmount >= 1.0f)
                {
                    asyncScene.allowSceneActivation = true;
                }
            }
            else
            {
                img_loadingbar.fillAmount = Mathf.Lerp(img_loadingbar.fillAmount, asyncScene.progress, timeCnt);
                if (img_loadingbar.fillAmount >= asyncScene.progress)
                {
                    timeCnt = 0f;
                }
            }
        }
        StopCoroutine(LoadAsyncScene(target));
    }
}

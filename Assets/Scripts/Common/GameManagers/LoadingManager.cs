using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    //public GameObject loadingBar_prefab;

    [HideInInspector]
    public Image img_loadingbar;

    private Image img_loadingbar_firstScene;

    private string prev_Scene;
    private string selectedStage;
    private string selectedChapter;
    private int chapter_num = -1;

    private bool had_prev = false;

    public void SetLoadingImage(Image img)
    {
        img_loadingbar = img;
    }

    public bool GetHadPrev()
    {
        return had_prev;
    }

    public void LoadSceneGMCallLoading()
    {
        SceneManager.LoadScene("GMCallLoading");
    }

    public void SetSelectedStage(string stage_number, int chapter)
    {
        selectedStage = "Stage" + stage_number; //new StringBuilder("Stage");
        //selectedStage.Append(stage_number);
        chapter_num = chapter;
    }

    public void StageEnd(int chp_num)
    {
        chapter_num = chp_num;
        if (chapter_num >= 0)
        {
            if (chapter_num < 10)
            {
                selectedChapter = "Chp0" + chapter_num.ToString();
                LoadScene(selectedChapter + "_StageSelect");
            }
            //if (chapter_num == 0)
            //{
            //    LoadScene("Chp00_StageSelect");
            //}
        }
    }

    public void StageEnd()
    {
        if (chapter_num >= 0)
        {
            if (chapter_num < 10)
            {
                selectedChapter = "Chp0" + chapter_num.ToString();
                LoadScene(selectedChapter + "_StageSelect");
            }
            //if (chapter_num == 0)
            //{
            //    LoadScene("Chp00_StageSelect");
            //}
        }
    }

    public void FirstSceneLoad(string name)
    {
        img_loadingbar_firstScene = GameObject.FindGameObjectWithTag("LoadingBar").GetComponent<Image>();
        img_loadingbar_firstScene.fillAmount = 0.0f;
        StartCoroutine(LoadAsyncScene_First(name));
    }

    public void LoadScene(string name)
    {
        //if (name == "Lobby")
        //{
        //    CallLoadingScene("Lobby");
        //}
        //else if (name == "ItemEquip")
        //{
        //    CallLoadingScene("ItemEquip");
        //}
        had_prev = false;
        prev_Scene = null;
        CallLoadingScene(name);
    }

    public void LoadScene(string name, string prev)
    {
        //if (name == "Lobby")
        //{
        //    prev_Scene = prev;
        //    CallLoadingScene("Lobby");
        //}
        //else if (name == "ItemEquip")
        //{
        //    prev_Scene = prev;
        //    CallLoadingScene("ItemEquip");
        //}
        prev_Scene = prev;
        had_prev = true;
        CallLoadingScene(name);
    }

    public void LoadPrevScene()
    {
        CallLoadingScene(prev_Scene);
        had_prev = false;
        prev_Scene = null;
    }

    public void LoadStage()
    {
        CallLoadingScene(selectedStage);
    }

    public void ReturnScene()
    {
        if (prev_Scene != null)
        {
            CallLoadingScene(prev_Scene);
        }
    }

    private void CallLoadingScene(string target)
    {
        SceneManager.LoadScene("Loading");
        
        StartCoroutine(LoadAsyncScene(target));
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

    IEnumerator LoadAsyncScene_First(string target)
    {
        yield return null;
        img_loadingbar.fillAmount = 0.0f;
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(target);
        asyncScene.allowSceneActivation = false;
        float timeCnt = 0;
        while (!asyncScene.isDone)
        {
            yield return null;
            timeCnt += Time.deltaTime;
            if (asyncScene.progress >= 0.9f)
            {
                img_loadingbar_firstScene.fillAmount = Mathf.Lerp(img_loadingbar_firstScene.fillAmount, 1, timeCnt);
                if (img_loadingbar_firstScene.fillAmount >= 1.0f)
                {
                    asyncScene.allowSceneActivation = true;
                }
            }
            else
            {
                img_loadingbar_firstScene.fillAmount = Mathf.Lerp(img_loadingbar_firstScene.fillAmount, asyncScene.progress, timeCnt);
                if (img_loadingbar.fillAmount >= asyncScene.progress)
                {
                    timeCnt = 0f;
                }
            }
        }
        img_loadingbar = null;
        StopCoroutine(LoadAsyncScene(target));
    }
}

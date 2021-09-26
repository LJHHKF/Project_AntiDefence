using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    //private Image img_loadingbar;
    private string loadingString;
    private bool is_stageEnd = false;

    private string prev_Scene;
    private string selectedStage;
    private string selectedChapter;
    private string loadingSceneName;
    private int chapter_num = -1;

    private bool had_prev = false;

    private bool is_loaded = false;
    private bool is_event_done = false;
    private bool is_looped = true;

    //public void SetLoadingImage(Image img)
    //{
    //    img_loadingbar = img;
    //}

    public void SetLoadingString(string str)
    {
        loadingString = str;
    }

    public string GetLoadingString()
    {
        return loadingString;
    }

    public bool GetHadPrev()
    {
        return had_prev;
    }

    public bool GetIsLoaded()
    {
        return is_loaded;
    }

    public void SetEventDone()
    {
        is_event_done = true;
    }

    public string GetSelectedStage()
    {
        return selectedStage;
    }

    public string GetLoadingSceneName()
    {
        return loadingSceneName;
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
                is_stageEnd = true;
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
        if(!is_stageEnd)
        {
            loadingString = "Now Loading...";
        }
        is_stageEnd = false;
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

    private void CallLoadingScene(string target)
    {
        loadingSceneName = target;
        SceneManager.LoadScene("Loading");
        
        //StartCoroutine(LoadAsyncScene(target));
    }


    //IEnumerator LoadAsyncScene(string target)
    //{
    //    yield return null;
    //    AsyncOperation asyncScene = SceneManager.LoadSceneAsync(target);
    //    asyncScene.allowSceneActivation = false;
    //    float timeCnt = 0;
    //    while (!asyncScene.isDone)
    //    {
    //        yield return null;
    //        timeCnt += Time.deltaTime;
    //        if (asyncScene.progress >= 0.9f)
    //        {
    //            img_loadingbar.fillAmount = Mathf.Lerp(img_loadingbar.fillAmount, 1, timeCnt);
    //            if (img_loadingbar.fillAmount >= 1.0f)
    //            {
    //                asyncScene.allowSceneActivation = true;
    //            }
    //        }
    //        else
    //        {
    //            img_loadingbar.fillAmount = Mathf.Lerp(img_loadingbar.fillAmount, asyncScene.progress, timeCnt);
    //            if (img_loadingbar.fillAmount >= asyncScene.progress)
    //            {
    //                timeCnt = 0f;
    //            }
    //        }
    //    }
    //    StopCoroutine(LoadAsyncScene(target));
    //}

    IEnumerator LoadAsyncScene_First(string target)
    {
        yield return null;
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(target);
        asyncScene.allowSceneActivation = false;
        while (is_looped)
        {
            yield return null;
            if (asyncScene.progress >= 0.9f)
            {
                is_loaded = true;
                if (is_event_done)
                {
                    asyncScene.allowSceneActivation = true;
                    is_looped = false;
                    break;
                }
            }
        }
        //img_loadingbar = null;
        is_loaded = false;
        is_event_done = false;
        is_looped = true;
        StopCoroutine(LoadAsyncScene_First(target));
    }
}

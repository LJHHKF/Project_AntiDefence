using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelect : MonoBehaviour
{

    private GameObject gm;
    private LoadingManager loadingM;
    private BGM_Manager bgmM;
    private AudioManager audioM;


    private Camera main_camera;
    public float delay_Scene_Change = 1.0f;
    public float cameraMoveSpeed = 0.1f;
    private bool btn_clicked = false;
    private Image img_FadeOut;
    public float fadeSpeed = 1.0f;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        loadingM = gm.GetComponent<LoadingManager>();
        bgmM = gm.GetComponent<BGM_Manager>();
        audioM = gm.GetComponent<AudioManager>();
        

        main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        img_FadeOut = gameObject.transform.Find("FadeOut").GetComponent<Image>();


        //tr_BTN_Space = gameObject.transform.Find("BTN_Space");
        //tr_BTN_Ready = gameObject.transform.Find("BTN_Ready");

        //for (int i = 0; i < arr_btns.Length; i++)
        //{
        //    arr_btns[i].SetActive(false);
        //    arr_btns[i].transform.SetParent(tr_BTN_Ready);
        //}
        //index_btns = 0;
        //arr_btns[index_btns].transform.SetParent(tr_BTN_Space);
        //arr_btns[index_btns].SetActive(true);
        

        bgmM.Play_LobbyAndShop();
    }

    private void Update()
    {

        
        if (btn_clicked)
        {
            main_camera.fieldOfView -= cameraMoveSpeed;
            img_FadeOut.color = new Color(img_FadeOut.color.r, img_FadeOut.color.g, img_FadeOut.color.b, img_FadeOut.color.a + fadeSpeed);
        }
    }

    public void BTN_Chp0()
    {
        if (!btn_clicked)
        {
            audioM.SFX_BTN_Click();
            btn_clicked = true;
            StartCoroutine(SetLoadScene(0));
        }
    }

    public void BTN_Return()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("Lobby");
    }

    

    IEnumerator SetLoadScene(int chpter_num)
    {
        string str = "Chp" + chpter_num.ToString("00") + "_StageSelect";
        yield return new WaitForSeconds(delay_Scene_Change);
        loadingM.LoadScene(str);

    }
}

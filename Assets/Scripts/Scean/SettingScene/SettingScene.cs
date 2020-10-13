using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScene : MonoBehaviour
{

    private GameObject gm;
    private AudioManager audioM;
    private LoadingManager loadingM;
    private BGM_Manager bgmM;

    private Slider sld_bg;
    private Slider sld_se;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        audioM = gm.GetComponent<AudioManager>();
        loadingM = gm.GetComponent<LoadingManager>();
        bgmM = gm.GetComponent<BGM_Manager>();

        sld_bg = gameObject.transform.Find("Panel_BG_Setting").Find("Slider").GetComponent<Slider>();
        sld_se = gameObject.transform.Find("Panel_SE_Setting").Find("Slider").GetComponent<Slider>();

        sld_bg.value = audioM.GetBgVolume();
        sld_se.value = audioM.GetSeVolume();

        bgmM.Play_LobbyAndShop();
    }

    public void SetBGValue()
    {
        audioM.SetBgVolume(sld_bg.value);
    }

    public void SetSEValue()
    {
        audioM.SetSeVolume(sld_se.value);
    }

    public void Click_BTN_Return()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadPrevScene();
    }
}

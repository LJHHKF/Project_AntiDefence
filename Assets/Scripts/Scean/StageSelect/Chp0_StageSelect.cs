using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chp0_StageSelect : MonoBehaviour
{
    private GameObject gm;
    private LoadingManager loadingM;
    private BGM_Manager bgmM;
    private AudioManager audioM;
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        loadingM = gm.GetComponent<LoadingManager>();
        bgmM = gm.GetComponent<BGM_Manager>();
        audioM = gm.GetComponent<AudioManager>();

        bgmM.Play_LobbyAndShop();
    }

    public void BTN_Stage1()
    {
        audioM.SFX_BTN_Click();
        loadingM.SetSelectedStage("0-1", 0);
        loadingM.LoadScene("ItemEquip");
    }

    public void BTN_Stage2()
    {
        audioM.SFX_BTN_Click();
        loadingM.SetSelectedStage("0-2", 0);
        loadingM.LoadScene("ItemEquip");
    }

    public void BTN_Return()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("ChapterSelect");
    }
}

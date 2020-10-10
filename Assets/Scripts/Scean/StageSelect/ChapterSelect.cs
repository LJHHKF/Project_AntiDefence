using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSelect : MonoBehaviour
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

    public void BTN_Chp0()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("Chp00_StageSelect");
    }

    public void BTN_Return()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("Lobby");
    }
}

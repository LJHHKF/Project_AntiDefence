using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSelect : MonoBehaviour
{

    private GameObject gm;
    private LoadingManager loadingM;
    private BGM_Manager bgmM;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        loadingM = gm.GetComponent<LoadingManager>();
        bgmM = gm.GetComponent<BGM_Manager>();
        
        bgmM.Play_LobbyAndShop();
    }

    public void BTN_Chp0()
    {
        loadingM.LoadScene("Chp00_StageSelect");
    }

    public void BTN_Return()
    {
        loadingM.LoadScene("Lobby");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Manager : MonoBehaviour
{
    private GameObject gm;
    private AudioManager audioM;

    private float bgVolume;

    private AudioSource m_bgm;

    public AudioClip[] bgm_list;
    private int cur_index = -1;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        audioM = gm.GetComponent<AudioManager>();

        //audioM.SetBgmManager(gameObject);
        bgVolume = audioM.GetBgVolume();

        
        m_bgm = gameObject.transform.Find("BGM").GetComponent<AudioSource>();
        m_bgm.volume = 1.0f * bgVolume;
    }

    private void SetBgm(int index)
    {
        m_bgm.clip = bgm_list[index];
        m_bgm.Play();
    }

    public void ValueChange()
    {
        bgVolume = audioM.GetBgVolume();
        m_bgm.volume = 1.0f * bgVolume;
    }

    public void Play_LobbyAndShop()
    {
        if (cur_index != 0)
        {
            cur_index = 0;
            SetBgm(0);
        }
    }

    public void Play_Stage()
    {
        if (cur_index != 1)
        {
            cur_index = 1;
            SetBgm(1);
        }
    }
}

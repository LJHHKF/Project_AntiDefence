﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private float bg_volume;
    private float se_volume;
    private BGM_Manager bgm_manager;

    private AudioSource sfx_BTN_Click;

    private void Start()
    {
        bg_volume = PlayerPrefs.GetFloat("BG_Volume", 0.5f);
        se_volume = PlayerPrefs.GetFloat("SE_Volume", 0.5f);
        bgm_manager = gameObject.GetComponent<BGM_Manager>();

        sfx_BTN_Click = gameObject.transform.Find("SFX_NonStage").Find("S_BTN_Click").GetComponent<AudioSource>();
        SetBTNsfxVolume();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("BG_Volume", bg_volume);
        PlayerPrefs.SetFloat("SE_Volume", se_volume);
    }

    private void SetBTNsfxVolume()
    {
        sfx_BTN_Click.volume = se_volume;
    }

    private IEnumerator Sound_BTN_Click()
    {
        sfx_BTN_Click.Play();
        yield return new WaitForSeconds(0.5f);
        sfx_BTN_Click.Stop();
        yield break;
    }

    public void SFX_BTN_Click()
    {
        StartCoroutine(Sound_BTN_Click());
    }

    public void SetBgVolume(float value)
    {
        bg_volume = value;
        bgm_manager.ValueChange();
    }

    public void SetSeVolume(float value)
    {
        se_volume = value;
        SetBTNsfxVolume();
    }

    //public void SetBgmManager(GameObject bgm)
    //{
    //    bgm_manager = bgm.GetComponent<BGM_Manager>();
    //}

    public float GetBgVolume()
    {
        return bg_volume;
    }

    public float GetSeVolume()
    {
        return se_volume;
    }

    
}

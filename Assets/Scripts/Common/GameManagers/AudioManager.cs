using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private float bg_volume;
    private float se_volume;
    private BGM_Manager bgm_manager;

    private void Start()
    {
        bg_volume = PlayerPrefs.GetFloat("BG_Volumn", 0.5f);
        se_volume = PlayerPrefs.GetFloat("SE_Volumn", 0.5f);
        bgm_manager = gameObject.GetComponent<BGM_Manager>();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("BG_Volumn", bg_volume);
        PlayerPrefs.SetFloat("SE_Volumn", se_volume);
    }

    public void SetBgVolume(float value)
    {
        bg_volume = value;
        bgm_manager.ValueChange();
    }

    public void SetSeVolume(float value)
    {
        se_volume = value;
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

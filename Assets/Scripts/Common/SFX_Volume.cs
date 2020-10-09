using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Volume : MonoBehaviour
{
    private AudioSource[] m_sfxs;
    private AudioManager audioM;
    private float sfx_volume;
    void Start()
    {
        audioM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();
        m_sfxs = gameObject.GetComponentsInChildren<AudioSource>();

        sfx_volume = audioM.GetSeVolume();

        for (int i = 0; i < m_sfxs.Length; i++)
        {
            m_sfxs[i].volume = m_sfxs[i].volume * sfx_volume;
        }
    }

    //(추후 작업용)
    //만일 스테이지 내부에서도 사운드 조절 등의 기능이 추가될 경우, 현재 스타트 한정인 볼륨 조절 및 사운드 볼륨 조절을 Update함수로도 확장할 것.
}

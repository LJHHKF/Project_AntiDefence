using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCharacterEvent : MonoBehaviour
{
    private GameObject m_Character;
    private GameObject m_Split;

    private GameObject[] m_Splits = new GameObject[40];
    private Color[] m_Colors = new Color[40];

    public int startNum = 3;
    public int maxNum = 39;
    public float animDelay = 0.001f;
    public float increaseDelay = 0.025f;

    // Start is called before the first frame update
    void Start()
    {
        m_Character = gameObject.transform.GetChild(0).gameObject;
        m_Split = gameObject.transform.GetChild(1).gameObject;

        for (int i = 0; i < 39; i++)
        {
            m_Splits[i] = m_Split.transform.GetChild(i).gameObject;
            m_Colors[i] = m_Splits[i].GetComponent<SpriteRenderer>().color;
            m_Splits[i].SetActive(false);
        }

        m_Character.SetActive(false);
        m_Split.SetActive(true);

        StartCoroutine(OnLobbyStartAnim());

    }

    IEnumerator OnLobbyStartAnim()
    {
        bool isEnded = false;
        int rand = 0;
        float randAlpha = 0.0f;
        float minAlpha = 0.0f;
        float maxAlpha = 0.5f;
        //Color tempColor;
        float startTime = Time.time;
        float nowTime = Time.time;

        while(isEnded == false)
        {
            for(int i = 0; i < 39; i++)
            {
                m_Splits[i].SetActive(false);
            }

            for (int i = 0; i < startNum; i++)
            {
                rand = Random.Range(0, 39);
                if (m_Splits[rand].activeSelf == false)
                {
                    randAlpha = Random.Range(minAlpha, maxAlpha);
                    m_Splits[rand].SetActive(true);
                    m_Colors[rand] = new Color(m_Colors[rand].r, m_Colors[rand].g, m_Colors[rand].b, randAlpha);
                }
            }
            nowTime = Time.time;
            
            if(nowTime - startTime >= increaseDelay)
            {
                nowTime = Time.time;
                startNum += 1;
                minAlpha = 0.0f + (0.5f * (startNum / (float)maxNum));
                maxAlpha = 0.5f + (0.5f * (startNum / (float)maxNum));
            }

            if(startNum == maxNum)
            {
                isEnded = true;
            }
            yield return new WaitForSeconds(animDelay);
        }
        m_Split.SetActive(false);
        m_Character.SetActive(true);
        yield break;
    }
}

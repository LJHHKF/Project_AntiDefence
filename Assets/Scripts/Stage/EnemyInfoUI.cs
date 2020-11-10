using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoUI : MonoBehaviour
{
    private EnemyInfoManager enemyInfoM;

    private Sprite[] monsterImage;
    private string[] monsterInfo;

    private Image img_monster;
    private Text txt_info;
    private Color[] m_Colors = new Color[3];
    private float[] oriAlphas = new float[3];

    private int maxSize;
    private bool[] is_showed;

    // Start is called before the first frame update
    void Start()
    {
        enemyInfoM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EnemyInfoManager>();

        monsterImage = enemyInfoM.GetMonsterImage();
        monsterInfo = enemyInfoM.GetMonsterInfoTxt();

        img_monster = gameObject.transform.Find("Image").GetComponent<Image>();
        txt_info = gameObject.transform.Find("Text").GetComponent<Text>();

        m_Colors[0] = gameObject.GetComponent<Image>().color;
        m_Colors[1] = img_monster.color;
        m_Colors[2] = txt_info.color;

        maxSize = monsterInfo.Length;
        is_showed = new bool[maxSize];
        for (int i = 0; i < maxSize; i++)
        {
            is_showed[i] = false;
        }

        for (int i = 0; i < 3; i++)
        {
            oriAlphas[i] = m_Colors[i].a;
            m_Colors[i].a = 0.0f;
        }
        gameObject.GetComponent<Image>().color = m_Colors[0];
        img_monster.color = m_Colors[1];
        txt_info.color = m_Colors[2];
    }

    public bool GetIsShowed(int index)
    {
        return is_showed[index];
    }

    public void ShowEnemyInfo(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            m_Colors[i].a = oriAlphas[i];
        }
        gameObject.GetComponent<Image>().color = m_Colors[0];
        img_monster.color = m_Colors[1];
        txt_info.color = m_Colors[2];

        img_monster.sprite = monsterImage[index];
        txt_info.text = monsterInfo[index];
        is_showed[index] = true;
        StartCoroutine(DeleyedOff());
    }

    IEnumerator DeleyedOff()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        for (int i = 0; i < 3; i++)
        {
            oriAlphas[i] = m_Colors[i].a;
            m_Colors[i].a = 0.0f;
        }
        gameObject.GetComponent<Image>().color = m_Colors[0];
        img_monster.color = m_Colors[1];
        txt_info.color = m_Colors[2];
    }
}

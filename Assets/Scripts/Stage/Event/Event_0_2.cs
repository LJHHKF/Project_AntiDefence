﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Event_0_2 : MonoBehaviour
{
    [Header("Object Set")]
    public StageManager m_stageManager;
    public GameObject ui_Panel_EventDlg;
    public GameObject ui_EventObjects;
    public ChangeTower changeTower;
    public Collider[] Colliders_snip;
    public Button[] buttons;

    [Header("SetSpawnEnemy")]
    public int[] spawnEnemyIndex;
    public int[] spawnPointIndex;
    public float[] eventDelay_AboutSpawn;

    [Header("Set Dialouge")]
    [TextArea]
    public string[] txt_dialogue;

    private Text txt_EventDlg;
    private GameObject[] point_Item = new GameObject[5];

    private int cnt_event = -1;
    private int cnt_subEvent = -1;
    private int cnt_Dialogue = -1;
    private bool prevEventIsDone = true;
    private bool is_firstEventDone = false;

    private void Start()
    {
        txt_EventDlg = ui_Panel_EventDlg.transform.Find("Text").GetComponent<Text>();

        point_Item[0] = ui_EventObjects.transform.Find("Point_Character").gameObject;
        point_Item[1] = ui_EventObjects.transform.Find("Point_Barricade_1").gameObject;
        point_Item[2] = ui_EventObjects.transform.Find("Point_Barricade_2").gameObject;
        point_Item[3] = ui_EventObjects.transform.Find("Point_Barricade_3").gameObject;
        point_Item[4] = ui_EventObjects.transform.Find("Point_Barricade_4").gameObject;

        ui_Panel_EventDlg.SetActive(false);
        for(int i = 0; i < 5; i++)
        {
            point_Item[i].SetActive(false);
        }

        m_stageManager.HadEvent();
    }

    private void Update()
    {
        if (m_stageManager.GetDlgIsDone())
        {
            if (is_firstEventDone == false)
            {
                is_firstEventDone = true;
                StartCoroutine(Event00());

                for (int i = 0; i < buttons.Length; i++)
                    buttons[i].interactable = false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                ProgressEvent();
            }
        }
    }
    private void ProgressEvent()
    {
        if(prevEventIsDone)
        {
            cnt_event++;
            switch(cnt_event)
            {
                case 0:
                    ui_Panel_EventDlg.SetActive(true);
                    ProgressDlg(); //0
                    break;
                case 1:
                    ProgressDlg(); //1
                    break;
                case 2:
                    ProgressDlg(); //2
                    break;
                case 4:
                    for (int i = 1; i < 5; i++)
                        point_Item[i].SetActive(true);
                    SubEvent(); // 0
                    break;
                case 5:
                    for (int i = 1; i < 5; i++)
                        point_Item[i].SetActive(false);
                    point_Item[0].SetActive(true);
                    SubEvent(); // 1
                    break;
                case 6:
                    point_Item[0].SetActive(false);
                    ProgressDlg(); // 3
                    break;
                case 7:
                    ProgressDlg(); // 4
                    break;
                case 8:
                    ProgressDlg(); // 5
                    break;
                case 9:
                    ProgressDlg(); // 6
                    break;
                case 10:
                    SubEvent(); //2
                    break;
                case 11:
                    ProgressDlg(); //7
                    break;
                case 12:
                    ProgressDlg(); // 8
                    break;
                case 13:
                    ProgressDlg(); // 9
                    break;
                case 14:
                    ui_Panel_EventDlg.SetActive(false);
                    ui_EventObjects.SetActive(false);
                    m_stageManager.EventEnd();
                    for (int i = 0; i < buttons.Length; i++)
                        buttons[i].interactable = true;
                    Destroy(gameObject);
                    break;
            }
        }
    }

    private void ProgressDlg()
    {
        cnt_Dialogue++;
        txt_EventDlg.text = txt_dialogue[cnt_Dialogue];
    }

    private void SubEvent()
    {
        cnt_subEvent++;
        prevEventIsDone = false;
        switch (cnt_subEvent)
        {
            case 0:
                StartCoroutine(Event01());
                break;
            case 1:
                StartCoroutine(Event02());
                break;
            case 2:
                StartCoroutine(Event03());
                break;
        }
    }

    IEnumerator Event00()
    {
        prevEventIsDone = false;
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(2.5f);
        Time.timeScale = 0.0f;
        prevEventIsDone = true;
        ProgressEvent();
        yield break;
    }

    IEnumerator Event01()
    {
        Color[] m_colors = new Color[4];
        float[] oriAlpha = new float[4];
        for (int i = 1; i < 5; i++)
        {
            m_colors[i - 1] = point_Item[i].GetComponent<Image>().color;
            oriAlpha[i-1] = m_colors[i-1].a;
        }
        bool reverse = false;

        while(prevEventIsDone == false)
        {
            if(reverse == false)
            {
                for(int i = 0; i < 4; i++)
                    m_colors[i] = new Color(m_colors[i].r, m_colors[i].g, m_colors[i].b, m_colors[i].a - (oriAlpha[i] * 0.005f));
                if(m_colors[0].a <= oriAlpha[0] * 0.5)
                {
                    reverse = true;
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                    m_colors[i] = new Color(m_colors[i].r, m_colors[i].g, m_colors[i].b, m_colors[i].a + (oriAlpha[i] * 0.005f));
                if(m_colors[0].a >= oriAlpha[0])
                {
                    prevEventIsDone = true;
                }
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield break;
    }

    IEnumerator Event02()
    {
        Color m_color = point_Item[0].GetComponent<Image>().color;
        float oriAlpha = m_color.a;
        bool reverse = false;

        while(prevEventIsDone == false)
        {
            if (reverse == false)
            {
                m_color = new Color(m_color.r, m_color.g, m_color.b, m_color.a - (oriAlpha * 0.005f));
                if (m_color.a <= oriAlpha * 0.5f)
                {
                    reverse = true;
                }
            }
            else
            {
                m_color = new Color(m_color.r, m_color.g, m_color.b, m_color.a + (oriAlpha * 0.005f));
                if(m_color.a >= oriAlpha)
                {
                    prevEventIsDone = true;
                }
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield break;
    }

    IEnumerator Event03()
    {
        m_stageManager.Pub_SpawnEnemy(spawnPointIndex[0], spawnEnemyIndex[0]);
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(eventDelay_AboutSpawn[0]);
        changeTower.InstSNT();
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 2; i++)
            Colliders_snip[i].enabled = true;
        yield return new WaitForSeconds(0.3f);
        //changeTower.InstBasic();
        //yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0.0f;
        prevEventIsDone = true;
        yield break;
    }

}

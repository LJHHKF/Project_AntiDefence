using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Event_0_1 : MonoBehaviour
{
    [Header("Object Set")]
    public StageManager m_stageManager;
    public GameObject ui_Panel_EventDlg;
    public GameObject ui_EventObjects;
    public EnemyInfoUI ui_EnemyInfo;
    public GameObject[] ui_Buttons_Highlight;
    public Button[] buttons;
    public GameObject[] towers_Basic;
    public GameObject[] towers_Snip;
    public GameObject[] towers_Push;
    public Collider[] Colliders_NonPush;
    public Collider[] Colliders_Push_Child1;
    public Collider[] Colliders_Push_Child2;

    [Header("SetSpawnEnemy")]
    public int[] spawnEnemyIndex;
    public int[] spawnPointIndex;
    public float[] eventDelay_AboutSpawn;

    [Header("Set Dialouge")]
    [TextArea]
    public string[] txt_dialogue;

    private Text txt_EventDlg;
    private GameObject point_Character;
    private GameObject[] points_Towers = new GameObject[2];
    private Transform t_towerBoard;
    private GameObject[] tutorialRanges = new GameObject[2];
    private SpriteRenderer[] spr_attackRanges = new SpriteRenderer[2];
    private float ori_alpha;
    private ChangeTower changeTower;

    private int cnt_event = -1;
    private int cnt_subEvent = -1;
    private int cnt_Dialogue = -1;
    private bool prevEventIsDone = true;


    private bool is_firstEventDone = false;

    private void Start()
    {
        txt_EventDlg = ui_Panel_EventDlg.transform.Find("Text").GetComponent<Text>();
        point_Character = ui_EventObjects.transform.Find("Img_Point_Character").gameObject;
        points_Towers[0] = ui_EventObjects.transform.Find("Img_Point_Tower_1").gameObject;
        points_Towers[1] = ui_EventObjects.transform.Find("Img_Point_Tower_2").gameObject;
        t_towerBoard = GameObject.FindGameObjectWithTag("TowerBoard").transform;
        changeTower = t_towerBoard.GetComponent<ChangeTower>();
        for (int i = 0; i < 2; i++)
        {
            tutorialRanges[i] = towers_Basic[i].transform.Find("AttackRange_Rect_pivot").Find("TutorialRange").gameObject;
            tutorialRanges[i].SetActive(false);
            spr_attackRanges[i] = towers_Basic[i].transform.Find("AttackRange_Rect_pivot").Find("AttackRange").GetComponent<SpriteRenderer>();
        }
        //Colliders_Push_Child1 = Colliders_Push[0].GetComponentsInChildren<BoxCollider>();
        //Colliders_Push_Child2 = Colliders_Push[1].GetComponentsInChildren<BoxCollider>();

        ui_Panel_EventDlg.SetActive(false);
        point_Character.SetActive(false);
        for (int i = 0; i < points_Towers.Length; i++)
            points_Towers[i].SetActive(false);
        for (int i = 0; i < ui_Buttons_Highlight.Length; i++)
            ui_Buttons_Highlight[i].SetActive(false);

           
        m_stageManager.HadEvent();
    }

    private void Update()
    {
        if (m_stageManager.GetDlgIsDone())
        {
            if(is_firstEventDone == false)
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
        if (prevEventIsDone)
        {
            cnt_event++;
            switch (cnt_event)
            {
                case 0:
                    ui_Panel_EventDlg.SetActive(true);
                    ProgressDlg(); //0
                    break;
                case 1:
                    ProgressDlg(); //1
                    point_Character.SetActive(true);
                    break;
                case 2:
                    SubEvent(); // 0
                    break;
                case 3:
                    point_Character.SetActive(false);
                    ProgressDlg(); //2
                    break;
                case 4:
                    ui_Panel_EventDlg.SetActive(false);
                    for (int i = 0; i < points_Towers.Length; i++)
                    {
                        points_Towers[i].SetActive(true);
                    }
                    break;
                case 5:
                    SubEvent(); // 1
                    break;
                case 6:
                    ui_Panel_EventDlg.SetActive(true);
                    ProgressDlg(); //3
                    break;
                case 7:
                    ui_Panel_EventDlg.SetActive(false);
                    SubEvent(); //3
                    break;
                case 8:
                    ui_Panel_EventDlg.SetActive(true);
                    ProgressDlg(); //4
                    ori_alpha = spr_attackRanges[0].color.a;
                    for (int i = 0; i < 2; i++)
                    {
                        tutorialRanges[i].SetActive(true);
                        spr_attackRanges[i].color = new Color(spr_attackRanges[i].color.r, spr_attackRanges[i].color.g, spr_attackRanges[i].color.b, 0);
                    }
                    SubEvent(); //4
                    break;
                case 9:
                    ProgressDlg(); //5
                    for (int i = 0; i < 2; i++)
                    {
                        tutorialRanges[i].SetActive(false);
                        spr_attackRanges[i].color = new Color(spr_attackRanges[i].color.r, spr_attackRanges[i].color.g, spr_attackRanges[i].color.b, ori_alpha);
                    }
                    StartCoroutine(DelayedActiveFalse(ui_Panel_EventDlg, 1.0f));
                    SubEvent(); //5
                    break;
                case 10:
                    ui_Panel_EventDlg.SetActive(true);
                    ProgressDlg(); // 6
                    SubEvent(); //6 & ProgressDlg() 7
                    break;
                case 11:
                    ui_Panel_EventDlg.SetActive(false);
                    SubEvent(); // 7
                    break;
                case 12:
                    ui_Panel_EventDlg.SetActive(true);
                    ProgressDlg(); //8
                    SubEvent(); // 8
                    break;
                case 13:
                    //ui_Panel_EventDlg.SetActive(false);
                    SubEvent(); // 9
                    break;
                case 14:
                    ui_Buttons_Highlight[1].SetActive(false);
                    SubEvent(); // 10
                    break;
                case 15:
                    SubEvent(); // 11
                    break;
                case 16:
                    SubEvent(); // 12
                    break;
                case 17:
                    ui_Buttons_Highlight[2].SetActive(false);
                    //ui_Panel_EventDlg.SetActive(true);
                    ProgressDlg();
                    break;
                case 18:
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
        switch(cnt_subEvent)
        {
            case 0:
                StartCoroutine(Event01());
                break;
            case 1:
                StartCoroutine(Event02());
                break;
            case 2:
                for (int i = 0; i < 2; i++)
                {
                    points_Towers[i].SetActive(false);
                }
                StartCoroutine(Event04());
                break;
            case 3:
                StartCoroutine(Event05());
                break;
            case 4:
                StartCoroutine(Event06());
                break;
            case 5:
                StartCoroutine(Event07());
                break;
            case 6:
                StartCoroutine(Event08());
                break;
            case 7:
                StartCoroutine(Event09());
                break;
            case 8:
                StartCoroutine(Event10());
                break;
            case 9:
                StartCoroutine(Event11());
                break;
            case 10:
                StartCoroutine(Event12());
                break;
            case 11:
                StartCoroutine(Event13());
                break;
        }
    }


    IEnumerator DelayedActiveFalse(GameObject target, float sec)
    {
        yield return new WaitForSecondsRealtime(sec);
        target.SetActive(false);
        yield break;
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
        prevEventIsDone = true;
        yield break;
    }

    IEnumerator Event02()
    {
        prevEventIsDone = true;
        yield break;
    }

    //03은 수정하며 삭제
    
    IEnumerator Event04()
    {
        float rotValue = 0.0f;
        while (prevEventIsDone == false)
        {
            if (rotValue < 180f)
            {
                rotValue += 0.9f;
                t_towerBoard.localEulerAngles = new Vector3(0, rotValue, 0);
            }
            else
            {
                prevEventIsDone = true;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield break;
    }

    IEnumerator Event05()
    {
        prevEventIsDone = true;
        yield break;
    }

    IEnumerator Event06()
    {
        m_stageManager.Pub_SpawnEnemy(spawnPointIndex[0], spawnEnemyIndex[0]);
        Time.timeScale = 1.0f;
        yield return new WaitForSecondsRealtime(eventDelay_AboutSpawn[0]);
        for (int i = 0; i < 2; i++)
            Colliders_NonPush[i].enabled = true;
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 0.0f;
        prevEventIsDone = true;
        yield break;
    }

    IEnumerator Event07()
    {
        m_stageManager.Pub_SpawnEnemy(spawnPointIndex[1], spawnEnemyIndex[1]);
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(eventDelay_AboutSpawn[1]);
        ProgressDlg(); // 7
        Time.timeScale = 0.0f;
        prevEventIsDone = true;
        yield break;
    }

    IEnumerator Event08()
    {
        ui_EnemyInfo.ShowEnemyInfo(spawnEnemyIndex[1]);
        prevEventIsDone = true;
        yield break;
    }

    IEnumerator Event09()
    {
        ui_Buttons_Highlight[1].SetActive(true);
        int cnt = 0;

        Time.timeScale = 1.0f;
        changeTower.InstSNT();
        while (prevEventIsDone == false)
        {
            cnt++;  // 기존에 그림 알파값을 조정해서 반짝이면서, 한번 반짝이는 동안 시간 흐름으로 제어했던 부분을, 방식 변경하면서 시간 흐름만 남겨두기 위한 코드
            if(cnt >= 200)
            {
                Time.timeScale = 0.0f;
                prevEventIsDone = true;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield break;
    }

    IEnumerator Event10()
    {
        ui_Buttons_Highlight[1].SetActive(true);
        int cnt = 0;

        Time.timeScale = 1.0f;
        for (int i = 2; i < 4; i++)
            Colliders_NonPush[i].enabled = true;
        while (prevEventIsDone == false)
        {
            cnt++;
            if (cnt >= 200)
            {
                Time.timeScale = 0.0f;
                prevEventIsDone = true;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield break;
    }

    IEnumerator Event11()
    {
        m_stageManager.Pub_SpawnEnemy(spawnPointIndex[2], spawnEnemyIndex[2]);
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(eventDelay_AboutSpawn[2]);
        Time.timeScale = 0.0f;
        prevEventIsDone = true;
        yield break;
    }

    IEnumerator Event12()
    {
        ui_Buttons_Highlight[2].SetActive(true);
        int cnt = 0;

        Time.timeScale = 1.0f;
        changeTower.InstPT();
        while (prevEventIsDone == false)
        {
            cnt++;
            if (cnt >= 200)
            {
                Time.timeScale = 0.0f;
                prevEventIsDone = true;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }


        yield break;
    }

    IEnumerator Event13()
    {
        ui_Buttons_Highlight[2].SetActive(true);
        int cnt = 0;

        Time.timeScale = 1.0f;

        for (int i = 0; i < Colliders_Push_Child1.Length; i++)
            Colliders_Push_Child1[i].enabled = true;
        for (int i = 0; i < Colliders_Push_Child2.Length; i++)
            Colliders_Push_Child2[i].enabled = true;

        while (prevEventIsDone == false)
        {
            cnt++;
            if (cnt >= 100)
            {
                Time.timeScale = 0.0f;
                prevEventIsDone = true;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield break;
    }

}

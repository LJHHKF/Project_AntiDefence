using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelect : MonoBehaviour
{

    private GameObject gm;
    private LoadingManager loadingM;
    private BGM_Manager bgmM;
    private AudioManager audioM;
    private TouchEfManager touchEfM;

    private Transform t_touchEfPool;
    private List<GameObject> listPool_touchEf = new List<GameObject>();
    private bool is_serched_touchEf;

    private Camera main_camera;
    public float delay_Scene_Change = 1.0f;
    public float cameraMoveSpeed = 0.1f;
    private bool btn_clicked = false;
    private Image img_FadeOut;
    public float fadeSpeed = 1.0f;

    //public GameObject[] arr_btns;
    //public float moveSpeed;
    //private int index_btns;
    //private Transform tr_BTN_Space;
    //private Transform tr_BTN_Ready;

    //private Vector2 touchBeganPos;
    //private Vector2 touchEndPos;
    //private Vector2 touchDif;
    //private bool onDrag;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        loadingM = gm.GetComponent<LoadingManager>();
        bgmM = gm.GetComponent<BGM_Manager>();
        audioM = gm.GetComponent<AudioManager>();
        touchEfM = gm.GetComponent<TouchEfManager>();

        t_touchEfPool = gameObject.transform.Find("TouchEffect_Pool");

        main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        img_FadeOut = gameObject.transform.Find("FadeOut").GetComponent<Image>();


        //tr_BTN_Space = gameObject.transform.Find("BTN_Space");
        //tr_BTN_Ready = gameObject.transform.Find("BTN_Ready");

        //for (int i = 0; i < arr_btns.Length; i++)
        //{
        //    arr_btns[i].SetActive(false);
        //    arr_btns[i].transform.SetParent(tr_BTN_Ready);
        //}
        //index_btns = 0;
        //arr_btns[index_btns].transform.SetParent(tr_BTN_Space);
        //arr_btns[index_btns].SetActive(true);
        

        bgmM.Play_LobbyAndShop();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnTouchEf(Input.mousePosition);
        }
        
        if (btn_clicked)
        {
            main_camera.fieldOfView -= cameraMoveSpeed;
            img_FadeOut.color = new Color(img_FadeOut.color.r, img_FadeOut.color.g, img_FadeOut.color.b, img_FadeOut.color.a + fadeSpeed);
        }
    }

    public void BTN_Chp0()
    {
        if (!btn_clicked)
        {
            audioM.SFX_BTN_Click();
            btn_clicked = true;
            StartCoroutine(SetLoadScene(0));
        }
    }

    public void BTN_Return()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("Lobby");
    }

    private void SpawnTouchEf(Vector3 mousePosition)
    {
        if (listPool_touchEf.Count == 0)
        {
            PoolingTouchEf();
        }
        is_serched_touchEf = false;

        for (int i = 0; i < listPool_touchEf.Count; i++)
        {
            if (listPool_touchEf[i].activeSelf == false)
            {
                Animator m_animator = listPool_touchEf[i].GetComponent<Animator>();
                RectTransform m_rect = listPool_touchEf[i].GetComponent<RectTransform>();
                m_rect.position = mousePosition;
                listPool_touchEf[i].SetActive(true);
                m_animator.SetTrigger("IsTouched_Trigger");
                StartCoroutine(StopEffect(listPool_touchEf[i], 0.5f));
                is_serched_touchEf = true;
                break;
            }
        }

        if (is_serched_touchEf == false)
        {
            PoolingTouchEf();
            SpawnTouchEf(mousePosition);
        }
    }

    private void PoolingTouchEf()
    {
        GameObject effect = touchEfM.Instantiate(t_touchEfPool);
        listPool_touchEf.Add(effect);
        effect.name = "TouchEffect_" + listPool_touchEf.Count.ToString("00");
        effect.SetActive(false);
    }

    IEnumerator StopEffect(GameObject target, float time)
    {
        yield return new WaitForSeconds(time);
        target.SetActive(false);
        yield break;
    }

    IEnumerator SetLoadScene(int chpter_num)
    {
        string str = "Chp" + chpter_num.ToString("00") + "_StageSelect";
        yield return new WaitForSeconds(delay_Scene_Change);
        loadingM.LoadScene(str);

    }

    //private void SwipeEvent()
    //{
    //    if (onDrag == false)
    //    {
    //        //마우스 기반
    //        touchBeganPos = Input.mousePosition;
    //        touchEndPos = Input.mousePosition;

    //        onDrag = true;
    //    }
    //    else if (onDrag)
    //    {
    //        //touchBeganPos = this.touchEndPos;
    //        touchEndPos = Input.mousePosition;

    //        touchDif = touchEndPos - touchBeganPos;
    //        touchDif.Normalize();
    //    }

    //}
    //private void SwipeEndEvent()
    //{
    //    RectTransform[] for_chk = tr_BTN_Space.GetComponentsInChildren<RectTransform>();

    //    Debug.Log(touchDif);
    //    if (for_chk.Length == 3)
    //    {
    //        if (touchDif.y > 0) // 위로 긁으면
    //        {
    //            if (index_btns != arr_btns.Length -1)
    //            {
    //                RectTransform rect = arr_btns[index_btns + 1].GetComponent<RectTransform>();
    //                rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y - 600, rect.localPosition.z);
    //                arr_btns[index_btns + 1].SetActive(true);
    //                StartCoroutine(MoveToZero(arr_btns[index_btns + 1], arr_btns[index_btns]));
    //                index_btns += 1;
    //            }
    //        }
    //        else if (touchDif.y < 0) // 아래로 긁으면
    //        {
    //            if (index_btns != 0)
    //            {
    //                RectTransform rect = arr_btns[index_btns - 1].GetComponent<RectTransform>();
    //                rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y + 600, rect.localPosition.z);
    //                arr_btns[index_btns - 1].SetActive(true);
    //                StartCoroutine(MoveToZero(arr_btns[index_btns - 1], arr_btns[index_btns]));
    //                index_btns -= 1;
    //            }

    //        }
    //    }

    //}

    //IEnumerator MoveToZero(GameObject obj_move, GameObject obj_ori)
    //{
    //    RectTransform rect = obj_move.GetComponent<RectTransform>();
    //    obj_ori.transform.SetParent(tr_BTN_Ready);
    //    while(rect.localPosition.y != 0)
    //    {
    //        if(rect.localPosition.y > 0)
    //        {
    //            rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y - moveSpeed, rect.localPosition.z);
                
    //        }
    //        else if(rect.localPosition.y < 0)
    //        {
    //            rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y + moveSpeed, rect.localPosition.z);
    //        }
    //        yield return null;
    //    }
    //    obj_move.transform.SetParent(tr_BTN_Space);
    //    obj_ori.SetActive(false);

    //    yield break;
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject gm;
    private LoadingManager loadingM;
    private SelectedItemManager itemM;
    private SkinManager skinM;
    private BGM_Manager bgmM;
    private AudioManager audioM;
    private TouchEfManager touchEfM;

    private Transform t_touchEfPool;
    private List<GameObject> listPool_touchEf = new List<GameObject>();
    private bool is_serched_touchEf;

    private Transform t_subButtons;
    private Text txt_money;
    private Text txt_level;
    private Image img_skin;
    private SpriteRenderer spr_skin;
    private Animator m_animator;
    private GameObject panel_exit;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        loadingM = gm.GetComponent<LoadingManager>();
        itemM = gm.GetComponent<SelectedItemManager>();
        skinM = gm.GetComponent<SkinManager>();
        bgmM = gm.GetComponent<BGM_Manager>();
        audioM = gm.GetComponent<AudioManager>();
        touchEfM = gm.GetComponent<TouchEfManager>();

        t_touchEfPool = gameObject.transform.Find("TouchEffect_Pool");

        t_subButtons = gameObject.transform.Find("Panel_SubButtons");
        txt_money = t_subButtons.Find("Panel_Money").Find("Text").GetComponent<Text>();
        txt_level = t_subButtons.Find("Panel_Level").Find("Text").GetComponent<Text>();
        panel_exit = gameObject.transform.Find("Panel_Exit").gameObject;

        //2D UI판
        //img_skin = gameObject.transform.Find("Panel_Main").Find("Image_Character").GetComponent<Image>();
        //m_animator = gameObject.transform.Find("Panel_Main").Find("Image_Character").GetComponent<Animator>();

        //3D 오브젝트판
        spr_skin = GameObject.FindGameObjectWithTag("Player").transform.Find("Clear_Panel").Find("Img_Character").GetComponent<SpriteRenderer>();
        m_animator = GameObject.FindGameObjectWithTag("Player").transform.Find("Clear_Panel").Find("Img_Character").GetComponent<Animator>();

        panel_exit.SetActive(false);

        txt_money.text = itemM.own_money.ToString();
        txt_level.text = "아직 미구현";
        m_animator.runtimeAnimatorController = skinM.anims[skinM.GetSkinIndex()];

        //img_skin.sprite = skinM.skins[skinM.GetSkinIndex()];
        spr_skin.sprite = skinM.skins[skinM.GetSkinIndex()];

        bgmM.Play_LobbyAndShop();
    }

    private void Update()
    {
        //if (Application.platform == RuntimePlatform.Android)
       // {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0.0f;
                panel_exit.SetActive(true);
            }
        //}

        if (Input.GetMouseButtonDown(0))
        {
            SpawnTouchEf(Input.mousePosition);
        }
    }

    public void BTN_Setting()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("GameSetting", "Lobby");
    }

    public void BTN_Inventory()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("Inventory", "Lobby");
    }

    public void BTN_Skin()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("SkinSetting");
    }

    public void BTN_Shop()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("ItemShop", "Lobby");
    }

    public void BTN_Stage()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("ChapterSelect");
    }

    public void BTN_Quit_Yes()
    {
        Application.Quit();
    }

    public void BTN_Quit_No()
    {
        Time.timeScale = 1.0f;
        panel_exit.SetActive(false);
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

}

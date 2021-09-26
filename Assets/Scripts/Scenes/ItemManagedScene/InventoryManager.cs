using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    private Transform t_content;
    private Transform t_ItemSet0;
    private Transform t_ItemSet1;
    private Transform t_ItemSet2;

    private GameObject sub_panel_information;
    private GameObject sub_panel_itemInfo;
    private Image sub_img_infoIcon;
    private Text sub_txt_price;
    private Text sub_txt_infoHeader;
    private Text sub_txt_itemInfo;

    struct Item
    {
        public GameObject buttonObject;
        public Image iconImage;
        public Text numTxt;
        public string name;
        public string infoText;
        public int price;
    };

    private Item mulS_B;
    private Item mulS_SN;
    private Item mulS_P;
    private Item aIBarrier;
    private Item protectWall;
    private Item extend_B;
    private Item extend_SN;
    private Item extend_P;
    private Item recovery;

    private Image[] i_imgs;
    private string[] i_names;
    private string[] i_infoTexts;
    private int[] i_prices;
    private int n_information = -1;

    private GameObject gm;
    private SelectedItemManager selectIManager;
    private LoadingManager loadingM;
    private BGM_Manager bgmM;
    private AudioManager audioM;
    private TouchEfManager touchEfM;

    private Transform t_touchEfPool;
    private List<GameObject> listPool_touchEf = new List<GameObject>();
    private bool is_serched_touchEf;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        selectIManager = gm.GetComponent<SelectedItemManager>();
        loadingM = gm.GetComponent<LoadingManager>();
        bgmM = gm.GetComponent<BGM_Manager>();
        audioM = gm.GetComponent<AudioManager>();

        bgmM.Play_LobbyAndShop();

        t_content = gameObject.transform.Find("Inventory_BG").Find("ScrollView").Find("Viewport").Find("Content");
        t_ItemSet0 = t_content.Find("ItemSet0");
        t_ItemSet1 = t_content.Find("ItemSet1");
        t_ItemSet2 = t_content.Find("ItemSet2");

        SetItemInfo();

        i_imgs = new Image[9] { mulS_B.iconImage, mulS_SN.iconImage, mulS_P.iconImage,
                                aIBarrier.iconImage, protectWall.iconImage, extend_B.iconImage,
                                extend_SN.iconImage, extend_P.iconImage, recovery.iconImage};
        i_names = new string[9] {mulS_B.name, mulS_SN.name, mulS_P.name,
                                aIBarrier.name, protectWall.name, extend_B.name,
                                extend_SN.name, extend_P.name, recovery.name};
        i_infoTexts = new string[9] {mulS_B.infoText, mulS_SN.infoText, mulS_P.infoText,
                                    aIBarrier.infoText, protectWall.infoText, extend_B.infoText,
                                    extend_SN.infoText, extend_P.infoText, recovery.infoText};
        i_prices = new int[9] {mulS_B.price, mulS_SN.price, mulS_P.price,
                                aIBarrier.price, protectWall.price, extend_B.price,
                                extend_SN.price, extend_P.price, recovery.price};

        sub_panel_information = gameObject.transform.Find("Panel_Information").gameObject;
        sub_panel_itemInfo = sub_panel_information.transform.Find("Panel_ItemInfo").gameObject;
        sub_img_infoIcon = sub_panel_itemInfo.transform.Find("Icon").GetComponent<Image>();
        sub_txt_price = sub_panel_itemInfo.transform.Find("Panel_Price").Find("Price").GetComponent<Text>();
        sub_txt_itemInfo = sub_panel_itemInfo.transform.Find("Panel_InfoText").Find("Text").GetComponent<Text>();
        sub_txt_infoHeader = sub_panel_itemInfo.transform.Find("Panel_InfoTextHeader").Find("Text").GetComponent<Text>();
        sub_panel_information.SetActive(false);

        touchEfM = gm.GetComponent<TouchEfManager>();
        t_touchEfPool = gameObject.transform.Find("TouchEffect_Pool");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnTouchEf(Input.mousePosition);
        }
    }

    public void BTN_Return()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadPrevScene();
    }

    public void BTN_sub_quit()
    {
        audioM.SFX_BTN_Click();
        sub_panel_information.SetActive(false);
    }

    public void BTN_Muls_B()
    {
        audioM.SFX_BTN_Click();
        On_Information(0);
    }

    public void BTN_Muls_SN()
    {
        audioM.SFX_BTN_Click();
        On_Information(1);
    }

    public void BTN_Muls_P()
    {
        audioM.SFX_BTN_Click();
        On_Information(2);
    }

    public void BTN_AI_Barrier()
    {
        audioM.SFX_BTN_Click();
        On_Information(3);
    }

    public void BTN_ProtectWall()
    {
        audioM.SFX_BTN_Click();
        On_Information(4);
    }

    public void BTN_Extend_B()
    {
        audioM.SFX_BTN_Click();
        On_Information(5);
    }

    public void BTN_Extend_SN()
    {
        audioM.SFX_BTN_Click();
        On_Information(6);
    }

    public void BTN_Extend_P()
    {
        audioM.SFX_BTN_Click();
        On_Information(7);
    }

    public void BTN_Recovery()
    {
        audioM.SFX_BTN_Click();
        On_Information(8);
    }

    private void On_Information(int i_num)
    {
        n_information = i_num;

        if (n_information >= 0)
        {
            sub_img_infoIcon.sprite = i_imgs[i_num].sprite;
            sub_txt_infoHeader.text = i_names[i_num];
            sub_txt_itemInfo.text = i_infoTexts[i_num];
            sub_txt_price.text = i_prices[i_num].ToString();
            sub_panel_information.SetActive(true);
        }
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
                StartCoroutine(StopEffect(listPool_touchEf[i], touchEfM.GetPlayTime()));
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

    private void SetItemInfo()
    {
        mulS_B.buttonObject = t_ItemSet0.Find("B_MultiShotB").gameObject;
        mulS_B.iconImage = mulS_B.buttonObject.transform.Find("Icon").GetComponent<Image>();
        mulS_B.numTxt = mulS_B.buttonObject.transform.Find("Panel_OwnText").Find("Text").GetComponent<Text>();
        mulS_B.iconImage.sprite = selectIManager.spr_Muls_B;
        mulS_B.numTxt.text = selectIManager.own_MulS_B.ToString();
        mulS_B.name = selectIManager.name_Muls_B;
        mulS_B.infoText = selectIManager.txt_Muls_B;
        mulS_B.price = selectIManager.price_Muls_B;

        mulS_SN.buttonObject = t_ItemSet0.Find("B_MultiShotSN").gameObject;
        mulS_SN.iconImage = mulS_SN.buttonObject.transform.Find("Icon").GetComponent<Image>();
        mulS_SN.numTxt = mulS_SN.buttonObject.transform.Find("Panel_OwnText").Find("Text").GetComponent<Text>();
        mulS_SN.iconImage.sprite = selectIManager.spr_Muls_SN;
        mulS_SN.numTxt.text = selectIManager.own_MulS_SN.ToString();
        mulS_SN.name = selectIManager.name_Muls_SN;
        mulS_SN.infoText = selectIManager.txt_Muls_SN;
        mulS_SN.price = selectIManager.price_Muls_SN;

        mulS_P.buttonObject = t_ItemSet0.Find("B_MultiShotP").gameObject;
        mulS_P.iconImage = mulS_P.buttonObject.transform.Find("Icon").GetComponent<Image>();
        mulS_P.numTxt = mulS_P.buttonObject.transform.Find("Panel_OwnText").Find("Text").GetComponent<Text>();
        mulS_P.iconImage.sprite = selectIManager.spr_Muls_P;
        mulS_P.numTxt.text = selectIManager.own_MulS_P.ToString();
        mulS_P.name = selectIManager.name_Muls_P;
        mulS_P.infoText = selectIManager.txt_Muls_P;
        mulS_P.price = selectIManager.price_Muls_P;

        aIBarrier.buttonObject = t_ItemSet1.Find("B_AIBarrier").gameObject;
        aIBarrier.iconImage = aIBarrier.buttonObject.transform.Find("Icon").GetComponent<Image>();
        aIBarrier.numTxt = aIBarrier.buttonObject.transform.Find("Panel_OwnText").Find("Text").GetComponent<Text>();
        aIBarrier.iconImage.sprite = selectIManager.spr_AiBarrier;
        aIBarrier.numTxt.text = selectIManager.own_AiBarrier.ToString();
        aIBarrier.name = selectIManager.name_AiBarrire;
        aIBarrier.infoText = selectIManager.txt_AiBarrier;
        aIBarrier.price = selectIManager.price_AiBarrier;

        protectWall.buttonObject = t_ItemSet1.Find("B_ProtectWall").gameObject;
        protectWall.iconImage = protectWall.buttonObject.transform.Find("Icon").GetComponent<Image>();
        protectWall.numTxt = protectWall.buttonObject.transform.Find("Panel_OwnText").Find("Text").GetComponent<Text>();
        protectWall.iconImage.sprite = selectIManager.spr_ProtectWall;
        protectWall.numTxt.text = selectIManager.own_ProtectWall.ToString();
        protectWall.name = selectIManager.name_ProtectWall;
        protectWall.infoText = selectIManager.txt_ProtectWall;
        protectWall.price = selectIManager.price_ProtectWall;

        extend_B.buttonObject = t_ItemSet1.Find("B_ExtendB").gameObject;
        extend_B.iconImage = extend_B.buttonObject.transform.Find("Icon").GetComponent<Image>();
        extend_B.numTxt = extend_B.buttonObject.transform.Find("Panel_OwnText").Find("Text").GetComponent<Text>();
        extend_B.iconImage.sprite = selectIManager.spr_Extend_B;
        extend_B.numTxt.text = selectIManager.own_Extend_B.ToString();
        extend_B.name = selectIManager.name_Extend_B;
        extend_B.infoText = selectIManager.txt_Extend_B;
        extend_B.price = selectIManager.price_Extend_B;

        extend_SN.buttonObject = t_ItemSet2.Find("B_ExtendSN").gameObject;
        extend_SN.iconImage = extend_SN.buttonObject.transform.Find("Icon").GetComponent<Image>();
        extend_SN.numTxt = extend_SN.buttonObject.transform.Find("Panel_OwnText").Find("Text").GetComponent<Text>();
        extend_SN.iconImage.sprite = selectIManager.spr_Extend_SN;
        extend_SN.numTxt.text = selectIManager.own_Extend_SN.ToString();
        extend_SN.name = selectIManager.name_Extend_SN;
        extend_SN.infoText = selectIManager.txt_Extend_SN;
        extend_SN.price = selectIManager.price_Extend_SN;

        extend_P.buttonObject = t_ItemSet2.Find("B_ExtendP").gameObject;
        extend_P.iconImage = extend_P.buttonObject.transform.Find("Icon").GetComponent<Image>();
        extend_P.numTxt = extend_P.buttonObject.transform.Find("Panel_OwnText").Find("Text").GetComponent<Text>();
        extend_P.iconImage.sprite = selectIManager.spr_Extend_P;
        extend_P.numTxt.text = selectIManager.own_Extend_P.ToString();
        extend_P.name = selectIManager.name_Extend_P;
        extend_P.infoText = selectIManager.txt_Extend_P;
        extend_P.price = selectIManager.price_Extend_P;

        recovery.buttonObject = t_ItemSet2.Find("B_Recovery").gameObject;
        recovery.iconImage = recovery.buttonObject.transform.Find("Icon").GetComponent<Image>();
        recovery.numTxt = recovery.buttonObject.transform.Find("Panel_OwnText").Find("Text").GetComponent<Text>();
        recovery.iconImage.sprite = selectIManager.spr_Recovery;
        recovery.numTxt.text = selectIManager.own_Recovery.ToString();
        recovery.name = selectIManager.name_Recovery;
        recovery.infoText = selectIManager.txt_Recovery;
        recovery.price = selectIManager.price_Recovery;
    }
}

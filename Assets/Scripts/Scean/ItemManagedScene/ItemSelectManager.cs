using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectManager : MonoBehaviour
{
    private Transform t_panel_ps;
    private Transform t_content;
    private Transform t_ItemSet0;
    private Transform t_ItemSet1;
    private Transform t_ItemSet2;
    private Transform t_ItemSet3;
    private Transform t_ItemSet4;

    private Text t_money;

    private GameObject button_p1;
    private GameObject button_p2;
    private Image img_p1;
    private Image img_p2;
    private Toggle togl_p1;
    private Toggle togl_p2;
    private static int n_p1 = 1;
    private static int n_p2 = 2;
    private int w_pn;

    private GameObject sub_panel_purchase;
    private GameObject sub_panel_itemInfo;
    private Image sub_img_infoIcon;
    private Text sub_txt_price;
    private Text sub_txt_infoHeader;
    private Text sub_txt_itemInfo;

    private GameObject sub_panel_warning;
    private Text sub_txt_warning;

    [HideInInspector]
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

    private int n_pSelected = -1;
    private Image[] i_imgs;
    private int index_imgs;
    private bool img_selected = false;
    private string[] i_names;
    private string[] i_infoTexts;
    private int[] i_prices;
    private int n_purchase = -1;
    private bool i_had = false;

    private bool p1_on = false;
    private int p1_select = -1;
    private bool p2_on = false;
    private int p2_select = -1;

    private GameObject gm;
    private SelectedItemManager selectIManager;
    private LoadingManager loadingM;
    private BGM_Manager bgmM;
    private AudioManager audioM;

    public Sprite img_nonPart;


    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        selectIManager = gm.GetComponent<SelectedItemManager>();
        selectIManager.End_Stage();
        loadingM = gm.GetComponent<LoadingManager>();
        bgmM = gm.GetComponent<BGM_Manager>();
        audioM = gm.GetComponent<AudioManager>();

        bgmM.Play_LobbyAndShop();

        t_panel_ps = gameObject.transform.Find("Panel_Ps");
        t_content = gameObject.transform.Find("ItemScroll").Find("Viewport").Find("Content");
        t_ItemSet0 = t_content.Find("Item_Set0");
        t_ItemSet1 = t_content.Find("Item_Set1");
        t_ItemSet2 = t_content.Find("Item_Set2");
        t_ItemSet3 = t_content.Find("Item_Set3");
        t_ItemSet4 = t_content.Find("Item_Set4");

        t_money = gameObject.transform.Find("Panel_OwnMoney").Find("Moeny").GetComponent<Text>();
        t_money.text = selectIManager.own_money.ToString();

        button_p1 = t_panel_ps.Find("B_P1").gameObject;
        button_p2 = t_panel_ps.Find("B_P2").gameObject;
        img_p1 = button_p1.transform.Find("Icon").GetComponent<Image>();
        img_p2 = button_p2.transform.Find("Icon").GetComponent<Image>();
        togl_p1 = button_p1.transform.Find("Toggle").GetComponent<Toggle>();
        togl_p2 = button_p2.transform.Find("Toggle").GetComponent<Toggle>();

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

        w_pn = PlayerPrefs.GetInt("where_Barrier");
        if (w_pn > 0)
        {
            if (w_pn == 1)
            {
                p1_select = 3;
                img_p1.sprite = i_imgs[3].sprite;
            }
            else if (w_pn == 2)
            {
                p2_select = 3;
                img_p2.sprite = i_imgs[3].sprite;
            }
        }

        sub_panel_purchase = gameObject.transform.Find("Panel_Purchase").gameObject;
        sub_panel_itemInfo = sub_panel_purchase.transform.Find("Panel_ItemInfo").gameObject;
        sub_img_infoIcon = sub_panel_itemInfo.transform.Find("Icon").GetComponent<Image>();
        sub_txt_price = sub_panel_itemInfo.transform.Find("Panel_Price").Find("Price").GetComponent<Text>();
        sub_txt_itemInfo = sub_panel_itemInfo.transform.Find("Panel_InfoText").Find("Text").GetComponent<Text>();
        sub_txt_infoHeader = sub_panel_itemInfo.transform.Find("Panel_InfoTextHeader").Find("Text").GetComponent<Text>();
        sub_panel_purchase.SetActive(false);

        sub_panel_warning = gameObject.transform.Find("Panel_Warning").gameObject;
        sub_txt_warning = sub_panel_warning.transform.Find("Panel_Text").Find("Text").GetComponent<Text>();
        sub_panel_warning.SetActive(false);
    }

    public void Click_B_P1()
    {
        audioM.SFX_BTN_Click();
        if (p1_on == false && p2_on == false)
        {
            p1_on = true;
            n_pSelected = 1;
            togl_p1.isOn = true;
        }
        else if (p1_on && img_selected)
        {
            if (p1_select < 0)
            {
                img_p1.sprite = i_imgs[index_imgs].sprite;
                p1_select = index_imgs;
            }
            else
            {
                if (p1_select == 3)
                {
                    selectIManager.BarrierBreak();
                }
                selectIManager.Item_Get(p1_select);
                UpdateText(p1_select);

                img_p1.sprite = i_imgs[index_imgs].sprite;
                p1_select = index_imgs;
            }
            img_selected = false;
            p1_on = false;
            togl_p1.isOn = false;
            n_pSelected = 0;
        }
        else if (p1_on)
        {
            p1_on = false;
            togl_p1.isOn = false;
            n_pSelected = 0;
            if (p1_select >= 0)
            {
                if (p1_select == 3)
                {
                    selectIManager.BarrierBreak();
                }
                selectIManager.Item_Get(p1_select);
                img_p1.sprite = img_nonPart;
                UpdateText(p1_select);
            }
            p1_select = -1;
        }
    }

    public void Click_B_P2()
    {
        audioM.SFX_BTN_Click();
        if (p1_on == false && p2_on == false)
        {
            p2_on = true;
            n_pSelected = 2;
            togl_p2.isOn = true;
        }
        else if (p2_on && img_selected)
        {
            img_p2.sprite = i_imgs[index_imgs].sprite;
            p2_select = index_imgs;
            img_selected = false;
            p2_on = false;
            togl_p2.isOn = false;
            n_pSelected = 0;
        }
        else if (p2_on)
        {
            p2_on = false;
            togl_p2.isOn = false;
            n_pSelected = 0;
            if (p2_select >= 0)
            {
                if (p2_select == 3)
                {
                    selectIManager.BarrierBreak();
                }
                selectIManager.Item_Get(p2_select);
                img_p2.sprite = img_nonPart;
                UpdateText(p2_select);
            }
            p2_select = -1;
        }
    }

    private void Item_Clicked(int i_num)
    {
        audioM.SFX_BTN_Click();
        if (n_pSelected > 0)
        {
            switch (i_num)
            {
                case 0:
                    if (selectIManager.own_MulS_B > 0)
                        i_had = true;
                    break;
                case 1:
                    if (selectIManager.own_MulS_SN > 0)
                        i_had = true;
                    break;
                case 2:
                    if (selectIManager.own_MulS_P > 0)
                        i_had = true;
                    break;
                case 3:
                    if (selectIManager.own_AiBarrier > 0)
                        i_had = true;
                    break;
                case 4:
                    if (selectIManager.own_ProtectWall > 0)
                        i_had = true;
                    break;
                case 5:
                    if (selectIManager.own_Extend_B > 0)
                        i_had = true;
                    break;
                case 6:
                    if (selectIManager.own_Extend_SN > 0)
                        i_had = true;
                    break;
                case 7:
                    if (selectIManager.own_Extend_P > 0)
                        i_had = true;
                    break;
                case 8:
                    if (selectIManager.own_Recovery > 0)
                        i_had = true;
                    break;
            }
            if (i_had)
            {
                if (img_selected)
                {
                    selectIManager.Item_Get(index_imgs);
                    UpdateText(index_imgs);
                }
                index_imgs = i_num;
                selectIManager.Item_PreUse(i_num);
                UpdateText(i_num);
                img_selected = true;
                i_had = false;
            }
            else
            {
                switch (i_num)
                {
                    case 0:
                        On_Warning(mulS_B.name + " 소진");
                        break;
                    case 1:
                        On_Warning(mulS_SN.name +" 소진");
                        break;
                    case 2:
                        On_Warning(mulS_P.name + " 소진");
                        break;
                    case 3:
                        On_Warning(aIBarrier.name + " 소진");
                        break;
                    case 4:
                        On_Warning(protectWall.name + " 소진");
                        break;
                    case 5:
                        On_Warning(extend_B.name + " 소진");
                        break;
                    case 6:
                        On_Warning(extend_SN.name + " 소진");
                        break;
                    case 7:
                        On_Warning(extend_P.name +" 소진");
                        break;
                    case 8:
                        On_Warning(recovery.name + " 소진");
                        break;
                }
            }
        }
        else
        {
            On_Purchase(i_num);
        }
    }

    public void Click_B_Muls_B()
    {
        Item_Clicked(0);
    }

    public void Click_B_Muls_SN()
    {
        Item_Clicked(1);
    }

    public void Click_B_Muls_P()
    {
        Item_Clicked(2);
    }

    public void Click_B_AiBarrer()
    {
        Item_Clicked(3);
    }

    public void Click_B_ProtectWall()
    {
        Item_Clicked(4);
    }

    public void Click_B_Extend_B()
    {
        Item_Clicked(5);
    }

    public void Click_B_Extend_SN()
    {
        Item_Clicked(6);
    }

    public void Click_B_Extend_P()
    {
        Item_Clicked(7);
    }

    public void Click_B_Recovery()
    {
        Item_Clicked(8);
    }

    public void Click_Start()
    {
        audioM.SFX_BTN_Click();
        if (p1_select >= 0)
        {
            selectIManager.Item_Use_Confirm(p1_select);
            if (p1_select == 3)
            {
                PlayerPrefs.SetInt("where_Barrier", n_p1);
            }
        }
        if (p2_select >= 0)
        {
            selectIManager.Item_Use_Confirm(p2_select);
            if (p2_select == 3)
            {
                PlayerPrefs.SetInt("where_Barrier", n_p2);
            }
        }

        if (loadingM.GetHadPrev())
        {
            sub_txt_warning.text = "스테이지가 준비되지 않았습니다. 정식 루트로 부탁드립니다."; // 이제 필요없는 부분이지만, 오류 방지 코드로서 남겨둠.
            sub_panel_warning.SetActive(true);
        }
        else
        {
            loadingM.LoadStage();
        }
    }

    public void Click_Cancle()
    {
        audioM.SFX_BTN_Click();
        if (loadingM.GetHadPrev())
        {
            loadingM.LoadPrevScene();
        }
        else
        {
            loadingM.StageEnd();
        }
        
    }

    public void Cheat_B()
    {
        audioM.SFX_BTN_Click();
        //for (int i = 0; i < 9; i++)
        //{
        //    selectIManager.Item_Get(i);
        //    UpdateText(i);
        //}
        selectIManager.Get_Money(1000);
        UpdateText_Money();
    }

    private void UpdateText(int i_num)
    {
        switch(i_num)
        {
            case 0:
                mulS_B.numTxt.text = selectIManager.own_MulS_B.ToString();
                break;
            case 1:
                mulS_SN.numTxt.text = selectIManager.own_MulS_SN.ToString();
                break;
            case 2:
                mulS_P.numTxt.text = selectIManager.own_MulS_P.ToString();
                break;
            case 3:
                aIBarrier.numTxt.text = selectIManager.own_AiBarrier.ToString();
                break;
            case 4:
                protectWall.numTxt.text = selectIManager.own_ProtectWall.ToString();
                break;
            case 5:
                extend_B.numTxt.text = selectIManager.own_Extend_B.ToString();
                break;
            case 6:
                extend_SN.numTxt.text = selectIManager.own_Extend_SN.ToString();
                break;
            case 7:
                extend_P.numTxt.text = selectIManager.own_Extend_P.ToString();
                break;
            case 8:
                recovery.numTxt.text = selectIManager.own_Recovery.ToString();
                break;
        }
    }

    private void UpdateText_Money()
    {
        t_money.text = selectIManager.own_money.ToString();
    }

    private void On_Purchase(int i_num)
    {
        n_purchase = i_num;

        if (n_purchase >= 0)
        {
            sub_img_infoIcon.sprite = i_imgs[i_num].sprite;
            sub_txt_infoHeader.text = i_names[i_num];
            sub_txt_itemInfo.text = i_infoTexts[i_num];
            sub_txt_price.text = i_prices[i_num].ToString();
            sub_panel_purchase.SetActive(true);
        }
    }

    public void Click_sub_yes()
    {
        audioM.SFX_BTN_Click();
        if (selectIManager.own_money >= i_prices[n_purchase])
        {
            selectIManager.Use_Money(i_prices[n_purchase]);
            selectIManager.Item_Get(n_purchase);
            selectIManager.Item_Get_Confirm(n_purchase);
            sub_panel_purchase.SetActive(false);
            UpdateText_Money();
            UpdateText(n_purchase);
            n_purchase = -1;
        }
        else
        {
            On_Warning("소지금이 부족합니다.");
            sub_panel_purchase.SetActive(false);
            n_purchase = -1;
        }
    }

    private void On_Warning(string phrase)
    {
        sub_txt_warning.text = phrase;
        sub_panel_warning.SetActive(true);
    }

    public void Click_sub_no()
    {
        audioM.SFX_BTN_Click();
        sub_panel_purchase.SetActive(false);
    }

    public void Click_sub_w_return()
    {
        audioM.SFX_BTN_Click();
        sub_panel_warning.SetActive(false);
    }

    private void SetItemInfo()
    {
        mulS_B.buttonObject = t_ItemSet0.Find("B_MultiShotB").gameObject;
        mulS_B.iconImage = mulS_B.buttonObject.transform.Find("Icon").GetComponent<Image>();
        mulS_B.numTxt = mulS_B.buttonObject.transform.Find("Num").GetComponent<Text>();
        mulS_B.iconImage.sprite = selectIManager.spr_Muls_B;
        mulS_B.numTxt.text = selectIManager.own_MulS_B.ToString();
        mulS_B.name = selectIManager.name_Muls_B;
        mulS_B.infoText = selectIManager.txt_Muls_B;
        mulS_B.price = selectIManager.price_Muls_B;

        mulS_SN.buttonObject = t_ItemSet0.Find("B_MultiShotSN").gameObject;
        mulS_SN.iconImage = mulS_SN.buttonObject.transform.Find("Icon").GetComponent<Image>();
        mulS_SN.numTxt = mulS_SN.buttonObject.transform.Find("Num").GetComponent<Text>();
        mulS_SN.iconImage.sprite = selectIManager.spr_Muls_SN;
        mulS_SN.numTxt.text = selectIManager.own_MulS_SN.ToString();
        mulS_SN.name = selectIManager.name_Muls_SN;
        mulS_SN.infoText = selectIManager.txt_Muls_SN;
        mulS_SN.price = selectIManager.price_Muls_SN;

        mulS_P.buttonObject = t_ItemSet1.Find("B_MultiShotP").gameObject;
        mulS_P.iconImage = mulS_P.buttonObject.transform.Find("Icon").GetComponent<Image>();
        mulS_P.numTxt = mulS_P.buttonObject.transform.Find("Num").GetComponent<Text>();
        mulS_P.iconImage.sprite = selectIManager.spr_Muls_P;
        mulS_P.numTxt.text = selectIManager.own_MulS_P.ToString();
        mulS_P.name = selectIManager.name_Muls_P;
        mulS_P.infoText = selectIManager.txt_Muls_P;
        mulS_P.price = selectIManager.price_Muls_P;

        aIBarrier.buttonObject = t_ItemSet1.Find("B_AIBarrier").gameObject;
        aIBarrier.iconImage = aIBarrier.buttonObject.transform.Find("Icon").GetComponent<Image>();
        aIBarrier.numTxt = aIBarrier.buttonObject.transform.Find("Num").GetComponent<Text>();
        aIBarrier.iconImage.sprite = selectIManager.spr_AiBarrier;
        aIBarrier.numTxt.text = selectIManager.own_AiBarrier.ToString();
        aIBarrier.name = selectIManager.name_AiBarrire;
        aIBarrier.infoText = selectIManager.txt_AiBarrier;
        aIBarrier.price = selectIManager.price_AiBarrier;

        protectWall.buttonObject = t_ItemSet2.Find("B_ProtectWall").gameObject;
        protectWall.iconImage = protectWall.buttonObject.transform.Find("Icon").GetComponent<Image>();
        protectWall.numTxt = protectWall.buttonObject.transform.Find("Num").GetComponent<Text>();
        protectWall.iconImage.sprite = selectIManager.spr_ProtectWall;
        protectWall.numTxt.text = selectIManager.own_ProtectWall.ToString();
        protectWall.name = selectIManager.name_ProtectWall;
        protectWall.infoText = selectIManager.txt_ProtectWall;
        protectWall.price = selectIManager.price_ProtectWall;

        extend_B.buttonObject = t_ItemSet2.Find("B_ExtendB").gameObject;
        extend_B.iconImage = extend_B.buttonObject.transform.Find("Icon").GetComponent<Image>();
        extend_B.numTxt = extend_B.buttonObject.transform.Find("Num").GetComponent<Text>();
        extend_B.iconImage.sprite = selectIManager.spr_Extend_B;
        extend_B.numTxt.text = selectIManager.own_Extend_B.ToString();
        extend_B.name = selectIManager.name_Extend_B;
        extend_B.infoText = selectIManager.txt_Extend_B;
        extend_B.price = selectIManager.price_Extend_B;

        extend_SN.buttonObject = t_ItemSet3.Find("B_ExtendSN").gameObject;
        extend_SN.iconImage = extend_SN.buttonObject.transform.Find("Icon").GetComponent<Image>();
        extend_SN.numTxt = extend_SN.buttonObject.transform.Find("Num").GetComponent<Text>();
        extend_SN.iconImage.sprite = selectIManager.spr_Extend_SN;
        extend_SN.numTxt.text = selectIManager.own_Extend_SN.ToString();
        extend_SN.name = selectIManager.name_Extend_SN;
        extend_SN.infoText = selectIManager.txt_Extend_SN;
        extend_SN.price = selectIManager.price_Extend_SN;

        extend_P.buttonObject = t_ItemSet3.Find("B_ExtendP").gameObject;
        extend_P.iconImage = extend_P.buttonObject.transform.Find("Icon").GetComponent<Image>();
        extend_P.numTxt = extend_P.buttonObject.transform.Find("Num").GetComponent<Text>();
        extend_P.iconImage.sprite = selectIManager.spr_Extend_P;
        extend_P.numTxt.text = selectIManager.own_Extend_P.ToString();
        extend_P.name = selectIManager.name_Extend_P;
        extend_P.infoText = selectIManager.txt_Extend_P;
        extend_P.price = selectIManager.price_Extend_P;

        recovery.buttonObject = t_ItemSet4.Find("B_Recovery").gameObject;
        recovery.iconImage = recovery.buttonObject.transform.Find("Icon").GetComponent<Image>();
        recovery.numTxt = recovery.buttonObject.transform.Find("Num").GetComponent<Text>();
        recovery.iconImage.sprite = selectIManager.spr_Recovery;
        recovery.numTxt.text = selectIManager.own_Recovery.ToString();
        recovery.name = selectIManager.name_Recovery;
        recovery.infoText = selectIManager.txt_Recovery;
        recovery.price = selectIManager.price_Recovery;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectManager : MonoBehaviour
{
    private Transform t_panel_ps;
    //private Transform t_itemScroll;
    //private Transform t_viewPort;
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

    private bool p1_on = false;
    private int p1_select = -1;
    private bool p2_on = false;
    private int p2_select = -1;

    private GameObject gm;
    private SelectedItemManager selectIManager;
    private LoadingManager loadingM;
    private BGM_Manager bgmM;

    public Sprite img_nonPart;


    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        selectIManager = gm.GetComponent<SelectedItemManager>();
        selectIManager.End_Stage();
        loadingM = gm.GetComponent<LoadingManager>();
        bgmM = gm.GetComponent<BGM_Manager>();

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

    public void Click_B_Muls_B()
    {
        if (n_pSelected > 0)
        {
            if (selectIManager.own_MulS_B > 0)
            {
                if (img_selected)
                {
                    selectIManager.Item_Get(index_imgs);
                    UpdateText(index_imgs);
                }
                index_imgs = 0;
                selectIManager.Item_PreUse(0);
                UpdateText(0);
                img_selected = true;
            }
            else
            {
                Debug.Log("연발장치(일반) 소진");
            }
        }
        else
        {
            On_Purchase(0);
        }
    }

    public void Click_B_Muls_SN()
    {
        if (n_pSelected > 0)
        {
            if (selectIManager.own_MulS_B > 0)
            {
                if (img_selected)
                {
                    selectIManager.Item_Get(index_imgs);
                    UpdateText(index_imgs);
                }
                index_imgs = 1;
                selectIManager.Item_PreUse(1);
                UpdateText(1);
                img_selected = true;
            }
            else
            {
                Debug.Log("연발장치(저격) 소진");
            }
        }
        else
        {
            On_Purchase(1);
        }
    }

    public void Click_B_Muls_P()
    {
        if (n_pSelected > 0)
        {
            if (selectIManager.own_MulS_B > 0)
            {
                if (img_selected)
                {
                    selectIManager.Item_Get(index_imgs);
                    UpdateText(index_imgs);
                }
                index_imgs = 2;
                selectIManager.Item_PreUse(2);
                UpdateText(2);
                img_selected = true;
            }
            else
            {
                Debug.Log("연발장치(충격) 소진");
            }
        }
        else
        {
            On_Purchase(2);
        }
    }

    public void Click_B_AiBarrer()
    {
        if (n_pSelected > 0)
        {
            if (selectIManager.own_MulS_B > 0)
            {
                if (img_selected)
                {
                    selectIManager.Item_Get(index_imgs);
                    UpdateText(index_imgs);
                }
                index_imgs = 3;
                selectIManager.Item_PreUse(3);
                UpdateText(3);
                img_selected = true;
            }
            else
            {
                Debug.Log("AI 배리어 소진");
            }
        }
        else
        {
            On_Purchase(3);
        }
    }

    public void Click_B_ProtectWall()
    {
        if (n_pSelected > 0)
        {
            if (selectIManager.own_MulS_B > 0)
            {
                if (img_selected)
                {
                    selectIManager.Item_Get(index_imgs);
                    UpdateText(index_imgs);
                }
                index_imgs = 4;
                selectIManager.Item_PreUse(4);
                UpdateText(4);
                img_selected = true;
            }
            else
            {
                Debug.Log("방호벽 소진");
            }
        }
        else
        {
            On_Purchase(4);
        }
    }

    public void Click_B_Extend_B()
    {
        if (n_pSelected > 0)
        {
            if (selectIManager.own_MulS_B > 0)
            {
                if (img_selected)
                {
                    selectIManager.Item_Get(index_imgs);
                    UpdateText(index_imgs);
                }
                index_imgs = 5;
                selectIManager.Item_PreUse(5);
                UpdateText(5);
                img_selected = true;
            }
            else
            {
                Debug.Log("확장장치(일반) 소진");
            }
        }
        else
        {
            On_Purchase(5);
        }
    }

    public void Click_B_Extend_SN()
    {
        if (n_pSelected > 0)
        {
            if (selectIManager.own_MulS_B > 0)
            {
                if (img_selected)
                {
                    selectIManager.Item_Get(index_imgs);
                    UpdateText(index_imgs);
                }
                index_imgs = 6;
                selectIManager.Item_PreUse(6);
                UpdateText(6);
                img_selected = true;
            }
            else
            {
                Debug.Log("확장장치(저격) 소진");
            }
        }
        else
        {
            On_Purchase(6);
        }
    }

    public void Click_B_Extend_P()
    {
        if (n_pSelected > 0)
        {
            if (selectIManager.own_MulS_B > 0)
            {
                if (img_selected)
                {
                    selectIManager.Item_Get(index_imgs);
                    UpdateText(index_imgs);
                }
                index_imgs = 7;
                selectIManager.Item_PreUse(7);
                UpdateText(7);
                img_selected = true;
            }
            else
            {
                Debug.Log("확장장치(충격) 소진");
            }
        }
        else
        {
            On_Purchase(7);
        }
    }

    public void Click_B_Recovery()
    {
        if (n_pSelected > 0)
        {
            if (selectIManager.own_MulS_B > 0)
            {
                if (img_selected)
                {
                    selectIManager.Item_Get(index_imgs);
                    UpdateText(index_imgs);
                }
                index_imgs = 8;
                selectIManager.Item_PreUse(8);
                UpdateText(8);
                img_selected = true;
            }
            else
            {
                Debug.Log("수복장치 소진");
            }
        }
        else
        {
            On_Purchase(8);
        }
    }

    public void Click_Start()
    {
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
            sub_txt_warning.text = "스테이지가 준비되지 않았습니다. 정식 루트로 부탁드립니다.";
            sub_panel_warning.SetActive(true);
        }
        else
        {
            loadingM.LoadStage();
        }
    }

    public void Click_Cancle()
    {
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
            sub_txt_warning.text = "소지금이 부족합니다.";
            sub_panel_warning.SetActive(true);
            sub_panel_purchase.SetActive(false);
            n_purchase = -1;
        }
    }

    public void Click_sub_no()
    {
        sub_panel_purchase.SetActive(false);
    }

    public void Click_sub_w_return()
    {
        sub_panel_warning.SetActive(false);
    }

    private void SetItemInfo()
    {
        button_p1 = t_panel_ps.Find("B_P1").gameObject;
        button_p2 = t_panel_ps.Find("B_P2").gameObject;
        img_p1 = button_p1.transform.Find("Icon").GetComponent<Image>();
        img_p2 = button_p2.transform.Find("Icon").GetComponent<Image>();
        togl_p1 = button_p1.transform.Find("Toggle").GetComponent<Toggle>();
        togl_p2 = button_p2.transform.Find("Toggle").GetComponent<Toggle>();

        mulS_B.buttonObject = t_ItemSet0.Find("B_MultiShotB").gameObject;
        mulS_B.iconImage = mulS_B.buttonObject.transform.Find("Icon").GetComponent<Image>();
        mulS_B.numTxt = mulS_B.buttonObject.transform.Find("Num").GetComponent<Text>();
        mulS_B.numTxt.text = selectIManager.own_MulS_B.ToString();
        mulS_B.name = "연발장치(일반)";
        mulS_B.infoText = "일반 타워의 공격력을 1.5배 증가시켜줍니다.";
        mulS_B.price = 300;

        mulS_SN.buttonObject = t_ItemSet0.Find("B_MultiShotSN").gameObject;
        mulS_SN.iconImage = mulS_SN.buttonObject.transform.Find("Icon").GetComponent<Image>();
        mulS_SN.numTxt = mulS_SN.buttonObject.transform.Find("Num").GetComponent<Text>();
        mulS_SN.numTxt.text = selectIManager.own_MulS_SN.ToString();
        mulS_SN.name = "연발장치(저격)";
        mulS_SN.infoText = "저격 타워의 공격력을 1.5배 증가시켜줍니다.";
        mulS_SN.price = 500;

        mulS_P.buttonObject = t_ItemSet1.Find("B_MultiShotP").gameObject;
        mulS_P.iconImage = mulS_P.buttonObject.transform.Find("Icon").GetComponent<Image>();
        mulS_P.numTxt = mulS_P.buttonObject.transform.Find("Num").GetComponent<Text>();
        mulS_P.numTxt.text = selectIManager.own_MulS_P.ToString();
        mulS_P.name = "연발장치(충격)";
        mulS_P.infoText = "충격 타워의 공격력을 1.5배 증가시켜줍니다.";
        mulS_P.price = 750;

        aIBarrier.buttonObject = t_ItemSet1.Find("B_AIBarrier").gameObject;
        aIBarrier.iconImage = aIBarrier.buttonObject.transform.Find("Icon").GetComponent<Image>();
        aIBarrier.numTxt = aIBarrier.buttonObject.transform.Find("Num").GetComponent<Text>();
        aIBarrier.numTxt.text = selectIManager.own_AiBarrier.ToString();
        aIBarrier.name = "A.I배리어";
        aIBarrier.infoText = "적의 공격을 1회 막아줍니다. 장착 후 맞지 않으면 소모되지 않고 장착 상태를 유지합니다.";
        aIBarrier.price = 200;

        protectWall.buttonObject = t_ItemSet2.Find("B_ProtectWall").gameObject;
        protectWall.iconImage = protectWall.buttonObject.transform.Find("Icon").GetComponent<Image>();
        protectWall.numTxt = protectWall.buttonObject.transform.Find("Num").GetComponent<Text>();
        protectWall.numTxt.text = selectIManager.own_ProtectWall.ToString();
        protectWall.name = "방호벽";
        protectWall.infoText = "캐릭터 주변에 4개의 방호벽을 세웁니다. 방호벽의 체력은 5입니다.";
        protectWall.price = 500;

        extend_B.buttonObject = t_ItemSet2.Find("B_ExtendB").gameObject;
        extend_B.iconImage = extend_B.buttonObject.transform.Find("Icon").GetComponent<Image>();
        extend_B.numTxt = extend_B.buttonObject.transform.Find("Num").GetComponent<Text>();
        extend_B.numTxt.text = selectIManager.own_Extend_B.ToString();
        extend_B.name = "확장장치(일반)";
        extend_B.infoText = "일반 타워의 공격 범위를 1.5배 길게 만듭니다.";
        extend_B.price = 450;

        extend_SN.buttonObject = t_ItemSet3.Find("B_ExtendSN").gameObject;
        extend_SN.iconImage = extend_SN.buttonObject.transform.Find("Icon").GetComponent<Image>();
        extend_SN.numTxt = extend_SN.buttonObject.transform.Find("Num").GetComponent<Text>();
        extend_SN.numTxt.text = selectIManager.own_Extend_SN.ToString();
        extend_SN.name = "확장장치(저격)";
        extend_SN.infoText = "저격 타워의 공격 범위를 1.5배 길게 만듭니다.";
        extend_SN.price = 650;

        extend_P.buttonObject = t_ItemSet3.Find("B_ExtendP").gameObject;
        extend_P.iconImage = extend_P.buttonObject.transform.Find("Icon").GetComponent<Image>();
        extend_P.numTxt = extend_P.buttonObject.transform.Find("Num").GetComponent<Text>();
        extend_P.numTxt.text = selectIManager.own_Extend_P.ToString();
        extend_P.name = "확장장치(충격)";
        extend_P.infoText = "충격 타워의 공격 범위를 1.5배 길게 만듭니다.";
        extend_P.price = 900;

        recovery.buttonObject = t_ItemSet4.Find("B_Recovery").gameObject;
        recovery.iconImage = recovery.buttonObject.transform.Find("Icon").GetComponent<Image>();
        recovery.numTxt = recovery.buttonObject.transform.Find("Num").GetComponent<Text>();
        recovery.numTxt.text = selectIManager.own_Recovery.ToString();
        recovery.name = "수복자재";
        recovery.infoText = "플레이어의 최대 체력을 1 증가시킵니다.";
        recovery.price = 1000;
    }
}

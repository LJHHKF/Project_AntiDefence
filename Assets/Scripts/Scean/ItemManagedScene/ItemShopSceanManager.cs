using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopSceanManager : MonoBehaviour
{

    private Transform t_content;
    private Transform t_ItemSet0;
    private Transform t_ItemSet1;
    private Transform t_ItemSet2;
    private Transform t_ItemSet3;
    private Transform t_ItemSet4;

    private Text txt_Money;
    private Text txt_Level;

    private GameObject sub_panel_purchase;
    private GameObject sub_panel_itemInfo;
    private Image sub_img_infoIcon;
    private Text sub_txt_price;
    private Text sub_txt_infoHeader;
    private Text sub_txt_itemInfo;

    public Sprite shopOwnerImg;
    private Transform t_panel_ShopOwner;
    private Image img_ShopOwner;

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

    private Image[] i_imgs;
    private string[] i_names;
    private string[] i_infoTexts;
    private int[] i_prices;
    private int n_purchase = -1;

    private GameObject gm;
    private SelectedItemManager selectIManager;
    private LoadingManager loadingM;
    private BGM_Manager bgmM;
    private AudioManager audioM;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        selectIManager = gm.GetComponent<SelectedItemManager>();
        loadingM = gm.GetComponent<LoadingManager>();
        bgmM = gm.GetComponent<BGM_Manager>();
        audioM = gm.GetComponent<AudioManager>();

        bgmM.Play_LobbyAndShop();

        txt_Money = gameObject.transform.Find("Panel_SubInfo").Find("Panel_Money").Find("Text").GetComponent<Text>();
        txt_Level = gameObject.transform.Find("Panel_SubInfo").Find("Panel_Level").Find("Text").GetComponent<Text>();
        txt_Money.text = selectIManager.own_money.ToString();
        txt_Level.text = "아직 미구현";

        t_panel_ShopOwner = gameObject.transform.Find("Panel_ShopOwner");
        img_ShopOwner = t_panel_ShopOwner.Find("Image_ShopOwner").GetComponent<Image>();
        img_ShopOwner.sprite = shopOwnerImg;

        t_content = gameObject.transform.Find("ItemScroll").Find("Viewport").Find("Content");
        t_ItemSet0 = t_content.Find("Item_Set0");
        t_ItemSet1 = t_content.Find("Item_Set1");
        t_ItemSet2 = t_content.Find("Item_Set2");
        t_ItemSet3 = t_content.Find("Item_Set3");
        t_ItemSet4 = t_content.Find("Item_Set4");

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

    public void Click_return()
    {
        audioM.SFX_BTN_Click();
        //loadingM.LoadScene("Lobby");
        loadingM.LoadPrevScene();
    }

    private void Item_Clicked(int i_num)
    {
        audioM.SFX_BTN_Click();
        On_Purchase(i_num);
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

    private void On_Warning(string phrase)
    {
        sub_txt_warning.text = phrase;
        sub_panel_warning.SetActive(true);
    }

    private void UpdateText_Money()
    {
        txt_Money.text = selectIManager.own_money.ToString();
    }
    private void UpdateText(int i_num)
    {
        switch (i_num)
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
}

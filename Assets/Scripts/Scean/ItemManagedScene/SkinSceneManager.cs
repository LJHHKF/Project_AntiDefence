using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinSceneManager : MonoBehaviour
{

    private int max_skin = 4;

    private GameObject gm;
    private LoadingManager loadingM;
    private SelectedItemManager itemM;
    private SkinManager skinM;
    private BGM_Manager bgmM;

    private Text txt_priceTag;
    private Image img_skin;
    private Animator anim_skin;
    private Text txt_activeBTN;
    private Image img_BtnNext;
    private Image img_BtnPrev;
    private GameObject sub_panel_warning;
    private Text sub_txt_warning;

    struct SkinInfo
    {
        //public int index;
        public int price;
        public int is_had;
    }

    private int cnt_skin = 0;
    private SkinInfo[] skins;

    void Start()
    {
        skins = new SkinInfo[max_skin];
        UpdateItemInfo();

        gm = GameObject.FindGameObjectWithTag("GameManager");
        loadingM = gm.GetComponent<LoadingManager>();
        itemM = gm.GetComponent<SelectedItemManager>();
        skinM = gm.GetComponent<SkinManager>();
        bgmM = gm.GetComponent<BGM_Manager>();

        bgmM.Play_LobbyAndShop();

        txt_priceTag = gameObject.transform.Find("Panel_PriceTag").Find("Text").GetComponent<Text>();
        txt_activeBTN = gameObject.transform.Find("Button_Active").Find("Text").GetComponent<Text>();
        img_skin = gameObject.transform.Find("Image_Skin").GetComponent<Image>();
        anim_skin = gameObject.transform.Find("Image_Skin").GetComponent<Animator>();
        img_BtnNext = gameObject.transform.Find("Button_Next").GetComponent<Image>();
        img_BtnPrev = gameObject.transform.Find("Button_Prev").GetComponent<Image>();
        sub_panel_warning = gameObject.transform.Find("Panel_Warning").gameObject;
        sub_txt_warning = sub_panel_warning.transform.Find("Panel_Text").Find("Text").GetComponent<Text>();

        UpdateScene();

    }

    private void UpdateScene()
    {
        sub_panel_warning.SetActive(false);
        img_skin.sprite = skinM.skins[cnt_skin];
        if (skinM.anims[cnt_skin] != null)
        {
            anim_skin.runtimeAnimatorController = skinM.anims[cnt_skin];
            StartCoroutine(Anim_Ctrl(cnt_skin));
        }
        else
        {
            anim_skin.runtimeAnimatorController = null;
        }

        if (skins[cnt_skin].is_had == 0)
        {
            txt_priceTag.text = "가격:" + skins[cnt_skin].price.ToString();
            txt_priceTag.alignment = TextAnchor.MiddleLeft;
            txt_activeBTN.text = "구매"; 
        }
        else
        {
            txt_priceTag.text = "구매 완료";
            txt_priceTag.alignment = TextAnchor.MiddleCenter;
            if(skinM.GetSkinIndex() == cnt_skin && cnt_skin != 0)
            {
                txt_activeBTN.text = "장착해제";
            }
            else if (cnt_skin == 0)
            {
                if (skinM.GetSkinIndex() != 0)
                {
                    txt_activeBTN.text = "장착";
                    txt_priceTag.text = "기본 스킨입니다.";
                }
                else
                {
                    txt_activeBTN.text = "X";
                    txt_priceTag.text = "기본 스킨입니다.";
                }
            }
            else
            {
                txt_activeBTN.text = "장착";
            }
        }

        if(cnt_skin <= 0)
        {
            img_BtnPrev.color = new Color(255, 0, 0);
        }
        else
        {
            img_BtnPrev.color = new Color(0, 255, 0);
        }

        if(cnt_skin >= max_skin-1)
        {
            img_BtnNext.color = new Color(255, 0, 0);
        }
        else
        {
            img_BtnNext.color = new Color(0, 255, 0);
        }
    }

    public void BTN_Activate()
    {
        if (skins[cnt_skin].is_had == 0)
        {
            if(itemM.own_money >= skins[cnt_skin].price)
            {
                skins[cnt_skin].is_had = 1;
                string key = "HadSkin" + cnt_skin.ToString();
                PlayerPrefs.SetInt(key, skins[cnt_skin].is_had);
                UpdateScene();
            }
            else
            {
                sub_txt_warning.text = "소지금이 부족합니다.";
                sub_panel_warning.SetActive(true);
            }
        }
        else
        {
            if (skinM.GetSkinIndex() == cnt_skin && cnt_skin != 0)
            {
                skinM.SetSkinIndex(0);
                UpdateScene();
            }
            else if (cnt_skin == 0)
            {
                if (skinM.GetSkinIndex() != 0)
                {
                    skinM.SetSkinIndex(0);
                    UpdateScene();
                }
                else
                {

                }
            }
            else
            {
                skinM.SetSkinIndex(cnt_skin);
                UpdateScene();
            }
        }
    }

    public void BTN_Next()
    {
        if (cnt_skin < max_skin-1)
        {
            StopCoroutine(Anim_Ctrl(cnt_skin));
            cnt_skin += 1;
            UpdateScene();
        }
        else
        {
            sub_txt_warning.text = "더는 오른쪽 방향으로 스킨이 없습니다.";
            sub_panel_warning.SetActive(true);
        }
    }

    public void BTN_Prev()
    {
        if (cnt_skin > 0)
        {
            StopCoroutine(Anim_Ctrl(cnt_skin));
            cnt_skin -= 1;
            UpdateScene();
        }
        else
        {
            sub_txt_warning.text = "더는 왼쪽 방향으로 스킨이 없습니다.";
            sub_panel_warning.SetActive(true);
        }

    }

    public void Sub_BTN_Return()
    {
        sub_panel_warning.SetActive(false);
    }

    public void BTN_Return()
    {
        loadingM.LoadScene("Lobby");
    }

    public void BTN_Reset()
    {
        string key;

        for (int i = 1; i < max_skin-1; i++)
        {
            skins[i].is_had = 0;
            key = "HadSkin" + i.ToString();
            PlayerPrefs.SetInt(key, 0);
        }

        UpdateScene();
    }

    private void UpdateItemInfo()
    {
        skins[0].price = 100;
        skins[0].is_had = 1;

        skins[1].price = 200;
        skins[1].is_had = PlayerPrefs.GetInt("HadSkin1", 0);

        skins[2].price = 300;
        skins[2].is_had = PlayerPrefs.GetInt("HadSkin2", 0);

        skins[3].price = 400;
        skins[3].is_had = PlayerPrefs.GetInt("HadSkin3", 0);
    }

    IEnumerator Anim_Ctrl(int index)
    {
        while (true)
        {
            if (anim_skin.GetBool("IsAttack"))
            {
                anim_skin.SetBool("IsAttack", false);
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                anim_skin.SetBool("IsAttack", true);
                anim_skin.SetTrigger("IsAttack_Trigger");
                yield return new WaitForSeconds(1.5f);
            }
        }
    }


}


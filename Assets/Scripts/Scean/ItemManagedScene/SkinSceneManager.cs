using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinSceneManager : MonoBehaviour
{

    private int max_skin = 2;

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

    private Transform t_skinSpace;
    private Text txt_priceTag;
    private Image img_skin;
    private Animator anim_skin;
    private Text txt_activeBTN;
    private Image img_BtnNext;
    private Image img_BtnPrev;
    private GameObject sub_panel_warning;
    private Text sub_txt_warning;

    public Sprite shopOwnerImg;
    private Transform t_panel_shopOwner;
    private Image img_ShopOwner;

    private Text txt_money;
    private Text txt_level;

    

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
        audioM = gm.GetComponent<AudioManager>();
        touchEfM = gm.GetComponent<TouchEfManager>();

        t_touchEfPool = gameObject.transform.Find("TouchEffect_Pool");

        bgmM.Play_LobbyAndShop();

        t_skinSpace = gameObject.transform.Find("SkinSpace");
        txt_priceTag = t_skinSpace.Find("Panel_PriceTag").Find("Text").GetComponent<Text>();
        txt_activeBTN = t_skinSpace.Find("Button_Active").Find("Text").GetComponent<Text>();
        img_skin = t_skinSpace.Find("Image_Skin").GetComponent<Image>();
        anim_skin = t_skinSpace.Find("Image_Skin").GetComponent<Animator>();
        img_BtnNext = t_skinSpace.Find("Button_Next").GetComponent<Image>();
        img_BtnPrev = t_skinSpace.Find("Button_Prev").GetComponent<Image>();
        sub_panel_warning = gameObject.transform.Find("Panel_Warning").gameObject;
        sub_txt_warning = sub_panel_warning.transform.Find("Panel_Text").Find("Text").GetComponent<Text>();

        t_panel_shopOwner = gameObject.transform.Find("Panel_ShopOwner");
        img_ShopOwner = t_panel_shopOwner.Find("Image_ShopOwner").GetComponent<Image>();
        img_ShopOwner.sprite = shopOwnerImg;

        txt_money = gameObject.transform.Find("Panel_SubInfo").Find("Panel_Money").Find("Text").GetComponent<Text>();
        txt_level = gameObject.transform.Find("Panel_SubInfo").Find("Panel_Level").Find("Text").GetComponent<Text>();
        txt_money.text = itemM.own_money.ToString();
        txt_level.text = "아직 미구현";
        UpdateScene();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnTouchEf(Input.mousePosition);
        }
    }
    private void OnDisable()
    {
        if(anim_skin.GetBool("IsSkinShop"))
            anim_skin.SetBool("IsSkinShop", false);
    }

    private void UpdateScene()
    {
        sub_panel_warning.SetActive(false);
        img_skin.sprite = skinM.skins[cnt_skin];
        if (skinM.anims[cnt_skin] != null)
        {
            if(anim_skin.runtimeAnimatorController != null)
                anim_skin.SetBool("IsSkinShop", false);
            anim_skin.runtimeAnimatorController = skinM.anims[cnt_skin];
            anim_skin.SetBool("IsSkinShop", true);
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
            txt_activeBTN.text = "Pot on"; 
        }
        else
        {
            txt_priceTag.text = "Cyberpunk";
            txt_priceTag.alignment = TextAnchor.MiddleCenter;
            if(skinM.GetSkinIndex() == cnt_skin && cnt_skin != 0)
            {
                txt_activeBTN.text = "release";
            }
            else if (cnt_skin == 0)
            {
                if (skinM.GetSkinIndex() != 0)
                {
                    txt_activeBTN.text = "Pot on";
                    txt_priceTag.text = "Nomal Skin";
                }
                else
                {
                    txt_activeBTN.text = "release";
                    txt_priceTag.text = "Nomal Skin";
                }
            }
            else
            {
                txt_activeBTN.text = "Pot on";
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
        audioM.SFX_BTN_Click();
        if (skins[cnt_skin].is_had == 0)
        {
            if(itemM.own_money >= skins[cnt_skin].price)
            {
                skins[cnt_skin].is_had = 1;
                string key = "HadSkin" + cnt_skin.ToString();
                //PlayerPrefs.SetInt(key, skins[cnt_skin].is_had);
                itemM.own_money -= skins[cnt_skin].price;
                DataSaveManager.ownItemCount[key] = skins[cnt_skin].is_had;
                DataSaveManager.WriteData("DB_Item.csv", DataSaveManager.ownItemCount);
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
        audioM.SFX_BTN_Click();
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
        audioM.SFX_BTN_Click();
        if (cnt_skin > 0)
        {
            StopCoroutine(Anim_Ctrl(cnt_skin));
            cnt_skin -= 1;
            UpdateScene();
        }
        else
        {
            //sub_txt_warning.text = "더는 왼쪽 방향으로 스킨이 없습니다.";
            //sub_panel_warning.SetActive(true);
        }

    }

    public void Sub_BTN_Return()
    {
        audioM.SFX_BTN_Click();
        sub_panel_warning.SetActive(false);
    }

    public void BTN_Return()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("Lobby");
    }

    private void UpdateItemInfo()
    {
        skins[0].price = 100;
        skins[0].is_had = 1;

        skins[1].price = 200;
        skins[1].is_had = DataSaveManager.ownItemCount["HadSkin1"];
    }

    IEnumerator Anim_Ctrl(int index)
    {
        int anim_num = 0;
        while (true)
        {
            if (index != cnt_skin)
            {
                yield break;
            }
            switch(anim_num)
            {
                case 0:
                    //Idle 애님
                    anim_skin.SetBool("IsLose", false);
                    yield return new WaitForSeconds(1.0f);
                    anim_num = 1;
                    break;
                case 1:
                    anim_skin.SetTrigger("IsAttack_Trigger");
                    yield return new WaitForSeconds(1.0f);
                    anim_num = 2;
                    break;
                case 2:
                    anim_skin.SetBool("IsWin", true);
                    yield return new WaitForSeconds(1.0f);
                    anim_num = 3;
                    break;
                case 3:
                    anim_skin.SetBool("IsLose", true);
                    anim_skin.SetBool("IsWin", false);
                    yield return new WaitForSeconds(1.0f);
                    anim_num = 0;
                    break;
            }
            //if (anim_skin.GetBool("IsAttack"))
            //{
            //    anim_skin.SetBool("IsAttack", false);
            //    yield return new WaitForSeconds(1.5f);
            //}
            //else
            //{
            //    anim_skin.SetBool("IsAttack", true);
            //    anim_skin.SetTrigger("IsAttack_Trigger");
            //    yield return new WaitForSeconds(1.5f);
            //}
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

}


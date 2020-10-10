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

    private Transform t_subButtons;
    private Text txt_money;
    private Text txt_level;
    private Image img_skin;
    private Animator m_animator;
    

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        loadingM = gm.GetComponent<LoadingManager>();
        itemM = gm.GetComponent<SelectedItemManager>();
        skinM = gm.GetComponent<SkinManager>();
        bgmM = gm.GetComponent<BGM_Manager>();
        audioM = gm.GetComponent<AudioManager>();

        t_subButtons = gameObject.transform.Find("Panel_SubButtons");
        txt_money = t_subButtons.Find("Panel_Money").Find("Text").GetComponent<Text>();
        txt_level = t_subButtons.Find("Panel_Level").Find("Text").GetComponent<Text>();
        img_skin = gameObject.transform.Find("Panel_Main").Find("Image_Character").GetComponent<Image>();
        m_animator = gameObject.transform.Find("Panel_Main").Find("Image_Character").GetComponent<Animator>();

        m_animator.runtimeAnimatorController = skinM.anims[skinM.GetSkinIndex()];

        txt_money.text = itemM.own_money.ToString();
        txt_level.text = "아직 미구현";
        img_skin.sprite = skinM.skins[skinM.GetSkinIndex()];

        bgmM.Play_LobbyAndShop();
    }

    public void BTN_Setting()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("GameSetting", "Lobby");
    }

    public void BTN_Inventory()
    {

    }

    public void BTN_Skin()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("SkinSetting");
    }

    public void BTN_Shop()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("ItemEquip", "Lobby");
    }

    public void BTN_Stage()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadScene("ChapterSelect");
    }
}

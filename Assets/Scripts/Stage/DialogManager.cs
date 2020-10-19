using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [Header("Set Images")]
    public Sprite spr_Ruru;
    public Sprite spr_Dialogue_Left;
    public Sprite spr_Dialogue_Right;

    //[Header("Set Scean Info")]
    //public int cur_scene_index;

    [Header("Set Dialouge")]
    [TextArea]
    public string[] txt_dialogue;
    public bool[] is_Ruru_Talk;

    private int cnt_dlg = 0;

    struct DialogueObject
    {
        public GameObject g_dlg;
        public GameObject g_img;
        public Image img_Ruru;
        public GameObject g_Text_1;
        public GameObject g_Text_2;
        public GameObject g_speech;
        public Image img_SpeechBuble;
        public Text txt_1;
        public Text txt_2;
    }

    private Transform t_UI_Canvas;
    private GameObject ui_Buttons;
    private Button[] btn_Buttons;
    private Image[] imgs_Buttons;
    private float[] ori_alpha_buttons;
    private Text[] txts_Buttons;
    private float[] ori_alpha_txt_buttons;

    private GameObject ui_LifePanel;
    private Image[] imgs_LifePanel;
    private float[] ori_alpha_life;

    private GameObject ui_RemainPanel;
    private Image[] imgs_RemainPanel;
    private float[] ori_alpha_remain;
    private Text[] txts_Remain;
    private float[] ori_alpha_txt_remain;


    private GameObject panel_Dialog;
    private DialogueObject[] Dialogues = new DialogueObject[5];

    private StageManager stageM;

    

    // Start is called before the first frame update
    void Start()
    {
        t_UI_Canvas = GameObject.FindGameObjectWithTag("UI_Canvas").transform;

        panel_Dialog = t_UI_Canvas.Find("Panel_Dialog").gameObject;

        ui_Buttons = t_UI_Canvas.Find("Buttons").gameObject;
        btn_Buttons = ui_Buttons.GetComponentsInChildren<Button>();
        imgs_Buttons = ui_Buttons.GetComponentsInChildren<Image>();
        txts_Buttons = ui_Buttons.GetComponentsInChildren<Text>();

        ui_LifePanel = t_UI_Canvas.Find("LifePanel").gameObject;
        imgs_LifePanel = ui_LifePanel.GetComponentsInChildren<Image>();

        ui_RemainPanel = t_UI_Canvas.Find("RemainPanel").gameObject;
        imgs_RemainPanel = ui_RemainPanel.GetComponentsInChildren<Image>();
        txts_Remain = ui_RemainPanel.GetComponentsInChildren<Text>();

        //stageM = GameObject.FindGameObjectWithTag("StageMObject").GetComponent<StageManager>();
        stageM = gameObject.GetComponent<StageManager>();

        for (int i = 0; i < 5; i++)
        {
            Dialogues[i].g_dlg = panel_Dialog.transform.Find("Dialog_" + i.ToString()).gameObject;
            Dialogues[i].g_img = Dialogues[i].g_dlg.transform.Find("Image").gameObject;
            Dialogues[i].img_Ruru = Dialogues[i].g_img.GetComponent<Image>();
            Dialogues[i].g_speech = Dialogues[i].g_dlg.transform.Find("Speech").gameObject;
            Dialogues[i].g_Text_1 = Dialogues[i].g_speech.transform.Find("Text_1").gameObject;
            Dialogues[i].g_Text_2 = Dialogues[i].g_speech.transform.Find("Text_2").gameObject;
            Dialogues[i].img_SpeechBuble = Dialogues[i].g_speech.GetComponent<Image>();
            Dialogues[i].txt_1 = Dialogues[i].g_Text_1.GetComponent<Text>();
            Dialogues[i].txt_2 = Dialogues[i].g_Text_2.GetComponent<Text>();

            Dialogues[i].img_Ruru.sprite = spr_Ruru;

            Dialogues[i].g_dlg.SetActive(false);
            Dialogues[i].g_img.SetActive(false);
            Dialogues[i].g_speech.SetActive(false);
            Dialogues[i].g_Text_1.SetActive(false);
            Dialogues[i].g_Text_2.SetActive(false);
        }

        if (txt_dialogue.Length > 0)
        {
            Time.timeScale = 0.0f;
            panel_Dialog.SetActive(true);

            for (int i = 0; i < btn_Buttons.Length; i++)
                btn_Buttons[i].interactable = false;

            ori_alpha_buttons = new float[imgs_Buttons.Length];
            ori_alpha_life = new float[imgs_LifePanel.Length];
            ori_alpha_remain = new float[imgs_RemainPanel.Length];

            ori_alpha_txt_buttons = new float[txts_Buttons.Length];
            ori_alpha_txt_remain = new float[txts_Remain.Length];

            for (int i = 0; i < imgs_Buttons.Length; i++)
            {
                ori_alpha_buttons[i] = imgs_Buttons[i].color.a;
                imgs_Buttons[i].color = new Color(imgs_Buttons[i].color.r, imgs_Buttons[i].color.g, imgs_Buttons[i].color.b, 0);
            }
            for (int i = 0; i < imgs_LifePanel.Length; i++)
            {
                ori_alpha_life[i] = imgs_LifePanel[i].color.a;
                
                imgs_LifePanel[i].color = new Color(imgs_LifePanel[i].color.r, imgs_LifePanel[i].color.g, imgs_LifePanel[i].color.b, 0);
            }
            for (int i = 0; i < imgs_RemainPanel.Length; i++)
            {
                ori_alpha_remain[i] = imgs_RemainPanel[i].color.a;
                imgs_LifePanel[i].color = new Color(imgs_RemainPanel[i].color.r, imgs_RemainPanel[i].color.g, imgs_RemainPanel[i].color.b, 0);
            }

            for (int i = 0; i < txts_Buttons.Length; i++)
            {
                ori_alpha_txt_buttons[i] = txts_Buttons[i].color.a;
                txts_Buttons[i].color = new Color(txts_Buttons[i].color.r, txts_Buttons[i].color.g, txts_Buttons[i].color.b, 0);
            }
            for (int i = 0; i < txts_Remain.Length; i++)
            {
                ori_alpha_txt_remain[i] = txts_Remain[i].color.a;
                txts_Remain[i].color = new Color(txts_Remain[i].color.r, txts_Remain[i].color.g, txts_Remain[i].color.b, 0);
            }
        }
        else
        {
            panel_Dialog.SetActive(false);
            stageM.DlgDone();
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            DlgProgress();
        }
    }

    private void DlgProgress()
    {
        if (cnt_dlg < txt_dialogue.Length)
        {
            int index = cnt_dlg % 5;

            if (cnt_dlg % 5 == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Dialogues[i].g_dlg.SetActive(false);
                    Dialogues[i].g_img.SetActive(false);
                    Dialogues[i].g_speech.SetActive(false);
                    Dialogues[i].g_Text_1.SetActive(false);
                    Dialogues[i].g_Text_2.SetActive(false);
                }
            }

            Dialogues[index].g_dlg.SetActive(true);
            if (is_Ruru_Talk[cnt_dlg])
            {
                Dialogues[index].g_speech.SetActive(true);
                Dialogues[index].g_img.SetActive(true);
                Dialogues[index].g_Text_1.SetActive(true);

                Dialogues[index].img_SpeechBuble.sprite = spr_Dialogue_Left;
                Dialogues[index].txt_1.text = txt_dialogue[cnt_dlg];
            }
            else
            {
                Dialogues[index].g_speech.SetActive(true);
                Dialogues[index].g_Text_2.SetActive(true);

                Dialogues[index].img_SpeechBuble.sprite = spr_Dialogue_Right;
                Dialogues[index].txt_2.text = txt_dialogue[cnt_dlg];
            }

            cnt_dlg += 1;
           
        }
        else
        {
            Time.timeScale = 1.0f;
            panel_Dialog.SetActive(false);
            stageM.DlgDone();

            for (int i = 0; i < btn_Buttons.Length; i++)
                btn_Buttons[i].interactable = true;

            for (int i = 0; i < imgs_Buttons.Length; i++)
                imgs_Buttons[i].color = new Color(imgs_Buttons[i].color.r, imgs_Buttons[i].color.g, imgs_Buttons[i].color.b, ori_alpha_buttons[i]);
            for (int i = 0; i < imgs_LifePanel.Length; i++)           
                imgs_LifePanel[i].color = new Color(imgs_LifePanel[i].color.r, imgs_LifePanel[i].color.g, imgs_LifePanel[i].color.b, ori_alpha_life[i]);
            for (int i = 0; i < imgs_RemainPanel.Length; i++)
                imgs_LifePanel[i].color = new Color(imgs_RemainPanel[i].color.r, imgs_RemainPanel[i].color.g, imgs_RemainPanel[i].color.b, ori_alpha_remain[i]);

            for (int i = 0; i < txts_Buttons.Length; i++)
                txts_Buttons[i].color = new Color(txts_Buttons[i].color.r, txts_Buttons[i].color.g, txts_Buttons[i].color.b, ori_alpha_txt_buttons[i]);
            for (int i = 0; i < txts_Remain.Length; i++)
                txts_Remain[i].color = new Color(txts_Remain[i].color.r, txts_Remain[i].color.g, txts_Remain[i].color.b, ori_alpha_txt_remain[i]);


            Destroy(this);
        }
    }
}

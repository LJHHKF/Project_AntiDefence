using System.Collections;
using System.Collections.Generic;
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

    private GameObject panel_Dialog;
    private DialogueObject[] Dialogues = new DialogueObject[5];

    private StageManager stageM;

    // Start is called before the first frame update
    void Start()
    {
        panel_Dialog = GameObject.FindGameObjectWithTag("UI_Canvas").transform.Find("Panel_Dialog").gameObject;

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
        }
        else
        {
            panel_Dialog.SetActive(false);
            stageM.DlgDone();
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
        }
    }
}

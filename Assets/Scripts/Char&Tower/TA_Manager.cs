using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TA_Manager : MonoBehaviour
{

    private GameObject[] imageObject = new GameObject[3];
    private Image[] cTimeImg = new Image[3];

    public float b_AtCT = 4f;
    public float sn_AtCT = 6f;
    public float p_AtCT = 12f;
    private float[] attackCT = { 0f, 0f, 0f };

    public float b_AtDmg = 1.0f;
    public float sn_AtDmg = 5.0f;
    public float p_AtDmg = 5.0f;

    private GameObject gm;
    private SelectedItemManager si_manager;

    private PlayerManager playerM;

    private GameObject ui_Canvas;
    private GameObject feverPanel;
    private GameObject feverEffectPanel;
    private bool on_fever;

    private GameObject sfx_manager;
    private AudioSource sfx_SNT_Attack;

    // Start is called before the first frame update
    void Start()
    {
        imageObject[0] = GameObject.FindGameObjectWithTag("BT_Cool");
        cTimeImg[0] = imageObject[0].GetComponent<Image>();
        imageObject[1] = GameObject.FindGameObjectWithTag("SNT_Cool");
        cTimeImg[1] = imageObject[1].GetComponent<Image>();
        imageObject[2] = GameObject.FindGameObjectWithTag("PT_Cool");
        cTimeImg[2] = imageObject[2].GetComponent<Image>();

        attackCT[0] = b_AtCT;
        attackCT[1] = sn_AtCT;
        attackCT[2] = p_AtCT;

        ui_Canvas = GameObject.FindGameObjectWithTag("UI_Canvas");
        feverPanel = ui_Canvas.transform.Find("FeverPanel").gameObject;
        feverEffectPanel = ui_Canvas.transform.Find("Buttons").Find("FeverEffectPanel").gameObject;

        playerM = gameObject.GetComponent<PlayerManager>();

        gm = GameObject.FindGameObjectWithTag("GameManager");
        si_manager = gm.GetComponent<SelectedItemManager>();

        if(si_manager.i_muls_b)
        {
            b_AtDmg *= 1.5f;
        }
        if (si_manager.i_muls_sn)
        {
            sn_AtDmg *= 1.5f;
        }
        if (si_manager.i_muls_p)
        {
            p_AtDmg *= 1.5f;
        }
        StartCoroutine(Get_SFX_Manager());
    }

    private void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            cTimeImg[i].fillAmount += Time.deltaTime / attackCT[i];
        }
    }

    public void BTActived()
    {
        cTimeImg[0].fillAmount = 0.0f;
        //StartCoroutine(CoolTime(0));

        playerM.OnAttackAnim();
    }

    public void SNTActived()
    {
        cTimeImg[1].fillAmount = 0.0f;
        //StartCoroutine(CoolTime(1));

        playerM.OnAttackAnim();
        StartCoroutine(Sound_SNT_Attack());
    }

    public void PTActived()
    {
        cTimeImg[2].fillAmount = 0.0f;
        //StartCoroutine(CoolTime(2));

        playerM.OnAttackAnim();
    }

    public void FeverActivate(float act_time)
    {
        if (on_fever == false)
        {
            StartCoroutine(Fever(act_time));
        }
    }

    IEnumerator Fever(float act_time)
    {
        on_fever = true;
        for (int i = 0; i < 3; i++)
        {
            attackCT[i] = 1.0f;
        }
        feverPanel.SetActive(true);
        feverEffectPanel.SetActive(true);

        yield return new WaitForSeconds(act_time);

        on_fever = false;
        attackCT[0] = b_AtCT;
        attackCT[1] = sn_AtCT;
        attackCT[2] = p_AtCT;
        feverPanel.SetActive(false);
        feverEffectPanel.SetActive(false);

        yield break;
    }


    private IEnumerator Sound_SNT_Attack()
    {
        sfx_SNT_Attack.Play();
        yield return new WaitForSeconds(1.0f);
        sfx_SNT_Attack.Stop();
        yield break;
    }

    private IEnumerator Get_SFX_Manager()
    {
        yield return new WaitForSeconds(1.0f);
        sfx_manager = GameObject.FindGameObjectWithTag("MainCamera").transform.Find("SFX_Manager(Clone)").gameObject;
        sfx_SNT_Attack = sfx_manager.transform.Find("S_SNT_Attack").gameObject.GetComponent<AudioSource>();
        StopCoroutine(Get_SFX_Manager());
    }

    //IEnumerator CoolTime(int type) //코루틴 방식은 왠지 가속도가 붙음. 1초마다, 가 아니라 1초, 2초, 3초씩 누적이 되는듯함.
    //{
    //    cTimeImg[type].fillAmount = 0.0f;
    //    while (cTimeImg[type].fillAmount <= 1f)
    //    {
    //        cTimeImg[type].fillAmount += Time.deltaTime / attackCT[type];
    //        yield return null;
    //    }
    //    yield break;

    //    //switch(type)
    //    //{
    //    //    case 0:

    //    //        while (cTimeImg1.fillAmount <= 1f)
    //    //        {
    //    //            timeStack[type] += Time.deltaTime;
    //    //            cTimeImg1.fillAmount = timeStack[type] / attackCT[type];
    //    //            yield return null;
    //    //        }
    //    //        yield break;
    //    //    case 1:

    //    //        while (cTimeImg2.fillAmount <= 1f)
    //    //        {
    //    //            timeStack[type] += Time.deltaTime;
    //    //            cTimeImg2.fillAmount = timeStack[type] / attackCT[type];
    //    //            yield return null;
    //    //        }
    //    //        yield break;
    //    //    case 2:

    //    //        while (cTimeImg3.fillAmount <= 1f)
    //    //        {
    //    //            timeStack[type] += Time.deltaTime;
    //    //            cTimeImg3.fillAmount = timeStack[type] / attackCT[type];
    //    //            yield return null;
    //    //        }
    //    //        yield break;
    //    //}
    //}
}

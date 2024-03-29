﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerAttack_Basic : MonoBehaviour
{
    public GameObject a_Effect;
    public Transform firePos;
    public GameObject fireRange;
    public Animator animator;

    private GameObject towerBoard;
    private TA_Manager ta_manager;

    private GameObject imageObject;
    private Image cTimeImg;
    private Collider m_Coll;

    private SpriteRenderer m_sprR;
    private Color m_color;

    private GameObject gm;
    private SelectedItemManager si_manager;

    private float touch_Time;

    private Transform t_objectPool_AtkEf;
    private List<GameObject> listPool_AtkEf = new List<GameObject>();
    private bool is_serched_atk = false;
    private StageManager stageM;

    private void Awake()
    {
        imageObject = GameObject.FindGameObjectWithTag("BT_Cool");
        cTimeImg = imageObject.GetComponent<Image>();

        towerBoard = GameObject.FindGameObjectWithTag("TowerBoard");
        ta_manager = towerBoard.GetComponent<TA_Manager>();

        m_Coll = gameObject.GetComponent<Collider>();
        m_Coll.enabled = false;

        m_sprR = gameObject.GetComponent<SpriteRenderer>();
        m_color = m_sprR.color;

        t_objectPool_AtkEf = GameObject.FindGameObjectWithTag("ObjectPools").transform.Find("AtkEffects");

        stageM = GameObject.FindGameObjectWithTag("StageMObject").GetComponent<StageManager>();

        gm = GameObject.FindGameObjectWithTag("GameManager");
        si_manager = gm.GetComponent<SelectedItemManager>();
        if (si_manager.i_extend_b)
        {
            Vector3 upScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(1f, 1f, 1.5f));
            gameObject.transform.localScale = upScale;
            Vector3 upPosition = Vector3.Scale(gameObject.transform.localPosition, new Vector3(1f, 1f, 1.5f));
            gameObject.transform.localPosition = upPosition;
            upScale = Vector3.Scale(fireRange.transform.localScale, new Vector3(1f, 1f, 1.5f));
            fireRange.transform.localScale = upScale;
            upPosition = Vector3.Scale(fireRange.transform.localPosition, new Vector3(1f, 1f, 1.5f));
            fireRange.transform.localPosition = upPosition;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(DelayEnable(0.2f));
    }
    private void Update()
    {
        if (stageM.GetEventIsDone())
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_Coll.enabled = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                touch_Time = 0f;
                m_Coll.enabled = true;
            }

            touch_Time += Time.deltaTime;

            if (touch_Time >= 0.5f)
            {
                m_Coll.enabled = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && (cTimeImg.fillAmount >= 1.0f || cTimeImg.fillAmount <= 0.01f))
        {

            m_Coll.enabled = false;
            fireRange.SetActive(true); // 이 fireRange 부분에 충돌한 '적'개체의 피가 빠지는 방식으로 구현 예정.
            animator.SetBool("IsAttack", true);
            StartCoroutine(FR_Stay());
            StartCoroutine(On_Clear());
            StartCoroutine(Off_Anim());
            PullingAtkEffect(1.0f);

            ta_manager.BTActived();
            
        }
    }

    public void OnOtherAttack()
    {
        StartCoroutine(On_Clear());
    }


    IEnumerator FR_Stay()
    {
        yield return new WaitForSeconds(0.05f);
        fireRange.SetActive(false);
        StopCoroutine(FR_Stay());
    }

    IEnumerator On_Clear()
    {
        m_sprR.color = new Color(m_color.r, m_color.g, m_color.b, 0.0f);
        yield return new WaitForSeconds(0.3f);
        m_sprR.color = new Color(m_color.r, m_color.g, m_color.b, m_color.a);
        
        StopCoroutine(On_Clear());
    }

    IEnumerator Off_Anim()
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("IsAttack", false);
        StopCoroutine(Off_Anim());
    }

    private void PullingAtkEffect(float time)
    {
        if (listPool_AtkEf.Count <= 0)
        {
            AttackEffectPooling();
        }
        is_serched_atk = false;
        for (int i = 0; i < listPool_AtkEf.Count; i++)
        {
            if (listPool_AtkEf[i].activeSelf == false)
            {
                listPool_AtkEf[i].transform.position = firePos.position;
                listPool_AtkEf[i].transform.rotation = firePos.rotation;
                listPool_AtkEf[i].SetActive(true);
                StartCoroutine(StopEffect(listPool_AtkEf[i], time));
                is_serched_atk = true;
                break;
            }
        }
        if (is_serched_atk == false)
        {
            AttackEffectPooling();
            PullingAtkEffect(time);
        }
    }

    private void AttackEffectPooling()
    {
        var effect = Instantiate(a_Effect, t_objectPool_AtkEf);
        listPool_AtkEf.Add(effect);
        effect.name = "Basic_Effect_" + listPool_AtkEf.Count.ToString("000");
        effect.SetActive(false);
    }

    IEnumerator StopEffect(GameObject effect, float time)
    {
        yield return new WaitForSeconds(time);
        effect.SetActive(false);
        yield break;
    }

    IEnumerator DelayEnable(float sec)
    {
        yield return new WaitForSeconds(sec);
        m_sprR.color = new Color(m_color.r, m_color.g, m_color.b, m_color.a);
        yield break;
    }
}

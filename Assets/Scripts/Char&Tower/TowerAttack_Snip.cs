﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerAttack_Snip : MonoBehaviour
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

    private MeshRenderer m_meshR;
    private Color m_color;

    private GameObject gm;
    private SelectedItemManager si_manager;

    private float touch_Time = 0f;

    private void Start()
    {
        imageObject = GameObject.FindGameObjectWithTag("SNT_Cool");
        cTimeImg = imageObject.GetComponent<Image>();

        towerBoard = GameObject.FindGameObjectWithTag("TowerBoard");
        ta_manager = towerBoard.GetComponent<TA_Manager>();

        m_Coll = this.gameObject.GetComponent<Collider>();
        m_Coll.enabled = false;

        m_meshR = gameObject.GetComponent<MeshRenderer>();
        m_color = m_meshR.material.color;

        gm = GameObject.FindGameObjectWithTag("GameManager");
        si_manager = gm.GetComponent<SelectedItemManager>();
        if (si_manager.i_extend_sn)
        {
            Vector3 upScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(1.5f, 1f, 1f));
            gameObject.transform.localScale = upScale;
            upScale = Vector3.Scale(fireRange.transform.localScale, new Vector3(1.5f, 1f, 1f));
            fireRange.transform.localScale = upScale;
        }
    }

    private void Update()
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
            GameObject effect = Instantiate(a_Effect, firePos.position, firePos.rotation);
            //if (si_manager.i_extend_sn)
            //{
            //    Vector3 upScale = Vector3.Scale(a_Effect.transform.localScale, new Vector3(1.5f, 1.0f, 1.0f));
            //    effect.transform.localScale = upScale;
            //}
            Destroy(effect, 1.0f);

            ta_manager.SNTActived();
            
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
        m_meshR.material.color = new Color(m_color.r, m_color.g, m_color.b, 0.0f);
        yield return new WaitForSeconds(1.0f);
        m_meshR.material.color = new Color(m_color.r, m_color.g, m_color.b, m_color.a);
        StopCoroutine(On_Clear());
    }

    IEnumerator Off_Anim()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("IsAttack", false);
        StopCoroutine(Off_Anim());
    }
}

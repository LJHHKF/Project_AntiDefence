using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack_Push : MonoBehaviour
{
    public GameObject a_Effect;
    public Transform firePos;
    public GameObject fireRange;
    public GameObject p_turret;
    public Animator animator;

    private GameObject towerBoard;
    private TA_Manager ta_manager;

    private bool pt_attacked = false;

    private MeshRenderer m_meshR;
    private Color m_color;

    private GameObject gm;
    private SelectedItemManager si_manager;

    // Start is called before the first frame update
    void Start()
    {
        towerBoard = GameObject.FindGameObjectWithTag("TowerBoard");
        ta_manager = towerBoard.GetComponent<TA_Manager>();

        m_meshR = gameObject.GetComponentInParent<MeshRenderer>();
        m_color = m_meshR.material.color;

        gm = GameObject.FindGameObjectWithTag("GameManager");
        si_manager = gm.GetComponent<SelectedItemManager>();
        if (si_manager.i_extend_p)
        {
            Vector3 upScale = Vector3.Scale(p_turret.transform.localScale, new Vector3(1f, 1.5f, 1f));
            p_turret.transform.localScale = upScale;
            //Vector3 upPosition = Vector3.Scale(p_turret.transform.localPosition, new Vector3(1f, 1.5f, 1f));
            p_turret.transform.localPosition = new Vector3(0f, 0f, 3f);
            upScale = Vector3.Scale(fireRange.transform.localScale, new Vector3(1f, 1.5f, 1f));
            fireRange.transform.localScale = upScale;
            Vector3 upPosition = Vector3.Scale(fireRange.transform.localPosition, new Vector3(1f, 1.5f, 1f));
            fireRange.transform.localPosition = upPosition;
        }
    }

    public void P_Contact()
    {
        if (pt_attacked == false)
        {
            pt_attacked = true;
            fireRange.SetActive(true);
            animator.SetBool("IsAttack", true);
            StartCoroutine(FR_Stay());
            StartCoroutine(On_Clear());
            StartCoroutine(Off_Anim());
            GameObject effect = Instantiate(a_Effect, firePos);
            Destroy(effect, 1.0f);
            ta_manager.PTActived();
        }
    }

    IEnumerator FR_Stay()
    {
        yield return new WaitForSeconds(0.05f);
        fireRange.SetActive(false);
        pt_attacked = false;
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

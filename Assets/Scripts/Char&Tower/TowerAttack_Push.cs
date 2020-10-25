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

    private Transform t_objectPool_AtkEf;
    private List<GameObject> listPool_AtkEf = new List<GameObject>();
    private int cnt_AtkEf = 0;
    private bool is_serched_atk = false;

    // Start is called before the first frame update
    void Start()
    {
        towerBoard = GameObject.FindGameObjectWithTag("TowerBoard");
        ta_manager = towerBoard.GetComponent<TA_Manager>();

        m_meshR = gameObject.GetComponentInParent<MeshRenderer>();
        m_color = m_meshR.material.color;

        gm = GameObject.FindGameObjectWithTag("GameManager");
        si_manager = gm.GetComponent<SelectedItemManager>();

        t_objectPool_AtkEf = GameObject.FindGameObjectWithTag("ObjectPools").transform.Find("AtkEffects");

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

    private void OnEnable()
    {
        StartCoroutine(DelayEnable(0.2f));
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
            PullingAtkEffect(1.0f);
            ta_manager.PTActived();
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

    private void PullingAtkEffect(float time)
    {
        if (cnt_AtkEf <= 0)
        {
            ChangeEffectPooling();
        }
        is_serched_atk = false;
        for (int i = 0; i < cnt_AtkEf; i++)
        {
            if (listPool_AtkEf[i].activeSelf == false)
            {
                listPool_AtkEf[i].transform.position = firePos.position;
                listPool_AtkEf[i].transform.rotation = firePos.rotation;
                listPool_AtkEf[i].SetActive(true);
                StartCoroutine(StopEffect(listPool_AtkEf[i], time));
                is_serched_atk = true;
            }
        }
        if (is_serched_atk == false)
        {
            ChangeEffectPooling();
            PullingAtkEffect(time);
        }
    }

    private void ChangeEffectPooling()
    {
        cnt_AtkEf++;
        var effect = Instantiate(a_Effect, t_objectPool_AtkEf);
        effect.name = "Push_Effect_" + cnt_AtkEf.ToString("000");
        effect.SetActive(false);
        listPool_AtkEf.Add(effect);
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
        m_meshR.material.color = new Color(m_color.r, m_color.g, m_color.b, m_color.a);
        yield break;
    }
}

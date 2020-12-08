using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour
{
    private enum State
    {
        MOVE,
        ATTACK,
        PUSHED,
        DIE
    }


    private State state = State.MOVE;
    private GameObject target;
    private GameObject attackTarget;

    private Barricade barricade;
    private EnemyBugTarget m_BugTarget;
    private Transform t_imgPanel;
    private Transform t_img;
    private float slope;

    [Header("EnemyIndex")]
    public int enemyIndex;

    [Header("HpBar Setting")]
    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    private Transform t_uiCanvas;
    private Transform t_HPBars;
    private Image hpBarImage;
    private GameObject hpBar;

    [Header("EnemyEffect Setting")]
    public GameObject dieEffectPrefab;
    private bool isDie;
    private bool isEffectiveDie = true;
    public GameObject m_AttackEffect;

    [Header("EnemyAnimator")]
    public Animator m_anim;

    private GameObject towerboard;
    private TA_Manager ta_manager;
    private PlayerManager pl_manager;

    [Header("EnemyStatus Setting")]
    public float attackDist = 1.0f;
    public float attackDamage = 1.0f;
    public float moveSpeed = 1.0f;
    public float rotSpeed = 1.0f;
    public float enemyHP = 2f;
    public float attack_delay = 1.0f;
    public bool isSuiBomber = false;
    public bool isNonAttack = false;
    public int dropMoneyValue = 10;
    public float pushed_power = 10.0f;
    private float initHP;

    [Header("Other Setting")]
    public GameObject bugTarget;

    private WaitForSeconds ws;
    private bool attack_now;
    
    private Vector3 direction;
    private GameObject o_stgM;
    private StageManager stgManager;
    private EnemyInfoUI enemyInfo;
    private Collider m_coll;
    private Rigidbody m_rigid;
    private EnemyTargetCtrl m_targetCtrl;

    private Transform sfx_M;
    private AudioSource sfx_Hit_BT;
    private AudioSource sfx_Hit_SNT;
    private AudioSource sfx_Hit_PT;

    void Start()
    {
        towerboard = GameObject.FindGameObjectWithTag("TowerBoard");
        ta_manager = towerboard.GetComponent<TA_Manager>();
        pl_manager = towerboard.GetComponent<PlayerManager>();

        ws = new WaitForSeconds(0.3f);

        o_stgM = GameObject.FindGameObjectWithTag("StageMObject");
        stgManager = o_stgM.GetComponent<StageManager>();

        enemyInfo = GameObject.FindGameObjectWithTag("UI_Canvas").transform.Find("Panel_EnemyInfo").GetComponent<EnemyInfoUI>();

        t_imgPanel = gameObject.transform.Find("Clear_Panel").GetComponent<Transform>();
        t_img = t_imgPanel.Find("Image").GetComponent<Transform>();
        m_coll = gameObject.GetComponent<Collider>();
        m_rigid = gameObject.GetComponent<Rigidbody>();
        m_targetCtrl = gameObject.transform.Find("SearchRange").GetComponent<EnemyTargetCtrl>();

        m_coll.enabled = true;
        m_rigid.useGravity = true;

        sfx_M = GameObject.FindGameObjectWithTag("SFX_Manager").transform;
        sfx_Hit_BT = sfx_M.Find("S_Hit_BT").GetComponent<AudioSource>();
        sfx_Hit_SNT = sfx_M.Find("S_Hit_SNT").GetComponent<AudioSource>();
        sfx_Hit_PT = sfx_M.Find("S_Hit_PT").GetComponent<AudioSource>();

        SetHPBar();
        initHP = enemyHP;
        StartCoroutine(CheckState());

        if(enemyIndex == 0)
        {
            bugTarget.GetComponent<EnemyBugTarget>().SetIsFirstTrue();
        }
    }

    private void OnEnable()
    {
        if (isDie)
        {
            state = State.MOVE;
            enemyHP = initHP;
            StartCoroutine(CheckState());
            ResetHPBar();
            isDie = false;
            m_coll.enabled = true;
            m_rigid.useGravity = true;
            isEffectiveDie = true;

            if(enemyIndex == 0)
            {
                bugTarget.SetActive(true);
                bugTarget.GetComponent<EnemyBugTarget>().SetIsFirstTrue();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ImageSlopeRotate();
        if (state == State.DIE)
        {
            if (isDie == false)
            {
                if (isEffectiveDie)
                {
                    isDie = true;
                    m_coll.enabled = false;
                    m_rigid.useGravity = false;
                    stgManager.EnemyDied(dropMoneyValue);
                    stgManager.PullingDropMoneyBar(gameObject.transform.position, dropMoneyValue, 1.0f);

                    stgManager.PullingEnemyDieEffect(gameObject.transform.localPosition, 1.0f);

                    hpBar.SetActive(false);
                    if (m_anim != null)
                    {
                        if (isSuiBomber)
                        {
                            if (!attack_now)
                            {
                                m_anim.SetTrigger("IsDie");
                            }
                        }
                        else
                        {
                            m_anim.SetTrigger("IsDie");
                        }
                    }

                    if(enemyIndex ==0)
                    {
                        bugTarget.SetActive(false);
                    }

                    if (attack_now)
                        StartCoroutine(DieDelay(1.5f));
                    else
                        StartCoroutine(DieDelay(1.0f));
                }
            }
        }
        else if (state == State.MOVE)
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

                direction = target.transform.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, rotSpeed * Time.deltaTime);

                if (direction.x > 0)
                {
                    t_img.localEulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    t_img.localEulerAngles = new Vector3(0, 0, 0);
                }
            }
        }
        else if (state == State.PUSHED)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, -(pushed_power * Time.deltaTime));
        }
        else if (state == State.ATTACK && attack_now == false)
        {
            StartCoroutine(Attacking());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SubAtkR"))
        {
            if(enemyInfo.GetIsShowed(enemyIndex) == false)
            {
                enemyInfo.ShowEnemyInfo(enemyIndex);
            }
        }
        if (stgManager.GetEventIsDone() == false)
        {
            if (other.CompareTag("BT_AtkR"))
            {
                enemyHP = 0;
                StartCoroutine(HitSoundPlay(0));
                stgManager.PullingEnemyDamagedEf(0, gameObject.transform.position);
            }
            else if (other.CompareTag("SNT_AtkR"))
            {
                enemyHP = 0;
                StartCoroutine(HitSoundPlay(1));
                stgManager.PullingEnemyDamagedEf(1, gameObject.transform.position);
            }
            else if (other.CompareTag("PT_AtkR") & state != State.PUSHED)
            {
                StartCoroutine(Pushed_delay());
                enemyHP = 0;
                StartCoroutine(HitSoundPlay(2));
                stgManager.PullingEnemyDamagedEf(2, gameObject.transform.position);
            }
        }
        else
        {
            if (other.CompareTag("BT_AtkR"))
            {
                enemyHP -= ta_manager.b_AtDmg;
                StartCoroutine(HitSoundPlay(0));
                stgManager.PullingEnemyDamagedEf(0, gameObject.transform.position);
            }
            else if (other.CompareTag("SNT_AtkR"))
            {
                enemyHP -= ta_manager.sn_AtDmg;
                StartCoroutine(HitSoundPlay(1));
                stgManager.PullingEnemyDamagedEf(1, gameObject.transform.position);
            }
            else if (other.CompareTag("PT_AtkR") & state != State.PUSHED)
            {
                stgManager.PullingEnemyDamagedEf(2, gameObject.transform.position);
                StartCoroutine(Pushed_delay());
                StartCoroutine(HitSoundPlay(2));
                enemyHP -= ta_manager.p_AtDmg;
            }
        }

        hpBarImage.fillAmount = enemyHP / initHP;
        if (enemyHP <= 0.0f)
        {
            state = State.DIE;
        }
    }

    void SetHPBar()
    {
        t_uiCanvas = GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<Transform>();
        t_HPBars = t_uiCanvas.Find("EnemyHPBar").transform;
        hpBar = Instantiate(hpBarPrefab, t_HPBars);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

        var _hpBar = hpBar.GetComponent<EnemyHPBar>();
        _hpBar.TargetTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffset;
    }

    void ResetHPBar()
    {
        hpBar.SetActive(true);
        var _hpBar = hpBar.GetComponent<EnemyHPBar>();
        _hpBar.TargetTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffset;
    }

    public void SetTarget(GameObject t)
    {
        target = t;
        if (t.CompareTag("Barricade"))
        {
            barricade = t.GetComponentInParent<Barricade>();
        }
        if (t.CompareTag("BugTarget"))
        {
            m_BugTarget = t.GetComponent<EnemyBugTarget>();
        }
    }

    private void ImageSlopeRotate()
    {
        if (gameObject.transform.localEulerAngles.y >= 0 && gameObject.transform.localEulerAngles.y <= 180)
        {
            slope = (60.0f + (gameObject.transform.localEulerAngles.y / 3));
        }
        else if (gameObject.transform.localEulerAngles.y >= 180 && gameObject.transform.localEulerAngles.y <= 360)
        {
            slope = (120.0f - ((gameObject.transform.localEulerAngles.y - 180) / 3));
        }
        t_imgPanel.localEulerAngles = new Vector3(slope, 0, 0);
    }

    IEnumerator CheckState()
    {
        float dist = 0;
        while (state != State.DIE)
        {
            if (state == State.DIE) yield break;

            if(target != null)
                dist = Vector3.Distance(target.transform.position, transform.position);

            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }
            else
            {
                state = State.MOVE;
            }
            yield return ws;
        }
    }

    IEnumerator Attacking()
    {
        attackTarget = target;
        if (!isNonAttack)
        {
            attack_now = true;
            while (state == State.ATTACK)
            {

                if (attackTarget != target)
                {
                    state = State.MOVE;
                }
                else
                {
                    if (target.CompareTag("Player"))
                    {
                        pl_manager.Player_Damaged(attackDamage);
                        m_anim.SetTrigger("IsAttack");
                        stgManager.PullingEnemyAtkEf(enemyIndex, m_AttackEffect, target.transform.position);
                        if (isSuiBomber)
                        {
                            state = State.DIE;
                            StopCoroutine(Attacking());
                        }
                        yield return new WaitForSeconds(attack_delay);
                    }
                    else if (target.CompareTag("Barricade"))
                    {
                        barricade.Barricade_damaged(attackDamage);
                        m_anim.SetTrigger("IsAttack");
                        stgManager.PullingEnemyAtkEf(enemyIndex, m_AttackEffect, target.transform.position);
                        if (isSuiBomber)
                        {
                            state = State.DIE;
                            StopCoroutine(Attacking());
                        }
                        yield return new WaitForSeconds(attack_delay);
                    }
                    else if (enemyIndex == 2)
                    {
                        if (target.CompareTag("BugTarget"))
                        {
                            m_BugTarget.SetBuged();
                            m_anim.SetTrigger("IsAttack");
                            stgManager.PullingEnemyAtkEf(enemyIndex, m_AttackEffect, target.transform.position);
                            m_targetCtrl.SetFindOther(false);
                            yield return new WaitForSeconds(attack_delay);
                        }
                    }
                }
            }
            attack_now = false;
        }
        else
        {
            attack_now = true;
        }
        yield break;
    }


    IEnumerator Pushed_delay()
    {
        state = State.PUSHED;
        yield return new WaitForSeconds(1.0f);
        state = State.MOVE;
        StopCoroutine(Pushed_delay());
    }

    IEnumerator DieDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        gameObject.SetActive(false);
        yield break;
    }

    IEnumerator HitSoundPlay(int index)
    {
        switch(index)
        {
            case 0:
                sfx_Hit_BT.Play();
                break;
            case 1:
                sfx_Hit_SNT.Play();
                break;
            case 2:
                sfx_Hit_PT.Play();
                break;
        }
        yield return new WaitForSeconds(1.0f);
        switch(index)
        {
            case 0:
                sfx_Hit_BT.Stop();
                break;
            case 1:
                sfx_Hit_SNT.Stop();
                break;
            case 2:
                sfx_Hit_PT.Stop();
                break;
        }
    }

    public int GetEnemyIndex()
    {
        return enemyIndex;
    }

    public void SetIsDie(bool setV)
    {
        isDie = setV;
        isEffectiveDie = false;
    }
}

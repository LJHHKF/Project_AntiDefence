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

    private Barricade barricade;
    private Transform t_imgPanel;
    private float slope;

    [Header("HpBar Setting")]
    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    private Canvas uiCanvas;
    private Image hpBarImage;
    private GameObject hpBar;

    [Header("DropMoneyNotice Setting")]
    public GameObject dropMoneySetPrefab;

    [Header("EnemyDieEffect Setting")]
    public GameObject dieEffectPrefab;
    private bool isDie;

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
    public int dropMoneyValue = 10;
    public float pushed_power = 10.0f;
    private float initHP;

    private WaitForSeconds ws;
    private bool attack_now;
    
    private Vector3 direction;
    //private Vector3 move_Vector;
    private GameObject o_stgM;
    private StageManager stgManager;



    void Start()
    {
        towerboard = GameObject.FindGameObjectWithTag("TowerBoard");
        ta_manager = towerboard.GetComponent<TA_Manager>();
        pl_manager = towerboard.GetComponent<PlayerManager>();

        ws = new WaitForSeconds(0.3f);

        o_stgM = GameObject.FindGameObjectWithTag("StageMObject");
        stgManager = o_stgM.GetComponent<StageManager>();

        t_imgPanel = gameObject.transform.Find("Clear_Panel").GetComponent<Transform>();

        SetHPBar();
        initHP = enemyHP;
        StartCoroutine(CheckState());
    }


    // Update is called once per frame
    void Update()
    {
        //direction = (character_t.position - enemy_t.position).normalized;
        //move_Vector = direction * moveSpeed;
        //this.gameObject.transform.Translate(move_Vector);

        ImageSlopeRotate();

        if (state == State.DIE)
        {
            if (isDie == false)
            {
                SetDropMoneyBar();
                stgManager.EnemyDied(dropMoneyValue);

                Quaternion qut = Quaternion.identity;
                GameObject effect = Instantiate(dieEffectPrefab,
                       new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z),
                       qut);

                Destroy(effect, 1.0f);
                Destroy(hpBar);
                m_anim.SetTrigger("IsDie");
                Destroy(gameObject, 1.0f);

                isDie = true;
            }
        }
        else if (state == State.MOVE)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

            direction = target.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, rotSpeed * Time.deltaTime);

            //Vector3 move_vector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z).normalized * moveSpeed * Time.deltaTime;
            //m_rigidbody.MovePosition(transform.position + move_vector);

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
        if (other.CompareTag("BT_AtkR"))
        {
            enemyHP -= ta_manager.b_AtDmg;
        }
        else if (other.CompareTag("SNT_AtkR"))
        {
            enemyHP -= ta_manager.sn_AtDmg;
        }
        else if (other.CompareTag("PT_AtkR") & state != State.PUSHED)
        {
            StartCoroutine(Pushed_delay());
            enemyHP -= ta_manager.p_AtDmg;
        }

        hpBarImage.fillAmount = enemyHP / initHP;
        if (enemyHP <= 0.0f)
        {
            state = State.DIE;
            hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
        }
    }

    void SetHPBar()
    {
        uiCanvas = GameObject.FindGameObjectWithTag("UI_Canvas").GetComponent<Canvas>();
        hpBar = Instantiate(hpBarPrefab, uiCanvas.transform);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

        var _hpBar = hpBar.GetComponent<EnemyHPBar>();
        _hpBar.TargetTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffset;
    }

    void SetDropMoneyBar()
    {
        GameObject _moneyBar = Instantiate(dropMoneySetPrefab, uiCanvas.transform);
        var _dropMoney = _moneyBar.GetComponent<EnemyDropMoneySet>();
        _dropMoney.TargetTr = new Vector3(this.gameObject.transform.position.x,
                                          this.gameObject.transform.position.y,
                                          this.gameObject.transform.position.z);
        _dropMoney.SetDropMoney(dropMoneyValue);
    }

    public void SetTarget(GameObject t)
    {
        target = t;
        if (t.CompareTag("Barricade"))
        {
            barricade = t.GetComponentInParent<Barricade>();
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
        while (state != State.DIE)
        {
            if (state == State.DIE) yield break;

            //float dist = (character_t.position - transform.position).sqrMagnitude; // 이게 더 빠르다곤 하는데 정확히 이해 못함.
            //윗 방식서는 아래 attackdist 뒤에 '* attackdist' 추가 필요.
            float dist = Vector3.Distance(target.transform.position, transform.position);

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
        attack_now = true;
        while (state == State.ATTACK)
        {
            if (target.CompareTag("Player"))
            {
                pl_manager.Player_Damaged(attackDamage);
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
                if (isSuiBomber)
                {
                    state = State.DIE;
                    StopCoroutine(Attacking());
                }
                yield return new WaitForSeconds(attack_delay);
            }
        }
        attack_now = false;
        StopCoroutine(Attacking());
    }


    IEnumerator Pushed_delay()
    {
        state = State.PUSHED;
        yield return new WaitForSeconds(1.0f);
        state = State.MOVE;
        StopCoroutine(Pushed_delay());
    }
}

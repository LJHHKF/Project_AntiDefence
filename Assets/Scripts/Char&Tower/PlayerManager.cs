using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private enum State
    {
        ALLIVE,
        DIE
    }

    private State state = State.ALLIVE;
    [Header("플레이어 생명력 관리")]
    public float player_HP = 3.0f;

    private GameObject lifePanel;
    private Image[] hearts;

    [Header("하트 이미지 관리")]
    public Image bonus_hearts;
    public Sprite[] heart_sprites;

    private GameObject gm;
    private SelectedItemManager selectedItemManager;
    private SkinManager skinM;

    private GameObject stage;
    private StageManager stageManager;

    //private Transform m_parent;
    public GameObject img_character;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;
    private AnimatorController m_animatorcontrollor;
   

    // Start is called before the first frame update
    void Start()
    {
        lifePanel = GameObject.Find("LifePanel");
        hearts = lifePanel.GetComponentsInChildren<Image>(); // 부모인 판넬 이미지도 0의 위치에 저장되어 하트 3개 기준 4개 저장.

        gm = GameObject.FindGameObjectWithTag("GameManager");
        selectedItemManager = gm.GetComponent<SelectedItemManager>();
        skinM = gm.GetComponent<SkinManager>();

        //m_parent = gameObject.GetComponentInParent<Transform>();
        //img_character = m_parent.Find("Clear_Panel").Find("Img_Character").GetComponent<SpriteRenderer>(); 

        m_spriteRenderer = img_character.GetComponent<SpriteRenderer>();
        m_spriteRenderer.sprite = skinM.skins[skinM.GetSkinIndex()];
        m_animator = img_character.GetComponent<Animator>();
        m_animatorcontrollor = img_character.GetComponent<AnimatorController>();
        m_animatorcontrollor = skinM.anims[skinM.GetSkinIndex()];


        stage = GameObject.FindGameObjectWithTag("StageMObject");
        stageManager = stage.GetComponent<StageManager>();
     

        if(selectedItemManager.i_recovery)
        {
            bonus_hearts.gameObject.SetActive(true);
            player_HP = 4.0f;
        }
        else if (bonus_hearts.IsActive())
        {
            bonus_hearts.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (state == State.DIE)
        {
            Debug.Log("게임 오버");
            //게임 오버 처리 코드 추가 필요

            stageManager.StageFailed();
            
        }
    }

    public void Player_Damaged(float dmg)
    {
        if (selectedItemManager.i_aiBarrier)
        {
            selectedItemManager.BarrierBreak();
        }
        else if (state != State.DIE)
        {
            player_HP -= dmg;
            if (selectedItemManager.i_recovery && player_HP >= 3)
            {
                bonus_hearts.sprite = heart_sprites[2];
            }
            else
            {
                hearts[(int)Mathf.Floor(player_HP) + 1].sprite = heart_sprites[2];
            }

            if (player_HP <= 0)
            {
                state = State.DIE;
            }
        }
    }

    public void OnAttackAnim()
    {
        StartCoroutine(OnAttackMotion());
    }

    IEnumerator OnAttackMotion()
    {
        m_animator.SetBool("IsAttack", true);
        yield return new WaitForSeconds(1.0f);
        m_animator.SetBool("IsAttack", false);
        yield break;
    }
}

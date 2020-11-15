using System.Collections;
using System.Collections.Generic;
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
    private bool is_ended = false;

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

    private Transform t_Character;
    private GameObject img_character;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    public GameObject effect_AI_Barrier;
    
    private GameObject sfx_Manager;
    private AudioSource sfx_AiBarrier_Break;
   

    // Start is called before the first frame update
    void Start()
    {
        lifePanel = GameObject.Find("LifePanel");
        hearts = lifePanel.GetComponentsInChildren<Image>(); // 부모인 판넬 이미지도 0의 위치에 저장되어 하트 3개 기준 4개 저장.

        gm = GameObject.FindGameObjectWithTag("GameManager");
        selectedItemManager = gm.GetComponent<SelectedItemManager>();
        skinM = gm.GetComponent<SkinManager>();

        t_Character = GameObject.FindGameObjectWithTag("Player").transform;
        img_character = t_Character.Find("Clear_Panel").Find("Img_Character").gameObject;

        m_spriteRenderer = img_character.GetComponent<SpriteRenderer>();
        m_spriteRenderer.sprite = skinM.skins[skinM.GetSkinIndex()];
        m_animator = img_character.GetComponent<Animator>();
        m_animator.runtimeAnimatorController = skinM.anims[skinM.GetSkinIndex()];

        stage = GameObject.FindGameObjectWithTag("StageMObject");
        stageManager = stage.GetComponent<StageManager>();

        sfx_Manager = GameObject.FindGameObjectWithTag("SFX_Manager");
        sfx_AiBarrier_Break = sfx_Manager.transform.Find("S_AiBarrier_Break").gameObject.GetComponent<AudioSource>();
     

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
            if (is_ended == false)
            {
                stageManager.StageFailed();
                is_ended = true;
            }
        }
    }

    public void Player_Damaged(float dmg)
    {
        if (selectedItemManager.i_aiBarrier)
        {
            selectedItemManager.BarrierBreak();
            effect_AI_Barrier.SetActive(false);
            StartCoroutine(OnSoundBarrierBreak());
        }
        else if(selectedItemManager.i_event_AiBarrier)
        {
            effect_AI_Barrier.SetActive(false);
            StartCoroutine(OnSoundBarrierBreak());
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
                m_animator.SetTrigger("IsLose_Trigger");
            }
        }
    }

    public void OnAttackAnim()
    {
        m_animator.SetTrigger("IsAttack_Trigger");
    }


    public void OnWinAnim()
    {
        t_Character.position = new Vector3(0, 1.0f, 0);
        m_animator.SetTrigger("IsWin_Trigger");
    }

    public void SetAiBarrier()
    {
        if (selectedItemManager.i_aiBarrier)
        {
            effect_AI_Barrier.SetActive(true);
        }
        else if (selectedItemManager.i_event_AiBarrier)
        {
            effect_AI_Barrier.SetActive(true);
        }
        else
        {
            effect_AI_Barrier.SetActive(false);
        }


    }

    private IEnumerator OnSoundBarrierBreak()
    {
        sfx_AiBarrier_Break.Play();
        yield return new WaitForSeconds(1.0f);
        sfx_AiBarrier_Break.Stop();
        yield break;
    }
}

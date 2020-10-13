using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{


    [Header ("Enemy Spawn Preset")]
    public Transform p_enemySpawnPoints;
    public GameObject b_spawnPoints;
    private Transform[] c_enemySpawnPoints;
    public GameObject spawn_Effects;
    public GameObject e_direction_Effects;
    public GameObject[] enemies;
    private GameObject ui_Canvas;
    private GameObject remainPanel;
    

    [Header ("Enemy Spawn Setting")]
    public float startDelay = 3.0f;
    public float[] delayTimesIndex = { 1.0f, 1.0f, 2.0f, 2.0f, 1.0f };
    public int[] spawnPointIndex = { 1, 9, 2, 10, 15 }; // 0은 부모 위치이므로, 캐릭터 바로 옆 생성이므로 주의
    public int[] spawnEnemyIndex = { 0, 0, 0, 0, 0 };

    [Header ("Chp,Stage info")]
    public int chpter_num = 0;
    public int stage_num = 1;  // 추후 리스타트용. 당장 유니티 인스펙터뷰에서 체크용이기도 함.

    [Header ("Fever Setting")]
    public bool fever_had = false;
    public int fever_limit = 20;
    public float fever_time = 3.0f;
    private int fever_cnt = 1;

    [Header("Other Setting")]
    public GameObject startEffect;
    public GameObject startEffect2;
    public GameObject sfx_manager;

    [Header("Wall&Tile Image Setting")]
    public Material[] tile_far_material;
    public Sprite[] tile_close_sprite;
    public float closeTileAlpha = 80f;
    public Sprite[] wall_far_sprite;
    public Sprite[] wall_close_sprite;
 
    [Header("Wall&Tile Index Setting")]
    public int closeTileIndex = 0;
    public int farTileIndex = 0;
    public int closeWallIndex = 0;
    public int farWallIndex = 0;


    // private Image s_bar;
    private Text t_cur;
    private Text t_full;
    private int cnt_EnemyDie = 0;
    private Transform p_transform;

    private GameObject gm;
    private LoadingManager loadingM;
    private SelectedItemManager itemM;
    private BGM_Manager bgmM;

    private TA_Manager ta_M;


    private void Awake()
    {
        Instantiate(sfx_manager,GameObject.FindGameObjectWithTag("MainCamera").transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        c_enemySpawnPoints = p_enemySpawnPoints.GetComponentsInChildren<Transform>();
        StartCoroutine(CountTimeForSpawn());

        ui_Canvas = GameObject.FindGameObjectWithTag("UI_Canvas");
        remainPanel = ui_Canvas.transform.Find("RemainPanel").gameObject;


        //s_bar = remainPanel.transform.Find("StageRBar").GetComponent<Image>();
        t_cur = remainPanel.transform.Find("T_Current").GetComponent<Text>();
        t_full = remainPanel.transform.Find("T_Full").GetComponent<Text>();

        t_full.text = spawnPointIndex.Length.ToString();

        gm = GameObject.FindGameObjectWithTag("GameManager");
        loadingM = gm.GetComponent<LoadingManager>();
        itemM = gm.GetComponent<SelectedItemManager>();
        bgmM = gm.GetComponent<BGM_Manager>();

        

        ta_M = GameObject.FindGameObjectWithTag("TowerBoard").GetComponent<TA_Manager>();

        if(itemM.i_protectWall)
        {
            b_spawnPoints.SetActive(true);
        }
        else
        {
            b_spawnPoints.SetActive(false);
        }

        GameObject effect = Instantiate(startEffect, gameObject.transform);
        GameObject effect2 = Instantiate(startEffect2, gameObject.transform);
        Destroy(effect, 1.0f);
        Destroy(effect2, 5.0f);

        PlayBGM(chpter_num ,stage_num);
    }

    private void Update()
    {
        if (fever_had)
        {
            //if (cnt_EnemyDie % fever_cnt == 0 && cnt_EnemyDie != 0)
            if (cnt_EnemyDie >= fever_limit * fever_cnt)
            {
                ta_M.FeverActivate(fever_time);
                fever_cnt++;
            }
        }
    }

    private void LateUpdate()
    {
        t_cur.text = cnt_EnemyDie.ToString();

        if (t_cur.text == t_full.text)
        {
            StageClear();
        }
    }

    private void SpawnEnemy(int sp_index, int m_index)
    {
        Instantiate(enemies[m_index], c_enemySpawnPoints[sp_index].position, Quaternion.identity) ;
        GameObject effect = Instantiate(spawn_Effects, c_enemySpawnPoints[sp_index]);
        Destroy(effect, 2.0f);
        GameObject d_effect = Instantiate(e_direction_Effects, c_enemySpawnPoints[sp_index]);
        Destroy(d_effect, 1.0f);
    }

    IEnumerator CountTimeForSpawn()
    {
        int count = 0;
        yield return new WaitForSeconds(startDelay);


        while (true)
        {
            if (spawnPointIndex.Length > count)
            {
                SpawnEnemy(spawnPointIndex[count], spawnEnemyIndex[count]);
                count++;
            }
            else
            {
                StopCoroutine(CountTimeForSpawn());
            }

            //ran = UnityEngine.Random.Range(0 ,delayTimes.Length);
            yield return new WaitForSeconds(delayTimesIndex[count - 1]);
        }
    }


    public void EnemyDied(int dropM)
    {
        cnt_EnemyDie += 1;
        itemM.Get_Money(dropM);
    }


    public Sprite Get_CloseTile()
    {
        return tile_close_sprite[closeTileIndex];
    }

    public float Get_CloseTileAlpha()
    {
        return closeTileAlpha;
    }

    public Material Get_FarTile()
    {
        return tile_far_material[farTileIndex];
    }

    public Sprite Get_CloseWall()
    {
        return wall_close_sprite[closeWallIndex];
    }

    public Sprite Get_FarWall()
    {
        return wall_far_sprite[farTileIndex];
    }

    private void StageEnd()
    {
        loadingM.StageEnd(chpter_num);
    }

    public void StageFailed()
    {
        //패배처리 추가 필요
        StageEnd();
    }

    private void StageClear()
    {
        itemM.End_Stage();
        StageEnd();
    }

    private void PlayBGM(int c_num, int s_num)
    {
        if (chpter_num == 0)
        {
            if (stage_num == 1)
            {
                bgmM.Play_Stage0_1();
            }
        }
    }
}

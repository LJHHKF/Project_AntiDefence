﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using System;

public class StageManager : MonoBehaviour
{

    private struct Pools
    {
        public List<GameObject> listPool;
        public int cnt;
        public bool is_serched;
    }

    [Header ("Enemy Spawn Preset")]
    public Transform p_enemySpawnPoints;
    public GameObject b_spawnPoints;
    private Transform[] c_enemySpawnPoints;
    public GameObject spawn_Effect;
    public GameObject e_direction_Effect;
    public GameObject e_Die_Effect;
    public GameObject dropMoneyBarPrefab;
    public GameObject[] enemies;
    public GameObject enemy_BugHuman;
    private GameObject ui_Canvas;
    private GameObject remainPanel;
    

    [Header ("Enemy Spawn Setting")]
    public float startDelay = 3.0f;
    public float[] delayTimesIndex = { 1.0f, 1.0f, 2.0f, 2.0f, 1.0f };
    public int[] spawnPointIndex = { 1, 9, 2, 10, 15 }; // 0은 부모 위치이므로, 캐릭터 바로 옆 생성이므로 주의
    public int[] spawnEnemyIndex = { 0, 0, 0, 0, 0 };
    private Transform t_objPools;
    private Transform t_objectPool_Enemy;
    private Pools[] arr_enemyPools;
    private Transform t_objectPool_SpawnEf;
    private Pools pools_SpawnEf;
    private Transform t_objectPool_DirEffect;
    private Pools pools_DirEf;
    private Transform t_objectPool_DieEffect;
    private Pools pools_DieEf;
    private Transform t_DropMoneyPool;
    private Pools pools_DropMoney;
    private Transform t_objectPool_AtkEffect;
    private Pools[] arr_pools_EnemyDamaged;
    private Transform t_objectPool_EnemyAtkEffect;
    private Pools[] arr_pools_EnemyAtkEf;

    [Header ("Chp,Stage info")]
    public int chapter_num = 0;
    public int stage_num = 1;  // 추후 리스타트용. 당장 유니티 인스펙터뷰에서 체크용이기도 함.

    [Header ("Fever Setting")]
    public bool fever_had = false;
    public int fever_limit = 20;
    public float fever_time = 3.0f;
    private int fever_cnt = 1;

    [Header("Other Setting")]
    public GameObject startEffect;
    public GameObject startEffect2;
    private bool is_started = false;
    public GameObject[] enemyDamgedEffect;

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

    [Header("Wall move Setting")]
    public float wallMoveSpeed = 0.001f;
    public float wallMinY = 4.95f;
    public float wallMaxY = 5.5f;


    private Text t_cur;
    private Text t_full;
    private int cnt_EnemyDie = 0;
    private int int_EnemyDieMax = 0;
    private bool dlg_isDone = false;
    private bool event_isDone = true;
    private bool now_Spawn = false;
    private int bugHumanIndex;
    private int stgGetMoney = 0;
    private bool stageEnded = false;


    private GameObject gm;
    private LoadingManager loadingM;
    private SelectedItemManager itemM;
    private BGM_Manager bgmM;
    private TouchEfManager touchEfM;

    private GameObject towerBoard;
    private TA_Manager ta_M;
    private PlayerManager playerM;

    private GameObject sfx_manager;
    private int alive_Barricade = 0;
    private bool isSet_Barriacde = false;
    private AudioSource sfx_Barricade_Idle;
    private AudioSource sfx_Barricade_Destruction;
    private AudioSource sfx_StageStart;

    private Transform t_touchEfPool;
    private Pools pools_TouchEffect;

    // Start is called before the first frame update
    void Start()
    {
        c_enemySpawnPoints = p_enemySpawnPoints.GetComponentsInChildren<Transform>();

        ui_Canvas = GameObject.FindGameObjectWithTag("UI_Canvas");
        remainPanel = ui_Canvas.transform.Find("RemainPanel").gameObject;

        t_cur = remainPanel.transform.Find("T_Current").GetComponent<Text>();
        t_full = remainPanel.transform.Find("T_Full").GetComponent<Text>();

        if (chapter_num == 0 && stage_num == 1)
        {
            t_full.text = (spawnPointIndex.Length + 3).ToString();
        }
        else if(chapter_num == 0 && stage_num == 2)
        {
            t_full.text = (spawnPointIndex.Length + 3).ToString();
        }
        else
        {
            t_full.text = spawnPointIndex.Length.ToString();
        }
        int_EnemyDieMax = Int32.Parse(t_full.text);

        gm = GameObject.FindGameObjectWithTag("GameManager");
        loadingM = gm.GetComponent<LoadingManager>();
        itemM = gm.GetComponent<SelectedItemManager>();
        bgmM = gm.GetComponent<BGM_Manager>();
        touchEfM = gm.GetComponent<TouchEfManager>();

        t_objPools = GameObject.FindGameObjectWithTag("ObjectPools").transform;
        t_objectPool_Enemy = t_objPools.Find("Enemies");
        t_objectPool_SpawnEf = t_objPools.Find("SpawnEffect");
        t_objectPool_DirEffect = t_objPools.Find("DirEffect");
        t_objectPool_DieEffect = t_objPools.Find("EnemyDieEffect");
        t_DropMoneyPool = ui_Canvas.transform.Find("EnemyDropMoney").transform;
        t_touchEfPool = ui_Canvas.transform.Find("TouchEffect_Pool").transform;
        t_objectPool_AtkEffect = t_objPools.Find("AtkEffects");
        t_objectPool_EnemyAtkEffect = t_objPools.Find("EnemyAtkEffects");


        towerBoard = GameObject.FindGameObjectWithTag("TowerBoard");
        ta_M = towerBoard.GetComponent<TA_Manager>();
        playerM = towerBoard.GetComponent<PlayerManager>();

        sfx_manager = GameObject.FindGameObjectWithTag("SFX_Manager");
        sfx_Barricade_Idle = sfx_manager.transform.Find("S_Barricade_Idle").GetComponent<AudioSource>();
        sfx_Barricade_Destruction = sfx_manager.transform.Find("S_Barricade_Destruction").GetComponent<AudioSource>();
        sfx_StageStart = sfx_manager.transform.Find("S_StageStart").GetComponent<AudioSource>();

        if(itemM.i_protectWall)
        {
            b_spawnPoints.SetActive(true);
            alive_Barricade = 4;
            isSet_Barriacde = true;
        }
        else if(itemM.i_event_ProtectWall)
        {
            b_spawnPoints.SetActive(true);
            alive_Barricade = 4;
            isSet_Barriacde = true;
        }
        else
        {
            b_spawnPoints.SetActive(false);
        }

        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] == enemy_BugHuman)
            {
                bugHumanIndex = i;
            }
        }

        PoolsInit();

        bgmM.Play_Stage();
    }

    private void Update()
    {
        if (fever_had)
        {
            if (cnt_EnemyDie >= fever_limit * fever_cnt)
            {
                ta_M.FeverActivate(fever_time);
                fever_cnt++;
            }
        }
        if (now_Spawn == false)
        {
            if(event_isDone)
            {
                StartCoroutine(CountTimeForSpawn());
                now_Spawn = true;

                if(itemM.i_protectWall)
                {
                    sfx_Barricade_Idle.Play();
                }
            }
        }

        if (isSet_Barriacde)
        {
            if (alive_Barricade == 0)
            {
                sfx_Barricade_Idle.Stop();
                isSet_Barriacde = false;
            }
        }

        if (dlg_isDone)
        {
            if (Time.timeScale != 0.0f)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    PullingTouchEf(Input.mousePosition);
                }
            }

            if (!is_started)
            {
                GameObject effect = Instantiate(startEffect, gameObject.transform);
                GameObject effect2 = Instantiate(startEffect2, gameObject.transform);
                Destroy(effect, 1.0f);
                Destroy(effect2, 5.0f);
                StartCoroutine(PlaySfxStageStart());
                is_started = true;
            }
        }
       
    }

    private void LateUpdate()
    {
        t_cur.text = cnt_EnemyDie.ToString();

        if (cnt_EnemyDie >= int_EnemyDieMax)
        {
            StageClear();
        }
    }

    public void Pub_SpawnEnemy(int sp_index, int enemy_index)
    {
        if (arr_enemyPools[enemy_index].cnt <= 0)
        {
            Pub_EnemyPooling(enemy_index);
        }

        arr_enemyPools[enemy_index].is_serched = false;

        for (int i = 0; i < arr_enemyPools[enemy_index].cnt; i++)
        {
            if (arr_enemyPools[enemy_index].listPool[i].activeSelf == false)
            {
                GameObject enemy = arr_enemyPools[enemy_index].listPool[i];
                enemy.transform.position = c_enemySpawnPoints[sp_index].position;
                enemy.transform.rotation = Quaternion.identity;
                enemy.SetActive(true);
                PullingSpawnEffect(sp_index, 2.0f);
                PullingDirEffect(sp_index, 1.0f);
                arr_enemyPools[enemy_index].is_serched = true;
                break;
            }
        }
        if (arr_enemyPools[enemy_index].is_serched == false)
        {
            Pub_EnemyPooling(enemy_index);
            Pub_SpawnEnemy(sp_index, enemy_index);
        }
    }

    public void SpawnBugEnemy(Transform spawnT)
    {
        if(arr_enemyPools[bugHumanIndex].cnt <= 0)
        {
            BugEnemyPooling();
        }

        arr_enemyPools[bugHumanIndex].is_serched = false;
        
        for(int i = 0; i < arr_enemyPools[bugHumanIndex].cnt; i++)
        {
            if(arr_enemyPools[bugHumanIndex].listPool[i].activeSelf == false)
            {
                GameObject enemy = arr_enemyPools[bugHumanIndex].listPool[i];
                enemy.transform.position = spawnT.position;
                enemy.transform.rotation = spawnT.rotation;
                enemy.SetActive(true);
                arr_enemyPools[bugHumanIndex].is_serched = true;
                break;
            }
        }
        if(arr_enemyPools[bugHumanIndex].is_serched == false)
        {
            BugEnemyPooling();
            SpawnBugEnemy(spawnT);
        }
    }

    private void SpawnEnemy(int sp_index, int cnt)
    {
        if (arr_enemyPools[spawnEnemyIndex[cnt]].cnt <= 0)
        {
            EnemyPooling(cnt);
        }

        arr_enemyPools[spawnEnemyIndex[cnt]].is_serched = false;

        for (int i = 0; i < arr_enemyPools[spawnEnemyIndex[cnt]].cnt; i++)
        {
            if (arr_enemyPools[spawnEnemyIndex[cnt]].listPool[i].activeSelf == false)
            {
                GameObject enemy = arr_enemyPools[spawnEnemyIndex[cnt]].listPool[i];
                enemy.transform.position = c_enemySpawnPoints[sp_index].position;
                enemy.transform.rotation = Quaternion.identity;
                enemy.SetActive(true);
                PullingSpawnEffect(sp_index, 2.0f);
                PullingDirEffect(sp_index, 1.0f);
                arr_enemyPools[spawnEnemyIndex[cnt]].is_serched = true;
                break;
            }
        }
        if (arr_enemyPools[spawnEnemyIndex[cnt]].is_serched == false)
        {
            EnemyPooling(cnt);
            SpawnEnemy(sp_index, cnt);
        }
    }

    private void PullingSpawnEffect(int sp_index, float time)
    {
        if(pools_SpawnEf.cnt <= 0)
        {
            SpawnEffectPooling();
        }

        pools_SpawnEf.is_serched = false;
        for (int i = 0; i < pools_SpawnEf.cnt; i++)
        {
            if (pools_SpawnEf.listPool[i].activeSelf == false)
            {
                pools_SpawnEf.listPool[i].transform.position = c_enemySpawnPoints[sp_index].position;
                pools_SpawnEf.listPool[i].transform.rotation = c_enemySpawnPoints[sp_index].rotation;
                pools_SpawnEf.listPool[i].SetActive(true);
                StartCoroutine(StopEffect(pools_SpawnEf.listPool[i], time));
                pools_SpawnEf.is_serched = true;
                break;
            }
        }
        if(pools_SpawnEf.is_serched == false)
        {
            SpawnEffectPooling();
            PullingSpawnEffect(sp_index, time);
        }
    }

    private void PullingDirEffect(int sp_index, float time)
    {
        if (pools_DirEf.cnt <= 0)
        {
            DirEffectPooling();
        }
        pools_DirEf.is_serched = false;
        for (int i = 0; i < pools_DirEf.cnt; i++)
        {
            if(pools_DirEf.listPool[i].activeSelf == false)
            {
                pools_DirEf.listPool[i].transform.position = c_enemySpawnPoints[sp_index].position;
                pools_DirEf.listPool[i].transform.rotation = c_enemySpawnPoints[sp_index].rotation;
                pools_DirEf.listPool[i].SetActive(true);
                StartCoroutine(StopEffect(pools_DirEf.listPool[i], time));
                pools_DirEf.is_serched = true;
                break;
            }
        }
        if (pools_DirEf.is_serched == false)
        {
            DirEffectPooling();
            PullingDirEffect(sp_index, time);
        }
    }

    public void PullingEnemyDieEffect(Vector3 t_enemy, float time)
    {
        if (pools_DieEf.cnt <= 0)
        {
            EnemyDieEffectPooling();
        }
        pools_DieEf.is_serched = false;
        for (int i = 0; i < pools_DieEf.cnt; i++)
        {
            if (pools_DieEf.listPool[i].activeSelf == false)
            {
                pools_DieEf.listPool[i].transform.position = new Vector3(t_enemy.x, t_enemy.y, t_enemy.z);
                pools_DieEf.listPool[i].SetActive(true);
                StartCoroutine(StopEffect(pools_DieEf.listPool[i], time));
                pools_DieEf.is_serched = true;
                break;
            }
        }
        if (pools_DieEf.is_serched == false)
        {
            EnemyDieEffectPooling();
            PullingEnemyDieEffect(t_enemy, time);
        }
    }

    public void PullingDropMoneyBar(Vector3 t_enemy, int dropMoneyValue, float time)
    {
        if(pools_DropMoney.cnt <= 0)
        {
            DropMoneyBarPooling();
        }

        pools_DropMoney.is_serched = false;

        for (int i = 0; i < pools_DropMoney.cnt; i++)
        {
            if (pools_DropMoney.listPool[i].activeSelf == false)
            {
                var _dropMoney = pools_DropMoney.listPool[i].GetComponent<EnemyDropMoneyBar>();
                _dropMoney.TargetTr = new Vector3(t_enemy.x, t_enemy.y, t_enemy.z);
                _dropMoney.SetDropMoney(dropMoneyValue);
                _dropMoney.SetTime(time);
                pools_DropMoney.listPool[i].SetActive(true);
                pools_DropMoney.is_serched = true;
                break;
            }
        }
        if (pools_DropMoney.is_serched == false)
        {
            DropMoneyBarPooling();
            PullingDropMoneyBar(t_enemy, dropMoneyValue, time);
        }
    }

    private void PullingTouchEf(Vector3 mousePosition)
    {
        if (pools_TouchEffect.cnt == 0)
        {
            PoolingTouchEf();
        }
        pools_TouchEffect.is_serched = false;

        for (int i = 0; i < pools_TouchEffect.cnt; i++)
        {
            if (pools_TouchEffect.listPool[i].activeSelf == false)
            {
                Animator m_animator = pools_TouchEffect.listPool[i].GetComponent<Animator>();
                RectTransform m_rect = pools_TouchEffect.listPool[i].GetComponent<RectTransform>();
                m_rect.position = mousePosition;
                pools_TouchEffect.listPool[i].SetActive(true);
                m_animator.SetTrigger("IsTouched_Trigger");
                StartCoroutine(StopEffect(pools_TouchEffect.listPool[i], touchEfM.GetPlayTime()));
                pools_TouchEffect.is_serched = true;
                break;
            }
        }

        if (pools_TouchEffect.is_serched == false)
        {
            PoolingTouchEf();
            PullingTouchEf(mousePosition);
        }
    }

    public void PullingEnemyDamagedEf(int towerIndex, Vector3 t_enemy)
    {
        if (arr_pools_EnemyDamaged[towerIndex].cnt == 0)
        {
            PoolingMonsterDamagedEf(towerIndex);
        }
        arr_pools_EnemyDamaged[towerIndex].is_serched = false;
        for(int i = 0; i < arr_pools_EnemyDamaged[towerIndex].cnt; i++)
        {
            if(arr_pools_EnemyDamaged[towerIndex].listPool[i].activeSelf == false)
            {
                arr_pools_EnemyDamaged[towerIndex].listPool[i].transform.position = new Vector3(t_enemy.x, t_enemy.y+0.2f, t_enemy.z);
                arr_pools_EnemyDamaged[towerIndex].listPool[i].SetActive(true);
                StartCoroutine(StopEffect(arr_pools_EnemyDamaged[towerIndex].listPool[i], 1.0f));
                arr_pools_EnemyDamaged[towerIndex].is_serched = true;
                break;
            }
        }
        if(arr_pools_EnemyDamaged[towerIndex].is_serched == false)
        {
            PoolingMonsterDamagedEf(towerIndex);
            PullingEnemyDamagedEf(towerIndex, t_enemy);
        }
    }

    public void PullingEnemyAtkEf(int m_index, GameObject atkEf, Vector3 atkTargetPos)
    {
        if(arr_pools_EnemyAtkEf[m_index -1].cnt == 0)
        {
            PoolingMonsterAtkEf(m_index, atkEf);
        }

        arr_pools_EnemyAtkEf[m_index - 1].is_serched = false;
        for(int i = 0; i < arr_pools_EnemyAtkEf[m_index -1].cnt; i++)
        {
            if(arr_pools_EnemyAtkEf[m_index-1].listPool[i].activeSelf == false)
            {
                arr_pools_EnemyAtkEf[m_index - 1].listPool[i].transform.position = new Vector3(atkTargetPos.x, atkTargetPos.y+0.2f, atkTargetPos.z);
                arr_pools_EnemyAtkEf[m_index - 1].listPool[i].SetActive(true);
                StartCoroutine(StopEffect(arr_pools_EnemyAtkEf[m_index - 1].listPool[i], 1.0f));
                arr_pools_EnemyAtkEf[m_index - 1].is_serched = true;
                break;
            }
        }
        if(arr_pools_EnemyAtkEf[m_index-1].is_serched == false)
        {
            PoolingMonsterAtkEf(m_index, atkEf);
            PullingEnemyAtkEf(m_index, atkEf, atkTargetPos);
        }
    }

    private void EnemyPooling(int sp_index)
    {
        GameObject enemy = Instantiate(enemies[spawnEnemyIndex[sp_index]], t_objectPool_Enemy);
        arr_enemyPools[spawnEnemyIndex[sp_index]].cnt++;
        enemy.name = "Enemy_" + spawnEnemyIndex[sp_index].ToString("00") + "_" + arr_enemyPools[spawnEnemyIndex[sp_index]].cnt.ToString("000");
        enemy.SetActive(false);
        arr_enemyPools[spawnEnemyIndex[sp_index]].listPool.Add(enemy);
    }

    private void Pub_EnemyPooling(int enemyIndex)
    {
        GameObject enemy = Instantiate(enemies[enemyIndex], t_objectPool_Enemy);
        arr_enemyPools[enemyIndex].cnt++;
        enemy.name = "Enemy_" + enemyIndex.ToString("00") + "_" + arr_enemyPools[enemyIndex].cnt.ToString("000");
        enemy.SetActive(false);
        arr_enemyPools[enemyIndex].listPool.Add(enemy);
    }

    private void BugEnemyPooling()
    {
        GameObject enemy = Instantiate(enemy_BugHuman, t_objectPool_Enemy);
        arr_enemyPools[bugHumanIndex].cnt++;
        enemy.name = "Enemy_" + bugHumanIndex.ToString("00") + "_" + arr_enemyPools[bugHumanIndex].cnt.ToString("000");
        enemy.SetActive(false);
        arr_enemyPools[bugHumanIndex].listPool.Add(enemy);
    }

    private void SpawnEffectPooling()
    {
        GameObject effect = Instantiate(spawn_Effect, t_objectPool_SpawnEf);
        pools_SpawnEf.cnt++;
        effect.name = "sp_Effect_" + pools_SpawnEf.cnt.ToString("000");
        effect.SetActive(false);
        pools_SpawnEf.listPool.Add(effect);
    }

    private void DirEffectPooling()
    {
        GameObject effct = Instantiate(e_direction_Effect, t_objectPool_DirEffect);
        pools_DirEf.cnt++;
        effct.name = "dir_Effect_" + pools_DirEf.cnt.ToString("000");
        effct.SetActive(false);
        pools_DirEf.listPool.Add(effct);
    }

    private void EnemyDieEffectPooling()
    {
        GameObject effect = Instantiate(e_Die_Effect, t_objectPool_DieEffect);
        pools_DieEf.cnt++;
        effect.name = "die_Effect_" + pools_DieEf.cnt.ToString("000");
        effect.SetActive(false);
        pools_DieEf.listPool.Add(effect);
    }

    private void DropMoneyBarPooling()
    {
        GameObject bar = Instantiate(dropMoneyBarPrefab, t_DropMoneyPool);
        pools_DropMoney.cnt++;
        bar.name = "DropMoneyBar_" + pools_DropMoney.cnt.ToString("000");
        bar.SetActive(false);
        pools_DropMoney.listPool.Add(bar);
    }

    private void PoolingTouchEf()
    {
        GameObject effect = Instantiate(touchEfM.prefab_TouchEffect, t_touchEfPool);
        pools_TouchEffect.cnt++;
        effect.name = "TouchEffect_" + pools_TouchEffect.cnt.ToString("00");
        effect.SetActive(false);
        pools_TouchEffect.listPool.Add(effect);
    }

    private void PoolingMonsterDamagedEf(int towerIndex)
    {
        GameObject effect = Instantiate(enemyDamgedEffect[towerIndex], t_objectPool_AtkEffect);
        arr_pools_EnemyDamaged[towerIndex].cnt++;
        effect.name = "DamagedEffect_" + towerIndex.ToString("0") + "_" + arr_pools_EnemyDamaged[towerIndex].cnt.ToString("000");
        effect.SetActive(false);
        arr_pools_EnemyDamaged[towerIndex].listPool.Add(effect);
    }

    private void PoolingMonsterAtkEf(int m_index, GameObject atkEf)
    {
        GameObject effect = Instantiate(atkEf, t_objectPool_EnemyAtkEffect);
        arr_pools_EnemyAtkEf[m_index - 1].cnt++;
        effect.name = "EnemyAtkEffect_" + m_index.ToString("00") + "_" + arr_pools_EnemyAtkEf[m_index - 1].cnt.ToString("000");
        effect.SetActive(false);
        arr_pools_EnemyAtkEf[m_index - 1].listPool.Add(effect);
    }

    IEnumerator StopEffect(GameObject effect, float time)
    {
        yield return new WaitForSeconds(time);
        effect.SetActive(false);
        yield break;
    }

    IEnumerator CountTimeForSpawn()
    {
        int count = 0;
        yield return new WaitForSeconds(startDelay);


        while (true)
        {
            if(event_isDone)
            {
                if (spawnPointIndex.Length > count)
                {
                    SpawnEnemy(spawnPointIndex[count], count);
                    count++;
                }
                else
                {
                    StopCoroutine(CountTimeForSpawn());
                }

                yield return new WaitForSeconds(delayTimesIndex[count - 1]);
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
    }


    public void EnemyDied(int dropM)
    {
        cnt_EnemyDie += 1;
        stgGetMoney += dropM;
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
        itemM.End_Stage();
        loadingM.StageEnd(chapter_num);
    }

    public void StageFailed()
    {
        if (stageEnded == false)
        {
            stageEnded = true;
            loadingM.SetLoadingString("SYSTEM OVERLOAD");
            StartCoroutine(DelayedStageEnd(1.0f));
            itemM.Get_Money(stgGetMoney / 2);
        }
    }

    private void StageClear()
    {
        if (stageEnded == false)
        {
            stageEnded = true;
            playerM.OnWinAnim();
            loadingM.SetLoadingString("SYSTEM STABILIZATION");
            StartCoroutine(DelayedStageEnd(1.0f));
            itemM.Get_Money(stgGetMoney);
        }
    }

    public void DlgDone()
    {
        dlg_isDone = true;
        playerM.SetAiBarrier();
    }

    public bool GetDlgIsDone()
    {
        return dlg_isDone;
    }

    public void HadEvent()
    {
        event_isDone = false;
    }

    public void EventEnd()
    {
        event_isDone = true;
        Time.timeScale = 1.0f;
    }

    public bool GetEventIsDone()
    {
        return event_isDone;
    }

    public float GetWallMoveSpeed()
    {
        return wallMoveSpeed;
    }

    public float GetWallMoveMin()
    {
        return wallMinY;
    }

    public float GetWallMoveMax()
    {
        return wallMaxY;
    }

    public void BarricadeBreak()
    {
        alive_Barricade -= 1;
        StartCoroutine(PlaySfxBarricadeDestruction());
    }

    IEnumerator PlaySfxBarricadeDestruction()
    {
        sfx_Barricade_Destruction.Play();
        yield return new WaitForSeconds(0.3f);
        sfx_Barricade_Destruction.Stop();
        yield break;
    }

    IEnumerator PlaySfxStageStart()
    {
        sfx_StageStart.Play();
        yield return new WaitForSeconds(1.0f);
        sfx_StageStart.Stop();
        yield break;
    }

    private void PoolsInit()
    {
        pools_SpawnEf.listPool= new List<GameObject>();
        pools_SpawnEf.cnt = 0;
        pools_SpawnEf.is_serched = false;
    
        pools_DirEf.listPool = new List<GameObject>();
        pools_DirEf.cnt = 0;
        pools_DirEf.is_serched = false;
    
        pools_DieEf.listPool = new List<GameObject>();
        pools_DieEf.cnt = 0;
        pools_DieEf.is_serched = false;

        pools_DropMoney.listPool = new List<GameObject>();
        pools_DropMoney.cnt = 0;
        pools_DropMoney.is_serched = false;

        pools_TouchEffect.listPool = new List<GameObject>();
        pools_TouchEffect.cnt = 0;
        pools_TouchEffect.is_serched = false;


        arr_pools_EnemyDamaged = new Pools[3];
        for (int i = 0; i < 3; i++)
        {
            arr_pools_EnemyDamaged[i].listPool = new List<GameObject>();
            arr_pools_EnemyDamaged[i].cnt = 0;
            arr_pools_EnemyDamaged[i].is_serched = false;
        }

        arr_enemyPools = new Pools[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            arr_enemyPools[i].listPool = new List<GameObject>();
            arr_enemyPools[i].cnt = 0;
            arr_enemyPools[i].is_serched = false;
        }

        arr_pools_EnemyAtkEf = new Pools[2];
        for (int i = 0; i < 2; i++)
        {
            arr_pools_EnemyAtkEf[i].listPool = new List<GameObject>();
            arr_pools_EnemyAtkEf[i].cnt = 0;
            arr_pools_EnemyAtkEf[i].is_serched = false;
        }
    }

    IEnumerator DelayedStageEnd(float time)
    {
        yield return new WaitForSeconds(time);
        StageEnd();
        yield break;
    }

    public float GetSpawnTimeSum(int max)
    {
        float res = 0.0f;
        for (int i = 0; i < max; i++)
        {
            res += delayTimesIndex[i];
        }
        res += startDelay;

        return res;
    }
}

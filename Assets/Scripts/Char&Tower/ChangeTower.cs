﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTower : MonoBehaviour
{
    public GameObject basicTower;
    public GameObject sniperTower;
    public GameObject pushTower;
    public GameObject c_Effects;

    private GameObject curTower;
    private GameObject[] t_Positions;
    private Transform[] t_Pos = new Transform[2];
    private GameObject[] curTowers = new GameObject[2];

    private GameObject[] t_img_panel = new GameObject[2];

    private float[] aim_rot = new float[2];

    private GameObject sfx_manager;
    private AudioSource sfx_BTN_Click;

    private TowerAttack_Basic[] taM_B = new TowerAttack_Basic[2];
    private TowerAttack_Snip[] taM_S = new TowerAttack_Snip[2];
    private TowerAttack_Push[] taM_P = new TowerAttack_Push[2];
    private MeshRenderer[] cur_meshR = new MeshRenderer[2];
    private Color[] cur_Color = new Color[2];


    // Start is called before the first frame update
    void Start()
    {
        t_Positions = GameObject.FindGameObjectsWithTag("t_Position");
        t_Pos[0] = t_Positions[0].GetComponent<Transform>();
        t_Pos[1] = t_Positions[1].GetComponent<Transform>();

        curTower = basicTower;

        for (int i = 0; i < t_Positions.Length; i++)
        {
            curTowers[i] = Instantiate(curTower, t_Pos[i]);
            t_img_panel[i] = curTowers[i].transform.GetChild(0).gameObject;
            taM_B[i] = curTowers[i].GetComponentInChildren<TowerAttack_Basic>();
        }
        StartCoroutine(Get_SFX_Manager());
    }

    private void Update()
    {
        Rotate();
    }

    public void InstSNT()
    {
        StartCoroutine(Sound_BTN_Click());
        curTower = sniperTower;
        for (int i = 0; i < curTowers.Length; i++)
        {
            Destroy(curTowers[i]);
            curTowers[i] = Instantiate(curTower, t_Pos[i]);
            t_img_panel[i] = curTowers[i].transform.GetChild(0).gameObject;
            taM_S[i] = curTowers[i].GetComponentInChildren<TowerAttack_Snip>();

            GameObject effect = Instantiate(c_Effects, t_Pos[i]);
            Destroy(effect, 2.0f);
        }
    }

    public void InstBasic()
    {
        StartCoroutine(Sound_BTN_Click());
        curTower = basicTower;
        for (int i = 0; i < curTowers.Length; i++)
        {
            Destroy(curTowers[i]);
            curTowers[i] = Instantiate(curTower, t_Pos[i]);
            t_img_panel[i] = curTowers[i].transform.GetChild(0).gameObject;
            taM_B[i] = curTowers[i].GetComponentInChildren<TowerAttack_Basic>();

            GameObject effect = Instantiate(c_Effects, t_Pos[i]);
            Destroy(effect, 2.0f);
        }
    }

    public void InstPT()
    {
        StartCoroutine(Sound_BTN_Click());
        curTower = pushTower;
        for (int i = 0; i < curTowers.Length; i++)
        {
            Destroy(curTowers[i]);
            curTowers[i] = Instantiate(curTower, t_Pos[i]);
            t_img_panel[i] = curTowers[i].transform.GetChild(0).gameObject;
            taM_P[i] = curTowers[i].GetComponentInChildren<TowerAttack_Push>();

            GameObject effect = Instantiate(c_Effects, t_Pos[i]);
            Destroy(effect, 2.0f);
        }
    }

    public void AttackActivated(int type_num) // 0 = basic, 1 = snip, 2 = push
    {
        switch (type_num)
        {
            case 0:
                for (int i = 0; i < 2; i++)
                {
                    taM_B[i].OnOtherAttack();
                }
                break;
            case 1:
                for (int i = 0; i < 2; i++)
                {
                    taM_S[i].OnOtherAttack();
                }
                break;
            case 2:
                for (int i = 0; i < 2; i++)
                {
                    taM_P[i].OnOtherAttack();
                }
                break;
        }
    }

    private void Rotate()
    {
        if (gameObject.transform.localEulerAngles.y >= 0 && gameObject.transform.localEulerAngles.y <= 180)
        {
            aim_rot[0] = (60.0f + (gameObject.transform.localEulerAngles.y / 3));
            aim_rot[1] = (120.0f - (gameObject.transform.localEulerAngles.y / 3));
        }
        else if (gameObject.transform.localEulerAngles.y >= 180 && gameObject.transform.localEulerAngles.y <= 360)
        {
            aim_rot[0] = (120.0f - ((gameObject.transform.localEulerAngles.y - 180) / 3));
            aim_rot[1] = (60.0f + ((gameObject.transform.localEulerAngles.y - 180) / 3));
        }
        for (int i = 0; i < 2; i++)
        { 
            t_img_panel[i].transform.localEulerAngles = new Vector3(aim_rot[i], 0, 0);
        }
    }

    private IEnumerator Sound_BTN_Click()
    {
        sfx_BTN_Click.Play();
        yield return new WaitForSeconds(1.0f);
        sfx_BTN_Click.Stop();
        yield break;
    }

    private IEnumerator Get_SFX_Manager()
    {
        yield return new WaitForSeconds(1.0f);
        sfx_manager = GameObject.FindGameObjectWithTag("MainCamera").transform.Find("SFX_Manager(Clone)").gameObject;
        sfx_BTN_Click = sfx_manager.transform.Find("S_BTN_Click").gameObject.GetComponent<AudioSource>();
        yield break;
    }

    IEnumerator On_Clear(int index)
    {
        cur_meshR[index].material.color = new Color(cur_Color[index].r, cur_Color[index].g, cur_Color[index].b, 0.0f);
        yield return new WaitForSeconds(1.0f);
        cur_meshR[index].material.color = new Color(cur_Color[index].r, cur_Color[index].g, cur_Color[index].b, cur_Color[index].a);
        yield break;
    }
}

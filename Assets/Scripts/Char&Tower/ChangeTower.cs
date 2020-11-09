using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTower : MonoBehaviour
{
    public GameObject c_Effects;

   // private GameObject curTower;
    private GameObject[] t_Positions;
    private Transform[] t_Pos = new Transform[2];
    private GameObject[] curTowers = new GameObject[2];

    private GameObject[] t_img_panel = new GameObject[2];

    private float[] aim_rot = new float[2];

    private GameObject sfx_manager;
    private AudioSource sfx_T_Change;

    private Transform t_objPools;
    private Transform t_objectPool_Basic;
    private Transform t_objectPool_Snip;
    private Transform t_objectPool_Push;
    private Transform t_objectPool_changeEf;
    public GameObject[] arrayPool_Basic = new GameObject[2];
    public GameObject[] arrayPool_Snip = new GameObject[2];
    public GameObject[] arrayPool_Push = new GameObject[2];
    private List<GameObject> listPool_changeEf = new List<GameObject>();
    private bool is_serched_c = false;

    private TowerAttack_Basic[] taM_B = new TowerAttack_Basic[2];
    private TowerAttack_Snip[] taM_S = new TowerAttack_Snip[2];
    private TowerAttack_Push[] taM_P = new TowerAttack_Push[2];


    // Start is called before the first frame update
    void Start()
    {
        t_Positions = GameObject.FindGameObjectsWithTag("t_Position");
        t_Pos[0] = t_Positions[0].GetComponent<Transform>();
        t_Pos[1] = t_Positions[1].GetComponent<Transform>();

        sfx_manager = GameObject.FindGameObjectWithTag("SFX_Manager").gameObject;
        sfx_T_Change = sfx_manager.transform.Find("S_T_Change").gameObject.GetComponent<AudioSource>();

        t_objPools = GameObject.FindGameObjectWithTag("ObjectPools").transform;
        t_objectPool_Basic = t_objPools.Find("BT");
        t_objectPool_Snip = t_objPools.Find("SNT");
        t_objectPool_Push = t_objPools.Find("PT");
        t_objectPool_changeEf = t_objPools.Find("ChangeEffect");

        for (int i = 0; i < 2; i++)
        {
            curTowers[i] = arrayPool_Basic[i];
            curTowers[i].transform.parent = t_Pos[i];
            curTowers[i].transform.localEulerAngles = new Vector3(0, 0, 0);
            curTowers[i].transform.localPosition = new Vector3(0, 0, 0);
            curTowers[i].SetActive(true);
            t_img_panel[i] = curTowers[i].transform.GetChild(0).gameObject;
            //PullingChangeEffect(i, 2.0f);

            taM_B[i] = arrayPool_Basic[i].GetComponentInChildren<TowerAttack_Basic>();
            taM_S[i] = arrayPool_Snip[i].GetComponentInChildren<TowerAttack_Snip>();
            taM_P[i] = arrayPool_Push[i].GetComponentInChildren<TowerAttack_Push>();
        }
    }

    private void Update()
    {
        Rotate();
    }

    public void InstSNT()
    {
        StartCoroutine(Sound_T_Change());

        for (int i = 0; i < 2; i++)
        {
            if (curTowers[i].CompareTag("BT"))
            {
                curTowers[i].transform.parent = t_objectPool_Basic;
                curTowers[i].SetActive(false);
            }
            else if (curTowers[i].CompareTag("PT"))
            {
                curTowers[i].transform.parent = t_objectPool_Push;
                curTowers[i].SetActive(false);
            }

            curTowers[i] = arrayPool_Snip[i];
            curTowers[i].transform.parent = t_Pos[i];
            curTowers[i].transform.localEulerAngles = new Vector3(0, 0, 0);
            curTowers[i].transform.localPosition = new Vector3(0, 0, 0);
            curTowers[i].SetActive(true);
            t_img_panel[i] = curTowers[i].transform.GetChild(0).gameObject;
            PullingChangeEffect(i, 2.0f);
        }
    }

    public void InstBasic()
    {
        StartCoroutine(Sound_T_Change());

        for (int i = 0; i < 2; i++)
        {
            if (curTowers[i].CompareTag("SNT"))
            {
                curTowers[i].transform.parent = t_objectPool_Snip;
                curTowers[i].SetActive(false);
            }
            else if (curTowers[i].CompareTag("PT"))
            {
                curTowers[i].transform.parent = t_objectPool_Push;
                curTowers[i].SetActive(false);
            }

            curTowers[i] = arrayPool_Basic[i];
            curTowers[i].transform.parent = t_Pos[i];
            curTowers[i].transform.localEulerAngles = new Vector3(0, 0, 0);
            curTowers[i].transform.localPosition = new Vector3(0, 0, 0);
            curTowers[i].SetActive(true);
            t_img_panel[i] = curTowers[i].transform.GetChild(0).gameObject;
            PullingChangeEffect(i, 2.0f);
        }
    }

    public void InstPT()
    {
        StartCoroutine(Sound_T_Change());

        for (int i = 0; i < 2; i++)
        {
            if (curTowers[i].CompareTag("BT"))
            {
                curTowers[i].transform.parent = t_objectPool_Basic;
                curTowers[i].SetActive(false);
            }
            else if (curTowers[i].CompareTag("SNT"))
            {
                curTowers[i].transform.parent = t_objectPool_Snip;
                curTowers[i].SetActive(false);
            }

            curTowers[i] = arrayPool_Push[i];
            curTowers[i].transform.parent = t_Pos[i];
            curTowers[i].transform.localEulerAngles = new Vector3(0, 0, 0);
            curTowers[i].transform.localPosition = new Vector3(0, 0, 0);
            curTowers[i].SetActive(true);
            t_img_panel[i] = curTowers[i].transform.GetChild(0).gameObject;
            PullingChangeEffect(i, 2.0f);
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

    private void PullingChangeEffect(int idx_t_pos, float time)
    {
        if (listPool_changeEf.Count <= 0)
        {
            ChangeEffectPooling();
        }
        is_serched_c = false;
        for (int i = 0; i < listPool_changeEf.Count; i++)
        {
            if (listPool_changeEf[i].activeSelf == false)
            {
                listPool_changeEf[i].transform.position = t_Pos[idx_t_pos].position;
                listPool_changeEf[i].transform.rotation = t_Pos[idx_t_pos].rotation;
                listPool_changeEf[i].SetActive(true);
                StartCoroutine(StopEffect(listPool_changeEf[i], time));
                is_serched_c = true;
                break;
            }
        }
        if (is_serched_c == false)
        {
            ChangeEffectPooling();
            PullingChangeEffect(idx_t_pos, time);
        }
    }

    private void ChangeEffectPooling()
    {
        var effect = Instantiate(c_Effects, t_objectPool_changeEf);
        listPool_changeEf.Add(effect);
        effect.name = "Change_Effect_" + listPool_changeEf.Count.ToString("000");
        effect.SetActive(false);
        
    }

    IEnumerator StopEffect(GameObject effect, float time)
    {
        yield return new WaitForSeconds(time);
        effect.SetActive(false);
        yield break;
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

    private IEnumerator Sound_T_Change()
    {
        sfx_T_Change.Play();
        yield return new WaitForFixedUpdate();
        sfx_T_Change.Stop();
        yield break;
    }
}

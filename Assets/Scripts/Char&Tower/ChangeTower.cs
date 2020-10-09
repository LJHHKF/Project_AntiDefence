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
            GameObject effect = Instantiate(c_Effects, t_Pos[i]);
            Destroy(effect, 2.0f);
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
}

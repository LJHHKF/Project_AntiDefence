using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScene : MonoBehaviour
{

    private GameObject gm;
    private AudioManager audioM;
    private LoadingManager loadingM;
    private BGM_Manager bgmM;
    private TouchEfManager touchEfM;

    private Slider sld_bg;
    private Slider sld_se;

    private Transform t_touchEfPool;
    private List<GameObject> listPool_touchEf = new List<GameObject>();
    private bool is_serched_touchEf;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        audioM = gm.GetComponent<AudioManager>();
        loadingM = gm.GetComponent<LoadingManager>();
        bgmM = gm.GetComponent<BGM_Manager>();
        touchEfM = gm.GetComponent<TouchEfManager>();

        sld_bg = gameObject.transform.Find("Panel_BG_Setting").Find("Slider").GetComponent<Slider>();
        sld_se = gameObject.transform.Find("Panel_SE_Setting").Find("Slider").GetComponent<Slider>();

        sld_bg.value = audioM.GetBgVolume();
        sld_se.value = audioM.GetSeVolume();

        t_touchEfPool = gameObject.transform.Find("TouchEffect_Pool");

        bgmM.Play_LobbyAndShop();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnTouchEf(Input.mousePosition);
        }
    }

    public void SetBGValue()
    {
        audioM.SetBgVolume(sld_bg.value);
    }

    public void SetSEValue()
    {
        audioM.SetSeVolume(sld_se.value);
    }

    public void Click_BTN_Return()
    {
        audioM.SFX_BTN_Click();
        loadingM.LoadPrevScene();
    }

    private void SpawnTouchEf(Vector3 mousePosition)
    {
        if (listPool_touchEf.Count == 0)
        {
            PoolingTouchEf();
        }
        is_serched_touchEf = false;

        for (int i = 0; i < listPool_touchEf.Count; i++)
        {
            if (listPool_touchEf[i].activeSelf == false)
            {
                Animator m_animator = listPool_touchEf[i].GetComponent<Animator>();
                RectTransform m_rect = listPool_touchEf[i].GetComponent<RectTransform>();
                m_rect.position = mousePosition;
                listPool_touchEf[i].SetActive(true);
                m_animator.SetTrigger("IsTouched_Trigger");
                StartCoroutine(StopEffect(listPool_touchEf[i], touchEfM.GetPlayTime()));
                is_serched_touchEf = true;
                break;
            }
        }

        if (is_serched_touchEf == false)
        {
            PoolingTouchEf();
            SpawnTouchEf(mousePosition);
        }
    }

    private void PoolingTouchEf()
    {
        GameObject effect = touchEfM.Instantiate(t_touchEfPool);
        listPool_touchEf.Add(effect);
        effect.name = "TouchEffect_" + listPool_touchEf.Count.ToString("00");
        effect.SetActive(false);
    }

    IEnumerator StopEffect(GameObject target, float time)
    {
        yield return new WaitForSeconds(time);
        target.SetActive(false);
        yield break;
    }
}

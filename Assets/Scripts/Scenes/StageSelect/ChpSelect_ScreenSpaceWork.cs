using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChpSelect_ScreenSpaceWork : MonoBehaviour
{
    private GameObject gm;
    private TouchEfManager touchEfM;

    private Transform t_touchEfPool;
    private List<GameObject> listPool_touchEf = new List<GameObject>();
    private bool is_serched_touchEf;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        touchEfM = gm.GetComponent<TouchEfManager>();

        t_touchEfPool = gameObject.transform.Find("TouchEffect_Pool");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnTouchEf(Input.mousePosition);
        }
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

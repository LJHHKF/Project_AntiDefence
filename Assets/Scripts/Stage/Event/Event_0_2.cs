using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_0_2 : MonoBehaviour
{

    private GameObject eventObject;
    private GameObject eventObject2;
    private StageManager stageM;

    // Start is called before the first frame update
    void Start()
    {
        eventObject = GameObject.FindGameObjectWithTag("UI_Canvas").transform.Find("EventObject").gameObject;
        eventObject2 = GameObject.FindGameObjectWithTag("UI_Canvas").transform.Find("EventObject2").gameObject;
        stageM = gameObject.GetComponent<StageManager>();

        stageM.HadEvent();
        eventObject.SetActive(false);
        eventObject2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(stageM.GetDlgIsDone())
        {
            StartCoroutine(ObjectMove());
        }
    }

    IEnumerator ObjectMove()
    {
        eventObject.SetActive(true);
        eventObject.transform.localPosition = new Vector3(-135, -750, 0);
        yield return new WaitForSeconds(1.5f);
        eventObject.SetActive(false);
        eventObject2.SetActive(true);
        eventObject2.transform.localPosition = new Vector3(135, -750, 0);
        yield return new WaitForSeconds(1.5f);
        stageM.EventEnd();
        Destroy(eventObject);
        Destroy(eventObject2);
        Destroy(this);
        yield break;
    }
}

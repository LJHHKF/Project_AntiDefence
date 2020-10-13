using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_0_1 : MonoBehaviour
{

    private GameObject eventObject;
    private StageManager stageM;
    private Transform t_TowerBoard;

    private float flowV = 0;
    private bool is_corutinStart;
    private bool firstMove = true;

    private float prev_rot_value;
    private float rot_value = 0;

    // Start is called before the first frame update
    void Start()
    {
        eventObject = GameObject.FindGameObjectWithTag("UI_Canvas").transform.Find("EventObject").gameObject;
        t_TowerBoard = GameObject.FindGameObjectWithTag("TowerBoard").transform;
        stageM = gameObject.GetComponent<StageManager>();

        eventObject.SetActive(false);
        stageM.HadEvent();

        prev_rot_value = t_TowerBoard.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (stageM.GetDlgIsDone())
        {
            flowV += Time.deltaTime * 500f;
            if (is_corutinStart == false)
            {
                StartCoroutine(MoveObject());
                is_corutinStart = true;
            }

            if(rot_value < 180)
            {
                rot_value += Mathf.Abs(prev_rot_value - t_TowerBoard.localEulerAngles.y);
                prev_rot_value = t_TowerBoard.localEulerAngles.y;
            }
            else
            {
                StopCoroutine(MoveObject());
                Destroy(eventObject);
                stageM.EventEnd();
                Destroy(this);
            }
        }
    }

    IEnumerator MoveObject()
    {
        if(firstMove)
        {
            firstMove = false;
            yield return new WaitForSeconds(1.0f);
        }
        flowV = 0.0f;
        eventObject.SetActive(true);
        while (flowV <= 600)
        {
            if (flowV <= 300)
            {
                eventObject.transform.localPosition = new Vector3(0 + flowV, 300 - flowV, 0);
            }
            else if(flowV <= 600)
            {
                eventObject.transform.localPosition = new Vector3(600 - flowV, 0 - (flowV - 300), 0);
            }
            yield return new WaitForEndOfFrame();
        }
        eventObject.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        is_corutinStart = false;
        yield break;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PT_Contact : MonoBehaviour
{
    private GameObject imageObject;
    private Image cTimeImg;

    private TowerAttack_Push ta_push;

    private Collider m_Coll;

    private float touch_Time;


    // Start is called before the first frame update
    void Start()
    {
        imageObject = GameObject.FindGameObjectWithTag("PT_Cool");
        cTimeImg = imageObject.GetComponent<Image>();

        ta_push = gameObject.GetComponentInParent<TowerAttack_Push>();

        m_Coll = this.gameObject.GetComponent<Collider>();
        m_Coll.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_Coll.enabled = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            touch_Time = 0f;
            m_Coll.enabled = true;
        }

        touch_Time += Time.deltaTime;

        if (touch_Time >= 0.5f)
        {
            m_Coll.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && (cTimeImg.fillAmount >= 1.0f || cTimeImg.fillAmount <= 0.01f))
        {
            m_Coll.enabled = false;
            ta_push.P_Contact();
        }
    }
}

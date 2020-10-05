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
            m_Coll.enabled = true;
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

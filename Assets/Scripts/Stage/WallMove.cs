using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMove : MonoBehaviour
{

    private Transform m_transform;
    private bool trigger = true;
    private float moveSpeed = 0.01f;
    private StageManager stageM;
    
    // Start is called before the first frame update
    void Start()
    {
        m_transform = gameObject.GetComponent<Transform>();
        stageM = GameObject.FindGameObjectWithTag("StageMObject").GetComponent<StageManager>();

        moveSpeed = stageM.GetWallMoveSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            m_transform.Translate(new Vector3(0, moveSpeed, 0));
            if (m_transform.localPosition.y >= stageM.GetWallMoveMax())
                trigger = false;
        }
        else
        {
            m_transform.Translate(new Vector3(0, -moveSpeed, 0));
            if (m_transform.localPosition.y <= stageM.GetWallMoveMin())
                trigger = true;
        }
    }
}

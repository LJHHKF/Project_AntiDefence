using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMove : MonoBehaviour
{

    private Transform m_transform;
    private bool trigger = true;
    private float moveSpeed = 0.01f;
    private StageManager stageM;

    private bool switch_move = false;
    
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
        if (Time.timeScale != 0.0f)
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

    public void TurnMoveSpeed()
    {
        if(!switch_move)
        {
            switch_move = true;
            moveSpeed = 0.0f;
        }
        else
        {
            switch_move = false;
            moveSpeed = stageM.GetWallMoveSpeed();
        }
    }
}

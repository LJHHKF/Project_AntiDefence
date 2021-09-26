using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    private enum State
    {
        ALLIVE,
        DIE
    }

    private State state = State.ALLIVE;
    public float hp = 5.0f;

    private int m_target_index;

    private StageManager stageM;

    private void Start()
    {
        stageM = GameObject.FindGameObjectWithTag("StageMObject").GetComponent<StageManager>();
    }

    private void OnEnable()
    {
        m_target_index = TargetListManager.instance.AddList(gameObject, myTarget.targetType.barricade);
        TargetListManager.instance.ev_RemoveAt += RemoveAtEvent;
    }

    private void OnDisable()
    {
        TargetListManager.instance.RemoveAtList(m_target_index);
        TargetListManager.instance.ev_RemoveAt -= RemoveAtEvent;
    }

    private void RemoveAtEvent(int a)
    {
        if (m_target_index > a)
            m_target_index -= 1;
    }

    public void Barricade_damaged(float dmg)
    {
        if (state != State.DIE)
        {
            hp -= dmg;

            if (hp <= 0)
            {
                state = State.DIE;
                stageM.BarricadeBreak();
                Destroy(gameObject);
            }
        }
    }
}

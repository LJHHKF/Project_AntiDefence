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

    [HideInInspector]
    public bool isDie = false;

    private StageManager stageM;

    private void Start()
    {
        stageM = GameObject.FindGameObjectWithTag("StageMObject").GetComponent<StageManager>();
    }

    public void Barricade_damaged(float dmg)
    {
        if (state != State.DIE)
        {
            hp -= dmg;

            if (hp <= 0)
            {
                state = State.DIE;
                isDie = true;
                stageM.BarricadeBreak();
                Destroy(gameObject);
            }
        }

    }
}

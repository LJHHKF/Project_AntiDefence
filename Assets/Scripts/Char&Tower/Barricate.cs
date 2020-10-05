using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricate : MonoBehaviour
{
    private enum State
    {
        ALLIVE,
        DIE
    }

    private State state = State.ALLIVE;
    public float hp = 2.0f;

    public void Barricate_damaged(float dmg)
    {
        if (state != State.DIE)
        {
            hp -= dmg;

            if (hp <= 0)
            {
                state = State.DIE;
                Destroy(gameObject);
            }
        }

    }
}

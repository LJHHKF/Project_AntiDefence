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

    public void Barricade_damaged(float dmg)
    {
        if (state != State.DIE)
        {
            hp -= dmg;

            if (hp <= 0)
            {
                state = State.DIE;
                isDie = true;
                Destroy(gameObject);
            }
        }

    }
}

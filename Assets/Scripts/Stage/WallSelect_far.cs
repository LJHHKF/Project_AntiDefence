﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSelect_far : MonoBehaviour
{
    private StageManager sm;
    private SpriteRenderer m_sprender;

    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("StageMObject").GetComponent<StageManager>();
        m_sprender = gameObject.GetComponent<SpriteRenderer>();

        m_sprender.sprite = sm.Get_FarWall();
    }

 
}

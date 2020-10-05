using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSelect_far : MonoBehaviour
{
    private StageManager sm;
    private SpriteRenderer m_sprender;
    private int index;

    public Sprite[] wallSprite;

    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("StageMObject").GetComponent<StageManager>();
        m_sprender = gameObject.GetComponent<SpriteRenderer>();

        index = sm.Get_FarWall();

        m_sprender.sprite = wallSprite[index];
    }

 
}

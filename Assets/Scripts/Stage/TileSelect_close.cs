using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelect_close : MonoBehaviour
{
    private StageManager sm;
    private SpriteRenderer m_sprender;
    private int index;

    public Sprite[] tileSprite;

    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("StageMObject").GetComponent<StageManager>();
        m_sprender = gameObject.GetComponent<SpriteRenderer>();

        index = sm.Get_CloseTile();

        m_sprender.sprite = tileSprite[index];

    }
}

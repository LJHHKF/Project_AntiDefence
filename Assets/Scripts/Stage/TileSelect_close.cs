using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelect_close : MonoBehaviour
{
    private StageManager sm;
    private SpriteRenderer m_sprender;


    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("StageMObject").GetComponent<StageManager>();
        m_sprender = gameObject.GetComponent<SpriteRenderer>();

        m_sprender.sprite = sm.Get_CloseTile();
        m_sprender.color = new Color(m_sprender.color.r, m_sprender.color.g, m_sprender.color.b, sm.Get_CloseTileAlpha()/255f);
    }
}

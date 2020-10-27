using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchEffect : MonoBehaviour
{
    private Image m_img;
    private SpriteRenderer m_spr;

    // Start is called before the first frame update
    void Start()
    {
        m_img = gameObject.GetComponent<Image>();
        m_spr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        m_img.sprite = m_spr.sprite;
    }
}

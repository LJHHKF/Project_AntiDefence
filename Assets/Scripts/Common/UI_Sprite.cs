using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class UI_Sprite : MonoBehaviour
{

    private SpriteRenderer m_sprender;
    private Image m_img;
    private Animator m_anim;

    // Start is called before the first frame update
    void Start()
    {
        m_sprender = gameObject.GetComponent<SpriteRenderer>();
        m_img = gameObject.GetComponent<Image>();
        m_anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_anim.runtimeAnimatorController != null)
        {
            m_img.sprite = m_sprender.sprite;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleLogoTwinkling : MonoBehaviour
{
    Image m_Img;
    float ori_alpha;
    bool trigger_reverse;

    public float speed;
    public float min_Alpha;

    void Start()
    {
        m_Img = gameObject.GetComponent<Image>();
        ori_alpha = m_Img.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if(!trigger_reverse)
        {
            m_Img.color = new Color(m_Img.color.r, m_Img.color.g, m_Img.color.b, m_Img.color.a - (speed * Time.deltaTime));
            
            if(m_Img.color.a <= min_Alpha)
            {
                trigger_reverse = true;
            }
        }
        else
        {
            m_Img.color = new Color(m_Img.color.r, m_Img.color.g, m_Img.color.b, m_Img.color.a + (speed * Time.deltaTime));

            if(m_Img.color.a >= ori_alpha)
            {
                trigger_reverse = false;
            }
        }
    }
}

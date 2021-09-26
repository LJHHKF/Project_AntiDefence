using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySpriteChange : MonoBehaviour
{
    public Sprite[] sprites;
    //public SpriteRenderer m_sprR;

    private GameObject gm;
    private SkinManager skinM;

    private bool isFirst;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        skinM = gm.GetComponent<SkinManager>();

        isFirst = true;
    }

    private void OnEnable()
    {
        if (isFirst)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[skinM.GetSkinIndex()];
            //m_sprR.sprite = sprites[skinM.GetSkinIndex()];
            isFirst = false;
        }
    }

}

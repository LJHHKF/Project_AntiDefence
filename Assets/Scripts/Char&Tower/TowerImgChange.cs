using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerImgChange : MonoBehaviour
{
    public Sprite[] skins;
    public RuntimeAnimatorController[] anims;

    private GameObject gm;
    private SkinManager skinM;


    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        skinM = gm.GetComponent<SkinManager>();
    }
    // Start is called before the first frame update
    void Start()
    {



    }

    private void OnEnable()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = skins[skinM.GetSkinIndex()];
        gameObject.GetComponent<Animator>().runtimeAnimatorController = anims[skinM.GetSkinIndex()];
    }
}

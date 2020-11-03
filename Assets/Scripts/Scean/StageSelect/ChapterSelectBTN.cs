using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelectBTN : MonoBehaviour
{
    private Transform tr_BTN_Space;
    // Start is called before the first frame update
    void Start()
    {
        tr_BTN_Space = GameObject.FindGameObjectWithTag("UI_Canvas").transform.Find("BTN_Space");
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.parent == tr_BTN_Space)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        
    }
}

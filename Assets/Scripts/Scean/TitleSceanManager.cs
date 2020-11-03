using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceanManager : MonoBehaviour
{
    private LoadingManager loadingM;
    private Text loadingTxt;

    // Start is called before the first frame update
    void Start()
    {
        loadingM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LoadingManager>();
        loadingM.FirstSceneLoad("Lobby");

        loadingTxt = GameObject.FindGameObjectWithTag("LoadingBar").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(loadingM.GetIsLoaded())
        {
            loadingTxt.text = "Touch Screen";
            if(Input.GetMouseButtonDown(0))
            {
                loadingM.SetEventDone();
            }
        }
        else
        {
            loadingTxt.text = "Now Loading...";
        }
        
    }
}

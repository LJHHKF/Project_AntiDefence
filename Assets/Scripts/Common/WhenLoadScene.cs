using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhenLoadScene : MonoBehaviour
{
    private GameObject gm;
    private LoadingManager loadingM;

    void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager");
        loadingM = gm.GetComponent<LoadingManager>();
        loadingM.SetLoadingImage(gameObject.GetComponent<Image>());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LoadingManager loadingM;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        loadingM = gameObject.GetComponent<LoadingManager>();
        loadingM.FirstSceneLoad("Lobby");
    }

    // Update is called once per frame
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}

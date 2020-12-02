using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedLoadScene());
    }

    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("GMCallLoading");
        yield break;
    }
}

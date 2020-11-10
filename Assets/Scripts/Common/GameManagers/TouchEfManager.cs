using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchEfManager : MonoBehaviour
{
    public GameObject prefab_TouchEffect;
    public float touchEf_PlayTime = 0.25f;

    public float GetPlayTime()
    {
        return touchEf_PlayTime;
    }
    public GameObject Instantiate(Transform parent)
    {
        return Instantiate(prefab_TouchEffect, parent);
    }
}

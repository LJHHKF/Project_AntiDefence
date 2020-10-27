using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchEfManager : MonoBehaviour
{
    public GameObject prefab_TouchEffect;

    public GameObject Instantiate(Transform parent)
    {
        return Instantiate(prefab_TouchEffect, parent);
    }
}

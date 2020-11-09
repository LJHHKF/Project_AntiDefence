using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfoManager : MonoBehaviour
{

    [Header("Set Info")]
    public Sprite[] monsterImage;
    [TextArea]
    public string[] monsterInfoTxt;

    public Sprite GetMonsterImage(int index)
    {
        return monsterImage[index];
    }

    public Sprite[] GetMonsterImage()
    {
        return monsterImage;
    }

    public string GetMonsterInfoTxt(int index)
    {
        return monsterInfoTxt[index];
    }

    public string[] GetMonsterInfoTxt()
    {
        return monsterInfoTxt;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public Sprite[] skins;
<<<<<<< HEAD

    public RuntimeAnimatorController[] anims;
=======
    public AnimatorController[] anims;
>>>>>>> parent of 61564a4... 스킨, 애니메이션 시스템 완성


    private int skin_index;

    // Start is called before the first frame update
    void Awake()
    {
        skin_index = PlayerPrefs.GetInt("EquipedSkin", 0);
    }

    public void SetSkinIndex(int index)
    {
        skin_index = index;
    }

    public int GetSkinIndex()
    {
        return skin_index;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("EquipedSkin", skin_index);
    }
}

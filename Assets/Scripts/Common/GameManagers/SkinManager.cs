using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public Sprite[] skins;
    public RuntimeAnimatorController[] anims;

    private int skin_index;

    // Start is called before the first frame update
    void Awake()
    {
        skin_index = DataSaveManager.ownItemCount["EquipedSkin"];
            //PlayerPrefs.GetInt("EquipedSkin", 0);
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
        //PlayerPrefs.SetInt("EquipedSkin", skin_index);
        DataSaveManager.ownItemCount["EquipedSkin"] = skin_index;
        DataSaveManager.WriteData("DB_Item.csv", DataSaveManager.ownItemCount);
    }
}

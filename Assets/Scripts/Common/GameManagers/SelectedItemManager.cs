using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedItemManager : MonoBehaviour
{
    [HideInInspector]
    public bool i_muls_b = false;
    [HideInInspector]
    public bool i_muls_sn = false;
    [HideInInspector]
    public bool i_muls_p = false;
    [HideInInspector]
    public bool i_aiBarrier = false;
    [HideInInspector]
    public bool i_protectWall = false;
    [HideInInspector]
    public bool i_extend_b = false;
    [HideInInspector]
    public bool i_extend_sn = false;
    [HideInInspector]
    public bool i_extend_p = false;
    [HideInInspector]
    public bool i_recovery = false;

    [HideInInspector]
    public int own_MulS_B;
    [HideInInspector]
    public int own_MulS_SN;
    [HideInInspector]
    public int own_MulS_P;
    [HideInInspector]
    public int own_AiBarrier;
    [HideInInspector]
    public int own_ProtectWall;
    [HideInInspector]
    public int own_Extend_B;
    [HideInInspector]
    public int own_Extend_SN;
    [HideInInspector]
    public int own_Extend_P;
    [HideInInspector]
    public int own_Recovery;

    private bool use_confirm = false;

    [HideInInspector]
    public int own_money;

    [HideInInspector]
    public string name_Muls_B;
    [HideInInspector]
    public string name_Muls_SN;
    [HideInInspector]
    public string name_Muls_P;
    [HideInInspector]
    public string name_AiBarrire;
    [HideInInspector]
    public string name_ProtectWall;
    [HideInInspector]
    public string name_Extend_B;
    [HideInInspector]
    public string name_Extend_SN;
    [HideInInspector]
    public string name_Extend_P;
    [HideInInspector]
    public string name_Recovery;

    [HideInInspector]
    public string txt_Muls_B;
    [HideInInspector]
    public string txt_Muls_SN;
    [HideInInspector]
    public string txt_Muls_P;
    [HideInInspector]
    public string txt_AiBarrier;
    [HideInInspector]
    public string txt_ProtectWall;
    [HideInInspector]
    public string txt_Extend_B;
    [HideInInspector]
    public string txt_Extend_SN;
    [HideInInspector]
    public string txt_Extend_P;
    [HideInInspector]
    public string txt_Recovery;

    [HideInInspector]
    public int price_Muls_B;
    [HideInInspector]
    public int price_Muls_SN;
    [HideInInspector]
    public int price_Muls_P;
    [HideInInspector]
    public int price_AiBarrier;
    [HideInInspector]
    public int price_ProtectWall;
    [HideInInspector]
    public int price_Extend_B;
    [HideInInspector]
    public int price_Extend_SN;
    [HideInInspector]
    public int price_Extend_P;
    [HideInInspector]
    public int price_Recovery;

    public Sprite spr_Muls_B;
    public Sprite spr_Muls_SN;
    public Sprite spr_Muls_P;
    public Sprite spr_AiBarrier;
    public Sprite spr_ProtectWall;
    public Sprite spr_Extend_B;
    public Sprite spr_Extend_SN;
    public Sprite spr_Extend_P;
    public Sprite spr_Recovery;

    [HideInInspector]
    public bool i_event_AiBarrier = false;
    [HideInInspector]
    public bool i_event_ProtectWall = false;

    private void Start()
    {
        //if (PlayerPrefs.GetInt("MulS_B") <= 0)
        //    own_MulS_B = 0;
        //else
        //    own_MulS_B = PlayerPrefs.GetInt("MulS_B");
        //if (PlayerPrefs.GetInt("MulS_SN") <= 0)
        //    own_MulS_SN = 0;
        //else
        //    own_MulS_SN = PlayerPrefs.GetInt("MulS_SN");
        //if (PlayerPrefs.GetInt("MulS_P") <= 0)
        //    own_MulS_P = 0;
        //else
        //    own_MulS_P = PlayerPrefs.GetInt("MulS_P");
        //if (PlayerPrefs.GetInt("AiBarrier") <= 0)
        //    own_AiBarrier = 0;
        //else
        //    own_AiBarrier = PlayerPrefs.GetInt("AiBarrier");
        //if (PlayerPrefs.GetInt("ProtectWall") <= 0)
        //    own_ProtectWall = 0;
        //else
        //    own_ProtectWall = PlayerPrefs.GetInt("ProtectWall");
        //if (PlayerPrefs.GetInt("Extend_B") <= 0)
        //    own_Extend_B = 0;
        //else
        //    own_Extend_B = PlayerPrefs.GetInt("Extend_B");
        //if (PlayerPrefs.GetInt("Extend_SN") <= 0)
        //    own_Extend_SN = 0;
        //else
        //    own_Extend_SN = PlayerPrefs.GetInt("Extend_SN");
        //if (PlayerPrefs.GetInt("Extend_P") <= 0)
        //    own_Extend_P = 0;
        //else
        //    own_Extend_P = PlayerPrefs.GetInt("Extend_P");
        //if (PlayerPrefs.GetInt("Recovery") <= 0)
        //    own_Recovery = 0;
        //else
        //    own_Recovery = PlayerPrefs.GetInt("Recovery");

        //if (PlayerPrefs.GetInt("Money") <= 0)
        //    own_money = 0;
        //else
        //    own_money = PlayerPrefs.GetInt("Money");

        own_MulS_B = DataSaveManager.ownItemCount["Muls_B"];
        own_MulS_SN = DataSaveManager.ownItemCount["Muls_SN"];
        own_MulS_P = DataSaveManager.ownItemCount["Muls_P"];
        own_AiBarrier = DataSaveManager.ownItemCount["AiBarrier"];
        own_ProtectWall = DataSaveManager.ownItemCount["ProtectWall"];
        own_Extend_B = DataSaveManager.ownItemCount["Extend_B"];
        own_Extend_SN = DataSaveManager.ownItemCount["Extend_SN"];
        own_Extend_P = DataSaveManager.ownItemCount["Extend_P"];
        own_Recovery = DataSaveManager.ownItemCount["Recovery"];

        own_money = DataSaveManager.ownItemCount["Money"];

        SetItemInfo();
    }

    private void OnApplicationQuit()
    {
        if (use_confirm)
        {
            //PlayerPrefs.SetInt("MulS_B", own_MulS_B);
            //PlayerPrefs.SetInt("MulS_SN", own_MulS_SN);
            //PlayerPrefs.SetInt("MulS_P", own_MulS_P);
            //PlayerPrefs.SetInt("AiBarrier", own_AiBarrier);
            //PlayerPrefs.SetInt("ProtectWall", own_ProtectWall);
            //PlayerPrefs.SetInt("Extend_B", own_Extend_B);
            //PlayerPrefs.SetInt("Extend_SN", own_Extend_SN);
            //PlayerPrefs.SetInt("Extend_P", own_Extend_P);
            //PlayerPrefs.SetInt("Recovery", own_Recovery);
            //PlayerPrefs.SetInt("Money", own_money);
            DataSaveManager.WriteData("DB_Item.csv", DataSaveManager.ownItemCount);
        }
        DataSaveManager.WriteData("DB_Item.csv", DataSaveManager.ownItemCount);

    }

    private void SetItemInfo()
    {
        name_Muls_B = "연발장치(일반)";
        txt_Muls_B = "일반 타워의 공격력을 1.5배 증가시켜줍니다.";
        price_Muls_B = 300;

        name_Muls_SN = "연발장치(저격)";
        txt_Muls_SN = "저격 타워의 공격력을 1.5배 증가시켜줍니다.";
        price_Muls_SN = 500;

        name_Muls_P = "연발장치(충격)";
        txt_Muls_P = "충격 타워의 공격력을 1.5배 증가시켜줍니다.";
        price_Muls_P = 750;

        name_AiBarrire = "A.I배리어";
        txt_AiBarrier = "적의 공격을 1회 막아줍니다. 장착 후 맞지 않으면 소모되지 않고 장착 상태를 유지합니다.";
        price_AiBarrier = 200;

        name_ProtectWall = "방호벽";
        txt_ProtectWall = "캐릭터 주변에 4개의 방호벽을 세웁니다. 방호벽의 체력은 5입니다.";
        price_ProtectWall = 500;

        name_Extend_B = "확장장치(일반)";
        txt_Extend_B = "일반 타워의 공격 범위를 1.5배 길게 만듭니다.";
        price_Extend_B = 450;

        name_Extend_SN = "확장장치(저격)";
        txt_Extend_SN = "저격 타워의 공격 범위를 1.5배 길게 만듭니다.";
        price_Extend_SN = 650;

        name_Extend_P = "확장장치(충격)";
        txt_Extend_P = "충격 타워의 공격 범위를 1.5배 길게 만듭니다.";
        price_Extend_P = 900;

        name_Recovery = "수복자재";
        txt_Recovery = "플레이어의 최대 체력을 1 증가시킵니다.";
        price_Recovery = 1000;
    }

    public void End_Stage()
    {
        i_muls_b = false;
        i_muls_sn = false;
        i_muls_p = false;
        i_protectWall = false;
        i_extend_b = false;
        i_extend_sn = false;
        i_extend_p = false;
        i_recovery = false;

        i_event_AiBarrier = false;
        i_event_ProtectWall = false;

        //PlayerPrefs.SetInt("Moeny", own_money);
        DataSaveManager.WriteData("DB_Item.csv", DataSaveManager.ownItemCount);
    }
    public void Item_Use_Confirm(int i_num)
    {
        use_confirm = true;
        //PlayerPrefs.SetInt("Money", own_money);
        DataSaveManager.ownItemCount["Money"] = own_money;
        switch (i_num)
        {
            case 0:
                i_muls_b = true;
                //PlayerPrefs.SetInt("MulS_B", own_MulS_B);
                DataSaveManager.ownItemCount["Muls_B"] = own_MulS_B;
                break;
            case 1:
                i_muls_sn = true;
                //PlayerPrefs.SetInt("MulS_SN", own_MulS_SN);
                DataSaveManager.ownItemCount["Muls_SN"] = own_MulS_SN;
                break;
            case 2:
                i_muls_p = true;
                //PlayerPrefs.SetInt("MulS_P", own_MulS_P);
                DataSaveManager.ownItemCount["Muls_P"] = own_MulS_P;
                break;
            case 3:
                i_aiBarrier = true;
                //PlayerPrefs.SetInt("AiBarrier", own_AiBarrier);
                //PlayerPrefs.SetInt("used_AiBarrier", 1);
                DataSaveManager.ownItemCount["AiBarrier"] = own_AiBarrier;
                DataSaveManager.ownItemCount["used_AiBarrier"] = 1;
                break;
            case 4:
                i_protectWall = true;
                //PlayerPrefs.SetInt("ProtectWall", own_ProtectWall);
                DataSaveManager.ownItemCount["ProtectWall"] = own_ProtectWall;
                break;
            case 5:
                i_extend_b = true;
                //PlayerPrefs.SetInt("Extend_B", own_Extend_B);
                DataSaveManager.ownItemCount["Extend_B"] = own_Extend_B;
                break;
            case 6:
                i_extend_sn = true;
                //PlayerPrefs.SetInt("Extend_SN", own_Extend_SN);
                DataSaveManager.ownItemCount["Extend_SN"] = own_Extend_SN;
                break;
            case 7:
                i_extend_p = true;
                //PlayerPrefs.SetInt("Extend_P", own_Extend_P);
                DataSaveManager.ownItemCount["Extend_P"] = own_Extend_P;
                break;
            case 8:
                //PlayerPrefs.SetInt("Recovery", own_Recovery);
                DataSaveManager.ownItemCount["Recovery"] = own_Recovery;
                i_recovery = true;
                break;
        }
        DataSaveManager.WriteData("DB_Item.csv", DataSaveManager.ownItemCount);
    }

    public void Item_PreUse(int i_num)
    {
        use_confirm = false;
        switch (i_num)
        {
            case 0:
                own_MulS_B -= 1;
                break;
            case 1:
                own_MulS_SN -= 1;
                break;
            case 2:
                own_MulS_P -= 1;
                break;
            case 3:
                own_AiBarrier -= 1;
                break;
            case 4:
                own_ProtectWall -= 1;
                break;
            case 5:
                own_Extend_B -= 1;
                break;
            case 6:
                own_Extend_SN -= 1;
                break;
            case 7:
                own_Extend_P -= 1;

                break;
            case 8:
                own_Recovery -= 1;
                break;
        }
    }

    public void Item_Get(int i_num)
    {
        switch (i_num)
        {
            case 0:
                own_MulS_B += 1;
                break;
            case 1:
                own_MulS_SN += 1;
                break;
            case 2:
                own_MulS_P += 1;
                break;
            case 3:
                own_AiBarrier += 1;
                break;
            case 4:
                own_ProtectWall += 1;
                break;
            case 5:
                own_Extend_B += 1;
                break;
            case 6:
                own_Extend_SN += 1;
                break;
            case 7:
                own_Extend_P += 1;
                break;
            case 8:
                own_Recovery += 1;
                break;
        }
    }

    public void Item_Get_Confirm(int i_num)
    {
        switch (i_num)
        {
            case 0:
                //PlayerPrefs.SetInt("MulS_B", own_MulS_B);
                DataSaveManager.ownItemCount["Muls_B"] = own_MulS_B;
                break;
            case 1:
                //PlayerPrefs.SetInt("MulS_SN", own_MulS_SN);
                DataSaveManager.ownItemCount["Muls_SN"] = own_MulS_SN;
                break;
            case 2:
                //PlayerPrefs.SetInt("MulS_P", own_MulS_P);
                DataSaveManager.ownItemCount["Muls_P"] = own_MulS_P;
                break;
            case 3:
                //PlayerPrefs.SetInt("AiBarrier", own_AiBarrier);
                DataSaveManager.ownItemCount["AiBarrier"] = own_AiBarrier;
                break;
            case 4:
                //PlayerPrefs.SetInt("ProtectWall", own_ProtectWall);
                DataSaveManager.ownItemCount["ProtectWall"] = own_ProtectWall;
                break;
            case 5:
                //PlayerPrefs.SetInt("Extend_B", own_Extend_B);
                DataSaveManager.ownItemCount["Extend_B"] = own_Extend_B;
                break;
            case 6:
                //PlayerPrefs.SetInt("Extend_SN", own_Extend_SN);
                DataSaveManager.ownItemCount["Extend_SN"] = own_Extend_SN;
                break;
            case 7:
                //PlayerPrefs.SetInt("Extend_P", own_Extend_P);
                DataSaveManager.ownItemCount["Extend_P"] = own_Extend_P;
                break;
            case 8:
                //PlayerPrefs.SetInt("Recovery", own_Recovery);
                DataSaveManager.ownItemCount["Recovery"] = own_Recovery;
                break;
        }
        DataSaveManager.WriteData("DB_Item.csv", DataSaveManager.ownItemCount);
    }

    public void Use_Money(int used)
    {
        own_money -= used;
        //PlayerPrefs.SetInt("Money", own_money);
        DataSaveManager.ownItemCount["Money"] = own_money;
        DataSaveManager.WriteData("DB_Item.csv", DataSaveManager.ownItemCount);
    }

    public void Get_Money(int get)
    {
        own_money += get;
    }


    public void BarrierBreak()
    {
        i_aiBarrier = false;
        //PlayerPrefs.SetInt("used_AiBarrier", 0);
        //PlayerPrefs.SetInt("where_Barrier", 0);
        DataSaveManager.ownItemCount["used_AiBarrier"] = 0;
        DataSaveManager.ownItemCount["where_Barrier"] = 0;
        DataSaveManager.WriteData("DB_Item.csv", DataSaveManager.ownItemCount);
    }

    public void SetEvent0_2()
    {
        i_event_AiBarrier = true;
        i_event_ProtectWall = true;
    }
}

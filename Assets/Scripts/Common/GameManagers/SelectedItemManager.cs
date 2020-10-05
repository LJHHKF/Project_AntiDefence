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
    public bool get_money = false;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("MulS_B") <= 0)
            own_MulS_B = 0;
        else
            own_MulS_B = PlayerPrefs.GetInt("MulS_B");
        if (PlayerPrefs.GetInt("MulS_SN") <= 0)
            own_MulS_SN = 0;
        else
            own_MulS_SN = PlayerPrefs.GetInt("MulS_SN");
        if (PlayerPrefs.GetInt("MulS_P") <= 0)
            own_MulS_P = 0;
        else
            own_MulS_P = PlayerPrefs.GetInt("MulS_P");
        if (PlayerPrefs.GetInt("AiBarrier") <= 0)
            own_AiBarrier = 0;
        else
            own_AiBarrier = PlayerPrefs.GetInt("AiBarrier");
        if (PlayerPrefs.GetInt("ProtectWall") <= 0)
            own_ProtectWall = 0;
        else
            own_ProtectWall = PlayerPrefs.GetInt("ProtectWall");
        if (PlayerPrefs.GetInt("Extend_B") <= 0)
            own_Extend_B = 0;
        else
            own_Extend_B = PlayerPrefs.GetInt("Extend_B");
        if (PlayerPrefs.GetInt("Extend_SN") <= 0)
            own_Extend_SN = 0;
        else
            own_Extend_SN = PlayerPrefs.GetInt("Extend_SN");
        if (PlayerPrefs.GetInt("Extend_P") <= 0)
            own_Extend_P = 0;
        else
            own_Extend_P = PlayerPrefs.GetInt("Extend_P");
        if (PlayerPrefs.GetInt("Recovery") <= 0)
            own_Recovery = 0;
        else
            own_Recovery = PlayerPrefs.GetInt("Recovery");

        if (PlayerPrefs.GetInt("Money") <= 0)
            own_money = 0;
        else
            own_money = PlayerPrefs.GetInt("Money");
    }

    private void OnApplicationQuit()
    {
        if (use_confirm)
        {
            PlayerPrefs.SetInt("MulS_B", own_MulS_B);
            PlayerPrefs.SetInt("MulS_SN", own_MulS_SN);
            PlayerPrefs.SetInt("MulS_P", own_MulS_P);
            PlayerPrefs.SetInt("AiBarrier", own_AiBarrier);
            PlayerPrefs.SetInt("ProtectWall", own_ProtectWall);
            PlayerPrefs.SetInt("Extend_B", own_Extend_B);
            PlayerPrefs.SetInt("Extend_SN", own_Extend_SN);
            PlayerPrefs.SetInt("Extend_P", own_Extend_P);
            PlayerPrefs.SetInt("Recovery", own_Recovery);
            PlayerPrefs.SetInt("Money", own_money);
        }
        else if(get_money)
        {
            PlayerPrefs.SetInt("Money", own_money);
        }
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

        PlayerPrefs.SetInt("Moeny", own_money);
    }
    public void Item_Use_Confirm(int i_num)
    {
        use_confirm = true;
        get_money = false;
        PlayerPrefs.SetInt("Money", own_money);
        switch (i_num)
        {
            case 0:
                i_muls_b = true;
                PlayerPrefs.SetInt("MulS_B", own_MulS_B);
                break;
            case 1:
                i_muls_sn = true;
                PlayerPrefs.SetInt("MulS_SN", own_MulS_SN);
                break;
            case 2:
                i_muls_p = true;
                PlayerPrefs.SetInt("MulS_P", own_MulS_P);
                break;
            case 3:
                i_aiBarrier = true;
                PlayerPrefs.SetInt("AiBarrier", own_AiBarrier);
                PlayerPrefs.SetInt("used_AiBarrier", 1);
                break;
            case 4:
                i_protectWall = true;
                PlayerPrefs.SetInt("ProtectWall", own_ProtectWall);
                break;
            case 5:
                i_extend_b = true;
                PlayerPrefs.SetInt("Extend_B", own_Extend_B);
                break;
            case 6:
                i_extend_sn = true;
                PlayerPrefs.SetInt("Extend_SN", own_Extend_SN);
                break;
            case 7:
                i_extend_p = true;
                PlayerPrefs.SetInt("Extend_B", own_Extend_P);
                break;
            case 8:
                PlayerPrefs.SetInt("Recovery", own_Recovery);
                i_recovery = true;
                break;
        }
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
                PlayerPrefs.SetInt("MulS_B", own_MulS_B);
                break;
            case 1:
                PlayerPrefs.SetInt("MulS_SN", own_MulS_SN);
                break;
            case 2:
                PlayerPrefs.SetInt("MulS_P", own_MulS_P);
                break;
            case 3:
                PlayerPrefs.SetInt("AiBarrier", own_AiBarrier);
                break;
            case 4:
                PlayerPrefs.SetInt("ProtectWall", own_ProtectWall);
                break;
            case 5:
                PlayerPrefs.SetInt("Extend_B", own_Extend_B);
                break;
            case 6:
                PlayerPrefs.SetInt("Extend_SN", own_Extend_SN);
                break;
            case 7:
                PlayerPrefs.SetInt("Extend_P", own_Extend_P);
                break;
            case 8:
                PlayerPrefs.SetInt("Recovery", own_Recovery);
                break;
        }
    }

    public void Use_Money(int used)
    {
        own_money -= used;
        PlayerPrefs.SetInt("Money", own_money);
    }

    public void Get_Money(int get)
    {
        own_money += get;
        get_money = true;
    }


    public void BarrierBreak()
    {
        i_aiBarrier = false;
        PlayerPrefs.SetInt("used_AiBarrier", 0);
        PlayerPrefs.SetInt("where_Barrier", 0);
    }
}

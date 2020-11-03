using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseBtn : MonoBehaviour
{
    public Sprite spr_Pause;
    public Sprite spr_Play;

    private Image m_img;
    private WallMove[] wallMoves;

    private bool is_paused = false;

    // Start is called before the first frame update
    void Start()
    {
        m_img = gameObject.GetComponent<Image>();

        m_img.sprite = spr_Pause;

        wallMoves = GameObject.FindGameObjectWithTag("StageMObject").transform.Find("walls").GetComponentsInChildren<WallMove>();
    }


    public void PauseClicked()
    {
        if (!is_paused)
        {
            m_img.sprite = spr_Play;
            is_paused = true;
            Time.timeScale = 0.0f;

            for(int i = 0; i < wallMoves.Length; i++)
            {
                wallMoves[i].TurnMoveSpeed();
            }
        }
        else
        {
            m_img.sprite = spr_Pause;
            is_paused = false;
            Time.timeScale = 1.0f;

            for (int i = 0; i < wallMoves.Length; i++)
            {
                wallMoves[i].TurnMoveSpeed();
            }
        }
    }
}

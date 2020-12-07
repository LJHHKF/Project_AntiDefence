using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetCtrl : MonoBehaviour
{
    private GameObject another;
    private GameObject player;

    private EnemyCtrl enemyCtrl;

    private bool findOther = false;
    private int m_enemyIndex;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        enemyCtrl = gameObject.GetComponentInParent<EnemyCtrl>();

        enemyCtrl.SetTarget(player);
        m_enemyIndex = enemyCtrl.GetEnemyIndex();
    }


    // Update is called once per frame
    void Update()
    {
        if (findOther)
        {
            if (another.gameObject == null)
            {
                findOther = false;
            }
            else if(another.gameObject.activeSelf == false)
            {
                findOther = false;
            }
            else
            {
                enemyCtrl.SetTarget(another);
            }
        }
        else
        {
            enemyCtrl.SetTarget(player);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!findOther)
        {
            if (m_enemyIndex == 2)
            {
                if (other.CompareTag("BugTarget"))
                {
                    findOther = true;
                    another = other.gameObject;
                }
            }
            if (other.CompareTag("Barricade"))
            {
                findOther = true;
                another = other.gameObject;
            }
        }
    }

    public void SetFindOther(bool value)
    {
        findOther = value;
    }
}

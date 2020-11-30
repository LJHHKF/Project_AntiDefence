using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBugTarget : MonoBehaviour
{
    public GameObject m_Enemy;
    private EnemyCtrl m_EnemyCtrl;
    private StageManager m_stgM;

    private bool isFirst;
    private void Start()
    {
        m_EnemyCtrl = m_Enemy.GetComponent<EnemyCtrl>();
        m_stgM = GameObject.FindGameObjectWithTag("StageMObject").GetComponent<StageManager>();
    }

    private void OnEnable()
    {
        isFirst = true;
    }

    public void SetBuged()
    {
        if (isFirst)
        {
            m_stgM.SpawnBugEnemy(m_Enemy.transform);
            m_EnemyCtrl.SetIsDie(true);
            m_Enemy.SetActive(false);
            isFirst = false;
        }
    }
}

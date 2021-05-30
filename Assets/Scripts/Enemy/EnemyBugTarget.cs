using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myTarget;

public class EnemyBugTarget : MonoBehaviour
{
    public GameObject m_Enemy;
    private EnemyCtrl m_EnemyCtrl;
    private StageManager m_stgM;

    private bool isFirst;
    private int m_target_index;

    private void Start()
    {
        m_EnemyCtrl = m_Enemy.GetComponent<EnemyCtrl>();
        m_stgM = GameObject.FindGameObjectWithTag("StageMObject").GetComponent<StageManager>();
    }

    private void OnEnable()
    {
        m_target_index = TargetListManager.instance.AddList(gameObject, targetType.bug_Target);
        TargetListManager.instance.ev_RemoveAt += RemoveAtEvent;
    }

    private void OnDisable()
    {
        TargetListManager.instance.RemoveAtList(m_target_index);
    }

    private void RemoveAtEvent(int a)
    {
        if (m_target_index > a)
            m_target_index -= 1;
    }

    public void SetIsFirstTrue()
    {
        isFirst = true;
    }

    public void SetBuged()
    {
        if (isFirst)
        {
            isFirst = false;
            m_stgM.SpawnBugEnemy(m_Enemy.transform);
            m_EnemyCtrl.SetIsDie(true);
            m_Enemy.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}

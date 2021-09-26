using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myTarget;
using System;

public class EnemyTargetCtrl : MonoBehaviour
{
    [Serializable]
    private struct TargetSetting
    {
        public targetType type;
        public float BonusLength;
    }

    [Header("Custom Setting")]
    [SerializeField] private EnemyCtrl m_enemyCtrl;
    [SerializeField] private bool isHadBonusLength;
    [SerializeField] private TargetSetting[] m_targets;

    private readonly float delay = 1.0f;
    private float delayTime = 0.0f;

    private float curLength;
    private int targetIndex;
    private int prevTargetIndex; // 대각선 등으로 인해 바리게이트 등 있을 때, 어느 타겟을 공격해야할까 고민하면서 속도 느려지는 놈이 생겨서 만듦.
    private int prevSum;

    private void OnEnable()
    {
        curLength = 1000f;
        targetIndex = -1;
        prevTargetIndex = -1;
        prevSum = 1;
    }

    private void Update()
    {
        if (delayTime > 0f)
            delayTime -= Time.deltaTime;
        else
            FindTarget();
    }

    private void FindTarget()
    {
        delayTime = delay;
        for (int i = 0; i < TargetListManager.instance.GetListMax(); i++)
        {
            if (targetIndex != i)
            {
                for (int j = 0; j < m_targets.Length; j++)
                {
                    if (TargetListManager.instance.GetIndex_Type(i) == m_targets[j].type)
                    {
                        if (isHadBonusLength)
                        {
                            if (curLength >= TargetListManager.instance.GetDistance_normal(transform, i) - m_targets[j].BonusLength)
                            {
                                if (prevTargetIndex == i)
                                    delayTime = 1+ ++prevSum * delay;
                                else
                                    prevSum = 0;
                                curLength = TargetListManager.instance.GetDistance_normal(transform, i) - m_targets[j].BonusLength;
                                prevTargetIndex = targetIndex;
                                targetIndex = i;
                                m_enemyCtrl.SetTarget(TargetListManager.instance.GetIndex_GameObject(i), TargetListManager.instance.GetIndex_Type(i));
                            }
                        }
                        else
                        {
                            if (curLength >= TargetListManager.instance.GetDistance_sqr(transform, i))
                            {
                                if (prevTargetIndex == i)
                                    delayTime = 1 + ++prevSum * delay;
                                else
                                    prevSum = 0;
                                curLength = TargetListManager.instance.GetDistance_sqr(transform, i);
                                prevTargetIndex = targetIndex;
                                targetIndex = i;
                                m_enemyCtrl.SetTarget(TargetListManager.instance.GetIndex_GameObject(i), TargetListManager.instance.GetIndex_Type(i));
                            }
                        }
                        break;
                    }
                }
            }
        }
    }

    public void SetFindOther()
    {
        curLength = 1000f;
        targetIndex = -1;
        FindTarget();
    }

    //private GameObject another;
    //private GameObject player;

    //private EnemyCtrl enemyCtrl;

    //private bool findOther = false;
    //private int m_enemyIndex;

    //private void Awake()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player");

    //    enemyCtrl = gameObject.GetComponentInParent<EnemyCtrl>();

    //    enemyCtrl.SetTarget(player);
    //    m_enemyIndex = enemyCtrl.GetEnemyIndex();
    //}


    //// Update is called once per frame
    //void Update()
    //{
    //    if (findOther)
    //    {
    //        if (another.gameObject == null)
    //        {
    //            findOther = false;
    //        }
    //        else if(another.gameObject.activeSelf == false)
    //        {
    //            findOther = false;
    //        }
    //        else
    //        {
    //            enemyCtrl.SetTarget(another);
    //        }
    //    }
    //    else
    //    {
    //        enemyCtrl.SetTarget(player);
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (!findOther)
    //    {
    //        if (m_enemyIndex == 2)
    //        {
    //            if (other.CompareTag("BugTarget"))
    //            {
    //                findOther = true;
    //                another = other.gameObject;
    //            }
    //        }
    //        if (other.CompareTag("Barricade"))
    //        {
    //            findOther = true;
    //            another = other.gameObject;
    //        }
    //    }
    //}

    //public void SetFindOther(bool value)
    //{
    //    findOther = value;
    //}
}

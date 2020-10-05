using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetCtrl : MonoBehaviour
{
    private GameObject another;
    private GameObject player;

    private EnemyCtrl enemyCtrl;

    private bool findOther = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        enemyCtrl = gameObject.GetComponentInParent<EnemyCtrl>();

        enemyCtrl.SetTarget(player);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Barricade"))
        {
            findOther = true;
            another = other.gameObject;
        }
    }
}

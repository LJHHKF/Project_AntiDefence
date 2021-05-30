using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myTarget;
using System;

namespace myTarget
{
    public enum targetType
    {
        player,
        barricade,
        bug_Target
    }
}

public class TargetListManager : MonoBehaviour
{
    private static TargetListManager m_instance;
    public static TargetListManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<TargetListManager>();
            return m_instance;
        }
    }

    //private struct target
    //{
    //    public GameObject obj;
    //    public targetType type;
    //}
    //private List<target> list_target = new List<target>();

    private List<GameObject> list_target_GameObject = new List<GameObject>();
    private List<targetType> list_target_type = new List<targetType>();
    public event Action<int> ev_RemoveAt;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        AddList(GameObject.FindGameObjectWithTag("Player"), targetType.player);
    }

    /// <summary>
    /// 리스트에 추가하고 추가한 것의 인덱스를 돌려받는 것.
    /// </summary>
    public int AddList(GameObject obj, targetType type)
    {
        list_target_GameObject.Add(obj);
        list_target_type.Add(type);
        return list_target_GameObject.Count - 1; // 인덱스 돌려주는 것
    }

    public void RemoveAtList(int index)
    {
        list_target_GameObject.RemoveAt(index);
        list_target_type.RemoveAt(index);
        ev_RemoveAt?.Invoke(index);
    }

    public int GetListMax()
    {
        return list_target_GameObject.Count;
    }

    public targetType GetIndex_Type(int t_index)
    {
        return list_target_type[t_index];
    }

    public GameObject GetIndex_GameObject(int t_index)
    {
        return list_target_GameObject[t_index];
    }

    public float GetDistance_sqr(Transform _transform, int t_index)
    {
        float result = (list_target_GameObject[t_index].transform.position - _transform.position).sqrMagnitude;
        return result;
    }

    public float GetDistance_normal(Transform _transform, int t_index)
    {
        float result = (list_target_GameObject[t_index].transform.position - _transform.position).magnitude;
        return result;
    }
}

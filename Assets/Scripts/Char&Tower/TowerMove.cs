using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TowerMove : MonoBehaviour
{
    private Transform tr;

    private Vector2 touchBeganPos;
    private Vector2 touchEndPos;
    private Vector2 touchDif;
    private bool onDrag;
    //private Touch[] m_touches;
    public float rotateSensitive = 1;
    private float rotYValue;
    



    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        //마우스 기반
        if (Input.GetMouseButtonDown(0))
        {
            SwipeEvent();
        }
        else if (Input.GetMouseButton(0))
        {
            SwipeEvent();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            onDrag = false;
        }


        //터치 기반 -테스트 실패
        //if (Input.touchCount > 0)
        //{
        //    m_touches = Input.touches;
        //    for (int i = 0; i < Input.touchCount; i++)
        //    {
        //        if (m_touches[i].phase == TouchPhase.Moved)
        //        {
        //            SwipeEvent(i);
        //        }
        //        else if (m_touches[i].phase == TouchPhase.Ended)
        //        {
        //            onDrag = false;
        //        }
        //    }
        //}



    }

    void SwipeEvent()
    {
        float rv;
        if (onDrag == false)
        {
            //마우스 기반
            touchBeganPos = Input.mousePosition;
            touchEndPos = Input.mousePosition;

            onDrag = true;
        }
        else if (onDrag)
        {
            touchBeganPos = this.touchEndPos;
            //마우스 기반
            touchEndPos = Input.mousePosition;

            touchDif = touchEndPos - touchBeganPos;
        }

        rv = rotateSensitive * touchDif.magnitude;

        if (touchDif.x > 0)
        {
            // 오른방향 스와이프
            rotYValue += rv;
            tr.rotation = Quaternion.Euler(0, rotYValue, 0);
        }
        else if (touchDif.x < 0)
        {
            // 왼쪽방향 스와이프
            rotYValue -= rv;
            tr.rotation = Quaternion.Euler(0, rotYValue, 0);
        }
    }

    //void SwipeEvent(int order)
    //{
    //    float rv;
    //    if (onDrag == false)
    //    {
    //        //터치기반 
    //        touchBeganPos = m_touches[order].position;
    //        touchEndPos = m_touches[order].position;

    //        onDrag = true;
    //    }
    //    else if (onDrag)
    //    {
    //        touchBeganPos = touchEndPos;

    //        //터치 기반
    //        touchEndPos = m_touches[order].position;

    //        touchDif = touchEndPos - touchBeganPos;
    //    }

    //    rv = rotateSensitive * touchDif.magnitude;

    //    if (touchDif.x > 0)
    //    {
    //        // 오른방향 스와이프
    //        rotYValue += rv;
    //        tr.rotation = Quaternion.Euler(0, rotYValue, 0);
    //    }
    //    else if (touchDif.x < 0)
    //    {
    //        // 왼쪽방향 스와이프
    //        rotYValue -= rv;
    //        tr.rotation = Quaternion.Euler(0, rotYValue, 0);
    //    }
    //}

}

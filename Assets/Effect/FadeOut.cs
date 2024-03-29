﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{ 
    float time;
    public float _fadeTime;

    // Update is called once per frame
    void Update()
    {
        if (time < _fadeTime)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f - time / _fadeTime);
        }
        else
        {
            time = 0f;
            this.gameObject.SetActive(false);
        }
        time += Time.deltaTime;
    }
        public void resetAnim()
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            this.gameObject.SetActive(true);
        }
    }

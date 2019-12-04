using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDHK_Tool.Component;
using SDHK_Tool.Dynamic;
using System;

public class GameManager : MonoBehaviour
{

    static public GameManager instance;

    public SC_ScrollGroup GroupBG;




    private void Awake()
    {
        instance = this;
        // Application.targetFrameRate = 60;//帧数锁定
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

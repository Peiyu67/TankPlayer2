using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadMenu : MonoBehaviour
{
    public static DeadMenu instance;
    private  GameObject DeadUI;

    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;

        DeadUI = transform.GetChild(0).gameObject;
        DeadUI.SetActive(false);
    }

    public void ShowDead(bool b)
    {
         DeadUI.SetActive(b);
    }
}

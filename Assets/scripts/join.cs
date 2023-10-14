using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class join : MonoBehaviour
{
    public InputField serveraddres_in;
    public InputField name_in;
    private Button btn;
    public static string playerName;
    public static string serverAddres;
    // Start is called before the first frame update
    void Start()
    {
        serveraddres_in = serveraddres_in.GetComponent<InputField>();
        name_in = name_in.GetComponent<InputField>();
        btn = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(name_in.text.Equals("") || serveraddres_in.text.Equals(""))
        {
            btn.interactable = false;
        }
        else
        {
            btn.interactable = true;
        }
    }

    public void OnClick()
    {
        playerName = name_in.text;
        serverAddres = serveraddres_in.text;
        SceneManager.LoadSceneAsync("Main");
    }
}

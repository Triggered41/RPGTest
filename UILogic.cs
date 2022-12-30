using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sfs2X.Core;
using TMPro;

public class UILogic : MonoBehaviour
{
    public GameObject textField;
    private TMP_InputField username;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("OKOK");
        username = textField.GetComponent<TMP_InputField>();
        username.onSubmit.AddListener(Sub);
        SFSConnection.Instance.sfs.AddEventListener(SFSEvent.LOGIN, OnLoginUI);

    }

    void Sub(string str)
    {
        Debug.Log("==========================");
        Debug.Log(str);
        // sfSf.Con.Login(str)
        Debug.Log("UI Logging");
        SFSConnection.Instance.Login(str);
        // sfs.Login(str);
    }

    void OnLoginUI(BaseEvent ev)
    {
        Debug.Log("Scene Change?");
        SceneManager.LoadScene("Game");
    }
}

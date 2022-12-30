using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sfs2X.Entities.Variables;

public class NameTextUpdate : MonoBehaviour
{
    public Slider slide; 
    // Start is called before the first frame update
    void Start()
    {
        SetName(SFSConnection.Instance.sfs.MySelf.Name);
    }

    void FixedUpdate()
    {
        UserVariable var = SFSConnection.Instance.sfs.MySelf.GetVariable("Health");
        slide.value = var.GetIntValue();

    }

    public void SetName(string name)
    {
        GetComponent<TMP_Text>().text = name;
    }
}

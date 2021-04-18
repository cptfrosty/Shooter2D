using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public void Awake()
    {
        instance = this;
    }

    public UISelectTeam selectTeam;

    private void Start()
    {
        HideAllUI();
    }

    private void HideAllUI()
    {
        selectTeam.gameObject.SetActive(false);
    }

    public void HideUISelectTeam()
    {
        selectTeam.gameObject.SetActive(false);
    }

    public void ShowUISelectTeam()
    {
        selectTeam.gameObject.SetActive(true);
    }

    public void ShowSelectTeam() => selectTeam.gameObject.SetActive(true);
}

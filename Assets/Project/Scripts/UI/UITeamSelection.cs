using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UITeamSelection : MonoBehaviour
{
    public static UITeamSelection instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private Text countPlayersAll;
    [SerializeField] private Text countPlayersCT;
    [SerializeField] private Text countPlayersT;

    [Command(requiresAuthority = false)]
    public void LoadData()
    {
        countPlayersAll.text    =   "Игроков: " + GameManager.manager.GetCountClients;
        countPlayersCT.text     =   GameManager.manager.GetTeamContrTerror + "/5";
        countPlayersT.text      =   GameManager.manager.GetTeamTerror + "/5";
    }

    //Called by pressing the button
    public void SelectTeamT()
    {
        CmdSelectTeamT();
    }

    //Called by pressing the button
    public void SelectTeamCT()
    {
        CmdSelectTeamCT();
    }

    
    [Command(requiresAuthority = false)]
    private void CmdSelectTeamT()
    {
        GameManager.manager.AddTeamTerror();
    }

    //Called by pressing the button    [Command(requiresAuthority = false)]
    private void CmdSelectTeamCT()
    {
        GameManager.manager.AddTeamContrTerror();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerTeamController : NetworkBehaviour
{
    public enum TypeTeam
    {
        None, CT, T
    }

    public TypeTeam team;

    public delegate void InfoSelectTeamHandler(string info);
    public event InfoSelectTeamHandler OnSelectTeam;
    public event InfoSelectTeamHandler OnErrorSelectTeam;

    public delegate void InfoTeams(int countPlayers, int countPlayersCT, int countPlayersT, int maxPlayerInTeam);
    public event InfoTeams OnPrintInfoTeams;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        InitEvents();

        //UIController.instance.ShowSelectTeam();

        //CmdInfoTeams(netIdentity.connectionToClient);
    }

    //[Command(requiresAuthority = false)]
    //public void CmdInfoTeams(NetworkConnectionToClient client = null) => SceneController.instance.RpcPrintInfoTeam(client);

    public void InitEvents()
    {
        OnSelectTeam += UIController.instance.selectTeam.PrintMessage;
        OnErrorSelectTeam += UIController.instance.selectTeam.PrintMessage;
        OnPrintInfoTeams += UIController.instance.selectTeam.InfoAboutTeams;

        if (isLocalPlayer)
        {
            UIController.instance.selectTeam.selectTeamCT.onClick.AddListener(SelectTeamCT);
            UIController.instance.selectTeam.selectTeamT.onClick.AddListener(SelectTeamT);
        }
    }

    [TargetRpc]
    public void StatusSelectTeam(NetworkConnection client, bool isJoined)
    {
        if (isJoined)
        {
            OnSelectTeam("Вы присоединились к команде");
            UIController.instance.HideUISelectTeam();
        }
        else
        {
            OnErrorSelectTeam("Команда полная");
        }
    }

    public void PrintInfoTeams(int countPlayers, int countPlayersCT, int countPlayersT, int maxPlayerInTeam)
    {
        OnPrintInfoTeams(countPlayers, countPlayersCT, countPlayersT, maxPlayerInTeam);
    }

    #region Выбор команды

    //Called from the button
    public void SelectTeamCT()
    {
        if(isClient)
            CmdSelectTeamCT(netIdentity);
    }
    //Called from the button
    public void SelectTeamT()
    {
        if (isClient)
            CmdSelectTeamT(netIdentity);
    }

    [Command(requiresAuthority = false)]
    public void CmdSelectTeamCT(NetworkIdentity client)
    {
        SceneController.instance.SelectTeamPlayerCT(client);
    }
    [Command(requiresAuthority = false)]
    public void CmdSelectTeamT(NetworkIdentity client)
    {
        SceneController.instance.SelectTeamPlayerT(client);
    }
    #endregion
}

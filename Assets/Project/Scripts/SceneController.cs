using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SceneController : NetworkBehaviour
{
    #region Singleton
    public static SceneController instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] private List<Transform> spawnPositionCT = new List<Transform>();
    [SerializeField] private List<Transform> spawnPositionT = new List<Transform>();
    [SerializeField] private List<DataPlayerCombat> dataPlayerCombatCT = new List<DataPlayerCombat>();
    [SerializeField] private List<DataPlayerCombat> dataPlayerCombatT = new List<DataPlayerCombat>();

    private List<NetworkIdentity> clients = new List<NetworkIdentity>();
    private List<NetworkIdentity> teamCT = new List<NetworkIdentity>();
    private List<NetworkIdentity> teamT = new List<NetworkIdentity>();

    private int countLife = 0;

    private int maxPlayerInTeam = 5;

    #region StartLocalPlayer
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //При старте игрока отобразить выбор команды
        UIController.instance.ShowUISelectTeam();
        UpdateInfoTeam();
    }
    #endregion

    [Server]
    public void DeadPlayer()
    {
        countLife--;
    }

    [Server]
    private void UpdateInfoTeam()
    {
        RpcUpdateInfoTeam();
    }

    [ClientRpc]
    public void RpcUpdateInfoTeam()
    {
        if (UIController.instance.selectTeam.gameObject.activeSelf)
        {
            UIController.instance.selectTeam.InfoAboutTeams(clients.Count, teamCT.Count, teamT.Count, maxPlayerInTeam);
        }
    }

    [Server]
    public void RpcStartNewRound()
    {
        for(int i = 0; i < teamCT.Count; i++)
        {
            if (teamCT[i] == null)
            {
                teamCT.RemoveAt(i);
                i--;
            }
            else 
            {
                RpcStartRoundPlayerCT(i);


                countLife++;
            }
        }

        for(int i = 0; i < teamT.Count; i++)
        {
            if (teamT[i] == null)
            {
                teamT.RemoveAt(i);
                i--;
            }
            else
            {

                RpcStartRoundPlayerT(i);

                countLife++;
            }
        }
    }

    [ClientRpc]
    public void RpcStartRoundPlayerCT(int i)
    {
        teamCT[i].GetComponent<PlayerCombatController>().StatusDead(false);
        teamCT[i].transform.position = spawnPositionCT[i].position;
    }

    [ClientRpc]
    public void RpcStartRoundPlayerT(int i)
    {
        teamT[i].GetComponent<PlayerCombatController>().StatusDead(false);
        teamT[i].transform.position = spawnPositionT[i].position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartNewRound();
        }
        /*if((countLife == 0 && (teamCT.Count != 0 || teamT.Count != 0)) || (countLife == 1 && (teamCT.Count > 1 || teamT.Count > 1)))
        {
            
        }*/
    }

    [Server]
    private void StartNewRound()
    {
        RpcStartNewRound();
    }

    #region Team

    public int GetCountClients
    {
        get
        {
            return clients.Count;
        }
    }

    public int GetCountTeamCT
    {
        get
        {
            return teamCT.Count;
        }
    }

    public int GetCountTeamT
    {
        get
        {
            return teamT.Count;
        }
    }

    public int GetMaxPlayerInTeam
    {
        get
        {
            return maxPlayerInTeam;
        }
    }

    [Server]
    public void SelectTeamPlayerCT(NetworkIdentity client)
    {
        if (teamCT.Count == maxPlayerInTeam)
        {
            RpcMessageError(client);
            return;
        }

        PlayerTeamController player = client.GetComponent<PlayerTeamController>();

        if(player.team == PlayerTeamController.TypeTeam.CT)
        {
            return;
        }
        else if (player.team == PlayerTeamController.TypeTeam.None)
        {
            player.team = PlayerTeamController.TypeTeam.CT;
            teamCT.Add(client);
        }
        else
        {
            teamT.Remove(client);
            player.team = PlayerTeamController.TypeTeam.CT;
        }

        RpcSelectDataPlayerCombat(client);

        player.StatusSelectTeam(client.connectionToClient ,true);
    }

    [ClientRpc]
    public void RpcSelectDataPlayerCombat(NetworkIdentity client)
    {
        //ВЫБОР СКИНА ДЛЯ ИГРОКА
        int n = Random.Range(0, dataPlayerCombatCT.Count);
        client.GetComponent<PlayerCombatController>().SetDataPlayerCombat = dataPlayerCombatCT[n];
        //КОНЕЦ ВЫБОРА СКИНА ДЛЯ ИГРОКА
    }

    [Server]
    public void SelectTeamPlayerT(NetworkIdentity client)
    {
        if (teamT.Count == maxPlayerInTeam)
        {
            RpcMessageError(client);
            return;
        }

        PlayerTeamController player = client.GetComponent<PlayerTeamController>();
        if (player.team == PlayerTeamController.TypeTeam.T)
        {
            return;
        }
        else if (player.team == PlayerTeamController.TypeTeam.None)
        {
            player.team = PlayerTeamController.TypeTeam.T;
            teamT.Add(client);
        }
        else
        {
            teamCT.Remove(client);
            player.team = PlayerTeamController.TypeTeam.T;
        }

        //ВЫБОР СКИНА ДЛЯ ИГРОКА
        int n = Random.Range(0, dataPlayerCombatT.Count);
        client.GetComponent<PlayerCombatController>().SetDataPlayerCombat = dataPlayerCombatT[n];
        //КОНЕЦ ВЫБОРА СКИНА ДЛЯ ИГРОКА

        player.StatusSelectTeam(client.connectionToClient, true);
    }

    [TargetRpc]
    public void RpcMessageError(NetworkIdentity client)
    {
        PlayerTeamController player = client.GetComponent<PlayerTeamController>();
        player.StatusSelectTeam(client.connectionToClient, false);
    }

    [Server]
    public void AddClient(NetworkIdentity client)
    {
        clients.Add(client);
    }

    /*[TargetRpc]
    public void RpcPrintInfoTeam(NetworkConnection client)
    {
        client.identity.GetComponent<PlayerTeamController>().PrintInfoTeams(clients.Count, teamCT.Count, teamT.Count, maxPlayerInTeam);
    }*/

    #endregion
}

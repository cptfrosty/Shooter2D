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

        SelectDataPlayerCombat(client,0);

        client.GetComponent<PlayerCombatController>().idSkinNow = 0;
        client.GetComponent<PlayerCombatController>().idSkin = 0;

        player.StatusSelectTeam(client.connectionToClient ,true);
        player.transform.position = spawnPositionCT[0].position; //Спавн игрока
        RpcNewPositionPlayer(client, spawnPositionCT[0].position);
    }

    public void SelectDataPlayerCombat(NetworkIdentity client, int num)
    {
        //ВЫБОР СКИНА ДЛЯ ИГРОКА
        //int n = Random.Range(0, dataPlayerCombatCT.Count);
        //client.GetComponent<PlayerCombatController>().SetDataPlayerCombat = dataPlayerCombatCT[n];
        client.GetComponent<PlayerCombatController>().idSkinNow = num;
        client.GetComponent<PlayerCombatController>().idSkin = num;
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
        //int n = Random.Range(0, dataPlayerCombatT.Count);
        //client.GetComponent<PlayerCombatController>().SetDataPlayerCombat = dataPlayerCombatT[n];2;
        SelectDataPlayerCombat(client, 1);
        //КОНЕЦ ВЫБОРА СКИНА ДЛЯ ИГРОКА

        player.StatusSelectTeam(client.connectionToClient, true);
        player.transform.position = spawnPositionT[0].position; //Спавн игрока
        RpcNewPositionPlayer(client, spawnPositionT[0].position);

    }
    

    [ClientRpc]
    public void RpcNewPositionPlayer(NetworkIdentity client, Vector3 newPos)
    {
        PlayerTeamController player = client.GetComponent<PlayerTeamController>();
        player.transform.position = newPos; //Спавн игрока
        player.GetComponent<PlayerCombatController>().OnDead(false);
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

    #endregion
}

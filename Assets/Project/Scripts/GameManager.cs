using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    #region Singletone
    public static GameManager manager;

    private void Awake()
    {
        manager = this;
    }
    #endregion

    [SyncVar] private int _countClients;
    [SyncVar] private int _countTeamTerror;
    [SyncVar] private int _countTeamContrTerror;

    //[SerializeField] GameObject playerPrefab;

    #region Свойства
    public int GetCountClients { 
        get
        {
            return _countClients;
        } 
    }

    public int GetTeamTerror
    {
        get
        {
            return _countTeamTerror;
        }
    }

    public int GetTeamContrTerror
    {
        get
        {
            return _countTeamContrTerror;
        }
    }

    #endregion
    public void OnCreatPlayer(NetworkConnection conn, GameObject playerPrefab)
    {
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = new Vector3(-777, -777, 0);
        player.GetComponent<PlayerMovement>().SetCanMove = false;

        SceneController.controller.AddPlayer(player.GetComponent<PlayerCombat>());
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    /*[TargetRpc]
    public void RpcUpdateUITeamSelection(NetworkConnection conn)
    {
        conn.identity.gameObject.GetComponent<>
    }*/

    [Server]
    public void AddPlayer()
    {
        _countClients++;
    }
    [Server]
    public void AddTeamTerror()
    {
        _countTeamTerror++;
    }

    [Server]
    public void AddTeamContrTerror()
    {
        _countTeamContrTerror++;
    }

    [Server]
    public void SpawnPlayer()
    {

    }
}

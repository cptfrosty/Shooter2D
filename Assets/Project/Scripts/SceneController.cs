using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SceneController : NetworkBehaviour
{
    #region Singleton
    public static SceneController controller;

    private void Awake()
    {
        controller = this;
    }
    #endregion

    [SerializeField] private List<Transform> pointSpawnTerror;
    [SerializeField] private List<Transform> pointSpawnContrTerror;
    [SerializeField] private List<PlayerCombat> players;

    [SyncVar] int scoreCT;
    [SyncVar] int scoreT;

    [Server]
    public void AddPlayer(PlayerCombat player)
    {
        players.Add(player);
    }

    [Server]
    public void SpawnPlayer()
    {
        int posT = 0;
        int posCT = 0;

        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].typeTeam == TypeTeam.CT)
            {
                players[i].transform.position = pointSpawnContrTerror[posCT].position;
                posCT++;
            }
            else
            {
                players[i].transform.position = pointSpawnContrTerror[posT].position;
                posT++;
            }
        }
    }

    //Срабатывает когда начинается новый раунд ПОСЛЕ таймера подготовки
    [Server]
    public void AllEnableMovement()
    {
        for(int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerMovement>().SetCanMove = true;
        }
    }

    //Срабатывает когда начинается новый раунд ДО таймера подготовки
    [Server]
    public void AllDisabledMovement()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerMovement>().SetCanMove = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPointsOcean;
    public Transform[] spawnPointsLand;
    public Transform[] spawnPointsBoth;
    CinemachineVirtualCamera vcam;

    private void Start()
    {
        if((int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 1 || (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 2)
        {
            int randomNumber = Random.Range(0, spawnPointsOcean.Length);
            Transform spawnPoint = spawnPointsOcean[randomNumber];
            GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
            PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
            GameObject player = GameObject.Find(playerToSpawn.name);
        }
        else if((int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 0 || (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 3)
        {
            int randomNumber = Random.Range(0, spawnPointsLand.Length);
            Transform spawnPoint = spawnPointsLand[randomNumber];
            GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
            PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
            GameObject player = GameObject.Find(playerToSpawn.name);
        }
        else if((int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 4)
        {
            int randomNumber = Random.Range(0, spawnPointsBoth.Length);
            Transform spawnPoint = spawnPointsBoth[randomNumber];
            GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
            PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
            GameObject player = GameObject.Find(playerToSpawn.name);
        }
        
    }
}

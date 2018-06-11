using ProBuilder2.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour {

    private string _room = "hoi aaron";
    private TurnManager turnManager;
    private PhotonView photonView;
    [SerializeField] private Transform[] spawnpoints;
    [SerializeField] private Transform playerCamera;

	private void Start () {
        PhotonNetwork.ConnectUsingSettings("0.1");
        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
    }

    private void Update()
    {
        Debug.Log(PhotonNetwork.playerList.Length);
    }
    private void OnJoinedLobby() {
        Debug.Log("Joined Lobby");
        RoomOptions roomOptions = new RoomOptions() { };
        PhotonNetwork.JoinOrCreateRoom(_room, roomOptions, TypedLobby.Default);
    }

    private void OnJoinedRoom() {

        GameObject player1 = PhotonNetwork.Instantiate("NetworkedPlayer1", spawnpoints[PhotonNetwork.playerList.Length - 1].position, Quaternion.identity, 0);
        //GameObject player2 = PhotonNetwork.Instantiate("NetworkedPlayer2", spawnpoints[1].position, Quaternion.identity, 0);

    }

}

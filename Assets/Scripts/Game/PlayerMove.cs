using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : TacticsMove {
    //public TurnManager turnManager;
    //public TurnClass turnClass;
    //public bool isTurn;
    public Text turnText;

    private void Start () {
        Init();

	}

    private void Update () {
        turnText.text = (turn.ToString());
        if (photonView.isMine) {
            photonView.RPC("SendBool", PhotonTargets.All, turn);
            if (!turn) {
                    return;
                }
            if (!moving) {
                    //myTurn = turn;
                    FindSelectableTiles();
                    CheckMouse();
                }
            else {
                    Move();
                   // myTurn = turn;
            }
        }
    }

    private void CheckMouse() {
        if (Input.GetMouseButtonUp(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.tag == "Tile") {
                    TileScript t = hit.collider.GetComponent<TileScript>();

                    if (t.selectable) {
                        MoveToTile(t); 
                    }
                }
            }
        }
    }

    [PunRPC]
    void SendBool(bool turn) {
        Debug.Log(turn);
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(turn);
        }

        else if (stream.isReading)
        {
            turn = (bool)stream.ReceiveNext();
        }
    }


}

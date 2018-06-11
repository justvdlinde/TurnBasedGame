using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedBody : Photon.MonoBehaviour {

    public GameObject avatar;
    public Transform playerGlobal;
    public Transform playerLocal;

    private void Start() {
        if (photonView.isMine) {
            playerGlobal = GameObject.Find("OVRPlayerController").transform;
            playerLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");
            transform.SetParent(playerLocal);
            transform.localPosition = Vector3.zero;
        }
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if (stream.isWriting){
            stream.SendNext(playerGlobal.position);
            stream.SendNext(playerGlobal.rotation);
            stream.SendNext(playerLocal.localPosition);
            stream.SendNext(playerLocal.localRotation);
        }
        else {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            avatar.transform.localPosition = (Vector3)stream.ReceiveNext();
            avatar.transform.localRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}

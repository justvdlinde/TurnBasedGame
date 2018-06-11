using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class TurnManager : Photon.MonoBehaviour {

    static Dictionary<string, List<TacticsMove>> units = new Dictionary<string, List<TacticsMove>>();
    static Queue<string> turnKey = new Queue<string>();
    static Queue<TacticsMove> turnTeam = new Queue<TacticsMove>();

    private void Start() {
         
    }

    private void Update() {
        if (!turnTeam.Peek().turn)
            if (turnTeam.Count == 0)
            {
                InitTurnQueue();
            } 
    }

    static void InitTurnQueue()
    {   
        
        List<TacticsMove> teamList = units[turnKey.Peek()];

        foreach (TacticsMove unit in teamList) {
            turnTeam.Enqueue(unit);
        }
        StartTurn();
    }

    static void StartTurn() {
        if (turnTeam.Count > 0) {
            turnTeam.Peek().BeginTurn();
        }
    }

    public static void EndTurn() {
        TacticsMove unit = turnTeam.Dequeue();
        unit.EndTurn();

        if (turnTeam.Count > 0) {
            StartTurn();
        }
        else {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTurnQueue();
        }
    }

    public static void AddUnit(TacticsMove unit) {
        List<TacticsMove> list;

        if (!units.ContainsKey(unit.tag)) {
            list = new List<TacticsMove>();
            units[unit.tag] = list;

            if (!turnKey.Contains(unit.tag)) {
                turnKey.Enqueue(unit.tag);
            }
        }
        else {
            list = units[unit.tag];
        }
        list.Add(unit);
    }




    //public TurnClass DeserializeFromString<TurnClass>(string dPlayer) {
    //    byte[] b = Convert.FromBase64String(dPlayer);
    //    using (var stream = new MemoryStream(b)) {
    //        var formatter = new BinaryFormatter();
    //        stream.Seek(0, SeekOrigin.Begin);
    //        return (TurnClass)formatter.Deserialize(stream);
    //    }
    //}

    //public string SerializeToString<TurnClass>(TurnClass sPlayer) {
    //    using (var stream = new MemoryStream()) {
    //        var formatter = new BinaryFormatter();
    //        formatter.Serialize(stream, playerGroup);
    //        stream.Flush();
    //        stream.Position = 0;
    //        return Convert.ToBase64String(stream.ToArray());
    //    }
    //}

    //private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
    //    if (stream.isWriting) {
    //        stream.SendNext(playerGroup.ToArray());
    //    }

    //    else if (stream.isReading) {
    //       playerGroup = (List<TurnClass>)stream.ReceiveNext();
    //    }
    //}
}

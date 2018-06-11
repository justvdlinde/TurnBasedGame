using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    public List<TileScript> adjacencyList = new List<TileScript>();

    public bool visited = false;
    public TileScript parent = null;
    public int distance = 0;

    private void Update () {
		if (current) {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (target) {
            GetComponent<Renderer>().material.color = Color.black;
        }
        else if (selectable) {
            GetComponent<Renderer>().material.color = Color.grey;
        }
        else {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void Reset() {
        adjacencyList.Clear();

        walkable = true;
        current = false;
        target = false;
        selectable = false;

        visited = false;
        parent = null;
        distance = 0;
    }

    public void FindNeighbours(float jumpHeight) {
        Reset();
        CheckTile(Vector3.forward, jumpHeight);
        CheckTile(-Vector3.forward, jumpHeight);
        CheckTile(Vector3.right, jumpHeight);
        CheckTile(-Vector3.right, jumpHeight);

    }

    public void CheckTile(Vector3 dir, float jumpHeight) {
        Vector3 halfExtends = new Vector3(.25f, (1 + jumpHeight) / 2, .25f);
        Collider[] cols = Physics.OverlapBox(transform.position + dir, halfExtends);
        
        foreach (Collider item in cols) {
            TileScript tile = item.GetComponent<TileScript>();
            if (tile != null && tile.walkable) {
                RaycastHit hit;
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1)) {
                    adjacencyList.Add(tile);
                }
            }
        }
    }
}

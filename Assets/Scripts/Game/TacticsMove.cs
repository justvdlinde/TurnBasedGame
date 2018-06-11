using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : Photon.MonoBehaviour {

    public bool turn = false;
  

    public int move = 3;
    public float jumpHeight = 2;
    public float moveSpeed = 2;
    public bool moving = false;

    private Vector3 velocity = new Vector3();
    private Vector3 heading = new Vector3();

    private float halfHeight = 0;
    private List<TileScript> selectableTiles = new List<TileScript>();
    private GameObject[] tiles;
    private Stack<TileScript> path = new Stack<TileScript>();
    private TileScript curTile;

    private void Update()
    {
    }

    protected void Init() {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        TurnManager.AddUnit(this);
    }

    public void GetCurrentTile() {
        curTile = GetTargetTile(gameObject);
        curTile.current = true;
    }

    public TileScript GetTargetTile(GameObject target) {
        RaycastHit hit;
        TileScript tile = null;
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1)) {
            tile = hit.collider.GetComponent<TileScript>();       
        }
        return tile;
    }

    public void ComputeAdjacencyLists() {
        foreach (GameObject tile in tiles) {
            TileScript t = tile.GetComponent<TileScript>();
            t.FindNeighbours(jumpHeight);
        }
    }

    public void FindSelectableTiles() {
        ComputeAdjacencyLists();
        GetCurrentTile();

        Queue<TileScript> process = new Queue<TileScript>();
        process.Enqueue(curTile);
        curTile.visited = true;
        
        while (process.Count > 0) {
            TileScript t = process.Dequeue();
            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < move) { 
                foreach (TileScript tile in t.adjacencyList) {
                    if(!tile.visited) {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(TileScript tile) {
        path.Clear();
        tile.target = true;
        moving = true;

        TileScript next = tile;
        while (next != null) {
            path.Push(next);
            next = next.parent;
        }
    }

    public void Move() {
        if (path.Count > 0) {
            TileScript t = path.Peek();
            Vector3 target = t.transform.position;

            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f) {
                CalculateHeading(target);
                SetHorizontalVelocity();

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else {
                transform.position = target;
                path.Pop(); 
            }
        }
        else {
                RemoveSelectableTiles();
                moving = false;

            TurnManager.EndTurn();
        }
        
    }

    protected void RemoveSelectableTiles() {
        if (curTile != null) {
            curTile.current = false;
            curTile = null;
        }
        foreach (TileScript tile in selectableTiles) {
            tile.Reset();
        }
    }

    private void CalculateHeading(Vector3 target) {
        heading = target - transform.position;
        heading.Normalize();
    }

    private void SetHorizontalVelocity() {
        velocity = heading * moveSpeed;
    }
    
    public void BeginTurn() { 
        turn = true;
    }

    public void EndTurn() {
        turn = false;
    }
}

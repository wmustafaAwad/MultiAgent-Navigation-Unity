using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PathFinding : MonoBehaviour
{

    Grid grid;
    PathRequestManager requestManager;
    


    void Awake() {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos));
    
    }


    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;


        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            startNode.hCost = GetDistance(startNode, targetNode); //EditHere
            startNode.gCost = 0; ////EditHere

            while (openSet.Count > 0)
            {
                //Get Node with minimum fCost (use hCost as tie-braker):
                Node currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) { continue; }
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour)) { openSet.Add(neighbour); }
                        else { openSet.UpdateItemDec(neighbour); }
                    }


                }

            }
        } yield return null;// End of while loop
        if (pathSuccess) {
            waypoints = RetracePath(startNode, targetNode);
            }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);

    }// End of Findpath

    int GetDistance(Node nodeA, Node nodeB){
        int dstX = Mathf.RoundToInt(Mathf.Abs(nodeA.gridX - nodeB.gridX));
        int dstY = Mathf.RoundToInt(Mathf.Abs(nodeA.gridY - nodeB.gridY));

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY-dstX);
    }

    Vector3[] RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        if (currentNode == startNode) {
            path.Add(currentNode);
        
        }
        Vector3[] waypoints= SimplifyPath(path);
        Array.Reverse(waypoints);  
        return waypoints;
        
    }


    Vector3[] SimplifyPath(List<Node> path){
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld) {
                waypoints.Add(path[i-1].worldPosition);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }


}



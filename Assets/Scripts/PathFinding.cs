using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;


    void Update() {
        FindPath(seeker.position, target.position);
    }

    void Awake() {
        grid = GetComponent<Grid>();
    
    }
    void FindPath(Vector3 startPos, Vector3 targetPos) {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        List<Node> openSet = new List<Node>();
        HashSet <Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        startNode.hCost = GetDistance(startNode, targetNode);
        startNode.hCost = 0;

        while (openSet.Count > 0) {
            //Get Node with minimum fCost (use hCost as tie-braker):
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost< currentNode.hCost)) {
                    currentNode = openSet[i];
                }
            
            }//End of for Loop

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                RetracePath(startNode, targetNode);
                return; //Will come back to that Edit#
            }
            foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) { continue; }
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour)){openSet.Add(neighbour);}
                }


            }
        
        }// End of while loop

    }// End of Findpath

    int GetDistance(Node nodeA, Node nodeB){
        int dstX = Mathf.RoundToInt(Mathf.Abs(nodeA.gridX - nodeB.gridX));
        int dstY = Mathf.RoundToInt(Mathf.Abs(nodeA.gridY - nodeB.gridY));

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY-dstX);
    }

    void RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        grid.path = path;
    }
}



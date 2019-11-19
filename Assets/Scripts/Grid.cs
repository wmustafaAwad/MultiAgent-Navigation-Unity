using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2 gridWorldSize; //Size of grid world in terms of unity points (Check gridSizeX and gridSizeY for contrast)
    public float nodeRadius;
    public LayerMask unwalkableMask;
    Node[,] grid;
    public Transform Player;
    public bool displayGridGizmos;

    float nodeDiameter; //Diameter of single node (grid square) in terms of unity points.
    int gridSizeX, gridSizeY; //Sizes of Grid in terms of number of grids

    public Node NodeFromWorldPoint(Vector3 wolrdPosition){
        float percentX= Mathf.Clamp01((wolrdPosition.x + gridWorldSize.x/2)/ gridWorldSize.x);
        float percentY= Mathf.Clamp01((wolrdPosition.z + gridWorldSize.y/2)/ gridWorldSize.y);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];

    
    }

    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) { 
                    neighbours.Add(grid[checkX,checkY]);
                }

            
            }//End of Y Loop 
        
        } //End of X Loop
        return neighbours;
    }//End of Function

    void Awake()
    {
        nodeDiameter = 2 * nodeRadius;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius, unwalkableMask));
                grid[x,y]= new Node(walkable,worldPoint,x,y);
            }
        
        }
    }

    void OnDrawGizmos()
    {
        if (grid != null && displayGridGizmos)
        {

            Node PlayerNode = NodeFromWorldPoint(Player.position);
            foreach (Node n in grid){
                Gizmos.color= (n.walkable)?Color.white:Color.red;
                //if (PlayerNode == n) { Gizmos.color = Color.cyan; } //Color Node on which player lies. //Testing: NodeFromWorldPoint

                Gizmos.DrawCube(n.worldPosition, Vector3.one * nodeDiameter);
            }
        
        } 
    }

    public int MaxSize
    {
        get {
            return gridSizeX * gridSizeY;
        }

    }

}

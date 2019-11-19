using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest; //Holds the current path request 
    PathFinding pathFinding; //instance of the pathfinding class
    bool isProcessingPath; //to show if a process is currently being processed

    static PathRequestManager instance;

    void Awake() {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }


    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback){

        PathRequest newPathRequested = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newPathRequested);
        instance.TryProcessNext();
    }

    void TryProcessNext() {

        if (!isProcessingPath && pathRequestQueue.Count > 0) {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }


    public void FinishedProcessingPath(Vector3[] path, bool success) {
        /* This process is called by PathFinding after it's done and returned tha path*/
        currentPathRequest.callback(path, success); // Draw and Move with the given Path !
        isProcessingPath = false; //We are done. No resources are being held.
        TryProcessNext(); //Let's try the next one in queue if it has an element (check is done inisde TryProcessNext();
    }


    struct PathRequest{
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
            pathStart= _start;
            pathEnd = _end;
            callback = _callback;
            
        }
    
    }
}

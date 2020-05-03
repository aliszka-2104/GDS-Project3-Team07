using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();     //Zapytania o ścieżkę są kolejkowane
    PathRequest currentRequest;

    static PathManager instance;        //Statyczna metoda potrzebuje dostępu do danych
    Pathfinding pathfinding;
    bool isProcessing;
    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }
    public static void RequestPath(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)    //Kolejkowanie zapytań
    {
        PathRequest newRequest = new PathRequest(start, end, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }
    void TryProcessNext()
    {
        if(!isProcessing && pathRequestQueue.Count > 0)     //Sprawdza czy na pewno nie wykonuje zbyt wielu zapytań na raz
        {
            currentRequest = pathRequestQueue.Dequeue();
            isProcessing = true;
            pathfinding.StartPathing(currentRequest.start, currentRequest.end);
        }
    }
    public void FinishedProcessingPath(Vector3[] path, bool success) 
    {
        currentRequest.callback(path, success);
        isProcessing = false;
        TryProcessNext();
    }
    struct PathRequest                  //Struktura zapytania
    {
        public Vector3 start;
        public Vector3 end;
        public Action<Vector3[], bool> callback;
        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            start = _start;
            end = _end;
            callback = _callback;
        }
    }
}

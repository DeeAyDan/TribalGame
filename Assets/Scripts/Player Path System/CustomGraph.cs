using System;
using UnityEngine;
using System.Collections.Generic;

public class CustomGraph<T>
{
    private Dictionary<T, List<T>> adjList;

    public CustomGraph()
    {
        adjList = new();
    }

    public void AddNode(T data)
    {
        if (!adjList.ContainsKey(data))
        {
            adjList.Add(data, new List<T>());
        }
        else
        {
            throw new Exception($"Node {data} already exists!");
        }
    }

    public void AddEdge(T src, T dst)
    {
        if (!adjList.ContainsKey(src) || !adjList.ContainsKey(dst))
        {
            throw new Exception($"Node does not exist! {src} , {dst}");
        }
        else
        {
            adjList[src].Add(dst);
        }
    }

    public int GetNodeCount(T currentNode)
    {
        if (!adjList.ContainsKey(currentNode))
            throw new Exception($"Node {currentNode} does not exist!");

        return adjList[currentNode].Count;
    }

    public T GetNode(T currentNode, int index)
    {
        if (!adjList.ContainsKey(currentNode))
            throw new Exception($"Node {currentNode} does not exist!");

        return adjList[currentNode][index];
    }

    public IEnumerable<T> GetNeighbors(T node)
    {
        if (!adjList.ContainsKey(node))
            throw new Exception($"Node {node} does not exist!");

        return adjList[node];
    }

    public IEnumerable<T> GetAllNodes()
    {
        return adjList.Keys;
    }

}

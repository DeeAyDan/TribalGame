using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    // Singleton Variable
    public static SceneManagerScript Instance;

    //[Tooltip("Drag and drop scenes from project tab.")]
    //[SerializeField] SceneAsset[] sceneAssets;
    [SerializeField] int totalScene;
    int sceneNumber = 0;

    [Header("Prefab References")]
    [SerializeField] LineScript LinePrefab;
    [SerializeField] GameObject NodePrefab;

    [Header("Parents")]
    [SerializeField] Transform Col1Parent;
    [SerializeField] Transform Col2Parent;
    [SerializeField] Transform Col3Parent;
    [SerializeField] Transform LineParent;

    GameObject firstNode;
    GameObject[] Col1Array = new GameObject[4];
    GameObject[] Col2Array = new GameObject[4];
    GameObject[] Col3Array = new GameObject[4];

    CustomGraph<GameObject> graph = new();
    Dictionary<int, GameObject> nodeSceneMap = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MapSaveData data = SaveSystem.LoadMap();
        if (data != null)
        {
            Load(data);
        }
        else
        {
            AddNodes();
            CreateRandomConnections(null, Col1Array, Col2Array);
            CreateRandomConnections(Col1Array, Col2Array, Col3Array);
            CreateRandomConnections(Col2Array, Col3Array, null);
            ActivateConnectedButtons();
        }

        SaveGame();
    }



    void AddNodes()
    {
        List<int> sceneNumbers = Enumerable.Range(0, totalScene).ToList();

        int sceneNumber = sceneNumbers[0];
        sceneNumbers.Remove(sceneNumber);
        GameObject obj = InstantiateNodes(Col2Parent, sceneNumber, Vector3.down * 425);
        nodeSceneMap[sceneNumber] = obj;


        SpawnNodes(Col1Parent, Col1Array, sceneNumbers);
        SpawnNodes(Col2Parent, Col2Array, sceneNumbers);
        SpawnNodes(Col3Parent, Col3Array, sceneNumbers);

        graph.AddNode(obj);
        ConnectNodes(obj, Col1Array[0]);
        ConnectNodes(obj, Col2Array[0]);
        ConnectNodes(obj, Col3Array[0]);

    }

    void SpawnNodes(Transform parent, GameObject[] objArray, List<int> sceneList)
    {
        int offsetY = 0;
        int offsetX = 0;

        for (int i = 0; i < objArray.Length; i++)
        {
            sceneNumber = sceneList[Random.Range(0, sceneList.Count)];
            sceneList.Remove(sceneNumber);

            GameObject obj = InstantiateNodes(parent, sceneNumber, new Vector3(offsetX, offsetY, 0));

            objArray[i] = obj;
            offsetY += Random.Range(400, 450);
            offsetX = Random.Range(-5, 5) * 10;

            graph.AddNode(obj);
            nodeSceneMap[sceneNumber] = obj;
        }
    }

    GameObject InstantiateNodes(Transform parent, int sceneNumber, Vector3 offset)
    {
        GameObject obj = Instantiate(NodePrefab, parent);
        obj.GetComponent<RectTransform>().localPosition = offset;
        Button button = obj.GetComponent<Button>();
        button.onClick.AddListener(() => LoadScene(sceneNumber));
        button.interactable = false;
        return obj;
    }

    void SpawnLine(GameObject from, GameObject to)
    {
        LineScript line = Instantiate(LinePrefab, LineParent);

        Vector3 fromPos = LineParent.InverseTransformPoint(from.transform.position);
        Vector3 toPos = LineParent.InverseTransformPoint(to.transform.position);
        Vector3 dir = toPos - fromPos;

        line.rect.anchoredPosition = (fromPos + toPos) / 2f;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        line.angle = angle;
        line.rect.rotation = Quaternion.Euler(0,0,angle + 90);

        line.rect.sizeDelta = new Vector2(line.rect.sizeDelta.x, dir.magnitude);
    }

    void CreateRandomConnections(GameObject[] prevCol, GameObject[] currentCol, GameObject[] nextCol)
    {
        for (int i = 0; i < currentCol.Length - 1; i++)
        {
            ConnectNodes(currentCol[i], currentCol[i + 1]);

            if (nextCol != null)
            {
                float randVal = Random.value * 100;

                if (randVal <= 30) // Percentage of having node to right
                {
                    ConnectNodes(currentCol[i], nextCol[i + 1]);
                }
            }
            if (prevCol != null)
            {
                float randVal = Random.value * 100;

                if (randVal <= 30) // Percentage of having node to left
                {
                    ConnectNodes(currentCol[i], prevCol[i + 1]);
                }
            }

        }
    }

    void ConnectNodes(GameObject from, GameObject to)
    {
        graph.AddEdge(from, to);
        SpawnLine(from, to);
    }

    void ActivateConnectedButtons()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        // Find which node corresponds to this scene
        GameObject currentNode = null;
        if (nodeSceneMap.ContainsKey(currentScene))
        {
            currentNode = nodeSceneMap[currentScene];
            currentNode.GetComponent<Image>().color = Color.blue; // Color of the current Node
        }

        if (currentNode == null)
        {
            Debug.LogWarning("No node found for current scene!");
            return;
        }

        // Disable all buttons first
        foreach (var node in nodeSceneMap.Values)
        {
            node.GetComponent<Button>().interactable = false;
        }

        // Activate only neighbors
        foreach (var neighbor in graph.GetNeighbors(currentNode))
        {
            neighbor.GetComponent<Button>().interactable = true;
        }
    }


    void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    #region Save and Load

    public void SaveGame()
    {
        MapSaveData data = Save();         // Step 1: convert map -> plain data
        SaveSystem.SaveMap(data);          // Step 2: write to disk
    }

    public void DeleteSavedData()
    {
        SaveSystem.DeleteSave();
    }

    public void LoadGame()
    {
        MapSaveData data = SaveSystem.LoadMap(); // Step 1: read from disk
        if (data != null)
        {
            Load(data);                    // Step 2: rebuild map from data
        }
    }

    public MapSaveData Save()
    {
        MapSaveData saveData = new();

        // Map each GameObject node to an index
        Dictionary<GameObject, int> nodeIndexMap = new();
        int index = 0;

        foreach (var kvp in nodeSceneMap)
        {
            GameObject node = kvp.Value;
            RectTransform rect = node.GetComponent<RectTransform>();

            int parentColumn = 0;
            if (node.transform.parent == Col1Parent) parentColumn = 1;
            else if (node.transform.parent == Col2Parent) parentColumn = 2;
            else if (node.transform.parent == Col3Parent) parentColumn = 3;

            NodeData nodeData = new()
            {
                sceneNumber = kvp.Key,
                offset = rect.localPosition,   // <-- Save as local offset
                parentColumn = parentColumn
            };

            saveData.nodes.Add(nodeData);
            nodeIndexMap[node] = index++;
        }

        // Save edges
        foreach (var node in graph.GetAllNodes())
        {
            int fromIndex = nodeIndexMap[node];

            foreach (var neighbor in graph.GetNeighbors(node))
            {
                int toIndex = nodeIndexMap[neighbor];
                saveData.connections.Add(new ConnectionData()
                {
                    fromNodeIndex = fromIndex,
                    toNodeIndex = toIndex
                });
            }
        }

        return saveData;
    }

    public void Load(MapSaveData saveData)
    {
        // Clear old graph and UI
        foreach (Transform child in Col1Parent) Destroy(child.gameObject);
        foreach (Transform child in Col2Parent) Destroy(child.gameObject);
        foreach (Transform child in Col3Parent) Destroy(child.gameObject);
        foreach (Transform child in LineParent) Destroy(child.gameObject);

        graph = new CustomGraph<GameObject>();
        nodeSceneMap = new Dictionary<int, GameObject>();

        List<GameObject> nodeObjects = new();

        // Recreate nodes
        foreach (var nodeData in saveData.nodes)
        {
            Transform parent = Col2Parent; // default
            if (nodeData.parentColumn == 1) parent = Col1Parent;
            else if (nodeData.parentColumn == 2) parent = Col2Parent;
            else if (nodeData.parentColumn == 3) parent = Col3Parent;

            // Reuse InstantiateNodes
            GameObject obj = InstantiateNodes(parent, nodeData.sceneNumber, nodeData.offset);

            graph.AddNode(obj);
            nodeSceneMap[nodeData.sceneNumber] = obj;
            nodeObjects.Add(obj);
        }

        // Recreate connections
        foreach (var conn in saveData.connections)
        {
            GameObject fromNode = nodeObjects[conn.fromNodeIndex];
            GameObject toNode = nodeObjects[conn.toNodeIndex];
            ConnectNodes(fromNode, toNode);
        }

        ActivateConnectedButtons();
    }



    #endregion
}


[System.Serializable]
public class NodeData
{
    public int sceneNumber;          // The scene this node loads
    public Vector3 offset;         // Position of the node (localPosition)
    public int parentColumn;         // 1 = Col1, 2 = Col2, 3 = Col3
}

[System.Serializable]
public class ConnectionData
{
    public int fromNodeIndex;        // Index of "from" node
    public int toNodeIndex;          // Index of "to" node
}

[System.Serializable]
public class MapSaveData
{
    public List<NodeData> nodes = new();
    public List<ConnectionData> connections = new();
}
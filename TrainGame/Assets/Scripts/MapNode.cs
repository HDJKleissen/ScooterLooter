using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    public List<MapNode> parents;
    public List<MapNode> children;
    [HideInInspector]
    public bool visited = false;
    [HideInInspector]
    public bool looted = false;
    public int index = -1;

    [SerializeField]
    Material lineMaterial;
    [SerializeField]
    string SceneName;

    static int nextNodeIndex = 0;
    Button button;
    private void Awake()
    {
        button = GetComponentInChildren<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(index == -1)
        {
            index = nextNodeIndex;
            nextNodeIndex++;
        }
        if (visited)
            DrawRoads();

        for (int i = 0; i < children.Count; i++)
            children[i].parents.Add(this);
    }

    private void OnMouseOver()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (parents.Contains(MapGameController.Instance.Target))
                MapGameController.Instance.Target = this;
        }
    }

    private void Update()
    {
        button.gameObject.SetActive(MapGameController.Instance.Target == this && !MapGameController.Instance.Travelling && !looted);
    }

    public void DrawRoads()
    {
        for(int i = 0; i < children.Count; i++)
        {
            GameObject instance = new GameObject($"Road {i}");
            instance.transform.SetParent(transform);
            LineRenderer line = instance.AddComponent<LineRenderer>();
            line.positionCount = 2;
            line.SetPositions(new Vector3[2] { transform.position, children[i].transform.position });
            line.material = lineMaterial;
            line.startColor = Color.white;
            line.endColor = Color.white;
            line.startWidth = .5f;
            line.endWidth = .5f;
        }
    }

    public void SwitchScene()
    {
        looted = true;
        MapGameController.Instance.Save();
        if (SceneName != "")
            SceneManager.LoadScene(SceneName);
        else
            SceneManager.LoadScene(MapGameController.Instance.GetRandomScavengeScene());
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < children.Count; i++)
        {
            if(children[i] != null)
                Gizmos.DrawLine(transform.position, children[i].transform.position);
        }
    }
}

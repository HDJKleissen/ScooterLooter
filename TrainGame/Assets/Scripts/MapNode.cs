using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    public List<MapNode> parents;
    public List<MapNode> children;
    public bool travelled = false;

    [SerializeField]
    Material lineMaterial;
    [SerializeField]
    string SceneName;

    Button button;
    private void Awake()
    {
        button = GetComponentInChildren<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (travelled)
            DrawRoads();

        for (int i = 0; i < children.Count; i++)
            children[i].parents.Add(this);
    }

    private void OnMouseOver()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Boop");
            if (parents.Contains(MapGameController.Instance.target))
                MapGameController.Instance.target = this;
        }
    }

    private void Update()
    {
        button.gameObject.SetActive(MapGameController.Instance.target == this && !MapGameController.Instance.travelling);
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
        SceneManager.LoadScene(SceneName);
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

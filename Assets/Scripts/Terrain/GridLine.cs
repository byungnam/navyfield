using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLine : MonoBehaviour {

    public GameObject main_camera;
    

    // private int map_size = 40000;
    // private int grid_interval = 2000;
    private float height = 1;
    private int grid_interval = 1000;

    void Start () {
        int map_size = (int) transform.localScale.x;
        
        for (int x = 0; x< map_size; x += grid_interval)
        {
            DrawLine(new Vector3(x, height, -map_size), new Vector3(x, height, map_size));
        }
        for (int x = -grid_interval; x > -map_size; x -= grid_interval)
        {
            DrawLine(new Vector3(x, height, -map_size), new Vector3(x, height, map_size));
        }
        for (int z = 0; z < map_size; z += grid_interval)
        {
            DrawLine(new Vector3(-map_size, height, z), new Vector3(map_size, height, z));
        }
        for (int z = -grid_interval; z > -map_size; z -= grid_interval)
        {
            DrawLine(new Vector3(-map_size, height, z), new Vector3(map_size, height, z));
        }
       
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        //Text text = hud.GetComponentInChildren<Text>();

        GameObject line_go = new GameObject("line: " + start + " to " + end);
        LineRenderer line = line_go.AddComponent<LineRenderer>();
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.startColor = Color.white - new Color(0, 0, 0, 0.7f);
        line.endColor = Color.white - new Color(0, 0, 0, 0.7f); ;
        line_go.layer = LayerMask.NameToLayer("GridLine");


        Material whiteDiffuseMat = new Material(Shader.Find("Transparent/Diffuse"));
        line.material = whiteDiffuseMat;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        line_go.transform.parent = transform;
        
        
    }
	
}

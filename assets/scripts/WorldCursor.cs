using UnityEngine;
using Singleton;

public class WorldCursor : Singleton<WorldCursor> {
    
    private MeshRenderer Renderer;

    // Use this for initialization
    void Start() {
        Renderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update() {
        var HeadPosition = Camera.main.transform.position;
        var GazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(HeadPosition, GazeDirection, out hitInfo)) {
            Renderer.enabled = true;
            this.transform.position = hitInfo.point;
            this.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
        else {
            Renderer.enabled = false;
        }
    }
}

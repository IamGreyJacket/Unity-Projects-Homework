using UnityEngine;

public class LookAtScript : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        transform.LookAt(_camera.transform);
    }
}

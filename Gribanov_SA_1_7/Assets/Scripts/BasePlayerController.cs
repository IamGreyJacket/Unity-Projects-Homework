using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerController : MonoBehaviour
{
    private Rigidbody _body;
    [SerializeField]
    private float _forwardSpeed;
    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        StartCoroutine(MoveForward());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MoveForward()
    {
        yield return null;
    }
}

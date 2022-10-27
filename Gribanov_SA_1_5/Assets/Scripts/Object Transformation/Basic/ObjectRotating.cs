using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectRotating : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public bool IsRotating;

    void Update()
    {
        if (IsRotating)
        {
            transform.Rotate(new Vector3(-5, 0, 0) * Time.deltaTime);
        }
    }
}

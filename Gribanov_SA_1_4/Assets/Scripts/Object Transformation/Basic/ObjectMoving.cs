using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoving : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public bool IsMoving = true;
    public float Speed = 10;
    void Update()
    {
        if (IsMoving)
        {
            transform.position += transform.forward * Time.deltaTime * Speed;
        }
    }
}

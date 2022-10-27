using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScaling : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public bool IsScaling;

    void Update()
    {
        if (IsScaling)
        {
            transform.localScale += new Vector3(-1.5f, -1.5f, -1.5f) * Time.deltaTime;
        }
    }
}

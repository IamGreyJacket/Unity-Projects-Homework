using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Rotate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            gameObject.transform.rotation *= Quaternion.Euler(new Vector3(0, 15, 0) * Time.deltaTime);
            yield return null;
        }
    }
}

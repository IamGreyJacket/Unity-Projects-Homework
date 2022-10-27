using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gribanov_SA_2_5
{
    public class PlayerController : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameManager.Self.Shoot(transform);
            }
        }
    }
}

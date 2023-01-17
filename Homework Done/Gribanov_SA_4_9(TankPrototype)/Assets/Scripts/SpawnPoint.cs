using UnityEngine;

namespace Tanks
{
    public class SpawnPoint : MonoBehaviour
    {

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position, new Vector3(2f, 2f, 0));
        }
    }
}
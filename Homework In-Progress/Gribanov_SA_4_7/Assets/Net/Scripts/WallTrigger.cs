using UnityEngine;

namespace Net
{
    public class WallTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player == null) return;
            Debugger.Log($"{player.gameObject.name} is out of bounds");
            player.Health -= 50f; //todo
            Managers.GameManager.Self.GameOver(player);
        }
    }
}
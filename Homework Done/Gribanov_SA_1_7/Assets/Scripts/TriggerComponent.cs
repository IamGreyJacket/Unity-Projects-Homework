using UnityEngine;

public class TriggerComponent : MonoBehaviour
{
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private bool IsDamage;
    [SerializeField]
    private int Damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        _collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsDamage)
        {
            GameManager.Self.TakeDamage(Damage);
            //Damage
        }
        else
        {
            GameManager.Self.UpdateLevel();
            //Level
        }
    }
}

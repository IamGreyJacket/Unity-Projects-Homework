using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int _progress;
    public static GameManager Self;

    [SerializeField, Min(1f), Tooltip("Это Здоровье и тестовый Tooltip.")]
    private int _health = 3;
    [SerializeField]
    private GameObject _player;
    [SerializeField, Space(30f)]
    private Transform[] Levels;
    [SerializeField, Space(20f)]
    private Transform LevelTrigger;
    private int levelCount = 0;
    [SerializeField]
    private Text _textProgress;
    [SerializeField]
    private Text _textHealth;
    private void Awake()
    {
        Self = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_player.transform.position.y <= -3)
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        var _body = _player.transform.GetComponent<Rigidbody>();
        _body.velocity = _body.velocity / 2.5f;
        _textHealth.text = $"Жизней осталось: {_health.ToString()}";

        if (_health <= 0)
        {
            UnityEditor.EditorApplication.isPaused = true;
            Debug.LogWarning("GAME OVER");
        }
    }

    public void UpdateLevel()
    {
        Levels[levelCount].position += new Vector3(0, 0, 240);
        LevelTrigger.position += new Vector3(0, 0, 60);
        levelCount++;
        _progress++;
        _textProgress.text = $"Пройдено уровней: {_progress.ToString()}";
        if(levelCount >= Levels.Length)
        {
            levelCount = 0;
        }
    }
}

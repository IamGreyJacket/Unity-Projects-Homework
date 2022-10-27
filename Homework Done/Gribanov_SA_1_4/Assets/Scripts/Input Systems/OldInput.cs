using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldInput : MonoBehaviour
{
    private List<GameObject> bullets;
    public float Speed;
    private float time = 0;
    private float bulletTime = 3;
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        bullets = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletTime += Time.deltaTime;
        StartCoroutine(Move());
        StartCoroutine(Logging());
        StartCoroutine(Shoot());
    }

    private IEnumerator Move()
    {
        var x = Input.GetAxis("Horizontal Movement");
        var z = Input.GetAxis("Vertical Movement");
        var r = Input.GetAxis("Horizontal Rotation");
        transform.position += ((transform.forward * z) + (transform.right * x)) * Time.deltaTime * Speed;
        transform.eulerAngles += new Vector3(0f, r, 0);
        yield return null;
    }

    private IEnumerator Logging()
    {
        if (Input.GetAxis("Action Log") != 0)
        {
            Debug.Log("Нажата либо клавиша F, либо Left Ctrl.");
        }
        yield return null;
    }

    private IEnumerator Shoot()
    {
        if (Input.GetAxis("Fire1") != 0 & bulletTime > 2f)
        {
            GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = transform.position;
            bullet.transform.eulerAngles = transform.eulerAngles;
            bullet.AddComponent<ObjectMoving>();
            bullets.Add(bullet);
            bulletTime = 0f;
            count++;
        }
        yield return null;
    }

    private IEnumerator Jump()
    {
        time += Time.deltaTime;
        if (time <= 1)
        {
            var y = Input.GetAxis("Jump");
            transform.position += new Vector3(0f, y, 0f) * Time.deltaTime;
        }
        else if (time > 1 || Input.GetKeyUp(KeyCode.Space))
        {
            transform.position += new Vector3(0f, 1, 0f) * Time.deltaTime;
        }
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareAndTriangle : MonoBehaviour
{
    private bool Marked = false;
    private GameObject[] Cubes = new GameObject[4];
    private int count = 0;
    public bool ChangeRoute; //Меняет маршрут на второй при нажатии.
    public float Speed; //Скорость указана в единицах в секунду.
    public float AwaitingTime; //Время ожидания указано в секундах
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cr());
    }

    private IEnumerator MoveFromTo(Vector3 startPosition, Vector3 endPosition, float Speed)
    {
        if (Speed < 0)
        {
            Speed = -Speed;
        }
        float time = (1 / Speed) * 10;
        var currentTime = 0f;
        while (currentTime < time)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, 1 - (time - currentTime) / time);
            currentTime += Time.deltaTime;

            yield return null;
        }
        transform.position = endPosition;

    }

    private IEnumerator CreateMark()
    {
        if (!Marked)
        {
            Cubes[count] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Cubes[count].transform.position = transform.position;
            Cubes[count].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            count++;
            yield return null;
        }
    }

    private IEnumerator Cr()
    {
        bool temp = ChangeRoute;
        while (true)
        {
            if (temp != ChangeRoute)
            {
                temp = ChangeRoute;
                Marked = false;
                for (int i = 0; i < count; i++)
                {
                    Destroy(Cubes[i]);
                }
                count = 0;
            }
            if (AwaitingTime < 0)
            {
                AwaitingTime = -AwaitingTime;
            }
            if (ChangeRoute)
            {
                yield return MoveFromTo(transform.position, transform.position + new Vector3(-5f, 0f, 10f), Speed);
                yield return CreateMark();
                yield return new WaitForSeconds(AwaitingTime);
                yield return MoveFromTo(transform.position, transform.position + new Vector3(-5f, 0f, -10f), Speed);
                yield return CreateMark();
                yield return new WaitForSeconds(AwaitingTime);
                yield return MoveFromTo(transform.position, transform.position + new Vector3(10f, 0f, 0f), Speed);
                yield return CreateMark();
                Marked = true;
                yield return new WaitForSeconds(AwaitingTime);

            }
            else
            {
                yield return MoveFromTo(transform.position, transform.position + new Vector3(0f, 0f, 10f), Speed);
                yield return CreateMark();
                yield return new WaitForSeconds(AwaitingTime);
                yield return MoveFromTo(transform.position, transform.position + new Vector3(-10f, 0f, 0f), Speed);
                yield return CreateMark();
                yield return new WaitForSeconds(AwaitingTime);
                yield return MoveFromTo(transform.position, transform.position + new Vector3(0f, 0f, -10f), Speed);
                yield return CreateMark();
                yield return new WaitForSeconds(AwaitingTime);
                yield return MoveFromTo(transform.position, transform.position + new Vector3(10f, 0f, 0f), Speed);
                yield return CreateMark();
                Marked = true;
                yield return new WaitForSeconds(AwaitingTime);
            }
        }
    }


}


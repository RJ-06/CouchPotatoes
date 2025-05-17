using System.Collections;
using UnityEngine;

public class TwoPointMovement : MonoBehaviour
{
    [SerializeField] float moveTime, idleTime;
    [SerializeField] Vector2 firstPoint, secondPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Move(moveTime, idleTime));
    }

    private IEnumerator Move(float timeToMove, float timeToIdle)
    {
        while (true)
        {
            // Move platform to second point
            yield return new WaitForSeconds(timeToIdle);
            float moveStartTime = Time.time, t = 0;
            while (t < 1)
            {
                t = (Time.time - moveStartTime) / timeToMove;
                transform.position = new Vector2(Mathf.SmoothStep(firstPoint.x, secondPoint.x, t), Mathf.SmoothStep(firstPoint.y, secondPoint.y, t));
                yield return null;
            }

            // Move platform back to first point
            yield return new WaitForSeconds(timeToIdle);
            moveStartTime = Time.time;
            t = 0;
            while (t < 1)
            {
                t = (Time.time - moveStartTime) / timeToMove;
                transform.position = new Vector2(Mathf.SmoothStep(secondPoint.x, firstPoint.x, t), Mathf.SmoothStep(secondPoint.y, firstPoint.y, t));
                yield return null;
            }
        }
    }
}

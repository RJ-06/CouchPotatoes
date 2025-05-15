using UnityEngine;

public class ItemHover : MonoBehaviour
{
    private float amplitude = .3f;
    private float period = 2f;
    protected Vector2 initPosition = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= 0) return;

        float offset = Mathf.Sin(Time.time * (2 * Mathf.PI / period)) * amplitude;
        transform.position = new Vector2(initPosition.x, initPosition.y + offset);
    }
}

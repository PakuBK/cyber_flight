using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CF.UI {
public class ScrollingBackground : MonoBehaviour
{
    public Transform LinkedElement;

    public Camera cam;

    public float MoveSpeed;

    private float bottomScreen;

    private void Start()
    {
        bottomScreen = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
    }

    private void Update()
    {
        float height = GetHeight(transform);

        if (transform.position.y + height / 2 < bottomScreen)
        {
            transform.position = new Vector2(transform.position.x, LinkedElement.position.y + height - 2);
        }

        transform.Translate(Vector2.down * Time.deltaTime * MoveSpeed, Space.World);

        
    }

    private float GetHeight(Transform obj)
    {
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        float height = sprite.size.y;
        return height;
    }
}
}
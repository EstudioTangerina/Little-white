using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float speed = 10.0f;
    public Vector3 direction = Vector3.right;

    private float xMin, xMax, yMin, yMax;

    public GameObject Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 upperRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        Renderer renderer = GetComponent<Renderer>();
        float halfWidth = renderer.bounds.size.x / 2.0f;
        float halfHeight = renderer.bounds.size.y / 2.0f;

        xMin = lowerLeftCorner.x - halfWidth;
        xMax = upperRightCorner.x + halfWidth;
        yMin = lowerLeftCorner.y - halfHeight;
        yMax = upperRightCorner.y + halfHeight;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        Renderer renderer = GetComponent<Renderer>();
        float halfWidth = renderer.bounds.size.x / 2.0f;
        float halfHeight = renderer.bounds.size.y / 2.0f;

        if (transform.position.x + halfWidth < xMin || transform.position.x - halfWidth > xMax ||
            transform.position.y + halfHeight < yMin || transform.position.y - halfHeight > yMax)
        {
            Destroy(gameObject);
        }

        if(direction.x == -1 && Player.GetComponent<PlayerMovement>().directionWhenStopped != Vector3.zero)
        {
            direction = Vector3.up;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        
        if(direction.x == 1 && Player.GetComponent<PlayerMovement>().directionWhenStopped != Vector3.zero)
        {
            direction = Vector3.up;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
    }
}

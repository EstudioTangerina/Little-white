using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<EnemyFollow>().ApplyForce(direction);
        }
    }
}

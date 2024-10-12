using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;

        position.x -= moveSpeed * Time.fixedDeltaTime;

        transform.position = position;

        if(position.x < -12)
        {
            Destroy(gameObject);
        }
    }
}

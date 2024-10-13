using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBehavior : MonoBehaviour
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
        DamageSource DamageSource = collision.GetComponent<DamageSource>();
        if (DamageSource != null)
        {
            Destroy(gameObject);
            Destroy(DamageSource.gameObject);
        }
    }
}

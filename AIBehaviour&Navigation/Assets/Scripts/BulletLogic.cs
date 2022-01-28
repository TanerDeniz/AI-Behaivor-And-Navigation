using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    Rigidbody rigidbody;
    float bulletSpeed = 8.0f;
    EnemyLogic enemyLogic;
    float bulletLifeTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.velocity = transform.up * bulletSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bulletLifeTime -= Time.deltaTime;
        if (bulletLifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
      
       if (other.tag == "Target")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);

        }else if (other.tag == "Enemy")
        {
            enemyLogic = other.GetComponent<EnemyLogic>();
            if (enemyLogic)
                enemyLogic.TakeDamage(35);
            Destroy(gameObject);
            
        }
    }
}

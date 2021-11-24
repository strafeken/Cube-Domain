using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody m_rigidbody;
    public Transform player;

    bool done;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //if(!done)
        {
            m_rigidbody.AddForce((player.position - transform.position).normalized * 50 * Time.deltaTime);
            //done = true;
        }
    }
}

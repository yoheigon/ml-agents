using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
    public Rigidbody sphere;
    
    void Create()
    {
        Vector3 pos = new Vector3(Random.Range(-5f,5f), 10, 18);
        Rigidbody SphereInstance = Instantiate(sphere, pos, Quaternion.identity) as Rigidbody;
    }
}

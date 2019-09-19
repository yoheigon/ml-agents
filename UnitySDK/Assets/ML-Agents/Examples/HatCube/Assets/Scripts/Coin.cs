using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    public HatCubeAgent hc;
    void Start()
    {
        Destroy(gameObject, 5);        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0,0,30) * Time.deltaTime);
        if(!hc.gameObject.activeSelf)
            Destroy(gameObject);
            
    }
       
}

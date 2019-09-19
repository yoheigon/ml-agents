using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableSphere : MonoBehaviour
{

//    public Coin token;
    public Rigidbody token;
    private Rigidbody rb;
    private HatCubeAgent hc;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hc = gameObject.GetComponent<SphereMove>().hc;
    }
        
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Laser"))
        {
            Vector3 pos = gameObject.transform.position;
            Destroy(gameObject);
            hc.contact = false;
            hc.SetReward(.5f);

            //Coin tok = Instantiate(token, pos, Quaternion.identity) as Coin;
            Rigidbody tok = Instantiate(token, pos, Quaternion.identity) as Rigidbody;
            
            tok.gameObject.GetComponent<Coin>().hc = hc;
        }   
    }

}

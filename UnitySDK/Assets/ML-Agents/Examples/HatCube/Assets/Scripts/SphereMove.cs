using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMove : MonoBehaviour
{
    public float m_Speed;// = -6f;
    //public GameObject Player;// = -6f;
    private Rigidbody m_Rigidbody;         
    public HatCubeAgent hc;
    // Start is called before the first frame update

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        //hc = Player.GetComponent<HatCube>();
        Destroy(gameObject, 8);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    //    Vector3 movement = transform.forward * m_Speed * Time.deltaTime;
    //    m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        

        //if (!Player.activeSelf){
        //    gameObject.SetActive(false);
        //}

        //if (m_Rigidbody.transform.localPosition.y < -10 || !hc.gameObject.activeSelf){
        //    //gameObject.SetActive(false);
        //    Destroy(gameObject);
        //}
    //    else{
        Vector3 v = m_Rigidbody.velocity;
        v.x = 0;
        v.z = m_Speed;
        m_Rigidbody.velocity = v;
    //    }
    }
    void OnCollisionEnter(Collision collision){
        GameObject agent = collision.gameObject;
        if (agent.CompareTag("Player")){
            Rigidbody rb = agent.GetComponent<Rigidbody>();            
            Vector3 v = rb.velocity;
            v.z = m_Speed;
            rb.velocity = v;
        }
    }

    void OnCollisionStay(Collision collision){
        //float collisionSpeed = collision.gameObject.m_Speed;
        GameObject agent = collision.gameObject;
        if (agent.CompareTag("Player")){
            Rigidbody rb = agent.GetComponent<Rigidbody>();            
            Vector3 v = rb.velocity;
            v.z = m_Speed;
            rb.velocity = v;
            //Vector3 push = transform.forward * m_Speed * Time.deltaTime;
            //rb.MovePosition(rb.position + push);
        }
    }

    void OnCollisionExit(Collision collision){
        //float collisionSpeed = collision.gameObject.m_Speed;
        GameObject agent = collision.gameObject;
        if (agent.CompareTag("Player")){
            Rigidbody rb = agent.GetComponent<Rigidbody>();            
            Vector3 v = rb.velocity;
            v.z = m_Speed;
            rb.velocity = v;
            //Vector3 push = transform.forward * m_Speed * Time.deltaTime;
            //rb.MovePosition(rb.position + push);
        }
    }

}

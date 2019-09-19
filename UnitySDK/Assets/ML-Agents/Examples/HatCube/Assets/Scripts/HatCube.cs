using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatCube : MonoBehaviour
{
    public float m_Speed = 10f;            
    public float m_TurnSpeed = 180f;       

    public GameObject myLaser;

    private Rigidbody rb;

    public bool contact;    
    private Vector3 m_LaserScale = new Vector3(0.3f, 0.01f, 4f);
    private Vector3 m_LaserLength = new Vector3(0.0f, 0.0f, 2.5f);
    private Vector3 ident = new Vector3(0f, 0f, 0f);

    private string m_ForwardAxisName;     
    private string m_StrafeAxisName;     
    private string m_TurnAxisName;         
    private string m_LaserAxisName;         

    private float m_CollisionTimer;    


    private float m_ForwardInputValue;    
    private float m_StrafeInputValue;    
    private float m_TurnInputValue;        
    private float m_LaserInputValue;        

    public int score;        


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

       // rb.isKinematic = true;
        score = 0;

        m_ForwardAxisName = "Vertical";
        m_StrafeAxisName = "Strafe";
        m_TurnAxisName = "Horizontal";
        m_LaserAxisName = "Fire1";
        contact = false;

    }

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_ForwardInputValue = Input.GetAxis(m_ForwardAxisName);
        m_StrafeInputValue = Input.GetAxis(m_StrafeAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
        m_LaserInputValue = Input.GetAxis(m_LaserAxisName);
    
    }

    void FixedUpdate()
    {
        Move();
        Turn();
        Shoot();
        m_CollisionTimer -= .02f;
        if (m_CollisionTimer <= 0)
            contact = false;
            
        if (rb.transform.position.y < -10)
            gameObject.SetActive(false);

    }

    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        if (contact)
            m_ForwardInputValue = Mathf.Min(m_ForwardInputValue, 0);
        
        Vector3 forward = transform.forward * m_ForwardInputValue * m_Speed * Time.deltaTime;
        Vector3 strafe = transform.right * m_StrafeInputValue * m_Speed * Time.deltaTime;
        if (m_LaserInputValue != 1)
            rb.MovePosition(rb.position + strafe + forward);
        //rb.AddForce(new Vector3(m_ForwardInputValue, 0.0f, m_StrafeInputValue) * m_Speed);
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        
        rb.MoveRotation(rb.rotation * turnRotation);
    }   

    private void Shoot()
    {
        if (m_LaserInputValue > 0.0f)
        {
            myLaser.transform.localScale = m_LaserScale;
            myLaser.transform.localPosition = m_LaserLength;
            //Vector3 position = transform.TransformDirection(RayPerception3D.PolarToCartesian(25f, 90f));
            //Debug.DrawRay(transform.position, position, Color.red, 0f, true);
        }
        else
        {
            myLaser.transform.localScale = ident;// new Vector3(0f, 0f, 0f);
            myLaser.transform.localPosition = ident;// new Vector3(0f, 0f, 0f);

        }    
    }

    public void Reset()
    {
        gameObject.transform.position = new Vector3(0f, .5f, -16f);
        gameObject.transform.rotation = Quaternion.identity;

        gameObject.SetActive(false);
        gameObject.SetActive(true);
        score = 0;
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("BreakableSphere") || collision.gameObject.CompareTag("UnbreakableSphere")){
            contact = true;
            m_CollisionTimer = 1f;
        }
    }

    void OnCollisionStay(Collision collision){
        if (collision.gameObject.CompareTag("BreakableSphere") || collision.gameObject.CompareTag("UnbreakableSphere")){
            m_CollisionTimer = 1f;
        }
    }
    void OnCollisionExit(Collision collision){
        if (collision.gameObject.CompareTag("BreakableSphere") || collision.gameObject.CompareTag("UnbreakableSphere")){
            contact = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin")){
            other.gameObject.SetActive(false);                
            score++;
        }

    }

}

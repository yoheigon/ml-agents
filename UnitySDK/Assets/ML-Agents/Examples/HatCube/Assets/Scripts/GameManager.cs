using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Rigidbody[] m_Spheres;           
    public GameObject Player;
    private HatCubeAgent hc;
    public float m_SphereDelay = .2f;
    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    public Text m_MessageText;              

    private WaitForSeconds m_SphereWait;     
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;     

    // Start is called before the first frame update
    void Start()
    {
        m_SphereWait = new WaitForSeconds(m_SphereDelay);
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        hc = Player.GetComponent<HatCubeAgent>();
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        StartCoroutine(GameLoop());
    }

    void Create()
    {
        int sphere = Random.Range(0, m_Spheres.Length);
        Vector3 pos = new Vector3(Random.Range(-10f,10f), 10, 23);
        Vector3 adjustedPos = pos + gameObject.transform.parent.transform.position;
        Rigidbody SphereInstance = Instantiate(m_Spheres[sphere], adjustedPos, Quaternion.identity, gameObject.transform) as Rigidbody;
        SphereInstance.gameObject.GetComponent<SphereMove>().hc = hc;
        
    }

    private IEnumerator RoundStarting()
    {
        hc.AgentReset();
        m_MessageText.text = "COINS: " + hc.score;
        yield return m_StartWait;
    }
    private IEnumerator RoundPlaying()
    {
        while(hc.gameObject.activeSelf){
            Create();
            m_MessageText.text = "COINS: " + hc.score;
            yield return m_SphereWait;
        } 
    }

    private IEnumerator RoundEnding()
    {
        foreach (Transform child in transform){
            Destroy(child.gameObject);
        }
        yield return m_EndWait;
    }
}

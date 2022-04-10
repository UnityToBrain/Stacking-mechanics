using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    [HideInInspector]public bool MoveByTouch,StartTheGame;
    private Vector3 _mouseStartPos, PlayerStartPos; 
    [SerializeField] private float RoadSpeed,SwipeSpeed,dis;
    [SerializeField] private GameObject Road;
    public List<GameObject> Balls = new List<GameObject>();
    public static StackManager StackManagerInstance;
    public GameObject ball,explosion;
    private Camera mainCam;
   
    void Start()
    {
        StackManagerInstance = this;
        Balls.Add(transform.gameObject);
        
        mainCam = Camera.main;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartTheGame = MoveByTouch = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            MoveByTouch = false;
        }

        if (MoveByTouch)
        {
            var plane = new Plane(Vector3.up, 0f);
            float distance;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (plane.Raycast(ray,out distance))
            {
                Vector3 mousePos = ray.GetPoint(distance);
                Vector3 desirePso = mousePos - _mouseStartPos;
                Vector3 move = PlayerStartPos + desirePso;
                
                move.x = Mathf.Clamp(move.x, -2.2f, 2.2f); 
                move.z = -7f;
                
                var Player = transform.position;
                Player = new Vector3(Mathf.Lerp(Player.x,move.x,Time.deltaTime * SwipeSpeed), Player.y, Player.z);
                transform.position = Player;
            } 
        }


        if (StartTheGame)
            Road.transform.Translate(Vector3.forward * (RoadSpeed * -1 * Time.deltaTime));
        
        
        if (Balls.Count > 1)
        {
            for (int i = 1; i < Balls.Count; i++)
            {
                var head = Balls.ElementAt(i - 1).transform;
                var body = Balls.ElementAt(i).transform;
                var desireDistance = Vector3.Distance(body.position , head.position);
                
                if (desireDistance <= dis)
                    body.position = new Vector3( Mathf.Lerp(body.position.x,head.position.x,SwipeSpeed * Time.deltaTime),body.position.y,
                        Mathf.Lerp(body.position.z,head.position.z + 0.5f,SwipeSpeed * Time.deltaTime));
                
            }
        }
        
    }

    private void LateUpdate()
    {
        if (StartTheGame)
        {
            mainCam.transform.position = new Vector3(
                Mathf.Lerp(mainCam.transform.position.x, transform.position.x, (SwipeSpeed - 5f) * Time.deltaTime),
                mainCam.transform.position.y, mainCam.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("blue"))
        {
            other.tag = "Player";
            other.GetComponent<Renderer>().material = GetComponent<Renderer>().material;
            other.transform.parent = null;
            other.gameObject.AddComponent<Rigidbody>();
            other.gameObject.AddComponent<Addball>();
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<Collider>().isTrigger = true;
            
            Balls.Add(other.gameObject);
            
        }

        if (other.CompareTag("add"))
        {
            var noAdd = Int16.Parse(other.transform.GetChild(0).name);

            for (int i = 0; i < noAdd; i++)
            {
               GameObject Newball = Instantiate(ball, Balls.ElementAt(Balls.Count - 1).transform.position + new Vector3(0f, 0f, 0.5f),
                    Quaternion.identity);
               
               Balls.Add(Newball);
            }
            
            other.GetComponent<Collider>().enabled = false;
        }
        
        if (other.CompareTag("subtraction"))
        {
            if (Balls.Count >= 0)
            {
                Balls.ElementAt(Balls.Count - 1).SetActive(false);
                Balls.RemoveAt(Balls.Count - 1);
                print("lose m");
            }
            
            if (Balls.Count == 0)
                StartTheGame = false;
            
            Instantiate(explosion, transform.position, Quaternion.identity);
            
        }
    }
}

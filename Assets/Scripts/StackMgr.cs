using System;
using System.Linq;
using UnityEngine;

public class StackMgr : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("blue"))
        {
             other.transform.parent = null;
             other.gameObject.AddComponent<Rigidbody>().isKinematic = true;
             other.gameObject.AddComponent<StackMgr>();
             other.gameObject.GetComponent<Collider>().isTrigger = true;
             other.tag = gameObject.tag;
             other.GetComponent<Renderer>().material = GetComponent<Renderer>().material;
             Gamemanager.GameManagerInstance.Balls.Add(other.transform);
        }
        
        if (other.CompareTag("add"))
        {
            var NoAdd = Int16.Parse(other.transform.GetChild(0).name);

            for (int i = 0; i < NoAdd; i++)
            {
                GameObject Ball =  Instantiate(Gamemanager.GameManagerInstance.Newball, Gamemanager.GameManagerInstance.Balls
                        .ElementAt(Gamemanager.GameManagerInstance.Balls.Count - 1).position + new Vector3(0f, 0f, 0.5f),
                    Quaternion.identity);
              
                Gamemanager.GameManagerInstance.Balls.Add(Ball.transform);
              
            }

            other.GetComponent<Collider>().enabled = false;
        }


        if (other.CompareTag("sub"))
        {
            var NoSub = Int16.Parse(other.transform.GetChild(0).name);

            if (Gamemanager.GameManagerInstance.Balls.Count > NoSub)
            {
                for (int i = 0; i < NoSub; i++)
                {
                   Gamemanager.GameManagerInstance.Balls.ElementAt(  Gamemanager.GameManagerInstance.Balls.Count - 1).gameObject.SetActive(false);
                   Gamemanager.GameManagerInstance.Balls.RemoveAt(Gamemanager.GameManagerInstance.Balls.Count - 1);
                }
                Instantiate(  Gamemanager.GameManagerInstance.Explosion,   Gamemanager.GameManagerInstance.
                    Balls.ElementAt(  Gamemanager.GameManagerInstance.Balls.Count - 1).position, Quaternion.identity);
            }

            if (Gamemanager.GameManagerInstance.Balls.Count == 0)
            {
                Gamemanager.GameManagerInstance.StartTheGame = false;
            }
            other.GetComponent<Collider>().enabled = false;
        }
    }
}

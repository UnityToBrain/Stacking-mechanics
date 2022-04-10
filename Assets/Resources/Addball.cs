using System;
using System.Linq;
using UnityEngine;

public class Addball : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("blue"))
        {
            other.tag = "Player";
            other.transform.parent = null;
            other.gameObject.AddComponent<Addball>();
            other.gameObject.AddComponent<Rigidbody>();
            other.GetComponent<Renderer>().material = GetComponent<Renderer>().material;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<Collider>().isTrigger = true;
            
            StackManager.StackManagerInstance.Balls.Add(other.gameObject);
        }
        
        if (other.CompareTag("add"))
        {
            var no = Int16.Parse(other.transform.GetChild(0).name);

            for (int i = 0; i < no; i++)
            {
                GameObject newball = Instantiate(StackManager.StackManagerInstance.ball, 
                    StackManager.StackManagerInstance.Balls.ElementAt(StackManager.StackManagerInstance.Balls.Count - 1).transform.position + new Vector3(0f, 0f, 0.5f),
                    Quaternion.identity);
               
                StackManager.StackManagerInstance.Balls.Add(newball);
                other.GetComponent<Collider>().enabled = false;
            }

        }
        
              
        if (other.CompareTag("subtraction"))
        {
            var no = Int16.Parse(other.transform.GetChild(0).name);
            
            if (StackManager.StackManagerInstance.Balls.Count > no)
            {
                for (int i = 0; i < no; i++)
                {
                    StackManager.StackManagerInstance.Balls.ElementAt( StackManager.StackManagerInstance.Balls.Count - 1).SetActive(false);
                    StackManager.StackManagerInstance.Balls.RemoveAt(StackManager.StackManagerInstance.Balls.Count - 1);
                    Instantiate(StackManager.StackManagerInstance.explosion, transform.position, Quaternion.identity);
                }
            }
     
            
            other.GetComponent<Collider>().enabled = false;
        }

    }
}

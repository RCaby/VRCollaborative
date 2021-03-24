using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other) {
        Rigidbody myRigidbody = other.GetComponent<Rigidbody>();
        if (myRigidbody != null) {
            myRigidbody.isKinematic = true;
        }
        Destroy(other.gameObject);
    }
}

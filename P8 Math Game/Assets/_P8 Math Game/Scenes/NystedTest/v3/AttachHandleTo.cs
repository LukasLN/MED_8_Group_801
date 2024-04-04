using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachHandleTo : MonoBehaviour
{
    [SerializeField] GameObject ObjectToAttachTo;
    [SerializeField] bool LockOnAllAxis;
    [SerializeField] bool KeepGrounded;
    // Start is called before the first frame update
   
   
    void Awake ()
    {
        if (ObjectToAttachTo == null)
            ObjectToAttachTo = GameObject.FindWithTag("GameManager");
    }   
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(KeepGrounded == false && LockOnAllAxis == false)
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, ObjectToAttachTo.transform.position.y, this.gameObject.transform.position.z);
        }
        else if(KeepGrounded == false && LockOnAllAxis == true)
        {
            this.gameObject.transform.position = new Vector3(ObjectToAttachTo.transform.position.x, ObjectToAttachTo.transform.position.y, ObjectToAttachTo.transform.position.z);
        }
        else if(KeepGrounded == true && LockOnAllAxis == false)
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z);
        }
         else if(KeepGrounded == true && LockOnAllAxis == true)
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        }

    }
}

using UnityEngine;
using System.Collections;

public class MoveFollow : MonoBehaviour {

    private Rigidbody thisRigidbody;
    public bool IsMoving = false;

    [SerializeField]
    private Transform headTransform;

    void Start ()
    {
        thisRigidbody = GetComponent<Rigidbody>();

    }
	
	
	 void FixedUpdate ()
    {
        
        

    }


    public IEnumerator FollowTheHead(Vector3 moveTo, float length)
    {
        
        IsMoving = true;
        
        while (Vector3.Distance(transform.position, moveTo) >= length + 0.3)
        {
            
            transform.position = Vector3.Lerp(transform.position, moveTo, 0.1f);
            transform.LookAt(moveTo);

            yield return null;
            
        }

        
        //thisRigidbody.velocity = Vector3.zero;
        thisRigidbody.velocity = headTransform.gameObject.GetComponent<Rigidbody>().velocity;

        IsMoving = false;
        
    }
}

using UnityEngine;
using System.Collections.Generic;

public class Head : MonoBehaviour {

    private Rigidbody thisRigidbody;

    [SerializeField]
    public int segmentsAmount = 20;

    [SerializeField]
    private float placementStep = 0.01f;

    [SerializeField]
    private GameObject SegmentPrefab;

    [SerializeField]
    private float speed = 10;

    float count;
     
    private List<MoveFollow> MoveCourutines;

	void Start ()
    {
        placementStep = transform.position.z - placementStep;
        count = placementStep;
        thisRigidbody = GetComponent<Rigidbody>();
        MoveCourutines = new List<MoveFollow>();


        // Filling the segments List
        for (int i = 0; i < segmentsAmount; i++)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z - count);
            MoveCourutines.Add((MoveFollow)Instantiate(SegmentPrefab.GetComponent<MoveFollow>(), pos, transform.rotation));
            count += placementStep;
        }


        //Coefficients for size gradient
        float coef = segmentsAmount * segmentsAmount;
        var k = segmentsAmount/coef;
        var k1 = k;
        float basicSize = 1;


        // Size gradient creating
        for (int i = 0; i < segmentsAmount; ++i )
        {
            if(i < segmentsAmount/4)
            {
                MoveCourutines[i].GetComponent<Transform>().localScale = new Vector3(basicSize / 1.5f + k, basicSize / 1.5f + k, basicSize / 1.5f + k);
                k += segmentsAmount / coef;
            }

            else
            {
                MoveCourutines[i].GetComponent<Transform>().localScale = new Vector3(basicSize - k1, basicSize - k1, basicSize - k1);
                k1 += segmentsAmount / coef;
            }
            
        }
    }


    void Update()
    {
        StartCoroutine(CoroutineGroup());

        if (Vector3.Angle(transform.forward, MoveCourutines[0].GetComponent<Transform>().forward) > 90)
            transform.forward = MoveCourutines[0].GetComponent<Transform>().forward;

        Quaternion headRotation = transform.rotation;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit))
            transform.up = hit.normal;

        float Yvel = thisRigidbody.velocity.y;
        
        thisRigidbody.velocity = Vector3.Normalize(Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal")) * Time.deltaTime * speed;
        thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x, Yvel, thisRigidbody.velocity.z);
        
        transform.forward = thisRigidbody.velocity;
        

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            transform.rotation = headRotation;


            /*if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                thisRigidbody.velocity = Vector3.zero;
            }



            var YAngle = Vector3.Angle(thisRigidbody.velocity, transform.forward);
            var XAngle = Vector3.Angle(thisRigidbody.velocity, Vector3.up);

            if (thisRigidbody.velocity != Vector3.zero)
            {


                if (thisRigidbody.velocity.x > 0)
                    transform.eulerAngles = new Vector3(0, YAngle, 0);
                //transform.rotation = Quaternion.Euler(0, YAngle, transform.rotation.z);
                else
                    transform.eulerAngles = new Vector3(0, -YAngle, 0);
                //transform.rotation = Quaternion.Euler(0, -YAngle, transform.rotation.z);

            }*/

         

        

        

    }


    IEnumerator<MoveFollow> CoroutineGroup()
    {
        for(int i = 0; i < MoveCourutines.Count; i++)
        {
            if (i != 0)
                MoveCourutines[i].StartCoroutine(MoveCourutines[i].FollowTheHead(MoveCourutines[i - 1].gameObject.GetComponent<Transform>().position, MoveCourutines[i - 1].gameObject.GetComponent<Transform>().localScale.z * 1.2f));
            else
            {

                
                MoveCourutines[i].StartCoroutine(MoveCourutines[i].FollowTheHead(transform.position, transform.localScale.z/1.25f));
            }

        }
        //while(true)
        yield return null;

    }
}

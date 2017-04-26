using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    [SerializeField]
    private Transform characterTransform;

	void LateUpdate ()
    {
        transform.position = new Vector3(characterTransform.position.x, transform.position.y, characterTransform.position.z -20);
	}
}

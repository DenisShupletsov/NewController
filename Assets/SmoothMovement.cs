using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMovement : MonoBehaviour
{
	[SerializeField] private Transform _targetTransform;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
	void FixedUpdate()
    {
	    transform.position = Vector3.Lerp(transform.position, _targetTransform.position, 1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contrl : MonoBehaviour
{
    private CharacterController _controller;

	//private Rigidbody _playerRigidbody;
    //private Animator _playerAnimator;
	private Transform _playerTransform;
	private const float _originalPlayerSpeed = 8f;
	private Transform _cameraTransform;
	private Transform _characterTransform;
	private float _halfCharacterHeight;
	
	private const float _jumpForce = 8f;
	private const float jumpHeight = 5f;
	private float startPositionY;
	private float stopPositionY;
	private bool isJump;
	private float smoothJumpEffect;
	
	[SerializeField] private LayerMask _graundLayer;
	
	
	//debug
	public Mesh mesh;
	
    void Start()
    {
	    //_playerRigidbody = this.GetComponent<Rigidbody>();
	    //_playerAnimator = this.GetComponent<Animator>();
	    
        _playerTransform = this.GetComponent<Transform>();
	    _controller = this.GetComponent<CharacterController>();
	    _characterTransform = this.GetComponent<Transform>();
	    _cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
	    
	    _halfCharacterHeight = _controller.height / 2;
	    
	    JumpEngage(false);
    }
    
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(_characterTransform.position - new Vector3(0, _halfCharacterHeight, 0), 0.4f);
		Gizmos.DrawWireSphere(_characterTransform.position + new Vector3(0, _halfCharacterHeight, 0), 0.4f);
	}

    private void FixedUpdate()
    {
	    MoveCherecter(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
	
    private void MoveCherecter(float horizontalInput, float verticalInput)
	{
		Vector3 _cameraPosition = new Vector3(_cameraTransform.position.x, _playerTransform.position.y, _cameraTransform.position.z);

		Vector3 diractionForward = (_playerTransform.position - _cameraPosition) * verticalInput;
		Vector3 diractionRight = _cameraTransform.right * horizontalInput;
		Vector3 diraction = diractionRight + diractionForward.normalized;
		
		float speed = _originalPlayerSpeed + (4 * Input.GetAxis("Run"));
	    
		//jump
		smoothJumpEffect = (stopPositionY - _characterTransform.position.y) * 1.7f;
		//max gravity force
		smoothJumpEffect = Mathf.Clamp(smoothJumpEffect, 0, 6f);
		
	    if(isJump)
	    {
	    	if(smoothJumpEffect <= 0.3f || ColliderCheck(_characterTransform.position + new Vector3(0, _halfCharacterHeight, 0)))
		    	isJump = false;
		    else
			    _controller.Move((Vector3.up * (_jumpForce * smoothJumpEffect)) * Time.deltaTime);
	    }
	    else
	    {
	    	if(ColliderCheck(_characterTransform.position - new Vector3(0, _halfCharacterHeight, 0)))
	    	{
	    		smoothJumpEffect = 1;
	    		
	    		if(Input.GetButton("Jump"))
	    			JumpEngage(true);
	    	}
	    }
		    		
		//gravity
		//Debug.Log(smoothJumpEffect);
		_controller.Move((Vector3.down * (4 * smoothJumpEffect)) * Time.deltaTime);
	    
		//movement
		_controller.Move(diraction.normalized * speed * Time.deltaTime);
	    
		/*
		RaycastHit hit;

		if (Physics.Raycast(transform.position, Vector3.down, out hit))
		if (hit.normal != Vector3.up)
		_controller.Move((Vector3.down * 4) * Time.deltaTime);
		*/
	    	
	}
    
	private bool ColliderCheck(Vector3 position)
	{
		Collider[] hitColliders = Physics.OverlapSphere(position, 0.4f, _graundLayer, QueryTriggerInteraction.Ignore);
		if(hitColliders.Length > 0)
			return true;
		else
			return false;
	}
    
	private void JumpEngage(bool flag)
	{
		startPositionY = _characterTransform.position.y;
		stopPositionY = startPositionY + jumpHeight;
		isJump = flag;
	}
}

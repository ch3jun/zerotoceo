﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public bool godMode;
	public static int Points;
	public static int Lives = 3;
	[Header("Movement config")]
	public float runSpeed = 1f;
    public float jumpHeight = 10f;
    public Vector2 velocity;

    private InteractableController[] _interactables;
    private Dictionary<InteractableController, float> _interactablesDistances = new Dictionary<InteractableController, float>();
    private GameObject _indicatorUI;
    private Animator _animator;
    private Rigidbody2D _rb;
    private bool _isGrounded;

    void Awake()
    {
        _interactables = GameObject.FindObjectsOfType<InteractableController>();
        _indicatorUI = GameObject.Find("Indicator");
        
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void UpdateIndicator()
    {
	    // Calculate distances from interactables and determine which is closest
	    foreach (var interactable in _interactables)
	    {
		    var distance = Vector3.Distance(interactable.transform.position, transform.position);
		    _interactablesDistances[interactable] = distance;
	    }
	    var closest = _interactablesDistances.OrderBy(k => k.Value).FirstOrDefault();

	    // Activate indicator & allow for interaction when the player is near an interactable
	    if (Mathf.RoundToInt(closest.Key.transform.position.x) == Mathf.RoundToInt(transform.position.x) && DialogueManager.CurrentDialogue == "")
	    {
		    _indicatorUI.SetActive(true);
		    var pos = closest.Key.transform.position;
		    _indicatorUI.transform.position = new Vector3(pos.x, pos.y + .9f, 0);
		    if (Input.GetButtonDown("Interact"))
		    {
			    closest.Key.GetComponent<InteractableController>().Interact();
		    }
	    } else
	    {
		    _indicatorUI.SetActive(false);
	    }
    }

    void OnCollisionStay2D()
    {
	    _isGrounded = true;
    }

    void Update()
    {
	    UpdateIndicator();
	    
	    if( Input.GetAxisRaw("Horizontal") == 1 )
	    {
		    velocity.x = 1;
		    // Flips sprite when necessary
		    if( transform.localScale.x < 0f && Time.timeScale == 1f)
			    transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
		    // Plays run animation when grounded
		    if( velocity.x != 0 && _isGrounded )
			    _animator.Play( Animator.StringToHash( "Run" ) );
	    }
	    else if( Input.GetAxisRaw("Horizontal") == -1 )
	    {
		    velocity.x = -1;
		    // Flips sprite when necessary
		    if( transform.localScale.x > 0f && Time.timeScale == 1f)
			    transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
		    // Plays run animation when grounded
		    if(  _isGrounded )
			    _animator.Play( Animator.StringToHash( "Run" ) );
	    }
	    else
	    {
		    velocity.x = 0;
		    // Plays idle animation when grounded
		    if( _isGrounded )
			    _animator.Play( Animator.StringToHash( "Idle" ) );
	    }
	    
	    // Only jump when grounded
	    if( (_isGrounded && Input.GetAxisRaw("Vertical") == 1) || (_isGrounded && Input.GetButtonDown("Jump")) )
	    {
		    _isGrounded = false;
		    _rb.AddForce(new Vector2(0.0f, 2.0f) * jumpHeight, ForceMode2D.Impulse);		
	    }

	    // Plays jumping animation when off the ground
	    if( !_isGrounded )
	    {
		    _animator.Play( Animator.StringToHash( "Jump" ) );
	    }
	    
	    if (!godMode) return;
	    Points = 9999;
	    Lives = 9999;
    }

    void FixedUpdate()
    {
	    _rb.velocity = new Vector2(velocity.x * runSpeed, _rb.velocity.y);
    }
}

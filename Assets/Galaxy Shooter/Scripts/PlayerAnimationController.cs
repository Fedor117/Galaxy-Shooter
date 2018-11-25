using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Options;

public class PlayerAnimationController : MonoBehaviour 
{
    Animator _animator;
    int _playerId;

	void Start () 
    {
        _animator = GetComponent<Animator>();
        var player = GetComponent<Player>();
        if (NullCheck.Some(player))
            _playerId = player.GetId();
	}

	void Update ()
    {
        var isPlayerOne = _playerId == 1;
        var right = isPlayerOne ? KeyCode.D : KeyCode.RightArrow;
        var left = isPlayerOne ? KeyCode.A : KeyCode.LeftArrow;

        if (Input.GetKeyDown(right))
        {
            _animator.SetBool("Turn_Right", true);
            _animator.SetBool("Turn_Left", false);
        }
        else if (Input.GetKeyDown(left))
        {
            _animator.SetBool("Turn_Left", true);
            _animator.SetBool("Turn_Right", false);
        }

        if (Input.GetKeyUp(right))
            _animator.SetBool("Turn_Right", false);
        else if (Input.GetKeyUp(left))
            _animator.SetBool("Turn_Left", false);
	}
}

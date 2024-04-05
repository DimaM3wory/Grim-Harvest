using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour{

    [Header("Movement")]
    private Controls input = null;
    private Vector2 moveVector = Vector2.zero;
    public float speed;


    [Header("Animation")]
    private Animator animator;
    private int idleHash = Animator.StringToHash("playerIdle");
    private int runHash = Animator.StringToHash("playerRun");

    private void Awake(){
        input = new Controls();
        animator = GetComponent<Animator>();
    }

    private void OnEnable(){
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCanceled;
    }

    private void OnDisable(){
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCanceled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext context){
        moveVector = context.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext context){
        moveVector = Vector2.zero;
    }

    private void FixedUpdate(){
        transform.position += (Vector3)moveVector * speed;
    }

    private void Update(){
        animator.StopPlayback();
        animator.CrossFade(getState(), 0, 0);

         Vector2 dir = getMoudePosition() - transform.position;
        if(dir.x != 0){
            transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
        }
    }

    private int getState(){
        if(moveVector != Vector2.zero) return runHash;
        return idleHash;
    }

    private Vector3 getMoudePosition(){
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }


}

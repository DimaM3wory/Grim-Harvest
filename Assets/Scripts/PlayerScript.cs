using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour{

    [Header("Player")]
    private SpriteRenderer spriteRenderer;

    [Header("Movement")]
    public float speed;
    private Controls controls;
    private Vector2 moveVector;
    private Rigidbody2D rigidBody;
    

    [Header("Animation")]
    private Animator animator;
    private int idleHash = Animator.StringToHash("playerIdle");
    private int runHash = Animator.StringToHash("playerRun");

    [Header("Shooting")]
    public Transform aim;

    private void Awake(){
        controls = new Controls();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable(){
        controls.Enable();
        controls.Player.Movement.performed += OnMovementPerformed;
        controls.Player.Movement.canceled += OnMovementCanceled;
    }

    private void OnDisable(){
        controls.Disable();
        controls.Player.Movement.performed -= OnMovementPerformed;
        controls.Player.Movement.canceled -= OnMovementCanceled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext context){
        moveVector = context.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext context){
        moveVector = Vector2.zero;
    }

    private void FixedUpdate(){
        rigidBody.velocity = moveVector * speed * Time.fixedDeltaTime;
    }

    private void Update(){
        animator.StopPlayback();
        animator.CrossFade(getState(), 0, 0);

        Vector2 dir = (getPointerPosition() - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(dir.x != 0){
            transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
        }

        aim.eulerAngles = new Vector3(0, 0, angle);

        if(angle > 90 || angle < -90){
            aim.localScale = new Vector3(-1, -1, 1);
        } else{
            aim.localScale = new Vector3(1, 1, 1);
        }
    }

    private int getState(){
        if(rigidBody.velocity != Vector2.zero) return runHash;
        return idleHash;
    }

    private Vector3 getPointerPosition(){
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }


}

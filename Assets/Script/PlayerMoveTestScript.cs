using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveTestScript : MonoBehaviour
{
    [SerializeField] private const float gravity = -9.81f*2;
    private LayerMask lm;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private float moveVec = 0;
    private float velocity = 0;

    private float distanceToCheck = 0.5f;
    private bool isGrounded;
    private bool isJumpReady;
    private bool jumpKey;
    private bool jumpKeyUp;

    // Start is called before the first frame update
    void Start()
    {
        lm = ~(1<<gameObject.layer);
        Debug.Log((int)lm);
        distanceToCheck = gameObject.GetComponent<BoxCollider2D>().size.y * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        ConditionUpdate();
    }
    void FixedUpdate()
    {
        RaycastHit2D hit;
        isGrounded = hit = Physics2D.Raycast(transform.position, Vector2.down, distanceToCheck, lm);
        MoveToGround(hit);
        Move(new Vector3(HorizontalForce(), VerticalForce()) * Time.deltaTime);
    }


    private void ConditionUpdate()
    {
        // if(Input.GetAxisRaw("Horizontal") != 0) moveVec = Input.GetAxis("Horizontal");
        // else moveVec -= MathF.Sign(moveVec) * 5 * Time.deltaTime;
        moveVec = Input.GetAxis("Horizontal");
        jumpKey = Input.GetKey(KeyCode.W);
        jumpKeyUp = Input.GetKeyUp(KeyCode.W);
    }
    private void Move(Vector3 force) => transform.Translate(force);

    private float VerticalForce()
    {
        velocity += gravity * Time.deltaTime;
        if(isGrounded)
        {
            if(velocity < 0) velocity = 0;
            // if(jumpKeyUp) isJumpReady = true;
        }
        if(jumpKey)
        {
            velocity = jumpForce/2;
            velocity = Mathf.Sin(velocity/jumpForce) * jumpForce;
            // if(velocity/jumpForce >= 0.9f) isJumpReady = false;
        }
        return velocity;
    }
    private float HorizontalForce()
    {
        return moveVec * speed;
    }

    private void MoveToGround(RaycastHit2D hit)
    {
        if(!isGrounded || hit.collider.isTrigger || Vector2.Distance(transform.position, hit.point) < distanceToCheck*0.7f) return;
        transform.position = new Vector3(hit.point.x, hit.point.y+distanceToCheck);
    }
}

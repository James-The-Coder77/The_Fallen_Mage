using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
  private Rigidbody2D rb;

  [Header("Moving")]
  public float moveSpeed = 5f;
  [SerializeField] float groundDrag = 6f;
  [SerializeField] float airDrag = 2f;
  [SerializeField] float airMultiplier = 0.4f;
  [SerializeField] public bool isRight;
  [SerializeField] public bool isLeft;

  [Header("Jump")]
  [SerializeField] float jumpHeight;
  [SerializeField] float groundDist;
  [SerializeField] float gravity = 6f;
  [SerializeField] LayerMask whatIsGround;
  [SerializeField] Transform groundCheck;
  [SerializeField] bool isGrounded;

  [Header("Keybinds")]
  [SerializeField] KeyCode JumpKey = KeyCode.Z;
  [SerializeField] public KeyCode rightKey;
  [SerializeField] public KeyCode leftKey;

  private float moveDir;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDist, whatIsGround);
    moveDir = Input.GetAxisRaw("Horizontal");
    if(isGrounded){
      rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
      rb.drag = groundDrag;
    }else{
      rb.velocity = new Vector2(moveDir * moveSpeed * airMultiplier, rb.velocity.y);
      rb.drag = airDrag;
    }
    Jump();
    DirectionCalculation();
  }

  void Jump(){
    if(Input.GetKeyDown(JumpKey) && isGrounded)
      rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
  }

  void OnDrawGizmosSelected(){
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(groundCheck.position, groundDist);
  }


  void DirectionCalculation(){
    if(rb.velocity.x > 0f || Input.GetKey(rightKey)){
      isRight = true;
      isLeft = false;
    }
    if(rb.velocity.x < 0f || Input.GetKey(leftKey)){
      isRight = false;
      isLeft = true;
    }
    if(isRight){
      transform.rotation = Quaternion.Euler(transform.right);
      isLeft = false;
    }
    if(isLeft){
      transform.rotation = Quaternion.Euler(-transform.right);
      isRight = false;
    }
  }
}

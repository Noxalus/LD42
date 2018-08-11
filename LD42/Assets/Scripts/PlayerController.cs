using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D Controller;
    public Animator Animator;
    public Rigidbody2D Rigidbody;
    public float speed = 40f;

    private float _horizontalMove = 0f;
    private bool _jump = false;
    private bool _crouch = false;

    private void Update()
    {
        _horizontalMove = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
            _jump = true;

        if (Input.GetButtonDown("Fire2"))
            _crouch = true;
        else if (Input.GetButtonUp("Fire2"))
            _crouch = false;

        Animator.SetBool("Crouching", _crouch);
        Animator.SetFloat("Speed", Rigidbody.velocity.magnitude);
    }

    void FixedUpdate()
    {
        Controller.Move(_horizontalMove * speed * Time.fixedDeltaTime, _crouch, _jump);
        _jump = false;
    }
}

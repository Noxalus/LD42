using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D Controller;
    public Animator Animator;
    public Rigidbody2D Rigidbody;
    public float speed = 40f;
    public AudioClip JumpSound;
    public AudioClip CrouchSound;

    private float _horizontalMove = 0f;
    private bool _jump = false;
    private bool _crouch = false;
    private bool _wantToStandUp = false;

    private bool _wasCrouched = false;

    private void Update()
    {
        _horizontalMove = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
            _jump = true;

        if (Input.GetButtonDown("Crouch"))
            _crouch = true;
        else if (Input.GetButtonUp("Crouch"))
        {
            if (Controller.CanStandUp())
                _crouch = false;
            else
                _wantToStandUp = true;
        }

        if (_crouch && _wantToStandUp && Controller.CanStandUp())
        {
            _crouch = false;
            _wantToStandUp = false;
        }

        Animator.SetBool("Crouching", _crouch);
        Animator.SetFloat("Speed", Rigidbody.velocity.magnitude);
    }

    void FixedUpdate()
    {
        if (Controller.IsGrounded() && _jump)
            GameManager.Instance().PlaySound(JumpSound);
        if (!_wasCrouched && _crouch)
            GameManager.Instance().PlaySound(CrouchSound);

        Controller.Move(_horizontalMove * speed * Time.fixedDeltaTime, _crouch, _jump);
        _jump = false;

        _wasCrouched = _crouch;
    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D Controller;
    public float speed = 40f;

    private float _horizontalMove = 0f;
    private bool _jump;

    private void Update()
    {
        _horizontalMove = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
            _jump = true;
    }

    void FixedUpdate()
    {
        Controller.Move(_horizontalMove * speed * Time.fixedDeltaTime, false, _jump);
        _jump = false;
    }
}

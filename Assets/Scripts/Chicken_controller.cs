using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chicken_controller : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;
    private Vector2 movement;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        animator.SetFloat("Speed", Mathf.Abs(movement.magnitude * movementSpeed));

        bool flipped = movement.x > 0;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180: 0f, 0f));
    }
    private void FixedUpdate()
    {
        var xMovement = movement.x * movementSpeed * Time.deltaTime;
        var yMovement = movement.y * movementSpeed * Time.deltaTime;
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1)
        {
            this.transform.Translate(new Vector3(xMovement, 0), Space.World);
        }
        else if (Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            this.transform.Translate(new Vector3(0, yMovement), Space.World);
        }

    }
}


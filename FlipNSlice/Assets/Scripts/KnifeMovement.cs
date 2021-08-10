using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeMovement : MonoBehaviour
{
    Rigidbody rb;
    Transform cameraHolder;
    Animator animator;
    GameManager gm;

    [SerializeField] float playerSpeed;
    [SerializeField] float jumpForce;

    bool jump = false;

    Vector3 vec;
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraHolder = Camera.main.transform.parent;
    }


    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            gm.isGameStarted = true;
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        if (gm.isGameStarted)
        {
            rb.useGravity = true;
        }
        if (jump)
        {
            this.GetComponent<AudioSource>().Play();
            rb.AddForce(Vector3.forward * playerSpeed * Time.fixedDeltaTime * 100);

            rb.velocity = new Vector3(rb.velocity.x, 0, Mathf.Clamp(rb.velocity.z, 0, 10));
            rb.AddForce(Vector3.up * jumpForce * Time.fixedDeltaTime * 3000);
            animator.enabled = true;
            jump = false;
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * 0.98f);
        }
    }

    private void LateUpdate()
    {
        vec.x = cameraHolder.transform.position.x;
        vec.y = transform.position.y;
        vec.z = transform.position.z;
        Vector3 smoothPosition = Vector3.Lerp(cameraHolder.position, vec, 0.01f);

        cameraHolder.transform.position = smoothPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Killer")
        {
            gm.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            animator.enabled = false;
        }
    }
}

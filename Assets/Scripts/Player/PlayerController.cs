using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static event System.Action onReachedFinish;

    [Header("Movement")]
    public float movementSpeed;
    public float jumpForce;
    public float jumpCooldown;
    public float airMult;

    bool isAbleToJump = true;

    public float groundDrag;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    bool isInGround;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    // public KeyCode spawnHealth = KeyCode.H;

    // public KeyCode spawnWaste = KeyCode.J;

    bool areControlsDisabled;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Esto nos ayuda a que el jugador no se "caiga".
        Drone.OnPlayerSpotted += disableControls;
    }

    void Update()
    {
        // Lanzamos Raycast  para ver si nos encontramos en contacto con el suelo
        // Debe de ser 1/2 de la altura del jugador + 0.2
        isInGround = Physics.Raycast(
            transform.position,
            Vector3.down,
            playerHeight * 0.5f + 0.2f,
            groundLayer
        );

        if (!areControlsDisabled)
        {
            // Obtenemos Inputs de Axes
            getInputs();
            // Control de Velocidad
            speedControl();
        }

        // Sí el jugador se encuentra en el suelo, aplicar fuerza de drag.

        if (isInGround)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!areControlsDisabled)
        {
            movePlayer();
        }
    }

    private void getInputs()
    {
        // Obtenemos nuestras Axes
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Brincar
        if (Input.GetKey(jumpKey) && isAbleToJump && isInGround)
        {
            isAbleToJump = false;
            jump();

            Invoke(nameof(reset), jumpCooldown);
        }
    }

    private void movePlayer()
    {
        // Aplicamos fuerza en la dirección a cual estamos viendo
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isInGround)
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f, ForceMode.Force);
        else if (!isInGround)
            rb.AddForce(moveDirection.normalized * movementSpeed * 10f * airMult, ForceMode.Force);
    }

    private void speedControl()
    {
        // Obtenemos velocida actual
        Vector3 vel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Sí es mayor a la debida
        if (vel.magnitude > movementSpeed)
        {
            // Creammos vector normalizado con la mmagnitud correcta de velocidad y lo aplicamos al rigidbody
            Vector3 limVel = vel.normalized * movementSpeed;
            rb.velocity = new Vector3(limVel.x, rb.velocity.y, limVel.z);
        }
    }

    private void jump()
    {
        // Empezamos con 0 fuerzas en y
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //  Aplicamos Fuerza
        //* Se utiliza ForceMode.Impulse ya que solo se aplica una vez
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void reset()
    {
        isAbleToJump = true;
    }

    void disableControls()
    {
        areControlsDisabled = true;
    }

    void OnDestroy()
    {
        Drone.OnPlayerSpotted -= disableControls;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Finish")
        {
            disableControls();

            if (onReachedFinish != null)
            {
                onReachedFinish();
            }
        }
    }
}

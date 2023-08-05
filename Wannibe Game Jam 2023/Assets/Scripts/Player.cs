using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerSO stats;
    [SerializeField] float health;
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] float freezeMax;
    [SerializeField] float freezeRate;
    [SerializeField] float freezeLength;
    //Misc 
    private InputSystem input = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D playerRB = null;

    private void Awake()
    {
        input = new InputSystem();
        playerRB = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health = stats.health;
        speed = stats.speed;
        damage = stats.damage;
        freezeMax = stats.freezeMax;
        freezeRate = stats.freezeRate;
        freezeLength = stats.freezeLength;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        playerRB.velocity = moveVector * speed;
    }


    // Movement
    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
}

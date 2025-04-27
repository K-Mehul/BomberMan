using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    private Vector2 direction = Vector2.down;
    public float speed = 5f;

    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;


    [SerializeField] AnimatedSpriteRenderer animatedSpriteRendererUp;
    [SerializeField] AnimatedSpriteRenderer animatedSpriteRendererDown;
    [SerializeField] AnimatedSpriteRenderer animatedSpriteRendererLeft;
    [SerializeField] AnimatedSpriteRenderer animatedSpriteRendererRight;
    [SerializeField] AnimatedSpriteRenderer animationSpriteRendererDie;

    private AnimatedSpriteRenderer activeSpriteRenderer;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = animatedSpriteRendererDown;
    }

    private void Update()
    {
        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up,animatedSpriteRendererUp);
        }
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down,animatedSpriteRendererDown); 
        }
        else if (Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left,animatedSpriteRendererLeft); 
        }
        else if (Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right,animatedSpriteRendererRight); 
        }
        else
        {
            SetDirection(Vector2.zero,activeSpriteRenderer);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;
        animatedSpriteRendererUp.enabled = spriteRenderer == animatedSpriteRendererUp;
        animatedSpriteRendererDown.enabled = spriteRenderer == animatedSpriteRendererDown;
        animatedSpriteRendererLeft.enabled = spriteRenderer == animatedSpriteRendererLeft;
        animatedSpriteRendererRight.enabled = spriteRenderer == animatedSpriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    public void IncreaseSpeed()
    {
        speed++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;

        animatedSpriteRendererDown.enabled = false;
        animatedSpriteRendererUp.enabled = false;
        animatedSpriteRendererLeft.enabled = false;
        animatedSpriteRendererRight.enabled = false;
        animationSpriteRendererDie.enabled = true;

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        //FindObjectOfType<GameManager>().CheckWinState();
    }
}

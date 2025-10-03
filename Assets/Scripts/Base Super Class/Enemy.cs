using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public abstract class Enemy : MonoBehaviour
{

    //Audio clips!
    public AudioClip EnemyDeathNoise;

    public Vector3 previousPosition;
    public EnemyData enemyData;
    public Animator animator;
    public Rigidbody2D rb;
    public int currentHp;
    public float currentSpeed;
    public bool isSlowed = false;
    public bool isPoisoned = false;
    public bool dead = false;
    public float length;
    public SplineContainer pathStorage;

    public Spline enemyPath;
    public SpriteRenderer spriteRenderer;

    public float progress = 0f;
    public bool isDead = false;
    protected virtual void Start()
    {
        currentHp = enemyData.health;
        currentSpeed = enemyData.speed;
        pathStorage = FindObjectOfType<SplineContainer>();
        enemyPath = pathStorage.Spline;
        length = pathStorage.CalculateLength();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        previousPosition = transform.position;
    }
    protected virtual void Update()
    {
        if(enemyPath != null && !dead)
        {
            progress += currentSpeed * Time.deltaTime;

            // Calculate normalized progress (t) from distance
            float t = progress / length;

            // Get position on spline at t
            Vector3 position = pathStorage.EvaluatePosition(t);

            // Update the object's position
            transform.position = position;


            //if enemy is facing leftward: swap enemy animation 
            float deltaX = position.x - previousPosition.x;

            if (deltaX > 0) // Moving right
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (deltaX < 0) // Moving left
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            previousPosition = position; // Update the reference

            //when an enemy reaches the end of the spline(the level)  
            if(progress >= length)
            {
                if(!MoneyManager.instance.livesLost(enemyData.livesTaken))
                {
                    UiManager.instance.GameOver();
                }
                Destroy(gameObject);
            }

            if(currentHp <= 0 && !dead)//runs when enemy is defeated
            {
                animator.SetTrigger("isDead");
                isDead = true;
                MoneyManager.instance.MoneyEarned(enemyData.moneyGained);
                SoundFXManager.Instance.PlaySoundFXClip(EnemyDeathNoise);
                dead = true;
            }
        }
    }
    public void ApplyColorEffect(float R, float G, float B)
    {
        if(spriteRenderer != null)
        {
            Color newColor = new Color(R, G, B);
            spriteRenderer.color = newColor;
        }
    }
    public void death()
    {
        Destroy(gameObject);
    }
}

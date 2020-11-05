using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Projectile : MonoBehaviour, IProjectile
{
    public float damage {get; set;}
    public float moveSpeed { get; set; }

    public float liveTime { get; set; }
    public int maxTargetHit { get; set; }

    public bool isAlliedForPlayer { get; set; }

    public bool isAlliedForEnemy { get; set; }

    public Transform ignoreTransform { get; set; }

    [SerializeField] GameObject explosion;
    [SerializeField] float explosionScale;

    [SerializeField] private float existTime = 0f;


    [SerializeField] MovementController movementController;

    public void AddExistTime(float existTime)
    {
        this.existTime += existTime;
    }

    public bool IsNeedTODestroy()
    {
        return existTime > liveTime;
    }


    public void Move()
    {
        if(!movementController.IsHaveRigidBody())
        {
            movementController.Move(transform.up, moveSpeed );
        }    
    }

    public void Destroy()
    {
        if (explosion != null)
        {
            GameObject explosionGameObject = Instantiate(explosion);
            explosionGameObject.transform.position = transform.position;
            explosionGameObject.transform.localScale *= explosionScale;
        }
        Messenger.Broadcast(GameEvents.PROJECTILE_DESTROYED, this as IProjectile);
        Destroy(gameObject);
    }
    private void Awake()
    {
        movementController = GetComponent<MovementController>();
    }

    private void Start()
    {
        movementController.Move(transform.up, moveSpeed);
        Messenger.Broadcast(GameEvents.PROJECTILE_SPAWNED, this as IProjectile);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        OnCollide(collider);
    }

    private void OnCollide(Collider2D collider)
    {
        IDamageble damageble = collider.GetComponent<IDamageble>();
        if (damageble != null)
        {
            PlayerInputController player = collider.GetComponent<PlayerInputController>();
            if (player != null)
            {
                if (!isAlliedForPlayer)
                {
                    DealDamage(damageble);
                }
            }
            else
            {
                IEnemy enemy = collider.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    if (!isAlliedForEnemy)
                    {
                        DealDamage(damageble);
                    }
                }
            }

        }
    }



    private void DealDamage(IDamageble damageble)
    {
        damageble.GetDamage(damage);
        Destroy();
    }
}

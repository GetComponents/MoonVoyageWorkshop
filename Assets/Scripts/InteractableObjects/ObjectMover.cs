using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{

    public float RotationSpeed, MovementSpeed;
    private Rigidbody2D rb;
    [SerializeField]
    List<Transform> Path;
    Vector2 previousDestination, nextDestination;
    [SerializeField]
    float minRotation, maxRotation;
    float previousRotation, nextRotation;
    [SerializeField]
    bool goFullCircle;
    int pathIndex;
    float destinationCompletion;
    public Vector2 ProjectileDirection;
    [SerializeField]
    EMovementType movement;
    [SerializeField]
    float StopTimer;
    float currentTimer;
    [SerializeField]
    GameObject dot;
    [SerializeField]
    float slowDownTime, baseMoveSpeed;

    private void Awake()
    {
        if (TryGetComponent<ObjectProperty>(out ObjectProperty op))
        {
            op.RotationSpeed = RotationSpeed;
            if (RotationSpeed != 0)
            {
                op.Rotates = true;
            }
            op.MoveSpeed = MovementSpeed;
            if (MovementSpeed != 0)
            {
                op.Moves = true;
            }
        }
        if (movement == EMovementType.PROJECTILE)
        {
            rb = GetComponent<Rigidbody2D>();
            return;
        }
        if (MovementSpeed > 0)
        {
            NextPoint();
        }
    }

    void Update()
    {
        Rotate();
        Move();
    }

    private void Rotate()
    {
        if (RotationSpeed == 0)
        {
            return;
        }
        if (movement == EMovementType.NONE)
        {
            transform.eulerAngles += new Vector3(0, 0, RotationSpeed * Time.deltaTime);
            return;
        }
        if (movement == EMovementType.STOPAndGO && currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            return;
        }
        if (movement != EMovementType.PROJECTILE)
            destinationCompletion += Time.deltaTime * RotationSpeed;
        if (destinationCompletion >= 1)
        {
            ChangeRotation();
        }
        float currentCompletion = 0;
        switch (movement)
        {
            case EMovementType.NONE:
                Debug.LogWarning("No Movementtype defined in Object" + gameObject.name);
                break;
            case EMovementType.LINEAR:
                currentCompletion = destinationCompletion;
                break;
            case EMovementType.SINE:
                currentCompletion = (Mathf.Sin((destinationCompletion * Mathf.PI) - 1.5f) / 2) + 0.5f;
                break;
            case EMovementType.STOPAndGO:
                currentCompletion = destinationCompletion;
                break;
            case EMovementType.PROJECTILE:
                rb.velocity = ProjectileDirection * MovementSpeed;
                return;
            default:
                break;

        }
        //transform.position = Vector3.Lerp(previousDestination, nextDestination, currentCompletion);
        Vector3 tmp = Vector3.zero;
        tmp.z = Mathf.LerpAngle(previousRotation, nextRotation, currentCompletion);
        transform.eulerAngles = tmp;
    }

    private void ChangeRotation()
    {
        //float tmp = minRotation;
        //minRotation = maxRotation;
        //maxRotation = tmp;
        //destinationCompletion = 0;
        //currentTimer = StopTimer;

        previousRotation = Path[pathIndex].localEulerAngles.z;
        pathIndex++;
        if (pathIndex >= Path.Count)
        {
            pathIndex = 0;
        }
        nextRotation = Path[pathIndex].localEulerAngles.z;
        destinationCompletion = 0;
        currentTimer = StopTimer;
    }

    private void Move()
    {
        if (MovementSpeed == 0)
            return;

        if (movement == EMovementType.STOPAndGO && currentTimer > 0)
        {
            if (currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;
                return;
            }
            else if (destinationCompletion == 0)
            {
                StartMovingSoundLoop();
            }
        }
        if (movement != EMovementType.PROJECTILE  && movement != EMovementType.SLOWDOWN)
            destinationCompletion += Time.deltaTime * MovementSpeed;
        if (destinationCompletion >= 1)
        {
            StopMovingSoundLoop();
            NextPoint();
            if (movement == EMovementType.STOPAndGO)
                return;
            StartMovingSoundLoop();
        }
        float currentCompletion = 0;
        switch (movement)
        {
            case EMovementType.NONE:
                Debug.LogWarning("No Movementtype defined in Object" + gameObject.name);
                break;
            case EMovementType.LINEAR:
                currentCompletion = destinationCompletion;
                break;
            case EMovementType.SINE:
                currentCompletion = (Mathf.Sin((destinationCompletion * Mathf.PI) - 1.5f) / 2) + 0.5f;
                break;
            case EMovementType.STOPAndGO:
                currentCompletion = destinationCompletion;
                break;
            case EMovementType.PROJECTILE:
                rb.velocity = ProjectileDirection * MovementSpeed;
                return;
            case EMovementType.SLOWDOWN:
                    return;
            default:
                break;

        }
        transform.position = Vector3.Lerp(previousDestination, nextDestination, currentCompletion);
    }

    public void StartToSlowDown(Vector2 playerDir)
    {
        destinationCompletion = 0;
        ProjectileDirection = playerDir;
        //baseMoveSpeed = playerDir.sqrMagnitude;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.AddForce(playerDir * baseMoveSpeed);
        movement = EMovementType.SLOWDOWN;
        GloopMain.Instance.MyMovement.MyBase.rb.simulated = false;
        GloopMain.Instance.MyMovement.MyBase.transform.position = transform.position;
    }

    private void NextPoint()
    {
        previousDestination = Path[pathIndex].position;
        pathIndex++;
        if (pathIndex >= Path.Count)
        {
            pathIndex = 0;
        }
        nextDestination = Path[pathIndex].position;
        destinationCompletion = 0;
        currentTimer = StopTimer;
    }

    private void StartMovingSoundLoop()
    {
        //WwisePlay ObMoveLoop

        //switch (MaterialType)
        //{
        //    case EPlant:
        //        //WwisePlay ObPlantMoveLoop
        //        break;
        //    case EStone:
        //        //WwisePlay ObStoneMoveLoop
        //        break;
        //    default:
        //        break;
        //}
    }

    private void StopMovingSoundLoop()
    {
        //WwiseStopPlay ObMoveLoop

        //switch (MaterialType)
        //{
        //    case EPlant:
        //        //WwiseStopPlay ObPlantMoveLoop
        //        break;
        //    case EStone:
        //        //WwiseStopPlay ObStoneMoveLoop
        //        break;
        //    default:
        //        break;
        //}
    }

    //private void OnDestroy()
    //{
    //    if (movement == EMovementType.SLOWDOWN)
    //    {
    //        GloopMoveBase tmp = GloopMain.Instance.MyMovement.MyBase;
    //        tmp.UnparentFromPlatform();
    //        tmp.rb.simulated = true;
    //    }
    //}
}

public enum EMovementType
{
    NONE,
    LINEAR,
    SINE,
    STOPAndGO,
    PROJECTILE,
    SLOWDOWN
}

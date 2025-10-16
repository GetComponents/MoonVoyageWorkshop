using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectProperty : MonoBehaviour
{
    public bool Moves, Rotates;
    //public bool movable;
    [SerializeField]
    public ObjectType Type;
    //public float stickiness;
    [SerializeField, Header("Launch")]
    float launchStrength;
    [SerializeField]
    Vector2 LockDir = new Vector2(1, 1);
    [SerializeField]
    GameObject lilDot;
    public GameObject CurrentDot;

    [HideInInspector]
    public float MoveSpeed;
    [HideInInspector]
    public float RotationSpeed;
    [SerializeField]
    Animator bounceAnim;
    [SerializeField]
    AudioClip interactionSXF;
    [SerializeField]
    private bool reactToGravity;
    //[SerializeField]
    //float soundVolume = 1;
    [SerializeField]
    bool isSticky, destroyOnInteract;
    [SerializeField]
    bool floorOnInteract;
    [SerializeField]
    bool ignoreCatapultCollision, stopCatapultCollision = true;
    public UnityEvent InteractedWithPlayer, ExitInteractionWithPlayer;
    [SerializeField]
    GameObject interactionParticle;
    bool playerInBubble;

    private void Start()
    {
        if ((Moves || Rotates) && lilDot != null)
        {
            CurrentDot = Instantiate(lilDot, transform);
            CurrentDot.transform.localScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y);
            CurrentDot.SetActive(false);
        }
        if (reactToGravity)
        {
            GameManager.Instance.GravitySwitch?.AddListener(Flip);
        }

        switch (Type)
        {
            case ObjectType.NONE:
                break;
            case ObjectType.BOUNCE:
                break;
            case ObjectType.STICKY:
                break;
            case ObjectType.HURT:
                break;
            case ObjectType.COLLECTABLE:
                SoundManager.Instance.PlaySFX(eSFX.EObAmStardust, this.gameObject);
                break;
            case ObjectType.KEY:
                SoundManager.Instance.PlaySFX(eSFX.EObAmKey, this.gameObject);
                break;
            case ObjectType.CATAPULT:
                break;
            case ObjectType.BOUNCEPLANT:
                break;
            default:
                break;
        }
    }

    public void EnterCollisionWithObject(Rigidbody2D obj)
    {
        if (ignoreCatapultCollision && GloopMain.Instance.MyMovement.MyBase.HoldingVelocity == true)
        {
            return;
        }
        switch (Type)
        {
            case ObjectType.NONE:
                break;
            case ObjectType.BOUNCE:
                obj.velocity *= LockDir;
                GloopMain.Instance.MyMovement.MyBase.AddForce(launchStrength * transform.up);
                if (bounceAnim != null)
                {
                    bounceAnim.SetBool("Bounce", true);
                }
                SoundManager.Instance.PlaySFX(eSFX.EObMushroomBounce, this.gameObject);
                break;
            case ObjectType.STICKY:
                var player = GloopMain.Instance.MyMovement.MyBase;
                player.StickToSurface(transform);
                if (Moves || Rotates)
                {
                    GloopMain.Instance.Rotation.UnrotateCursor = true;
                    player.UnstickToSurfaceEvent.AddListener(MomentumLaunchPlayer);
                    Vector2 rot = transform.position - player.transform.position;
                    GloopMain.Instance.Rotation.RotatePlayerToRotation(Quaternion.AngleAxis((Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg) + 90, Vector3.forward));
                    GloopMain.Instance.Rotation.disableOtherRotations = true;
                }
                break;
            case ObjectType.HURT:
                GloopMain.Instance.DamagePlayer();
                break;
            case ObjectType.COLLECTABLE:
                SoundManager.Instance.PlaySFX(eSFX.EObCollectStardust, null);
                Backpack.Instance.AddStar(gameObject);
                gameObject.SetActive(false);
                //TODO: Collect object
                break;
            case ObjectType.KEY:
                SoundManager.Instance.PlaySFX(eSFX.EObCollectKey, null);
                Backpack.Instance.AddKey(gameObject);
                gameObject.SetActive(false);
                break;
            case ObjectType.CATAPULT:
                var gloop = GloopMain.Instance.MyMovement.MyBase;
                gloop.transform.position = transform.position;
                gloop.StickToSurface(transform);
                gloop.UnstickToSurfaceEvent.AddListener(CatapultPlayer);
                if (Moves || Rotates)
                {
                    GloopMain.Instance.Rotation.UnrotateCursor = true;
                }
                break;
            case ObjectType.BOUNCEPLANT:
                SoundManager.Instance.PlaySFX(eSFX.EObBouncePlantBounce, this.gameObject);
                obj.velocity *= 0;
                Vector2 launchDir = (obj.transform.position - transform.position).normalized;
                obj.AddForce(launchStrength * launchDir);
                if (bounceAnim != null)
                {
                    bounceAnim.SetBool("Bounce", true);
                }
                break;
            case ObjectType.GRAVITYSWITCH:
                GameManager.Instance.GravitySwitch?.Invoke();
                SoundManager.Instance.PlaySFX(eSFX.EObGravitySwitch, this.gameObject);
                break;
            case ObjectType.BUBBLE:
                if (GloopMain.Instance.CurrentMode != EMode.DASH)
                {
                    obj.velocity *= LockDir;
                    GloopMain.Instance.MyMovement.MyBase.AddForce(launchStrength * transform.up);
                    if (bounceAnim != null)
                    {
                        bounceAnim.SetBool("Bounce", true);
                    }
                    SoundManager.Instance.PlaySFX(eSFX.EObPopBubble, null);
                    break;
                }
                GloopDash tmp = (GloopDash)GloopMain.Instance.MyMovement;
                if (tmp.IsDashing)
                {
                    //GetComponent<Bubble>().EncapsulatePlayer();
                    GloopMain.Instance.MyMovement.MyBase.StickToSurface(transform);
                    GloopMain.Instance.MyMovement.MyBase.transform.localPosition = Vector3.zero;
                    GetComponent<ObjectMover>().StartToSlowDown(tmp.DashDir * tmp.DashStrength);
                    GloopMain.Instance.MyMovement.MyBase.UnstickToSurfaceEvent.AddListener(PopBubble);
                    destroyOnInteract = false;
                    playerInBubble = true;
                }
                else
                {
                    obj.velocity *= LockDir;
                    GloopMain.Instance.MyMovement.MyBase.AddForce(launchStrength * transform.up);
                    if (bounceAnim != null)
                    {
                        bounceAnim.SetBool("Bounce", true);
                    }
                    break;
                }
                break;
            default:
                break;
        }

        InteractedWithPlayer?.Invoke();
        if ((Moves || Rotates) && !(Type == ObjectType.COLLECTABLE || Type == ObjectType.KEY))
        {
            GloopMain.Instance.Rotation.UnrotateCursor = true;
            GloopMain.Instance.MyMovement.MyBase.ParentToPlatform(transform);
        }

        if (stopCatapultCollision && GloopMain.Instance.MyMovement.MyBase.HoldingVelocity == true)
        {
            GloopMain.Instance.MyMovement.MyBase.HoldingVelocity = false;
            GloopMain.Instance.MyMovement.MyBase.InputLocked--;
        }
        if (floorOnInteract)
        {
            GloopMain.Instance.MyMovement.MyBase.GroundEnter();
        }
        if (interactionParticle != null)
        {
            Instantiate(interactionParticle, transform.position, Quaternion.identity);
        }
        if (interactionSXF != null)
        {
            //SoundManager.Instance.PlayEffect(interactionSXF, soundVolume);
            //SoundManager.Instance.PlaySFX()
            //SoundManager.Instance.PlayEffect(interactionSXF);
            //AudioSource.PlayClipAtPoint(interactionSXF, transform.position);
        }
        if (destroyOnInteract)
        {
            if (floorOnInteract)
            {
                GloopMain.Instance.MyMovement.MyBase.GroundExit();
            }
            if (Moves || Rotates)
            {
                GloopMain.Instance.MyMovement.MyBase.UnparentFromPlatform();
            }
            Destroy(gameObject);
        }
    }

    public Rigidbody2D AttachToLilDot(Vector2 position)
    {
        CurrentDot.transform.position = position;
        CurrentDot.SetActive(true);
        return CurrentDot.GetComponent<Rigidbody2D>();
    }

    public TonguePoint CreateTonguePoint(Vector2 position)
    {
        //if (Moves || Rotates)
        //{
        if (CurrentDot == null)
        {
            return new TonguePoint(position);
            //CurrentDot = Instantiate(lilDot, transform);
            //CurrentDot.transform.localScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y);
        }

        CurrentDot.transform.position = position;
        CurrentDot.SetActive(true);
        return new TonguePoint(CurrentDot.GetComponent<Rigidbody2D>(), CurrentDot.transform);
        //}
        //return new TonguePoint(position);
    }

    private void MomentumLaunchPlayer()
    {
        if (Moves)
        {
            GloopMain.Instance.MyMovement.MyBase.Jump();
        }
        if (Rotates)
        {
            Vector2 launchAngle = GloopMain.Instance.MyMovement.transform.position - transform.position;
            var rot = Quaternion.AngleAxis((RotationSpeed / RotationSpeed) * 90, Vector3.forward);
            launchAngle = rot * launchAngle;
            GloopMain.Instance.MyMovement.MyBase.rb.AddForce(launchAngle * launchStrength);
            GloopMain.Instance.Rotation.UnrotateCursor = false;
        }
        SoundManager.Instance.PlaySFX(eSFX.EPlUnstickFromStickySurface, this.gameObject);
        GloopMain.Instance.Rotation.disableOtherRotations = false;
        GloopMain.Instance.Rotation.RotateToGravity();
        GloopMain.Instance.MyMovement.MyBase.UnstickToSurfaceEvent.RemoveListener(MomentumLaunchPlayer);
    }

    private void CatapultPlayer()
    {
        SoundManager.Instance.PlaySFX(eSFX.EObCatapultPlayerStart, this.gameObject);
        GloopMain.Instance.MyMovement.MyBase.VelocityToHold = transform.up.normalized * launchStrength;
        GloopMain.Instance.MyMovement.MyBase.HoldingVelocity = true;
        GloopMain.Instance.MyMovement.MyBase.InputLocked++;
        //GloopMain.Instance.MyMovement.MyBase.rb.gravityScale = 0;
        GloopMain.Instance.MyMovement.MyBase.UnstickToSurfaceEvent.RemoveListener(CatapultPlayer);
        if (GloopMain.Instance.CurrentMode == EMode.DASH)
        {
            GloopDash tmp = (GloopDash)GloopMain.Instance.MyMovement;
            tmp.DashCharge = 1;
        }
    }

    private void PopBubble()
    {
        SoundManager.Instance.PlaySFX(eSFX.EObPopBubble, null);
        Debug.Log("POP");
        GloopMain.Instance.MyMovement.MyBase.UnparentFromPlatform();
        //GloopMain.Instance.MyMovement.MyBase.UnstickToSurfaceEvent?.Invoke();
        GloopMain.Instance.MyMovement.MyBase.rb.simulated = true;
        Destroy(gameObject);
    }

    private void Flip()
    {
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {

            GetComponent<Rigidbody2D>().gravityScale *= -1;
            transform.eulerAngles += new Vector3(0, 0, 180);
        }
        else
        {
            Debug.LogWarning($"Object {gameObject.name} wants to flip, but has no rigidbody attached!");
        }
    }

    public void KillLilDot()
    {
        CurrentDot.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnterCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnterCollision(collision.collider);
    }

    private void EnterCollision(Collider2D collision)
    {
        if (playerInBubble)
        {
            PopBubble();
        }
        if (collision.gameObject.tag == "Player")
        {
            EnterCollisionWithObject(GloopMain.Instance.MyMovement.MyBase.rb);
        }
        if (collision.gameObject.tag == "Hookshot")
        {
            GrappleShot gs = collision.gameObject.GetComponent<GrappleShot>();
            if (gs != null)
                gs.AttachToObject(CreateTonguePoint(collision.transform.position));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ExitCollision(collision.collider);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitCollision(collision);
    }

    private void ExitCollision(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (floorOnInteract)
            {
                GloopMain.Instance.MyMovement.MyBase.GroundExit();
            }
            if (collision.transform.parent == transform && !playerInBubble)
            {
                GloopMain.Instance.MyMovement.MyBase.UnparentFromPlatform();
            }
            ExitInteractionWithPlayer?.Invoke();
        }
    }
}

public enum ObjectType
{
    NONE,
    BOUNCE,
    STICKY,
    HURT,
    COLLECTABLE,
    KEY,
    CATAPULT,
    BOUNCEPLANT,
    GRAVITYSWITCH,
    BUBBLE
}

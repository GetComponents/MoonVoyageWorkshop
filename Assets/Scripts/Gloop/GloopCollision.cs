using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GloopCollision : MonoBehaviour
{
    [SerializeField]
    GloopMoveBase gloopMove;

    bool gravitySwitched;

    private void Start()
    {
        GameManager.Instance.GravitySwitch.AddListener(ChangePosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            if (GloopMain.Instance.MyMovement.MyBase.GroundedAmount == 0)
                SoundManager.Instance.PlaySFX(eSFX.EObPlayerHitsGround, this.gameObject);
            gloopMove.GroundEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            gloopMove.GroundExit();
        }
    }

    public void ChangePosition()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y, transform.localPosition.z);
    }
}

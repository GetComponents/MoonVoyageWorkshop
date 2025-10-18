using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleShot : MonoBehaviour
{
    public LineRenderer Lr;
    public GoopGrapple player;
    [SerializeField]
    float countdown;

    private void Start()
    {
        Lr.SetPosition(0, player.transform.position);
        Lr.SetPosition(1, transform.position);
        Lr.enabled = true;
    }
    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            Destroy(gameObject);
            Lr.enabled = false;
        }
        //if ((!Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Space)) || GloopMain.Instance.MyMovement != player)
        //{
        //    Destroy(gameObject);
        //}
        Lr.SetPosition(0, player.transform.position);
        Lr.SetPosition(1, transform.position);
    }

    public void AttachToObject(TonguePoint point)
    {
        SoundManager.Instance.PlaySFX(eSFX.EPlShotHitGround, null);
        player.AttachPoint(point);
        player.EnableTongue();
        Destroy(gameObject);
    }
}

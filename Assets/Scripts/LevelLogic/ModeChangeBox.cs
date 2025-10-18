using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeChangeBox : MonoBehaviour
{
    [SerializeField]
    EMode myTransformation;
    [SerializeField]
    bool OneTimeUse;
    [SerializeField]
    AudioClip interactionSound;
    [SerializeField]
    GameObject interactionPFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (OneTimeUse)
            {
                Backpack.Instance.LosableObjects.Add(gameObject);
                gameObject.SetActive(false);
            }
            TransformPlayer();
            Instantiate(interactionPFX, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySFX(eSFX.EObCollectStardust, null);
        }
    }

    private void TransformPlayer()
    {
        GloopMain.Instance.CurrentMode = myTransformation;
    }
}

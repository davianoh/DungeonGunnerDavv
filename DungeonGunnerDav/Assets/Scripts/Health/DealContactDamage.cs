using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DealContactDamage : MonoBehaviour
{
    [SerializeField] private int contactDamageLvl1;
    [SerializeField] private int contactDamageLvl2;
    private int contactDamageAmount;
    [SerializeField] private int levelIndexToLevel2;
    [SerializeField] private LayerMask layerMask;
    private bool isColliding = false;

    private void Start()
    {
        if(GameResources.Instance.selectedLevelIndex >= levelIndexToLevel2)
        {
            contactDamageAmount = contactDamageLvl2;
        }
        else
        {
            contactDamageAmount = contactDamageLvl1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isColliding) return;
        ContactDamage(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isColliding) return;
        ContactDamage(collision);
    }

    private void ContactDamage(Collider2D collision)
    {
        int collisionObjectLayerMask = (1 << collision.gameObject.layer);
        if((layerMask.value & collisionObjectLayerMask) == 0)
        {
            return;
        }

        ReceiveContactDamage receiveContactDamage = collision.gameObject.GetComponent<ReceiveContactDamage>();
        if(receiveContactDamage != null)
        {
            isColliding = true;
            Invoke("ResetContactCollision", Settings.contactDamageCollisionResetDelay);
            receiveContactDamage.TakeContactDamage(contactDamageAmount);
        }
    }

    private void ResetContactCollision()
    {
        isColliding = false;
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(contactDamageAmount), contactDamageAmount, true);
    }

#endif
    #endregion
}

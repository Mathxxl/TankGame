using UnityEngine;
using Utilities;

namespace Physics
{
    public class GravityDoor : LockedDoor
    {
        protected override void OpenDoor()
        {
            if (TryGetComponent(out Collider col))
            {
                col.isTrigger = true;
            }

            if (TryGetComponent(out Rigidbody rb))
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }
    }
}
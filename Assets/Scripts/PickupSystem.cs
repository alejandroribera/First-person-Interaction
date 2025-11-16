using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    public Transform cam;
    public Transform holder;
    public float maxDistance = 2f;
    public float minHoldDistance = 1f;
    public float maxHoldDistance = 3f;
    public float holdForce = 300f;

    Rigidbody heldObj;
    float holdDistance = 2f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryPickup();

        if (Input.GetMouseButtonUp(1))
            Drop();

        ScrollDistance();
    }

    void FixedUpdate()
    {
        if (heldObj)
            MoveHeldObject();
    }

    void TryPickup()
    {
        if (heldObj) return;

        Ray r = new Ray(cam.position, cam.forward);
        if (Physics.Raycast(r, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Pickup"))
            {
                heldObj = hit.collider.GetComponent<Rigidbody>();
                if (heldObj != null)
                {
                    // Desactiva gravedad y bloquea rotaci�n
                    heldObj.useGravity = false;
                    heldObj.constraints = RigidbodyConstraints.FreezeRotation;
                }
            }
        }
    }

    void MoveHeldObject()
    {
        Vector3 targetPos = holder.position + cam.forward * holdDistance;
        Vector3 dir = targetPos - heldObj.position;

        // Mueve el objeto suavemente hacia el holder
        heldObj.linearVelocity = dir * holdForce * Time.fixedDeltaTime;
    }

    void Drop()
    {
        if (!heldObj) return;

        // Reactiva la f�sica normal y desbloquea la rotaci�n
        heldObj.useGravity = true;
        heldObj.constraints = RigidbodyConstraints.None;

        heldObj = null;
    }

    void ScrollDistance()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        holdDistance = Mathf.Clamp(holdDistance + scroll, minHoldDistance, maxHoldDistance);
    }
}

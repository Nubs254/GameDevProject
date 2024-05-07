using UnityEngine;

public class Grappling_Gun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public Tutorial_GrapplingRope grappleRope;

    [Header("Layers Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 9;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = true;
    [SerializeField] private float maxDistnace = 20;

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;
    [SerializeField] PlayerMovement playerCreature;

    [HideInInspector] public Transform attachedPlatform; // Track the attached platform
    [HideInInspector] public Vector2 attachmentPoint; // Track the attachment point

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (grappleRope.enabled)
            {
                RotateGun(grapplePoint, false);
                UpdateGrapplePointOnMovingPlatform();
            }
            else
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }

            if (launchToPoint && grappleRope.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistnace;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = 1;
        }
        else
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos, true);
        }
    }
    void UpdateGrapplePointOnMovingPlatform()
{
    // Check if the grapple is attached to a moving platform
    if (attachedPlatform != null && attachedPlatform.GetComponent<Rigidbody2D>() != null)
    {
        // Get the velocity of the platform
        Vector2 platformVelocity = attachedPlatform.GetComponent<Rigidbody2D>().velocity;

        // Calculate the new position of the grapple point based on platform velocity
        Vector2 newGrapplePoint = grapplePoint + platformVelocity * Time.deltaTime;

        // Calculate the difference between the new position and the previous position
        Vector2 deltaGrapplePoint = newGrapplePoint - grapplePoint;

        // Update the grapple point
        grapplePoint = newGrapplePoint;

        // Update the attachment point
        attachmentPoint += deltaGrapplePoint;
    }
}
    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        // Get the position of the gunPivot in world space
        Vector3 gunPosition = gunPivot.position;

        // Get the direction vector from gunPivot to lookPoint
        Vector3 direction = lookPoint - gunPosition;

        // Correct the direction vector based on the parent's scale
        Vector3 correctedDirection = new Vector3(direction.x / Mathf.Abs(transform.lossyScale.x), direction.y / Mathf.Abs(transform.lossyScale.y), direction.z / Mathf.Abs(transform.lossyScale.z));

        // Calculate the angle based on the corrected direction vector
        float angle = Mathf.Atan2(correctedDirection.y, correctedDirection.x) * Mathf.Rad2Deg;

        // Adjust the angle if the x scale is negative
        if (transform.lossyScale.x < 0)
        {
            angle += 180f;
        }

        // Perform rotation based on conditions
        if (rotateOverTime && allowRotationOverTime)
        {
            // Smoothly rotate over time using Lerp
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            // Set the rotation immediately
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void SetGrapplePoint()
    {
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            if (_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
            {
                if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistnace || !hasMaxDistance)
                {
                    grapplePoint = _hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    grappleRope.enabled = true;

                    // Update attached platform and attachment point
                    attachedPlatform = _hit.transform;
                    attachmentPoint = _hit.point;
                }
            }
        }
    }

    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequncy;
        }
        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }

            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }
        else
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    m_springJoint2D.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    m_springJoint2D.distance = distanceVector.magnitude;
                    m_springJoint2D.frequency = launchSpeed;
                    m_springJoint2D.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    m_rigidbody.gravityScale = 0;
                    m_rigidbody.velocity = Vector2.zero;
                    break;
            }
        }
    }
}

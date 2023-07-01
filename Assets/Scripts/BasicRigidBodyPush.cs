using UnityEngine;

public class BasicRigidBodyPush : MonoBehaviour
{
    public bool myCanPush;
    public LayerMask myPushLayers;
    [Range(0.5f, 5f)] public float myStrength = 1.1f;

    private void OnControllerColliderHit(ControllerColliderHit theHit)
    {
        if (myCanPush) pushRigidBodies(theHit);
    }

    private void pushRigidBodies(ControllerColliderHit theHit)
    {
        // https://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html

        // make sure we hit a non kinematic rigidbody
        var Body = theHit.collider.attachedRigidbody;
        if (Body == null || Body.isKinematic) return;

        // make sure we only push desired layer(s)
        var BodyLayerMask = 1 << Body.gameObject.layer;
        if ((BodyLayerMask & myPushLayers.value) == 0) return;

        // We dont want to push objects below us
        if (theHit.moveDirection.y < -0.3f) return;

        // Calculate push direction from move direction, horizontal motion only
        Vector3 PushDir = new(theHit.moveDirection.x, 0.0f, theHit.moveDirection.z);

        // Apply the push and take strength into account
        Body.AddForce(PushDir * myStrength, ForceMode.Impulse);
    }
}
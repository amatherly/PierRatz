using UnityEngine;

public class PlayerMoveWithSphere : MonoBehaviour
{
    public Animator myAnimator;
    public float myOffset;
    public Transform myShpere;

    // Start is called before the first frame update
    private void start()
    {
    }

    // Update is called once per frame
    private void update()
    {
        transform.position = new Vector3(myShpere.position.x, myShpere.position.y + myOffset, myShpere.position.z);

        Vector3 MovementDirection = new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        var InputMagnitude = Mathf.Clamp01(MovementDirection.magnitude);
        myAnimator.SetFloat("InputMagnitude", InputMagnitude, 0.05f, Time.deltaTime);
    }
}
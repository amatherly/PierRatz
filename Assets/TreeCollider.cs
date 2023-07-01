using UnityEngine;

public class TreeCollider : MonoBehaviour
{
    // Start is called before the first frame update
    private void awake()
    {
        GetComponent<TerrainCollider>().enabled = false;

        GetComponent<TerrainCollider>().enabled = true;
    }

    // Update is called once per frame
    private void update()
    {
    }
}
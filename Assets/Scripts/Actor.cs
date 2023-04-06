using UnityEngine;

public class Actor : MonoBehaviour {
    public Vector3 position;
    public float bodySize = 0.5f;

    private void Update() {
        position = transform.position;
    }
}
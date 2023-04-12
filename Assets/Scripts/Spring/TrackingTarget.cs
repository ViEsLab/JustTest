using System;
using UnityEngine;

namespace Spring {
    public class TrackingTarget : MonoBehaviour {
        public Vector3 prePos;
        public Vector3 velocity;

        private void Update() {
            prePos = this.transform.position;

            Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            this.velocity = GetComponent<Rigidbody>().velocity = 5 * dir;
        }
    }
}
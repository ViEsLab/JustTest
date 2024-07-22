using System;
using UnityEngine;

namespace Spring {
    public class SpringTest : MonoBehaviour {
        public TrackingTarget target;
        public Vector3 prePos;

        private void Update() {
            Vector3 targetPos = target.transform.position;
            Vector3 curPos = this.transform.position;
            Vector3 v = target.velocity;

            // SpringInterpolation.TrackingSpringUpdate(
                // curPos, v, targetPos, 1f, Time.deltaTime,
                // out Vector3 posResult, out Vector3 velocityResult);

            float offset = Vector3.Distance(curPos, targetPos);
            Vector3 posResult = SpringInterpolation.DamperExactFast(
                curPos, targetPos, Mathf.Min(0.3f / offset, 0.05f), Time.deltaTime);

            this.transform.position = posResult;
            this.prePos = curPos;
        }
    }
}
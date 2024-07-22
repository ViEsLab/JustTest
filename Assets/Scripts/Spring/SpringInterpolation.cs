using UnityEngine;

namespace Spring {
    public static class SpringInterpolation {
        public static float FastNegExp(float num) {
            return 1.0f / (1.0f + num + 0.48f * num * num + 0.235f * num * num * num);
        }

        public static float Lerp(float origin, float target, float a) {
            return (1.0f - a) * origin + a * target;
        }

        public static Vector3 DamperExactStandard(
            Vector3 origin, Vector3 target, float halfLife, float timespan) {
            float x = Lerp(origin.x, target.x, 1.0f - Mathf.Pow(2, -timespan / halfLife));
            float y = Lerp(origin.y, target.y, 1.0f - Mathf.Pow(2, -timespan / halfLife));
            float z = Lerp(origin.z, target.z, 1.0f - Mathf.Pow(2, -timespan / halfLife));

            return new Vector3(x, y, z);
        }

        public static Vector3 DamperExactFast(
            Vector3 origin, Vector3 target, float halfLife, float timespan, float eps = 1e-5f) {
            float x = Lerp(origin.x, target.x,
                1.0f - FastNegExp((0.69314718056f * timespan) / (halfLife + eps)));
            float y = Lerp(origin.y, target.y,
                1.0f - FastNegExp((0.69314718056f * timespan) / (halfLife + eps)));
            float z = Lerp(origin.z, target.z,
                1.0f - FastNegExp((0.69314718056f * timespan) / (halfLife + eps)));

            return new Vector3(x, y, z);
        }

        public static void TrackingSpringUpdate(
            Vector3 x, Vector3 v,
            Vector3 xGoal, float xHalfLife,
            float dt, out Vector3 xResult, out Vector3 vResult) {
            v = DamperExactFast(v, (xGoal - x) / dt, xHalfLife, dt);

            vResult = v;
            xResult = x + dt * vResult;
        }

        public static Vector3 TrackingTargetAcceleration(
            Vector3 xNext, Vector3 xCurr, Vector3 xPrev, float timespan) {
            return (((xNext - xCurr) / timespan) - ((xCurr - xPrev) / timespan)) / timespan;
        }

        public static Vector3 TrackingTargetVelocity(Vector3 xNext, Vector3 xCurr, float timespan) {
            return (xNext - xCurr) / timespan;
        }
    }
}
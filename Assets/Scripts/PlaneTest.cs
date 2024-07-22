using System;
using UnityEngine;

namespace Spring {
    public class PlaneTest : MonoBehaviour {
        public GameObject fstPoint;
        public GameObject secPoint;
        public GameObject trdPoint;

        private void Update() {
            GetPlane(
                fstPoint.transform.position,
                secPoint.transform.position,
                trdPoint.transform.position,
                out float a, out float b, out float c, out float d);
            Debug.Log(new Vector3(a, b, c));
        }

        public void GetPlane(Vector3 fst, Vector3 sec, Vector3 trd,
            out float a, out float b, out float c, out float d) {
            //                   |   i     j     k   |
            // n = p1p2 x p1p3 = | x2-x1 y2-y1 z2-z1 | = a*i + b*j + c*k = (a, b, c)
            //                   | x3-x1 y3-y1 z3-z1 |
            a = (sec.y - fst.y) * (trd.z - fst.z) - (trd.y - fst.y) * (sec.z - fst.z);
            b = (sec.z - fst.z) * (trd.x - fst.x) - (trd.z - fst.z) * (sec.x - fst.x);
            c = (sec.x - fst.x) * (trd.y - fst.y) - (trd.x - fst.x) * (sec.y - fst.y);
            d = -(a * fst.x + b * fst.y + c * fst.z);
        }
    }
}
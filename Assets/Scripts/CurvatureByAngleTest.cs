using System;
using UnityEngine;

namespace Spring {
    public class CurvatureByAngleTest : MonoBehaviour {
        public GameObject startPoint;
        public GameObject endPoint;
        public GameObject curPoint;

        private void Update() {
            FuncFst();
        }

        private void FuncSnd() {
            var curPos = curPoint.transform.position;
            Vector3 fst = startPoint.transform.position - curPos;
            Vector3 snd = endPoint.transform.position - curPos;

            Vector3 foot = GetFootOfPerpendicular(
                curPos, startPoint.transform.position, endPoint.transform.position);
            Vector3 normalVec = foot - curPos;
        }

        private void FuncFst() {
            var curPos = curPoint.transform.position;
            Vector3 fst = startPoint.transform.position - curPos;
            Vector3 snd = endPoint.transform.position - curPos;

            Vector3 foot = GetFootOfPerpendicular(curPos, startPoint.transform.position, endPoint.transform.position);
            curPoint.transform.up = -foot;
        }

        private Vector3 GetFootOfPerpendicular(Vector3 pt, Vector3 begin, Vector3 end) {
            Vector3 retVal = Vector3.zero;

            float dx = begin.x - end.x;
            float dy = begin.y - end.y;
            float dz = begin.z - end.z;
            if (Mathf.Abs(dx) < float.Epsilon &&
                Mathf.Abs(dy) < float.Epsilon &&
                Mathf.Abs(dz) < float.Epsilon) {
                retVal = begin;
                return retVal;
            }

            float u = (pt.x - begin.x) * (begin.x - end.x) +
                      (pt.y - begin.y) * (begin.y - end.y) +
                      (pt.z - begin.z) * (begin.z - end.z);
            u = u / (dx * dx + dy * dy + dz * dz);

            retVal.x = begin.x + u * dx;
            retVal.y = begin.y + u * dy;
            retVal.y = begin.z + u * dz;

            return retVal;

        }
    }
}
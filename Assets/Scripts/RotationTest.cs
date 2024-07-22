using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour {
    public LineRenderer controlPointVec;
    public GameObject endPointGo;
    public LineRenderer endPointVec;
    public LineRenderer realResult;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {
        endPointVec.SetPosition(1, endPointGo.transform.position.normalized);

        RotationByNewCoordinateSystem();
    }

    public void RotationByNewCoordinateSystem() {
        // 建立新坐标系
        Vector3 newForward = endPointGo.transform.position - this.transform.position;
        Vector3 newRight = Vector3.zero;
        newRight = Mathf.Abs(Vector3.Dot(newForward, Vector3.up) - 1) <= float.Epsilon ?
            Vector3.right :
            Vector3.Cross(Vector3.up, newForward).normalized;
        Vector3 newUp = Vector3.Cross(newForward, newRight).normalized;

        // 根据新坐标系在各个基向量上做偏移
        Vector3 oriPos = controlPointVec.GetPosition(1);
        Vector3 newPos = this.transform.position +
                         oriPos.x * newRight +
                         oriPos.y * newUp +
                         oriPos.z * newForward;
        realResult.SetPosition(1, newPos);
    }

    public Quaternion FromToRotation(Vector3 dir1, Vector3 dir2) {
        float r = 1f + Vector3.Dot(dir1, dir2);
        Vector3 w;

        if(r < 1E-6f) {
            r = 0f;
            w = Mathf.Abs(dir1.x) > Mathf.Abs(dir1.z) ?
                new Vector3(-dir1.y, dir1.x, 0f) : new Vector3(0f, -dir1.z, dir1.y);
        } else {
            w = Vector3.Cross(dir1, dir2);
        }

        return new Quaternion(w.x, w.y, w.z, r).normalized;
    }
}
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

        // NewRotationFunc();
        OldRotationFunc();
    }

    public void NewRotationFunc() {
        Vector3 oriPos = controlPointVec.GetPosition(1);
        float dis = Vector3.Distance(oriPos, Vector3.zero);
        Quaternion turn = Quaternion.identity;
            turn = FromToRotation(Vector3.forward, endPointGo.transform.position.normalized);
        realResult.SetPosition(1, dis * (turn * oriPos.normalized));
    }

    public void OldRotationFunc() {
        Vector3 oriPos = controlPointVec.GetPosition(1);
        float dis = Vector3.Distance(oriPos, Vector3.zero);
        Quaternion turn = FromToRotation(Vector3.forward, oriPos.normalized);
        realResult.SetPosition(1, dis * (turn * endPointGo.transform.position.normalized));
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
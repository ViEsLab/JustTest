using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Spring;
using UnityEngine;
using UnityEngine.Serialization;

public class TestObj {
    public int test;
}

public class Test : MonoBehaviour {
    public GameObject lookAtTarget;
    public GameObject signedAngleTest_mainGo;
    public GameObject signedAngleTest_viceGo;
    public Actor actor;
    public Transform getChildTestTrans;
    public TestObj nullObj;

    public ComponentAccessTarget componentTargetTarget;
    public int componentTestFrame = 120;

    void Start() {
        EqualsTest();
    }

    void Update() {
    }

    private void EqualsTest() {
        float floatTest = 1;
        long longTest = 1;
        int intTest = 1;
        Debug.Log(floatTest.Equals(longTest));
        // Warning: "Suspicious comparison: there is no type in the solution which is inherited from both 'long' and 'float'"
        Debug.Log(longTest.Equals(floatTest));
        Debug.Log(longTest.Equals(intTest));

        // How to judge an object is value type
        Type type = floatTest.GetType();
        if (type.IsValueType) {
            // ...
        }
    }

    private void ArrayAddTest() {
        int[] test = Array.Empty<int>();
        test[0] = 0;
        test[1] = 1;
    }

    private void RecycleObjectUpdate() {
        Debug.Log(componentTargetTarget == null);
        Debug.Log(object.ReferenceEquals(componentTargetTarget, null));
        componentTargetTarget?.gameObject.SetActive(false);
        // Debug.Log(nullObj.test);
    }

    private void RecycleObjectStart() {
        Debug.Log(componentTargetTarget == null);
        Destroy(componentTargetTarget.gameObject);
    }

    private void ComponentAccessTest() {
        if (componentTestFrame < 0) {
            Debug.Log(componentTargetTarget.test);
        }

        if (componentTestFrame == 60) {
            Destroy(componentTargetTarget.gameObject);
            Debug.Log("[ViE] 销毁！");
        }

        componentTestFrame--;
    }

    private void GetChildTest() {
        int count = getChildTestTrans.childCount;
        for (int i = 0; i < count; i++) {
            Transform curChild = getChildTestTrans.GetChild(i);
            if (curChild.name.EndsWith("@Skin")) {
                Debug.Log(curChild.name);
                break;
            }
        }
    }

    private void NumberTest() {
        Debug.Log(Convert.ToString(Int32.MaxValue, 2));
        Debug.Log(Convert.ToString(-1, 2));
    }

    private void NegativeToBinary() {
        long test = long.MaxValue;
        Debug.Log(Convert.ToString(test, 2));
        test >>= 7;
        Debug.Log(Convert.ToString(test, 2));
        test = ~test;
        Debug.Log(Convert.ToString(test, 2));
    }

    private void IpParseTest() {
        if (IPAddress.TryParse("viehere.fun", out IPAddress address)) {
            Debug.Log($"[ViE] {address}");
        } else {
            Debug.Log($"不合法");
        }
        Debug.Log("-----------------");
        if (IPAddress.TryParse("10.30.30.71", out address)) {
            Debug.Log($"[ViE] {address}");
        } else {
            Debug.Log($"不合法");
        }
        Debug.Log("-----------------");
        if (IPAddress.TryParse("39b1:1efb:396b:d4dd:cf77:a9e6:990b:888", out address)) {
            Debug.Log($"[ViE] {address}");
        } else {
            Debug.Log($"不合法");
        }
    }

    private void StringDefaultTest() {
        string test = default;
        Debug.Log(test);
    }

    private void InternTest() {
        string foobar0 = "foobar";
        string foobar1 = new StringBuilder().Append("foo").Append("bar").ToString();
        string foobar2 = string.Intern(foobar1);
        string foobar3 = new StringBuilder().Append("f").Append("oo").Append("b").Append("ar").ToString();
        string foobar4 = string.Intern(foobar3);
        string foobar5 = "foobar";

        Debug.Log(foobar0 == foobar1);   // True
        Debug.Log(foobar0 == foobar2);   // True
        Debug.Log(foobar0 == foobar3);   // True
        Debug.Log(foobar0 == foobar4);   // True
        Debug.Log(foobar0 == foobar5);   // True
        Debug.Log(System.Object.ReferenceEquals(foobar0, foobar1)); // False
        Debug.Log(System.Object.ReferenceEquals(foobar0, foobar2)); // True
        Debug.Log(System.Object.ReferenceEquals(foobar0, foobar3)); // False
        Debug.Log(System.Object.ReferenceEquals(foobar0, foobar4)); // True
        Debug.Log(System.Object.ReferenceEquals(foobar0, foobar5)); // True
    }

    private void TimeTest() {
        Debug.Log(DateTime.Now);
    }

    // 是否处于指定角度的扇形范围内
    private bool CheckInSectorRangeOfDirection(Vector3 position, Actor target, float range,
        float angle, float dir = 0) {

        angle = angle * 0.5f;
        Vector3 dirBase = target.position - position;
        Vector3 forward = Quaternion.Euler(0, dir, 0) * Vector3.forward;
        float curAngle = Vector3.Angle(forward, dirBase.normalized);


        float curDis = GetPointToTargetDis(position, target);
        if (curDis <= range) {
            if (curAngle <= angle) {
                return true;
            } else {
                // 起点到圆心向量的角度超出range，检查圆形与扇形相交
                Vector3 pos1 = GetPosByDirAndDis(position, dir - angle, range);
                Vector3 pos2 = GetPosByDirAndDis(position, dir + angle, range);
                bool isInArcEdge = GetPointToTargetDis(pos1, target) <= 0 ||
                                   GetPointToTargetDis(pos2, target) <= 0;

                Vector3 po = (target.position - position).normalized;
                Vector3 pf = (pos1 - position).normalized;
                Vector3 fo = (target.position - pos1).normalized;
                Vector3 ps = (pos2 - position).normalized;
                Vector3 so = (target.position - pos2).normalized;
                bool isCircleCanInsideFst = GetPointToSegmentDistance(target.position, pos1, position) < target.bodySize;
                bool isCircleCanInsideSnd = GetPointToSegmentDistance(target.position, pos2, position) < target.bodySize;
                bool isInStraightEdge =
                    (isCircleCanInsideFst && Vector3.Dot(po, pf) > 0 == Vector3.Dot(-pf, fo) > 0) ||
                    (isCircleCanInsideSnd && Vector3.Dot(po, ps) > 0 == Vector3.Dot(so, -ps) > 0);

                return isInStraightEdge || isInArcEdge;
            }
        }

        return false;
    }

    public float GetPointToSegmentDistance(Vector3 point, Vector3 startPos, Vector3 endPos) {
        Vector3 sp = point - startPos;
        Vector3 se = endPos - startPos;

        Vector3 projection = Vector3.Project(sp, se);
        Vector3 verSegment = Vector3.zero;
        if (projection.normalized == se.normalized) {
            verSegment = sp - projection;
        } else {
            Vector3 ep = point - endPos;
            verSegment = sp.magnitude > ep.magnitude ? ep : sp;
        }

        return verSegment.magnitude;
    }

    public Vector3 GetPosByDirAndDis(Vector3 oriPos, float angle, float length) {
        Vector3 forward = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        return oriPos + forward * length;
    }

    public float GetPointToTargetDis(Vector3 point, Actor target) {
        float dis = Vector3.Distance(point, target.position);
        return dis - target.bodySize;
    }

    public void SignedAngleTest() {
        Vector3 mainVec = signedAngleTest_mainGo.transform.position.normalized;
        Vector3 viceVec = signedAngleTest_viceGo.transform.position.normalized;
        float angle = Vector3.SignedAngle(mainVec, viceVec, Vector3.up);
        Debug.Log(angle);
    }

    public void ArrayTest() {
        string[] testArray = new string[] {"a", "b", "c", null, null};
        Debug.Log(testArray.Length);
    }

    public void EnvironmentVariablesTest() {
        string dosFilePath = @"%CP_BATTLE_OG_DIR%";
        // string dosFilePath = @"%TEMP%\具体的Log文件.txt";
        IDictionary result = System.Environment.GetEnvironmentVariables();
        Debug.Log(System.Environment.ExpandEnvironmentVariables(dosFilePath));
    }

    public void GuidTest() {
        long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() / 1000;
        string logId = Guid.NewGuid().ToString();
        logId = timestamp + logId.Replace("-", string.Empty);
        Debug.Log(logId);
    }

    public void FileExistsTest() {
        string rootPath = Directory.GetParent(Application.dataPath).FullName;
        string path = Path.Combine(rootPath, "ViE.txt");
        if (File.Exists(path)) {
            Debug.Log("喜多！喜多！");
        } else {
            Debug.Log("郁代！郁代！");
        }
    }

    public void TimestampTest() {
        long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() / 1000;
        DateTime startDt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        DateTime dt = startDt.AddSeconds(timestamp);
        Debug.Log(dt.ToString("yyyy-MM-dd-hh:mm"));
    }

    public void TestListCopy() {
        List<int> test1 = new List<int>() {1, 3, 4};
        List<int> test2 = new List<int>(test1);
        Debug.Log(test2);
    }

    public void TestLookAt() {
        this.transform.LookAt(lookAtTarget.transform.position);
    }

    [Flags]
    public enum TestEnum1 {
        fst = 1 << 0,
        snd = 1 << 1,
        trd = 1 << 2,
        _4th = 1 << 3,
    }

    public void TestEnumFlag() {
        TestEnum1 e = TestEnum1.fst | TestEnum1.snd | TestEnum1.trd;
        Debug.Log(e & TestEnum1.fst);
        Debug.Log(e & TestEnum1.snd);
        Debug.Log(e & (TestEnum1.snd | TestEnum1.trd));
        Debug.Log(e & (TestEnum1.fst | TestEnum1.snd));
        Debug.Log(e & (TestEnum1.fst | TestEnum1._4th));
        Debug.Log(e & TestEnum1._4th);

        Debug.Log((e & TestEnum1.fst) != 0);
        Debug.Log((e & TestEnum1.snd) != 0);
        Debug.Log((e & (TestEnum1.snd | TestEnum1.trd)) != 0);
        Debug.Log((e & (TestEnum1.fst | TestEnum1.snd) ) != 0);
        Debug.Log((e & (TestEnum1.fst | TestEnum1._4th) ) != 0);
        Debug.Log((e & TestEnum1._4th) != 0);
    }

    public void TestInsertList() {
        int level = 3;
        List<int> shields = new List<int>(){5, 5, 3, 2};
        int insertIndex = 0;
        for (int i = 0; i < shields.Count; i++) {
            if (shields[i] <= level) {
                break;
            }
            insertIndex += 1;
        }

        shields.Insert(insertIndex, level);
        Debug.Log(shields);
    }

    public void TestTypeConversion() {
        object test0 = 1;
        Debug.Log((int) test0);

        object test1 = 1;
        Debug.Log((long) test1);

        object test2 = 99999999999;
        Debug.Log((long) test2);

        int test3 = 1;
        Debug.Log((long) test3);

        object test4 = 1;
        Debug.Log((byte) test4);
    }
}
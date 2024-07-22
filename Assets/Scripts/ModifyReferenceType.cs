using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class ModifyReferenceType : MonoBehaviour
{
    private MyReferenceType myReferenceType;

    private void Start()
    {
        MyReferenceType myReferenceType = new MyReferenceType();
        myReferenceType.myValue = 1;

        MyJob myJob = new MyJob();
        myJob.myReferenceTypePtr = new NativeReference<MyReferenceType>(this.myReferenceType, new AllocatorManager.AllocatorHandle());

        JobHandle jobHandle = myJob.Schedule();

        jobHandle.Complete();

        Debug.Log("Main thread value: " + myReferenceType.myValue);  // 2
    }

    private struct MyReferenceType
    {
        public int myValue;
    }

    private struct MyJob : IJob
    {
        public NativeReference<MyReferenceType> myReferenceTypePtr;

        public void Execute()
        {
            MyReferenceType myReferenceType = myReferenceTypePtr.Value;
            myReferenceType.myValue += 1;
            myReferenceTypePtr.Value = myReferenceType;
        }
    }
}
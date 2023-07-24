using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComponent : MonoBehaviour {
    private bool hasAwake = false;
    private bool hasStarted = false;
    private bool hasUpdated = false;
    private bool hasLateUpdated = false;
    private bool hasFixedUpdated = false;

    private void Awake() {
        if (!hasAwake) {
            Debug.Log($"{Time.frameCount}  Test Comp Awake");
        }
        hasAwake = true;
    }

    void Start() {
        if (!hasStarted) {
            Debug.Log($"{Time.frameCount}  Test Comp Start");
        }
        hasStarted = true;
    }

    void Update() {
        if (!hasUpdated) {
            Debug.Log($"{Time.frameCount}  Test Comp Update");
        }
        hasUpdated = true;
    }

    private void LateUpdate() {
        if (!hasLateUpdated) {
            Debug.Log($"{Time.frameCount}  Test Comp LateUpdate");
        }
        hasLateUpdated = true;
    }

    private void FixedUpdate() {
        if (!hasFixedUpdated) {
            Debug.Log($"{Time.frameCount}  Test Comp FixedUpdate");
            gameObject.AddComponent<TestComponent2>();
        }
        hasFixedUpdated = true;
    }
}
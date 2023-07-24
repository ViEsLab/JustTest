﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor {
    public static class NetCodeTool {
        [MenuItem("ViE Test/Test Window")]
        private static void Open() {
            EditorWindow window = EditorWindow.GetWindow<TestWindow>("TestWindow");
            window.minSize = new Vector2(440, 240);
        }
    }

    public class TestWindow : EditorWindow {
        private static int nextWindowId;
        private List<int> subWindowKeyList = new List<int>();
        private List<Rect> subWindowRectList = new List<Rect>();
        private int windowCount = 0;

        private void OnEnable() {
            nextWindowId = 0;
            subWindowKeyList.Clear();
            subWindowRectList.Clear();
            windowCount = 0;
        }

        private void OnDestroy() {
            subWindowKeyList.Clear();
            subWindowRectList.Clear();
        }

        private void OnGUI() {
            BeginWindows();
            for (int i = 0; i < windowCount; i++) {
                subWindowRectList[i] = GUILayout.Window(subWindowKeyList[i], subWindowRectList[i], Yeah, $"ViE{subWindowKeyList[i]}");
            }
            EndWindows();

            if (GUILayout.Button("添加一个窗口（无意义，备用）")) {
                AddWindow();
            }
        }

        private void Yeah(int windowId) {
            if (GUILayout.Button("测试")) {
                Debug.Log($"Yeah{windowId}");
            }
            if (GUILayout.Button("关闭")) {
                RemoveWindow(windowId);
            }

            GUI.DragWindow();
        }

        private void AddWindow() {
            subWindowKeyList.Add(nextWindowId++);
            subWindowRectList.Add(new Rect(20, 20, 240, 50));
            windowCount++;
        }

        private void RemoveWindow(int windowId) {
            int index = subWindowKeyList.IndexOf(windowId);
            if (index >= 0) {
                subWindowKeyList.Remove(windowId);
                subWindowRectList.RemoveAt(index);
                windowCount--;
            } else {
                Debug.Log($"{windowId}移除错误！");
            }
        }
    }
}
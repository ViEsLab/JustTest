using System.Collections.Generic;
using UnityEngine;

namespace Campfire.Battle.Visual {
    public class BSpline {
        public List<Vector3> spline { get; private set; }
        public int resolution;
        public List<Vector3> controlPoints;
        private int k;
        private int[] nodeVector;
        private float segmentStep;
        private Vector3[] fragments;
        private int controlPointsCount = 0;

        public BSpline(int k, int resolution, List<Vector3> controlPoints) {
            this.controlPoints = controlPoints;
            this.resolution = resolution;
            this.k = k;

            controlPointsCount = this.controlPoints.Count;

            if (k > controlPointsCount) {
                this.k = controlPointsCount;
            }

            nodeVector = new int[k + controlPointsCount];
            fragments = new Vector3[k];
            UpdateParam();
        }

        public void UpdateParam() {
            segmentStep = (float)controlPointsCount / this.resolution;
            int nodeNumber = 0;
            for (int i = 0, count = nodeVector.Length; i < count; i++) {
                if (i > k - 1 && i < controlPointsCount + 1) {
                    nodeNumber++;
                }
                nodeVector[i] = nodeNumber;
            }
        }

        public Vector3 CreateCurvePointByDirectRate(float rate) {
            return CreateBSpline(rate * nodeVector[controlPointsCount]);
        }

        public void WholeGenerate() {
            spline = new List<Vector3>(50);
            UpdateParam();

            if (nodeVector[k - 1] >= nodeVector[controlPointsCount]) {
                spline.Add(controlPoints[0]);
            }
            for (float u = nodeVector[k - 1]; u < nodeVector[controlPointsCount]; u += segmentStep) {
                spline.Add(CreateBSpline(u));
            }
            spline.Add(controlPoints[^1]);
        }

        public void FragmentGenerate(float rate) {
            spline = new List<Vector3>(50);
            UpdateParam();

            if (rate > 1) {
                rate = 1;
            } else if (rate < nodeVector[k - 1]) {
                rate = nodeVector[k - 1];
            }

            if (nodeVector[k - 1] >= nodeVector[controlPointsCount]) {
                spline.Add(controlPoints[0]);
            }
            for (float u = nodeVector[k - 1]; u < rate; u += segmentStep) {
                spline.Add(CreateBSpline(u));
            }
            spline.Add(controlPoints[^1]);
        }

        private Vector3 CreateBSpline(float rate) {
            int step = 0;

            // 确定片段起点
            for (int i = k; rate > nodeVector[i]; i++, step++);
            // 抽出控制点
            for (int i = 0; i < k; i++) {
                fragments[i] = controlPoints[step + i];
            }
            // 从高阶开始，便于将控制点直接纳入计算
            for (int j = k; j > 1; j--) {
                // 遍历参与插值的点
                for (int i = 0; i < k - 1; i++) {
                    Vector3 left = fragments[i];
                    Vector3 right = fragments[i + 1];

                    int min = nodeVector[step + i + 1];
                    int max = nodeVector[step + j + i];

                    float leftFactor = (max - rate) / (max - min);
                    float rightFactor = (rate - min) / (max - min);

                    fragments[i] = leftFactor * left + rightFactor * right;
                }
                step++;
            }

            return fragments[0];
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceTest : MonoBehaviour  {
    public RenderTexture lowRT;
    public int pointCount = 0;

    private static double _r2a1;
    private static double _r2a2;

    void Start() {
       double g = 1.32471795724474602596;

       _r2a1 = 1.0 / g;
       _r2a2 = 1.0 / (g * g);

    }

    void Update() {
       DrawPoints();
    }

    private void DrawPoints() {
        Texture2D tex = new Texture2D(lowRT.width, lowRT.height);
        RenderTexture.active = lowRT;

        for (int index = 0; index < pointCount; index++) {
            Vector2 result = R2Distribution(index);
            result = new Vector2(result.x * lowRT.width, result.y * lowRT.height);

            for (int j = 0, xCount = 3; j < xCount; j++) {
                for (int k = 0, yCount = 3; k < yCount; k++) {
                    tex.SetPixel((int)result.x + j, (int)result.y + k, Color.black);
                }
            }
        }

        tex.Apply();
        Graphics.Blit(tex, lowRT);
        RenderTexture.active = null;
    }

    private Vector2 R2Distribution(int index) {
        float x = (float)((0.5 + _r2a1 * index) % 1);
        float y = (float)((0.5 + _r2a2 * index) % 1);

        return new Vector2(x, y);
    }

    private static float HaltonSequence(int index, int b) {
        float res = 0f;
        float f = 1f / b;

        int i = index;

        while (i > 0) {
            res = res + f * (i % b);
            i = Mathf.FloorToInt(i / b);
            f = f / b;
        }

        return res;
    }
}
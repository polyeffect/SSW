using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetVanishingPoint : MonoBehaviour
{
    private float panSpeed = 0.15f;
    private float panLimit = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.ResetProjectionMatrix();
        StartCoroutine(PanMove());
    }

    IEnumerator PanMove()
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * panSpeed;
            print(t);
            float x = Mathf.Lerp(panLimit, -panLimit, t);
            SetVanishingPoint(Camera.main, new Vector2(x, 0.0f));
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetVanishingPoint(Camera cam, Vector2 perspectiveOffset)
    {
        Matrix4x4 m = cam.projectionMatrix;
        float w = 2 * cam.nearClipPlane / m.m00;
        float h = 2 * cam.nearClipPlane / m.m11;

        float left = -w / 2 - perspectiveOffset.x;
        float right = left + w;
        float bottom = -h / 2 - perspectiveOffset.y;
        float top = bottom + h;

        cam.projectionMatrix = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);
    }


    static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;
        return m;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To draw spheres along Bezier Curve on editor for better visualization
/// of path which the nexus follow
/// </summary>

public class Path : MonoBehaviour
{

    public Transform[] pathPoints;

    /// <summary>
    /// draw spheres along Bezier Curve on editor for better understanding
    /// of path
    /// </summary>
    private void OnDrawGizmos() {
        Vector3 gizmosPos;
        //show curve on editor
        for (float t = 0; t <= 1; t += 0.015f) {
            //https://en.wikipedia.org/wiki/B%C3%A9zier_curve
            gizmosPos = getPosInPath(t);

            //draw a point at every t along the bezier curve
            Gizmos.DrawSphere(gizmosPos, 0.5f);
        }

        //draw a sphere on last point
        Gizmos.DrawSphere(pathPoints[3].position, 0.5f);
    }

    /// <summary>
    /// get the next position in a Bezier Curve. This method
    /// follow an equation in https://en.wikipedia.org/wiki/B%C3%A9zier_curve 
    /// (Quadratic Bézier curves)
    /// </summary>
    /// <param name="t"></param> t is precentage along the curve [0,1]
    /// <param name="path"></param> path consists of 4 points that makes the curve
    /// <returns></returns>
    private Vector3 getPosInPath(float t) {
        //https://en.wikipedia.org/wiki/B%C3%A9zier_curve (Quadratic Bézier curves)
        Vector3 pos = Mathf.Pow(1 - t, 3) * pathPoints[0].position
                        + 3 * Mathf.Pow(1 - t, 2) * t * pathPoints[1].position
                        + 3 * (1 - t) * Mathf.Pow(t, 2) * pathPoints[2].position
                        + Mathf.Pow(t, 3) * pathPoints[3].position;
        return pos;
    }
}

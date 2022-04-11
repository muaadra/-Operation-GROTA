using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// to sort transforms based on distance to a given transform
/// </summary>
class TransformDistComparer : IComparer<Transform> {
    
    Transform myTr;//the main transform
    
    public TransformDistComparer(Transform tr) {
        myTr = tr;
    }

    public int Compare(Transform x, Transform y) {
        if (Vector3.Distance(myTr.position, x.position) < Vector3.Distance(myTr.position, y.position)) {
            return -1;
        }else if (Vector3.Distance(myTr.position, x.position) > Vector3.Distance(myTr.position, y.position)) {
            return 1;
        }
        return 0;
    }
}
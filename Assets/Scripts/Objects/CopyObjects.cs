using System.Collections.Generic;
using UnityEngine;

public class CopyObjects {

    /// <summary>
    /// Overloaded version with GameObject parameters instead of Transform ones.
    /// </summary>
    /// <param name="src">The source transform</param>
    /// <param name="dst">The destination transform</param>
    /// <param name="objects">The list of objects to create copies of</param>
    public static void Copy(GameObject src, GameObject dst, List<GameObject> objects) {
        List<Transform> objTransforms = objects.ConvertAll<Transform>(o => o.transform);
        Copy(src.transform, dst.transform, objTransforms);
    }

    /// <summary>
    /// Creates copies of objects from src to dst.
    ///
    /// The objects can be thought of being children of src, and then making a copy
    /// of them and moving them to dst. Both src and dst need to exist before calling
    /// this function.
    /// </summary>
    /// <param name="src">The source transform</param>
    /// <param name="dst">The destination transform</param>
    /// <param name="objects">The list of objects to create copies of</param>
    public static void Copy(Transform src, Transform dst, List<Transform> objects) {
        if (src == null || dst == null || (objects?.Count ?? 0) == 0) {
            return;
        }

        foreach (Transform obj in objects) {
            Vector3 pos = dst.position + (obj.position - src.position);
            float rotDelta = Quaternion.Angle(src.rotation, dst.rotation);

            GameObject instance = GameObject.Instantiate(obj.gameObject, pos, obj.rotation);
            instance.transform.RotateAround(dst.position, dst.up, rotDelta);
        }
    }
}

using System;
using UnityEngine;

namespace SmartDataViewer.Helpers
{
    public class ObjectHelper
    {
        /// <summary>
        /// Simple test for an object being visible. We test the field of view and the view distance.
        /// Future version will check for objects blocking the view.
        /// </summary>
        /// <param name="rPosition"></param>
        /// <param name="rForward"></param>
        /// <param name="rFOV"></param>
        /// <param name="rDistance"></param>
        /// <param name="rTarget"></param>
        /// <returns></returns>
        public static float IsObjectVisible(Vector3 rPosition, Vector3 rForward, float rFOV, float rDistance, Transform rTarget)
        {
            // First, test if the target is in the field of view
            Vector3 lTargetPosition = rTarget.transform.position;

            // Test each object to ensure that it
            float lAngle = NumberHelper.GetHorizontalAngle(rForward, lTargetPosition - rPosition);
            if (Mathf.Abs(lAngle) < rFOV * 0.5f)
            {
                float lDistance = Vector3.Distance(rPosition, lTargetPosition);
                if (lDistance <= rDistance)
                {
                    return lDistance;
                }
            }

            // If we got here, the target isn't in our FOV or is blocked
            return 0f;
        }

        /// <summary>
        /// Simple test for an object being visible. We test the field of view and the view distance.
        /// Future version will check for objects blocking the view.
        /// 
        /// This version allows us to search for any object on a specific layer. If requested, we will
        /// return the closest one.
        /// </summary>
        /// <param name="rPosition"></param>
        /// <param name="rForward"></param>
        /// <param name="rFOV"></param>
        /// <param name="rDistance"></param>
        /// <param name="rTargetLayerMask"></param>
        /// <param name="rClosest"></param>
        /// <returns></returns>
        public static GameObject IsObjectVisible(Vector3 rPosition, Vector3 rForward, float rFOV, float rDistance, LayerMask rTargetLayerMask, bool rClosest)
        {
            GameObject lClosestObject = null;

            // Grab all the object in a sphere around the center
            Collider[] lColliders = UnityEngine.Physics.OverlapSphere(rPosition, rDistance, rTargetLayerMask);
            if (lColliders != null)
            {
                // If we don't carea bout the closest one, just return the first one
                if (!rClosest) { return lColliders[0].gameObject; }

                // Test each of the objects to find the closest (with in the field of view
                float lClosestDistance = float.MaxValue;
                for (int i = 0; i < lColliders.Length; ++i)
                {
                    GameObject lTargetObject = lColliders[i].gameObject;
                    if (lTargetObject != null)
                    {
                        // Test each object to ensure that it
                        float lDistance = IsObjectVisible(rPosition, rForward, rFOV, rDistance, lTargetObject.transform);
                        if (lDistance > 0f && lDistance < lClosestDistance)
                        {
                            lClosestDistance = lDistance;
                            lClosestObject = lTargetObject;
                        }
                    }
                }
            }

            return lClosestObject;
        }
    }
}

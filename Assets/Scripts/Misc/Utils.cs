using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Misc
{
    public class Utils
    {
        /*Start of singleton declaration*/
        private static Utils s_instance = null;
        public static Utils Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new Utils();
                return s_instance;
            }
        }
        private Utils() { }
        /*End of singleton declaration*/

        public Vector3 VectorToEulerAngle(Vector3 vector)
        {
            return new Vector3(
                Mathf.Atan2(Mathf.Sqrt(vector.y * vector.y + vector.z * vector.z), vector.y),
                Mathf.Atan2(Mathf.Sqrt(vector.z * vector.z + vector.x * vector.x), vector.y),
                Mathf.Atan2(Mathf.Sqrt(vector.x* vector.x+vector.y*vector.y),vector.z));
        }
        //public Vector3 EulerAngleToVector(Vector3 eulerAngle,Vector3 forward)
        //{
        //    (Quaternion.Euler(eulerAngle)*forward);
        //    return new Vector3(Mathf.Cos(eulerAngle.x);
        //}

        public Vector3 Limit(Vector3 vector, float maxLength)
        {
            float length = vector.magnitude;
            if (length > maxLength)
            {
                vector = vector.normalized * maxLength;
            }
            return vector;
        }
        public Vector3 YawPitchToVector(float yaw, float pitch)
        {
            return new Vector3(Mathf.Cos(yaw) * Mathf.Cos(pitch), Mathf.Sin(yaw) * Mathf.Cos(pitch),Mathf.Sin(pitch));
        }

        public delegate void WaitForSecondsThenCallback();
        public void WaitForSecondsThen(MonoBehaviour mono, float time,WaitForSecondsThenCallback callback)
        {
            mono.StartCoroutine(waitForSecondsThen(time,callback));
        }
        private IEnumerator waitForSecondsThen(float time, WaitForSecondsThenCallback callback)
        {
            yield return new WaitForSeconds(time);
            if(callback!=null)
                callback();
        }
    }

}

using UnityEngine;

namespace core
{
    public class CameraScript : MonoBehaviour
    {
        public GameObject target;

        public Camera mainCamera;
        
        //TODO add smoothly transform with inertial
        private void FixedUpdate()
        {
            var targetTransform = target.GetComponent<Transform>();
            var targetPosition = targetTransform.position;
            
            mainCamera.transform.position = new Vector3(targetPosition.x, targetPosition.y, -1);
        }
    }
}
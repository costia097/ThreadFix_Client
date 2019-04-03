using UnityEngine;

namespace core
{
    public class CameraShake : MonoBehaviour
    {
        private Vector3 _cameraInitPosition;
        
        public float shakeMagnitude = 0.05f;
        
        public float shakeTime = 0.5f;

        public Camera mainCamera;

        public void ShakeIt()
        {
            _cameraInitPosition = mainCamera.transform.position;
            InvokeRepeating("StartCameraShaking", 0, 0.005f);
            Invoke("StopCameraShaking", shakeTime);
        }

        private void StartCameraShaking()
        {
            var cameraShakingOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
            var cameraShakingOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;

            var mainCameraTransform = mainCamera.transform;
            var cameraIntermediatePosition = mainCameraTransform.position;

            cameraIntermediatePosition.x += cameraShakingOffsetX;
            cameraIntermediatePosition.y += cameraShakingOffsetY;

            mainCameraTransform.position = cameraIntermediatePosition;
        }

        private void StopCameraShaking()
        {
            CancelInvoke("StartCameraShaking");
            mainCamera.transform.position = _cameraInitPosition;
        }
    }
}
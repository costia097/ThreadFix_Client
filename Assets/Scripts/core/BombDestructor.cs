using UnityEngine;

namespace core
{
    public class BombDestructor: MonoBehaviour
    {

        /*
         * trigger on animation event
         */
        // ReSharper disable once UnusedMember.Local
        private void DoDestruct()
        {
            var cameraGameObject = GameObject.Find("Camera");
            cameraGameObject.GetComponent<CameraShake>().ShakeIt();
            Destroy(gameObject);
        }
    }
}
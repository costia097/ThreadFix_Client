using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace core
{
    public class GroundDestructor : MonoBehaviour
    {
        /*
         * trigger from animator event
         */
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void DoDestruct()
        {
            Destroy(gameObject);
            var cameraGameObject = GameObject.Find("Camera");
            cameraGameObject.GetComponent<CameraShake>().ShakeIt();
            GetComponent<ParticleGroundSystem>().GenerateParticles();
        }
    }
}
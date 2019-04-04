using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace core
{
    public class ParticleGroundSystem : MonoBehaviour
    {
        private static List<Transform> _particiants = new List<Transform>();

        private void Start()
        {
            InvokeRepeating("DoClean", 5.0f, 15.0f);
        }

        public void GenerateParticles()
        {
            var groundGrassElementTransform = GetComponent<Transform>();
            
            var particle = Resources.Load<GameObject>("Prefabs/particleOfGrassGround");

            var groundGrassElementPosition = groundGrassElementTransform.position;

            GenerateParticleWithGivenForce(new Vector2(-100, 0), particle,
                new Vector2(groundGrassElementPosition.x - 0.3f, groundGrassElementPosition.y + 0.1f));
            GenerateParticleWithGivenForce(new Vector2(-100, 0), particle,
                new Vector2(groundGrassElementPosition.x - 0.3f, groundGrassElementPosition.y));
            GenerateParticleWithGivenForce(new Vector2(-100, 0), particle,
                new Vector2(groundGrassElementPosition.x - 0.3f, groundGrassElementPosition.y - 0.1f));

            GenerateParticleWithGivenForce(new Vector2(0, 100), particle,
                new Vector2(groundGrassElementPosition.x - 0.3f, groundGrassElementPosition.y+ 0.3f));
            GenerateParticleWithGivenForce(new Vector2(0, 100), particle,
                new Vector2(groundGrassElementPosition.x, groundGrassElementPosition.y + 0.3f));
            GenerateParticleWithGivenForce(new Vector2(0, 100), particle,
                new Vector2(groundGrassElementPosition.x + 0.3f, groundGrassElementPosition.y + 0.3f));
            
            GenerateParticleWithGivenForce(new Vector2(100, 0), particle,
                new Vector2(groundGrassElementPosition.x, groundGrassElementPosition.y + 0.3f));
            GenerateParticleWithGivenForce(new Vector2(100, 0), particle,
                new Vector2(groundGrassElementPosition.x, groundGrassElementPosition.y + 0.3f));
            GenerateParticleWithGivenForce(new Vector2(100, 0), particle,
                new Vector2(groundGrassElementPosition.x, groundGrassElementPosition.y + 0.3f));
        }

        //TODO 
        private void DoClean()
        {
            _particiants.RemoveAll(e =>
            {
                var isReadyToDelete = e.position.y < -100;
                
                if (isReadyToDelete)
                {
                    Destroy(e.gameObject);
                }
                return isReadyToDelete;
            });
        }

        private static void GenerateParticleWithGivenForce(Vector2 force, GameObject particle, Vector2  groundGrassElementPosition)
        {
            var particleInstantiated = Instantiate(particle, new Vector2(groundGrassElementPosition.x, groundGrassElementPosition.y),
                Quaternion.identity);
            var particleRigidbody2D = particleInstantiated.GetComponent<Rigidbody2D>();

            particleInstantiated.name = particleInstantiated.gameObject.GetHashCode().ToString();

            var transform = particleInstantiated.GetComponent<Transform>();
            
            _particiants.Add(transform);

            particleRigidbody2D.AddForce(force);
        }
    }
}
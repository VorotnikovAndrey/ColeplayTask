using UnityEngine;

namespace Units.Other
{
    public class SmoothRotator : MonoBehaviour
    {
        [SerializeField] private Vector3 rotationAxis = new(0, 1, 0);
        [SerializeField] private float rotationSpeed = 20f;

        private void Update()
        {
            transform.Rotate(rotationAxis * (rotationSpeed * Time.deltaTime));
        }
    }
}
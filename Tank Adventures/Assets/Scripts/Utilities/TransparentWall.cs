using System;
using UnityEngine;

namespace Utilities
{
    public class TransparentWall : MonoBehaviour
    {
        [SerializeField] private MeshRenderer wallRenderer;
        [SerializeField] private EntityDetector detector;
        [SerializeField] private float goalAlpha = 0.2f;

        
        
        private void OnEnable()
        {
            if(detector == null) return;

            detector.OnEntityDetected += Detected;
            detector.OnEntityLeft += Exited;
        }

        private void OnDisable()
        {
            if(detector == null) return;

            detector.OnEntityDetected -= Detected;
            detector.OnEntityLeft -= Exited;
        }

        private void Detected(Transform entity)
        {
            Debug.Log("Detected");
            SetAlpha(wallRenderer.material, goalAlpha);
        }

        private void Exited(Transform entity)
        {
            Debug.Log("Left");
            SetAlpha(wallRenderer.material, 1);
        }

        private static void SetAlpha(Material mat, float value)
        {
            var subColor = mat.color;
            subColor.a = value;
            mat.color = subColor;
        }
    }
}
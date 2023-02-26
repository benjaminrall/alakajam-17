using System;
using System.Linq;
using UnityEngine;

namespace Menu
{
    public class MenuWaveManager : Singleton<MenuWaveManager>
    {
        [Serializable]
        public class Wave
        {
            public float speed;
            public float xOffset;
            public float xLength;
            public float xAmplitude;
            public float zOffset;
            public float zLength;
            public float zAmplitude;

            public void IncreaseOffset(float deltaOffset)
            {
                xOffset += deltaOffset;
                zOffset += deltaOffset;
            }
        }
        
        private MeshFilter _meshFilter;

        public Wave[] waves;

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }

        private void Update()
        {
            foreach (Wave w in waves)
            {
                w.IncreaseOffset(Time.deltaTime * w.speed);
            }
            
            UpdateVertices();
        }

        public float GetWaveHeight(float x, float z)
        {
            return waves.Sum(w =>
                (w.xLength > 0 ? w.xAmplitude * Mathf.Sin(x / w.xLength + w.xOffset) : 0) +
                (w.zLength > 0 ? w.zAmplitude * Mathf.Sin(z / w.zLength + w.zOffset) : 0));
        }

        private void UpdateVertices()
        {
            Vector3[] vertices = _meshFilter.mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].y = Instance.GetWaveHeight(vertices[i].x + transform.position.x,
                    vertices[i].z + transform.position.z);
            }

            _meshFilter.mesh.vertices = vertices;
            _meshFilter.mesh.RecalculateNormals();
        }
    }
}

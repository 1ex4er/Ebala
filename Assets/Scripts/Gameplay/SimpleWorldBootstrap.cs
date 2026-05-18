using UnityEngine;

namespace Ebala.Gameplay
{
    public class SimpleWorldBootstrap : MonoBehaviour
    {
        [SerializeField] private Vector2Int worldSize = new Vector2Int(30, 30);
        [SerializeField] private float tileScale = 1f;
        [SerializeField] private Material groundMaterial;

        private void Awake()
        {
            BuildGround();
            BuildLight();
        }

        private void BuildGround()
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(worldSize.x * 0.1f * tileScale, 1f, worldSize.y * 0.1f * tileScale);

            if (groundMaterial != null)
            {
                ground.GetComponent<MeshRenderer>().material = groundMaterial;
            }

            var collider = ground.GetComponent<MeshCollider>();
            collider.sharedMesh = ground.GetComponent<MeshFilter>().sharedMesh;

            for (var i = 0; i < 12; i++)
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = $"Obstacle_{i}";
                cube.transform.position = new Vector3(Random.Range(-12, 12), 0.5f, Random.Range(-12, 12));
                cube.transform.localScale = new Vector3(Random.Range(1f, 3f), Random.Range(1f, 4f), Random.Range(1f, 3f));
            }
        }

        private static void BuildLight()
        {
            var sun = new GameObject("Sun");
            var light = sun.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.1f;
            sun.transform.rotation = Quaternion.Euler(45f, -35f, 0f);
        }
    }
}

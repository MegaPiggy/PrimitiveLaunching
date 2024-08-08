using OWML.Common;
using OWML.ModHelper;
using UnityEngine;
using System.Collections.Generic;

namespace PrimitiveLaunching
{
    public class PrimitiveLaunching : ModBehaviour
    {
        public IMeteorLaunching MeteorLaunching { get; set; }

        private void Start()
        {
            MeteorLaunching = ModHelper.Interaction.GetModApi<IMeteorLaunching>("12090113.MeteorLaunching");
            MeteorLaunching.GetProjectilesInitializedEvent().AddListener(AddProjectiles);
            MeteorLaunching.GetProjectileLaunchedEvent().AddListener(MeteorLaunched);
        }

        private List<int> indexes = new List<int>();

        private void AddProjectiles()
        {
            indexes.Add(MeteorLaunching.AddProjectile(CreatePrimitive(PrimitiveType.Sphere)));
            indexes.Add(MeteorLaunching.AddProjectile(CreatePrimitive(PrimitiveType.Cube)));
            indexes.Add(MeteorLaunching.AddProjectile(CreatePrimitive(PrimitiveType.Cylinder)));
            indexes.Add(MeteorLaunching.AddProjectile(CreatePrimitive(PrimitiveType.Capsule)));
            indexes.Add(MeteorLaunching.AddProjectile(CreatePrimitive(PrimitiveType.Plane)));
            indexes.Add(MeteorLaunching.AddProjectile(CreatePrimitive(PrimitiveType.Quad)));
        }

        private void MeteorLaunched(int p, GameObject newMeteor)
        {
            if (indexes.Contains(p)) OnPrimitiveLaunched(newMeteor);
        }

        private void OnPrimitiveLaunched(GameObject primitive)
        {
            Transform launcher = MeteorLaunching.GetLauncher();
            float launchSize = MeteorLaunching.GetLaunchSize();
            primitive.transform.position = launcher.position + launcher.forward * launchSize * 2;
            primitive.transform.rotation = launcher.rotation;
        }

        private GameObject CreatePrimitive(PrimitiveType primitiveType)
        {
            GameObject main = new GameObject(primitiveType.ToString());
            OWRigidbody owrigid = main.AddComponent<OWRigidbody>();
            GameObject primitive = GameObject.CreatePrimitive(primitiveType);
            primitive.transform.parent = main.transform;
            GameObject Detector = new GameObject("Detector");
            Detector.layer = LayerMask.NameToLayer("AdvancedDetector");
            Detector.transform.parent = main.transform;
            switch (primitiveType)
            {
                case PrimitiveType.Cylinder:
                case PrimitiveType.Capsule:
                    Detector.AddComponent<CapsuleCollider>().height = 2;
                    break;
                case PrimitiveType.Cube:
                    Detector.AddComponent<BoxCollider>();
                    break;
                case PrimitiveType.Sphere:
                    Detector.AddComponent<SphereCollider>();
                    break;
                case PrimitiveType.Plane:
                case PrimitiveType.Quad:
                default:
                    MeshCollider mc = Detector.AddComponent<MeshCollider>();
                    mc.sharedMesh = primitive.GetComponent<MeshCollider>().sharedMesh;
                    mc.convex = true;
                    break;
            }
            Detector.AddComponent<OWCollider>();
            owrigid.RegisterAttachedForceDetector(Detector.AddComponent<DynamicForceDetector>());
            owrigid.RegisterAttachedFluidDetector(Detector.AddComponent<DynamicFluidDetector>());
            return main;
        }
    }
}

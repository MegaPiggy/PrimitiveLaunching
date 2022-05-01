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
            GameObject sphere = CreatePrimitive(PrimitiveType.Sphere);
            GameObject cube = CreatePrimitive(PrimitiveType.Cube);
            GameObject cylinder = CreatePrimitive(PrimitiveType.Cylinder);
            GameObject capsule = CreatePrimitive(PrimitiveType.Capsule);
            indexes.Add(MeteorLaunching.AddProjectile(sphere));
            indexes.Add(MeteorLaunching.AddProjectile(cube));
            indexes.Add(MeteorLaunching.AddProjectile(sphere));
            indexes.Add(MeteorLaunching.AddProjectile(capsule));
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
            if (!(primitiveType == PrimitiveType.Cube || primitiveType == PrimitiveType.Sphere || primitiveType == PrimitiveType.Cylinder || primitiveType == PrimitiveType.Capsule)) return null;
            GameObject main = new GameObject(primitiveType.ToString());
            OWRigidbody owrigid = main.AddComponent<OWRigidbody>();
            GameObject primitive = GameObject.CreatePrimitive(primitiveType);
            primitive.transform.parent = main.transform;
            GameObject Detector = new GameObject("Detector");
            Detector.transform.parent = main.transform;
            DynamicFluidDetector fluid = Detector.AddComponent<DynamicFluidDetector>();
            DynamicForceDetector force = Detector.AddComponent<DynamicForceDetector>();
            owrigid._attachedForceDetector = force;
            owrigid._attachedFluidDetector = fluid;
            switch (primitiveType)
            {
                case PrimitiveType.Cylinder:
                case PrimitiveType.Capsule:
                    Detector.AddComponent<CapsuleCollider>().height = 2;
                    Detector.AddComponent<OWCapsuleCollider>();
                    break;
                case PrimitiveType.Cube:
                    Detector.AddComponent<BoxCollider>();
                    Detector.AddComponent<OWCustomCollider>();
                    break;
                case PrimitiveType.Sphere:
                    Detector.AddComponent<SphereCollider>();
                    Detector.AddComponent<OWCustomCollider>();
                    break;
                default:
                    break;
            }
            return main;
        }
    }
}

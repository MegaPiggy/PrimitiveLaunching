using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PrimitiveLaunching
{
    public interface IMeteorLaunching
    {
        public Transform GetLauncher();

        public float GetLaunchSpeed();

        public float GetLaunchSize();

        public SimpleFluidVolume GetSunFluid();

        public GameObject[] GetProjectiles();

        public int AddProjectile(GameObject projectile);

        public bool IsInitialized();

        public bool IsLateInitialized();

        public bool IsProjectilesInitialized();

        public int GetSelectedProjectileIndex();

        public GameObject LaunchProjectile();

        public void SwitchProjectile();

        public void SwitchToProjectile(int index);

        public UnityEvent GetProjectilesInitializedEvent();

        public UnityEvent<int, GameObject> GetProjectileLaunchedEvent();

        public UnityEvent<int> GetProjectileSwitchedEvent();
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Projectiles.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Projectiles.Projectiles
{
	public class MachineGunProjectile : IProjectile
	{
		protected Material material;
		public Material GetMaterial { get { return material; } }

		private Mesh mesh;
		public Mesh GetMesh { get { return mesh; } }

		public MachineGunProjectile()
		{
			material = Resources.Load<Material>("Materials/MachineGunBullet");
			//TODO once meshes are made replace with direct reference to mesh.
			GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Meshes/Sphere")) as GameObject;
			mesh = go.GetComponent<MeshFilter>().mesh;
		}
	}
}

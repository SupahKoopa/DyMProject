﻿using System;
using Assets.Scripts.Collision.IntersectionTests;
using Assets.Scripts.CollisionBoxes.ThreeD;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Collision.SweepTests
{
	public class Sweep
	{
		AABBIntersection aabbIntersection = new AABBIntersection();

		private List<AABB3D> intervalHalvedRects = new List<AABB3D>();
		public void ResetRectangles()
		{
			intervalHalvedRects.Clear();
		}

		private AABB3D b = new AABB3D();
		public bool TestMovingAABB(AABB3D b0, Vector3 d, float time0, float time1,
			AABB3D b1, ref float time)
		{
			float mid = (time0 + time1) * 0.5f;
			float midTest = mid - time0;
			Vector3 adjustedD = d * mid;
			//Debug.Log("Time tested: " + centerTest);
			b.Center = b0.Center + adjustedD;
			b.HalfWidths[0] = midTest * d.magnitude + b0.HalfWidth;
			b.HalfWidths[1] = midTest * d.magnitude + b0.HalfHeight;
			b.HalfWidths[2] = midTest * d.magnitude + b0.HalfDepth;
			//b = new AABB3D(b0.Center + adjustedD, midTest * d.magnitude
			//	+ (b0.HalfWidth * 2), midTest * d.magnitude
			//	+ (b0.HalfHeight * 2), midTest * d.magnitude
			//	+ (b0.HalfDepth * 2));
			//Debug.Log(b);

			//intervalHalvedRects.Add(b);
			//b0.DrawBoundingBox(Color.green);
			//foreach (var rects in intervalHalvedRects)
			//{
			//	rects.DrawBoundingBox(Color.red);
			//}
			if (!aabbIntersection.Intersect(b, b1))
				return false;


			if (time1 - time0 < 0.1f)
			{
				normalCollision(b, b0, b1);
				time = time0;
				return true;
			}

			if (TestMovingAABB(b0, d, time0, mid, b1, ref time))
				return true;

			return TestMovingAABB(b0, d, mid, time1, b1, ref time);
		}

		private float wy;
		private float hx;
		private void normalCollision(AABB3D movingBox, AABB3D b0, AABB3D b1)
		{
			resetNormals(b0);

			wy = (movingBox.HalfWidth * 2 + b1.HalfWidth * 2) * (movingBox.Center.y - b1.Center.y);
			hx = (movingBox.HalfHeight * 2 + b1.HalfHeight * 2) * (movingBox.Center.x - b1.Center.x);

			if (wy > hx)
			{
				if (wy > -hx && b1.IgnoreCollision != IgnoreCollision.Y_AXIS)
				{
					b0.NormalCollision[1] = Vector3.down;
				}
				else if(b1.IgnoreCollision != IgnoreCollision.X_AXIS)
				{
					b0.NormalCollision[0] = Vector3.right;
				}
			}
			else
			{
				if (wy > -hx && b1.IgnoreCollision != IgnoreCollision.X_AXIS)
				{
					b0.NormalCollision[0] = Vector3.left;
				}
				else if(b1.IgnoreCollision != IgnoreCollision.Y_AXIS)
				{
					b0.NormalCollision[1] = Vector3.up;
				}
			}
		}

		private void resetNormals(AABB3D checkedObject)
		{
			for (int i = 0; i < checkedObject.NormalCollision.Length; i++)
			{
				checkedObject.NormalCollision[i] = Vector3.zero;
			}
		}
	}
}
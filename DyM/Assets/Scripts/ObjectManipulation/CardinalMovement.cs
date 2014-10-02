﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.ObjectManipulation.Interfaces;
using Assets.Scripts.Utilities;
using UnityEngine;
using Physics = UnityEngine.Physics;

namespace Assets.Scripts.ObjectManipulation
{
	public class CardinalMovement : ICardinalMovement
	{
		private Vector3 position;
	    private Vector3 velocity;
	    private Vector3 acceleration;

	    private Vector3 jumpVelocity;
	    private Vector3 jumpAcceleration = new Vector3(0f, .3f, 0f);
		private bool falling;
		public bool Falling
		{
			get { return falling; }
		}

		public CardinalMovement()
		{
		}

		public Vector3 Move(float stickInput, Vector3 acceleration, float time)
		{
		    velocity = PhysicsFuncts.calculateVelocity(acceleration,stickInput);
			return velocity;
		}

		public Vector3 Jump(bool pressed, float playerPos)
		{
		  
			return jumpVelocity;
		}
		
	}
}

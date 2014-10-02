﻿using System;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.ObjectManipulation;
using Assets.Scripts.ObjectManipulation.Interfaces;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

//TODO add dependencyInjection

namespace Assets.Scripts.GameObjects
{
	public class Player : MonoBehaviour
	{
		private IPlaneShift planeShift;
		[Inject]
		private PlaneShiftFactory factory;

		[Inject]
		private ICardinalMovement cardinalMovement;
		[Inject]
		public ICharacter character;

		[Inject]
		public IPooledGameObjects PooledBUlletGameObjects;
		public static GameObject GunModel;
		private GameObject bullet;

		private bool planeShiftUp = false;
		private bool planeShiftdown = false;
        private Vector3 acceleration = new Vector3(1f,0f,0f);

		void Start()
		{
			planeShift = factory.Create(transform.position);
			GunModel = GameObject.Find("Gun");
			PooledBUlletGameObjects.Initialize();
		}
		// Update is called once per frame
		void FixedUpdate ()
		{
			//Debug.Log("Left thumbstick position: " + Input.GetAxisRaw("Horizontal"));
			if(planeShiftdown)
			{
				transform.Translate(planeShift.ShiftPlane(KeyCode.Joystick1Button4, 
					transform.position));
				planeShiftdown = false;
			}
			else if(planeShiftUp)
			{
				transform.Translate(planeShift.ShiftPlane(KeyCode.Joystick1Button5,
					transform.position));
				planeShiftUp = false;
			}
            
			transform.Translate(planeShift.Dodge(transform.position, dodgeKeysToCheck(), Time.deltaTime));
			transform.Translate(cardinalMovement.Move(Input.GetAxis("Horizontal"), acceleration, Time.deltaTime));
		
		}

		void Update()
		{
			if (Input.GetButtonDown("PlaneShiftDown"))
			{
				planeShiftdown = true;
				planeShiftUp = false;
			}
			else if (Input.GetButtonDown("PlaneShiftUp"))
			{
				planeShiftUp = true;
				planeShiftdown = false;
			}

			if (Input.GetAxis("Fire1") > 0f && character.EquippedWeapon())
			{
				PooledBUlletGameObjects.GetPooledBullet().GetComponent<Bullet>().Projectile = 
					character.Weapon.Fire();
			}

			if (Input.GetButtonDown("ActivateAbility") && character.EquippedAbility())
			{
				character.Ability.Activate(character);
				Debug.Log("StatusEffect is: " + character.StatusEffect);
			}
				
		}

		private bool dodgeKeysToCheck()
		{
			if (Input.GetButton("PlaneShiftDown") || Input.GetButton("PlaneShiftUp"))
				return true;
			else
			{
				return false;
			}
		}
	} 
}

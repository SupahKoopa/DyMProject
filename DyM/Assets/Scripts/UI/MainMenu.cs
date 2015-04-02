﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
	private enum Menus
	{
		MAIN_MENU,
		OPTIONS,
		CREDITS,
		CONTROLS
	};

	private Menus menus;

    public Texture2D logo;

	public Texture2D controls;

    public GUISkin skin;
	public GUISkin Label;

    private Menus GUIMenu = Menus.MAIN_MENU;

    public float musicValue;
    public float soundValue;

    public GameObject musicManager;
    public GameObject soundManager;

	bool[] mainMenuSelections = new bool[5] {false, false, false, false, false};
	bool[] optionMenuSelections = new bool[3] {false, false, false};
	private bool backButton;
	private bool keyPressed;

	string[] mainMenuTexts = new string[5] { "Start", "Options", "controls", "credits", "exit" };
	string[] optionMenuTexts = new string[3] { "Back", "Music", "Sound"};
	string[] miscTexts = new string[1] { "Back"};

	private Dictionary<Menus, string[]> currentMenu = new Dictionary<Menus, string[]>(); 

	private int selected = 0;

	private MenuSounds menuSounds;

    void Start()
    {
		currentMenu.Add(Menus.MAIN_MENU, mainMenuTexts);
		currentMenu.Add(Menus.OPTIONS, optionMenuTexts);
		currentMenu.Add(Menus.CREDITS, miscTexts);
		currentMenu.Add(Menus.CONTROLS, miscTexts);

        GUIMenu = Menus.MAIN_MENU;

        PlayerPrefs.SetFloat("MusicVolume", 1.00f);
        musicValue = PlayerPrefs.GetFloat("MusicVolume");

        PlayerPrefs.SetFloat("SoundVolume", 1.00f);
        soundValue = PlayerPrefs.GetFloat("SoundVolume");

		menuSounds = GameObject.Find("MainMenuSounds").GetComponent<MenuSounds>();
    }

	private int previousSelected;
	void Update()
	{
		selected = menuSelection(currentMenu[GUIMenu], selected);
		if (previousSelected != selected)
			navigationSound();
		previousSelected = selected;

		if (Input.GetButtonDown("Jump"))
			keyPressed = true;
	}

    void OnGUI()
    {
		GUI.skin = skin;
		GUI.BeginGroup(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 350, 615, 550));

	    switch (GUIMenu)
	    {
		    case Menus.MAIN_MENU:
				MainMenuScreen();
			    break;
		    case Menus.OPTIONS:
				//OptionsScreen();
			    break;
		    case Menus.CREDITS:
				Credits();
			    break;
		    case Menus.CONTROLS:
				Controls();
			    break;
		    default:
			    throw new ArgumentOutOfRangeException();
	    }

		GUI.EndGroup();
    }

    void MainMenuScreen()
    {
        GUI.Label(new Rect(0, 0, logo.width, logo.height), logo);
	    startMultiFocusArea(mainMenuSelections, mainMenuTexts);

		SetMenuSelectionTrue(mainMenuSelections);

		if (mainMenuSelections[0])
		{
			selection();
			resetSelected();
			mainMenuSelections[0] = false;
			Application.LoadLevel("Level_01");
		}

        if (mainMenuSelections[1])
        {
            selection();
            resetSelected();
            mainMenuSelections[1] = false;
            GUIMenu = Menus.OPTIONS;
        }

		if (mainMenuSelections[2])
		{
			selection();
			resetSelected();
			mainMenuSelections[2] = false;
			GUIMenu = Menus.CONTROLS;
		}

		if (mainMenuSelections[3])
		{
			selection();
			resetSelected();
			mainMenuSelections[3] = false;
			GUIMenu = Menus.CREDITS;
		}

		if (mainMenuSelections[4])
		{
			selection();
			Application.Quit();
		}

		endMultiFocusArea(mainMenuTexts);
    }

	private void selection()
	{
		if(!menuSounds.AudioSources[0].isPlaying)
			menuSounds.AudioSources[0].Play();
	}

	void startMultiFocusArea(bool[] menuSelection, string[] texts)
	{
		for (int i = 0; i < texts.Length; i++)
		{
			GUI.SetNextControlName(texts[i]);
			menuSelection[i] = GUI.Button(new Rect(225, 210 + (60 * i), 150, 50), texts[i]);
		}
	}

	void endMultiFocusArea(string[] texts)
	{
		GUI.FocusControl(texts[selected]);
	}

	private void resetSelected()
	{
		selected = 0;
	}

	private void SetMenuSelectionTrue(bool[] selection)
	{
		if (keyPressed)
		{
			selection[selected] = true;
			keyPressed = false;
		}
	}

	private void navigationSound()
	{
		if(!menuSounds.AudioSources[2].isPlaying)
			menuSounds.AudioSources[2].Play();
	}

	private void SetMenuSelectionTrue(ref bool selection)
	{
		if (keyPressed)
		{
			selection = true;
			keyPressed = false;
		}
	}

	private float timer;
	float input;
	private int menuSelection(string[] buttons, int selected)
	{
		bool inputCheck = ScrollUpThroughMenu();

		if (inputCheck)
			input = Input.GetAxis("Vertical");

		if (input < 0f)
		{
			if (selected == 0)
				selected = buttons.Length - 1;
			else
				selected -= 1;
		}
		else if (input > 0f)
		{
			if (selected == buttons.Length - 1)
				selected = 0;
			else
				selected += 1;
		}
			
		return selected;
	}

	private bool ScrollUpThroughMenu()
	{
		bool inputCheck = false;
		if (Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f &&
			(Input.GetAxis("Horizontal") < 0.1f && Input.GetAxis("Horizontal") > -0.1f))
		{
			if (timer > 0.5f)
			{
				timer = 0f;
			}

			if (Util.compareEachFloat(timer, 0.0f))
			{
				inputCheck = true;
			}
			else
			{
				input = 0f;
			}

			timer += Time.deltaTime;
		}
		
		if (Input.GetAxis("Vertical") < 0.1f && Input.GetAxis("Vertical") > -0.1f)
		{
			input = 0f;
			timer = 0f;
			inputCheck = false;
		}
		return inputCheck;
	}

    void OptionsScreen()
    {
        GUI.Label(new Rect(0, 0, logo.width, logo.height), logo);

		startMultiFocusArea(optionMenuSelections, optionMenuTexts, Label);
		SetMenuSelectionTrue(optionMenuSelections);

		if(optionMenuSelections[0])
		{
			goBackSound();
			returnToMainMenu();
		}
		if(selected == 1)
			musicValue = AdjustSlider(musicValue);
		else if (selected == 2)
			soundValue = AdjustSlider(soundValue);

		exitCurrentMenu();
     
        musicValue = GUI.HorizontalSlider(new Rect(210, 235, 200, 20), musicValue, 0.0F, 1.00F);
        PlayerPrefs.SetFloat("MusicVolume", musicValue);

	
        //GUI.Label(new Rect(185, 210, 254, 47), "Music", skin.button);

		soundValue = GUI.HorizontalSlider(new Rect(210, 300, 200, 20), soundValue, 0.0F, 1.00F);
        PlayerPrefs.SetFloat("SoundVolume", soundValue);

        //GUI.Label(new Rect(185, 275, 254, 47), "Sound");

		endMultiFocusArea(optionMenuTexts);
    }

	void startMultiFocusArea(bool[] menuSelection, string[] texts, GUISkin skin)
	{
		for (int i = 0; i < texts.Length; i++)
		{
			GUI.SetNextControlName(texts[i]);
			if(i > 0)
				menuSelection[i] = GUI.Button(new Rect(235, 150 + (60 * i), 150, 50), texts[i], skin.label);
			else
				menuSelection[i] = GUI.Button(new Rect(225, 450, 150, 50), texts[i]);
		}
	}

	private void exitCurrentMenu()
	{
		if (backButton)
		{
			goBackSound();
			resetSelected();
			resetBackButton();
			returnToMainMenu();
		}
	}

	private void goBackSound()
	{
		if(!menuSounds.AudioSources[1].isPlaying)
			menuSounds.AudioSources[1].Play();
	}

	void startMultiFocusArea(string[] texts)
	{
		GUI.SetNextControlName(texts[0]);
		backButton = GUI.Button(new Rect(225, 450, 150, 50), texts[0]);
	}

	private void returnToMainMenu()
	{
		GUIMenu = Menus.MAIN_MENU;
	}

	private void resetBackButton()
	{
		backButton = false;
	}

	private float AdjustSlider(float valueToAdjust)
	{
		if (Input.GetAxis("Horizontal") < 0.0f || Input.GetAxis("Horizontal") > 0.0f)
			valueToAdjust += (Input.GetAxis("Horizontal") / 100f);

		if (valueToAdjust > 1f)
			valueToAdjust = 1f;
		else if (valueToAdjust < 0f)
			valueToAdjust = 0f;

		return valueToAdjust;
	}

	void Credits()
    {
        GUI.Label(new Rect(0, 0, logo.width, logo.height), logo);

		GUI.Label(new Rect(125, 225, 350, 150), " Alex Lueck - Lead Design");
		GUI.Label(new Rect(53, 243, 500, 150), " Michael Lojkovic - Lead Programmer");
		GUI.Label(new Rect(42, 261, 500, 150), " Justin Terry - Programmer");
		GUI.Label(new Rect(0, 279, 500, 150), " Patric Harmon - Artist");
		GUI.Label(new Rect(18, 297, 500, 150), " Geoff Lightbourn - Sound Liason");
		GUI.Label(new Rect(28, 315, 500, 150), " Kanoa Doblin - Composer");
		GUI.Label(new Rect(8, 333, 500, 150), " Jessica Borlovan - UI Scripting");

		startMultiFocusArea(miscTexts);
		SetMenuSelectionTrue(ref backButton);

		exitCurrentMenu();

		endMultiFocusArea(miscTexts);
    }

	void Controls()
	{
		GUI.Label(new Rect(0, 0, logo.width, logo.height), logo);

		GUI.Label(new Rect(0, 215, controls.width, controls.height), controls);
		GUI.Label(new Rect(350, 275, 250, 200), "To Plane Shift and Stay on the plane hold the Plane Shift Button down. To Dodge tap the plane Shift Button.");

		startMultiFocusArea(miscTexts);
		SetMenuSelectionTrue(ref backButton);

		exitCurrentMenu();

		endMultiFocusArea(miscTexts);
	}
}

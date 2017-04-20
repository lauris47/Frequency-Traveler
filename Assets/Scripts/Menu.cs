﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{


    public Texture2D startGameIcon, resumeGame, restartGame, optionsButton;
    public Texture2D menuLogo;
    public Texture2D livesIconFull, livesIconEmpty;
    private Texture2D[] liveIcons;
    float opacity = 0;
    public GameObject overScreen;
    bool gameRestarted;
    int maxLives;
    Vector3 eventCollisionPoint;
    Camera camera;
    int initialEventFontSize, tempEventFontSize;
    float eventFontTransparency = 1;
    bool inOptions;

    float waveBlurSlider;
    float waveSpeedSlider;
    float waveIntensitySLider;

    void Start()
    {
        liveIcons = new Texture2D[2];
        overScreen = Instantiate(overScreen, new Vector3(0, 0, -2.39f), Quaternion.identity) as GameObject;
        overScreen.transform.localRotation = new Quaternion(0, 90, -90, 0);
        overScreen.GetComponent<Renderer>().material.color = new Vector4(0.5f, 0, 0, 0);
        camera = FindObjectOfType<Camera>();
        initialEventFontSize = 25 * Screen.width / 1920;
        tempEventFontSize = initialEventFontSize / 10;

        waveBlurSlider = Wave._waveBlur;
        waveSpeedSlider = Wave._waveSpeed;
        waveIntensitySLider = Wave._waveIntensity;
    }

    void OnGUI()
    {
        //Create my style
        //Collision event font
        GUIStyle eventStyle = new GUIStyle(GUI.skin.label);
        GUIStyle FTStyle = new GUIStyle(GUI.skin.label);
        GUIStyle gameOverText1 = new GUIStyle(GUI.skin.label);
        GUIStyle gameOverText2 = new GUIStyle(GUI.skin.label);
        Font gameFont = (Font)Resources.Load("Fonts/OstrichRegular", typeof(Font));
        Font endGameFont = (Font)Resources.Load("Fonts/HelveticaNeueLTCom-XBlkCn", typeof(Font));

        eventStyle.font = (Font)Resources.Load("Fonts/OstrichRegular", typeof(Font)); ;

        FTStyle.font = gameFont;
        FTStyle.fontSize = 50 * Screen.width / 2560;
        FTStyle.normal.textColor = new Color32(0x18, 0x34, 0x41, 0xFF); //#386678 - BLUE; ffb502 - Yellow

        gameOverText1.font = endGameFont;
        gameOverText2.font = endGameFont;
        gameOverText2.normal.textColor = new Color32(0xff, 0x52, 0x2a, 0xFF); // Red ff3000 // ff522a
        gameOverText1.normal.textColor = new Color32(0xff, 0xc3, 0x8e, 0xFF); // Light yellow ffc38e
        gameOverText1.alignment = TextAnchor.MiddleCenter;
        gameOverText2.alignment = TextAnchor.MiddleCenter;
        gameOverText1.fontSize = 60 * Screen.width / 1920;
        gameOverText2.fontSize = 80 * Screen.width / 1920;

        // has to be above Color.clear
        if (inOptions)
        {
            //Wave blur
            GUI.Label(new Rect(Screen.width / 2 - Screen.width / 5 / 2, Screen.height / 2, Screen.width / 4, Screen.height/10), "Wave Blur", FTStyle);
            waveBlurSlider = GUI.HorizontalSlider(new Rect(Screen.width / 2 - Screen.width / 5 / 2, Screen.height / 2, Screen.width / 5, 5), waveBlurSlider, 0, 100);



            //Set stats for wave
            Wave._waveBlur = (int)waveBlurSlider;
            Wave._waveSpeed = waveSpeedSlider;
            Wave._waveIntensity = (int)waveIntensitySLider;
        }

        GUI.backgroundColor = Color.clear;





        //Main Menu if not playing
        if (GameManager.instance.isPlaying == false)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - (Screen.width - Screen.width / 8) / 2, menuLogo.height / 4, Screen.width - Screen.width / 8, Screen.height/ 3), menuLogo);

            //if game never started
            if (!GameManager.instance.gameStartedOnce)
            {
                
                if (GUI.Button(new Rect(Screen.width / 2 - startGameIcon.width / 4, Screen.height / 2 + startGameIcon.height * 0.2f, startGameIcon.width / 2, startGameIcon.height / 2), startGameIcon) || Input.GetKeyDown(KeyCode.Return))
                {
                    GameManager.instance.StartGame();
                }
            }
            // if game started once
            else
            {
                //if not in options window
                if (!inOptions)
                {
                    // Resume button
                    if (GUI.Button(new Rect(Screen.width / 2 - resumeGame.width / 4, Screen.height / 2 - restartGame.height / 2, resumeGame.width / 2, resumeGame.height / 2), resumeGame) || Input.GetKeyDown(KeyCode.Return))
                    {
                        GameManager.instance.StartGame(); ;
                    }
                    //Restart button
                    if (GUI.Button(new Rect(Screen.width / 2 - restartGame.width / 4, Screen.height / 2, restartGame.width / 2, restartGame.height / 2), restartGame))
                    {
                        GameManager.instance.RestartGame();
                        GameManager.instance.isPlaying = true;
                    }
                    // Options button and its functions
                    if (GUI.Button(new Rect(Screen.width / 2 - optionsButton.width / 4, Screen.height / 2 + optionsButton.height / 2, optionsButton.width / 2, optionsButton.height / 2), optionsButton))
                    {
                        inOptions = true;
                    }
                }

                //disable options screen on ESC
                if (Input.GetKey(KeyCode.Escape))
                {
                    inOptions = false;
                }

            }
            fadeInScreen(1);
        }

        //In Game
        if (GameManager.instance.isPlaying == true)
        {
            if (!GameManager.instance.gameOver)
            {
                GUI.Label(new Rect(Screen.width / 25, Screen.height / 50, 300, 50), "SCORE: " + GameManager.instance.getScore().ToString(), FTStyle);

                if (GameManager.instance.getLives() > maxLives)
                {
                    maxLives = GameManager.instance.getLives();
                }
                //Lives icon Empty
                for (int i = 0; i < maxLives; i++)
                {
                    GUI.DrawTexture(new Rect(Screen.width - Screen.width / 6 + (35 * i), Screen.height / 50, 44, 44), livesIconEmpty);
                }
                //Lives icon Full
                for (int i = 0; i < GameManager.instance.getLives(); i++)
                {
                    GUI.DrawTexture(new Rect(Screen.width - Screen.width / 6 + (35 * i), Screen.height / 50, 44, 44), livesIconFull);
                }

                //Indicate what collision with game object does
                if (GameManager.instance.indicateEvent)
                {
                    eventCollisionPoint = camera.WorldToScreenPoint(GameManager.instance.eventCollisionPoint);

                    eventStyle.fontSize = tempEventFontSize++;
                    //eventStyle.font.material.color = new Vector4 (eventStyle.font.material.color.r, eventStyle.font.material.color.g, eventStyle.font.material.color.b, eventFontTransparency- 0.01f); 
                    eventFontTransparency -= 0.008f;
                    eventStyle.normal.textColor = new Color(1, 0.8f, 0, eventFontTransparency);

                    GUI.Label(new Rect(eventCollisionPoint.x, Screen.height - eventCollisionPoint.y, Screen.width, Screen.height), GameManager.eventDescription, eventStyle);
                }
                else
                {
                    tempEventFontSize = initialEventFontSize;
                    eventFontTransparency = 1;
                }
            }
            //Game over screen
            if (GameManager.instance.gameOver)
            {

                GUI.Label(new Rect(Screen.width / 2 - Screen.width / 5 / 2, Screen.height - Screen.height / 1.2f, Screen.width / 5, Screen.height / 10), "Game Over", gameOverText2);
                GUI.Label(new Rect(Screen.width / 2 - Screen.width / 5 / 2, Screen.height - Screen.height / 1.2f + Screen.height / 10, Screen.width / 5, Screen.height / 10), "Your score is", gameOverText1);
                GUI.Label(new Rect(Screen.width / 2 - Screen.width / 5 / 2, Screen.height - Screen.height / 1.2f + Screen.height / 10, Screen.width / 5, Screen.height / 4), GameManager.instance.getScore().ToString(), gameOverText1);

                if (GUI.Button(new Rect(Screen.width / 2 - restartGame.width / 4, Screen.height / 2, restartGame.width / 2, restartGame.height / 2), restartGame) || Input.GetKeyDown(KeyCode.Return))
                {
                    GameManager.instance.RestartGame();
                    GameManager.instance.isPlaying = true;
                }

                GameObject.Find("Wave").GetComponent<Wave>().waveIntensity = 1;
                fadeInScreen(3);
            }
            else
            {
                fadeInScreen(2);
            }


        }

    }
    //Fade In screen, used when game starts or ends
    void fadeInScreen(int fadeIn)
    {
        //GameObject.Find("Plane").GetComponent<MeshRenderer>().enabled = true;

        switch (fadeIn)
        {
            //Fade In
            case 1:
                if (0.8f > opacity)
                {
                    overScreen.GetComponent<Renderer>().material.color = new Vector4(1, 1, 1, opacity += 0.005f);
                }
                break;
            //Fade out
            case 2:
                if (0 < opacity)
                {
                    overScreen.GetComponent<Renderer>().material.color = new Vector4(1, 1, 1, opacity -= 0.005f);
                }
                break;

            case 3:
                if (1f > opacity)
                {
                    overScreen.GetComponent<Renderer>().material.color = new Vector4(0, 0, 1, opacity += 0.005f);
                }
                break;

            default:

                break;
        }

    }
}
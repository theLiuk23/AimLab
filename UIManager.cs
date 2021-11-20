using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class UIManager : MonoBehaviour
{
    #region variables
    public Shooter shooter;
    public AudioMixer shoot;
    public AudioMixer music;

	public Text countText;
    public Text timerText;
    public Text accuracyText;
    public Text stopText;
    public Text pointsText;
    public Image stopImage;
    public Text volumeText;
    public Text fpsText;
    public Slider pointsSlider;
    public Slider volumeSlider1;
    public Slider volumeSlider2;

    public GameObject menu;
    public GameObject settings;
    public GameObject game;

    public GameObject stopGame;

    public Button resumeButton;
    public Button settingsButton;
    public Button quitButton;

    public InputField inputField;
    public Toggle vSync;

    private int _resolutionIndex;
    public int finalPoints;
    public int goalPoints;
    public float timeValue;
    public bool hasStarted;
    public float accuracy;
    public int count;
    public int fps;
    public int fps_i;
    public float fpsTime;

    Resolution[] res;
    Resolution resolution;
    public Dropdown resDropdown;
	#endregion

	// Start is called before the first frame update
	void Start()
    {
        //set vsync
        QualitySettings.vSyncCount = 1;

        //set volume to -10db
        //shoot.volume = -10f;

        //get avaible resolutions
        res = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        resDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResIndex = 0;
        for (int x=0; x < res.Length; x++)
		{
            string option = res[x].width + " x " + res[x].height;
            options.Add(option);
            if (res[x].width == Screen.currentResolution.width && res[x].height == Screen.currentResolution.height)
			{
                currentResIndex = x;
			}
		}
        resDropdown.AddOptions(options);
        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();

        game.gameObject.SetActive(true);
        settings.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
        stopGame.gameObject.SetActive(false);

        shooter = FindObjectOfType(typeof(Shooter)) as Shooter;
    }

    // Update is called once per frame
    void Update()
    {
        accuracy = shooter.accuracy;
        count = shooter.count;
        timeValue = shooter.timeValue;
        hasStarted = shooter.hasStarted;
        finalPoints = shooter.finalPoints;
        goalPoints = shooter.goalPoints;

        if (timeValue > 0)
		{
            timerText.text = Math.Round(timeValue, 2).ToString();
        }
		else
		{
            Cursor.lockState = CursorLockMode.None;
            stopGame.gameObject.SetActive(true);

            timerText.text = timeValue.ToString();
            pointsText.text = "You scored: " + finalPoints + ".\nGoal: " + goalPoints;
            pointsSlider.value = finalPoints;
            pointsSlider.maxValue = goalPoints;
        }

        accuracyText.text = "Accuracy: " + Math.Round(accuracy, 0).ToString() + "%";
        countText.text = count.ToString();

        //FPS
        fpsTime += Time.deltaTime;

        if (fpsTime >= 0.1f)
		{
            fps = (int)(1.0f / Time.deltaTime);
            fpsText.text = "FPS: " + fps;
            fpsTime = 0f;
        }

        if (Input.GetButton("Cancel"))
		{
            Cursor.lockState = CursorLockMode.None;

            game.gameObject.SetActive(false);
            settings.gameObject.SetActive(false);
            menu.gameObject.SetActive(true);
        }
    }

    public void ResumeButtonClicked()
	{
        game.gameObject.SetActive(true);
        settings.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitButtonClicked()
	{
        Application.Quit();
    }

    public void SettingsButtonClicked()
	{
        game.gameObject.SetActive(false);
        settings.gameObject.SetActive(true);
        menu.gameObject.SetActive(false);
    }

    public void PlayAgain()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

    public void ChangeVolumeShoot(float volume)
	{
        if (volume <= -30)
		{
            shoot.SetFloat("volume", -80f);
		}
		else
		{
            shoot.SetFloat("volume", volume);
        }
	}

    public void ChangeVolumeMusic(float volume)
    {
        if (volume <= -30)
        {
            music.SetFloat("volume", -80f);
        }
        else
        {
            music.SetFloat("volume", volume);
        }
    }

    public void SetResolution(int resolutionIndex)
	{
        _resolutionIndex = resolutionIndex;
        resolution = res[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

    public void VSync()
	{
        if (QualitySettings.vSyncCount == 1)
		{
            QualitySettings.vSyncCount = 0;
        }
        else if (QualitySettings.vSyncCount == 0)
        {
            QualitySettings.vSyncCount = 1;
        }
    }

    public void LimitFPS(string fps_s)
	{
		try
		{
            if (fps_s.ToLower() == "no")
			{
                Application.targetFrameRate = 0;
            }
			else
			{
                fps_i = Convert.ToInt32(fps_s);
                Application.targetFrameRate = fps_i;
            }
		}
		catch
		{
            inputField.text = "";
		}        
    }

    public void SaveSettings()
	{
        string path = @"C:\Users\Luca\Documenti\settings.txt";

        StreamWriter writer = new StreamWriter(path);

        writer.WriteLine(volumeSlider1.value);
        writer.WriteLine(volumeSlider2.value);
        writer.WriteLine(resDropdown.value);
        writer.WriteLine(vSync.isOn);
        writer.WriteLine(inputField.text);

        writer.Close();
    }

	public void LoadSettings()
	{
        string path = @"C:\Users\Luca\Documenti\settings.txt";

        StreamReader reader = new StreamReader(path);

        volumeSlider1.value = float.Parse(reader.ReadLine());
        volumeSlider2.value = float.Parse(reader.ReadLine());
        resDropdown.value = Convert.ToInt32(reader.ReadLine());
        resolution = res[resDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        vSync.isOn = Convert.ToBoolean(reader.ReadLine());
        VSync();
        inputField.text = reader.ReadLine();
        Debug.Log(inputField.text);
        LimitFPS(inputField.text);

        reader.Close();
    }
}
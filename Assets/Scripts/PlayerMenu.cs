using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
	private GameObject _playerCharacter;

	private GameObject _canvas;
	private Button _playButton;

	private void Start()
	{
		_playButton = GetComponentInChildren<Button>();
		_playButton.onClick.AddListener(StartGame);
	}

	public void SetPlayer(GameObject playerCharacter)
	{
		_playerCharacter = playerCharacter;
	}

	public void ShowPlayButton()
	{
		_playButton.gameObject.SetActive(true);
	}

	public void StartGame()
	{
		_playerCharacter.SetActive(true);
		_playButton.gameObject.SetActive(false);
	}
	// Update is called once per frame
	void Update () {
		
	}
}

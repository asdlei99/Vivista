﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioPanelEditor : MonoBehaviour
{
	public Button done;
	public ExplorerPanel explorerPanelPrefab;

	public InputField url;
	public InputField title;
	public AudioControl audioControl;

	public bool answered;
	public string answerTitle;
	public string answerURL;
	public int answerTagId;

	public bool allowCancel => explorerPanel == null;

	public TagPicker tagPicker;

	private ExplorerPanel explorerPanel;
	private bool fileOpening;

	private static Color errorColor = new Color(1, 0.8f, 0.8f, 1f);

	void Update()
	{
		if (fileOpening)
		{
			if (explorerPanel != null)
			{
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					Destroy(explorerPanel.gameObject);
				}
			}

			if (explorerPanel != null && explorerPanel.answered)
			{
				url.text = explorerPanel.answerPath;

				Destroy(explorerPanel.gameObject);
				audioControl.enabled = true;
				audioControl.Init(explorerPanel.answerPath);
			}
		}
	}

	public void Init(string initialTitle, string initialUrl, int tagId = -1)
	{
		title.onValueChanged.AddListener(_ => OnInputChange(title));
		url.onValueChanged.AddListener(_ => OnInputChange(url));

		title.text = initialTitle;
		url.text = initialUrl;
		if (String.IsNullOrEmpty(initialUrl))
		{
			audioControl.enabled = false;
		}
		else
		{
			audioControl.Init(initialUrl);
		}

		tagPicker.Init(tagId);
	}

	public void Answer()
	{
		bool errors = false;
		if (String.IsNullOrEmpty(title.text))
		{
			title.image.color = errorColor;
			errors = true;
		}

		if (String.IsNullOrEmpty(url.text))
		{
			url.image.color = errorColor;
			errors = true;
		}

		if (!errors)
		{
			answered = true;
			answerURL = url.text;
			answerTitle = title.text;
			answerTagId = tagPicker.currentTagId;
		}
	}

	public void Browse()
	{
		 var searchPattern = "*.mp3;*.wav;*.aif;*.ogg";

		explorerPanel = Instantiate(explorerPanelPrefab);
		explorerPanel.transform.SetParent(Canvass.main.transform, false);
		explorerPanel.GetComponent<ExplorerPanel>().Init("", searchPattern, "Select audio");

		fileOpening = true;
	}

	public void OnInputChange(InputField input)
	{
		input.image.color = Color.white;
	}
}
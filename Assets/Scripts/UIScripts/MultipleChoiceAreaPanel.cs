﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultipleChoiceAreaPanel : MonoBehaviour
{
	public GameObject multipleChoiceAreaEntryPrefab;
	public Text title;
	public RectTransform areaList;

	private List<MultipleChoiceAreaEntry> entries = new List<MultipleChoiceAreaEntry>();
	private List<Area> areas;
	private int correct;
	private Guid guid;

	private void OnEnable()
	{
		if (areas == null)
		{
			return;
		}

		for (int i = 0; i < areas.Count; i++)
		{
			var filename = areas[i].miniatureName;
			var path = Path.Combine(Application.persistentDataPath, guid.ToString(), SaveFile.miniaturesPath);
			var fullPath = Path.Combine(path, filename);

			StartCoroutine(entries[i].SetArea(areas[i], fullPath, true));
		}
	}

	void Update()
	{
		if (SceneManager.GetActiveScene().name == "Editor")
		{
			GetComponent<Canvas>().transform.rotation = Camera.main.transform.rotation;
		}
	}

	public void Move(Vector3 position)
	{
		GetComponent<Canvas>().GetComponent<RectTransform>().position = position;
	}

	public void Init(string newTitle, Guid newGuid, List<Area> newAreas, int newCorrect)
	{
		title.text = newTitle;
		areas = newAreas;
		guid = newGuid;
		correct = newCorrect;

		for (int i = 0; i < newAreas.Count; i++)
		{
			var filename = newAreas[i].miniatureName;
			var path = Path.Combine(Application.persistentDataPath, newGuid.ToString(), SaveFile.miniaturesPath);
			var fullPath = Path.Combine(path, filename);

			var go = Instantiate(multipleChoiceAreaEntryPrefab, areaList);
			var entry = go.GetComponent<MultipleChoiceAreaEntry>();
			entry.toggle.interactable = false;

			if (i == correct)
			{
				entry.toggle.SetIsOnWithoutNotify(true);
			}
			
			entries.Add(entry);
			StartCoroutine(entry.SetArea(newAreas[i], fullPath, true));
		}

		Update();
	}
}

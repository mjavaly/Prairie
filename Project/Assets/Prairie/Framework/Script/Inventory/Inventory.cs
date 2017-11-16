﻿using UnityEngine;
using UnityEngine.EventSystems; 
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class InventoryContent
{
	public Text objName;
	public GameObject obj;

	public InventoryContent(Text t, GameObject o)
	{
		objName = t;
		obj = o;
	}
}
	
[AddComponentMenu("Prairie/Inventory/Inventory")]
public class Inventory : Interaction
, IPointerClickHandler
{	
	public const int numSlots = 4;
	public InventoryContent[] contents = new InventoryContent[numSlots];

	private bool active = false;

	public void AddToInventory (GameObject objToAdd)
	{
		for (int i = 0; i < contents.Length; i++) {
			if (contents [i].obj == null) {
				Debug.Log ("Add " + objToAdd.name + "to Inventory slot " + i.ToString () + ".");
				Text t = GetComponentsInChildren<Text> () [i];
				t.text = objToAdd.name;
				contents [i] = new InventoryContent(t, objToAdd);
				return;
			}
		}
	}

	public void RemoveFromInventory (GameObject objToRemove)
	{
		for (int i = 0; i < contents.Length; i++) {
			if (contents [i].obj == objToRemove) {
				Debug.Log ("Remove " + objToRemove.name + "from Inventory slot " + i.ToString () + ".");
				contents [i].objName.text = "Empty Slot";
				contents [i].obj = null;
				return;
			}
		}
	}

	protected override void PerformAction()
	{
		active = true;
		FirstPersonInteractor player = this.GetPlayer();
		if (player != null)
		{
			player.SetCanMove (false);
			player.SetDrawsGUI (false);
			player.SetUseCursor (true);
		}
	}

	void Update()
	{
		if (active)
		{
			if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Escape))
			{
				returnToGameStateFromInventory ();
			}
		}
	}


	private void returnToGameStateFromInventory() {
		active = false;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		FirstPersonInteractor player = this.GetPlayer ();
		if (player != null) {
			player.SetCanMove (true);
			player.SetDrawsGUI (true);
			player.SetUseCursor (false);
		}
	}

	// Interactions with the inventory UI
	public void OnPointerClick(PointerEventData eventData)
	{
//		foreach (GameObject o in eventData.hovered) {
//			Debug.Log ("Inventory clicked through: " + o.name);
//		}
		AttemptRemoveFromInventory (eventData);
	}

	private void AttemptRemoveFromInventory(PointerEventData eventData) {
		if (eventData == null) {
			return;
		}
		Text t = eventData.pointerEnter.GetComponent<Text> ();
		if (t != null) {
			InventoryContent ic = getInventoryContentFromName (t.text);
			if (ic != null) {
				DropAtCurrentLocation (ic.obj);
				RemoveFromInventory (ic.obj);
				returnToGameStateFromInventory ();
			}
		}
	}

	private InventoryContent getInventoryContentFromName(string name) {
		for (int i = 0; i < contents.Length; i++) {
			if (contents [i].objName != null && contents [i].objName.text == name) {
				return contents [i];
			}
		}
		return null;
	}

	private void DropAtCurrentLocation (GameObject obj) 
	{
		FirstPersonInteractor player = this.GetPlayer ();

		Transform t = player.transform;
		Vector3 pos = t.position;
		// Drop the object right in front of the player
		obj.transform.SetPositionAndRotation (pos+t.TransformDirection(new Vector3(0.5f, 0f, 0.5f)),
			obj.transform.rotation);
		obj.SetActive (true);
	}
}
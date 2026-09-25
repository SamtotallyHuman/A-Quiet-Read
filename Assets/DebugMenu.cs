using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugMenu : MonoBehaviour
{
	public PlayerMovement player;

	private bool showMenu = true;
	private InputAction debugMenuAction;

	void Start()
	{
		debugMenuAction = InputSystem.actions.FindAction("DebugMenu");
	}

	// Update is called once per frame
	void Update()
	{
		if (debugMenuAction.WasPressedThisFrame()) showMenu = !showMenu;
	}

	void OnGUI()
	{
		if (!showMenu) return;

		GUI.Box(new Rect(10, 10, 250, 120), "DEBUG");
		GUI.Label(new Rect(20, 40, 230, 25), $"Position: {player.Position}");
		GUI.Label(new Rect(20, 60, 230, 25), $"Hex Position: {TileManager.WorldToHex(player.Position).x}, {TileManager.WorldToHex(player.Position).y}");
		GUI.Label(new Rect(20, 80, 230, 25), $"Hex ID: {RoomManager.HexToId(TileManager.WorldToHex(player.Position))}");
		GUI.Label(new Rect(20, 100, 230, 25), $"ID Test: {TestHexId()}");
	}

	string TestHexId()
	{
		var seen = new HashSet<BigInteger>();

		for (int x = -20; x <= 20; x++)
		{
			for (int y = -20; y <= 20; y++)
			{
				if (!TileManager.IsOnPattern(new BigIntVector2(x, y)))
				{
					continue;
				}

				BigInteger n = RoomManager.HexToId(new BigIntVector2(x, y));

				if (!seen.Add(n))
				{
					return $"Collision at ({x},{y}): n = {n}";
				}
			}
		}

		return $"Passed: {seen.Count} unique points";
	}
}

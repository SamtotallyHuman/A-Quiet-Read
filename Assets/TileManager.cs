using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class TileManager : MonoBehaviour
{
	[SerializeField] private PlayerMovement player;

	[SerializeField] private GameObject standardRoom;

	private static readonly float size = 5;
	private BigIntVector2 playerLocationOnGrid;

	private List<GameObject> roomList;

	BigIntVector2 GetPlayerLocationOnGrid() { return WorldToHex(player.Position); }

	void Start()
	{
		playerLocationOnGrid = GetPlayerLocationOnGrid();
		GenerateRooms(new BigIntVector2(0, 0));
	}

	void GenerateRooms(BigIntVector2 centre)
	{
		roomList = new();
		for (int i = -4; i < 5; i++)
		{
			for (int j = -4; j < 5; j++)
			{
				BigIntVector2 location = new(i, j);
				location += centre;
				if (IsOnPattern(location))
				{
					GameObject room = Instantiate(standardRoom, HexToWorld(location), Quaternion.identity);
					room.GetComponent<RoomManager>().Initialise(location);
					roomList.Add(room);
				}
			}
		}
	}

	public static bool IsOnPattern(BigIntVector2 location)
	{
		return ((location.x - location.y - 1) % 3 + 3) % 3 != 0;
	}

	void Update()
	{
		BigIntVector2 newPlayerLocationOnGrid = GetPlayerLocationOnGrid();
		if (!newPlayerLocationOnGrid.IsEqual(playerLocationOnGrid))
		{
			HexLocationChange(newPlayerLocationOnGrid);
			playerLocationOnGrid = newPlayerLocationOnGrid;
		}
	}

	void HexLocationChange(BigIntVector2 newLocation)
	{
		for (int i = 0; i < roomList.Count; i++)
		{
			Destroy(roomList[i]);
		}

		GenerateRooms(newLocation);
	}

	public static BigInteger HexDistance(BigIntVector2 a, BigIntVector2 b)
	{
		BigInteger dq = a.x - b.x;
		BigInteger dr = a.y - b.y;
		return BigInteger.Max(BigInteger.Max(BigInteger.Abs(dq), BigInteger.Abs(dr)), BigInteger.Abs(dq + dr));
	}

	public static BigIntVector2 WorldToHex(Vector3 position)
	{
		float q = (Mathf.Sqrt(3f) / 3f * position.x - 1f / 3f * position.z) / size;
		float r = (2f / 3f * position.z) / size;

		return HexRound(q, r);
	}

	private static BigIntVector2 HexRound(float q, float r)
	{
		float s = -q - r;

		Int64 rq = Mathf.RoundToInt(q);
		Int64 rr = Mathf.RoundToInt(r);
		Int64 rs = Mathf.RoundToInt(s);

		float dq = Mathf.Abs(rq - q);
		float dr = Mathf.Abs(rr - r);
		float ds = Mathf.Abs(rs - s);

		if (dq > dr && dq > ds)
			rq = -rr - rs;
		else if (dr > ds)
			rr = -rq - rs;

		return new BigIntVector2(rq, rr);
	}

	public static Vector3 HexToWorld(BigIntVector2 location)
	{
		float x = size * Mathf.Sqrt(3f) * ((float)location.x + (float)location.y / 2f);
		float z = size * 1.5f * (float)location.y;

		return new Vector3(x, 0f, z);
	}
}

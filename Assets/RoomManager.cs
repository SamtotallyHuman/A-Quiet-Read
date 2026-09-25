using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RoomManager : MonoBehaviour
{
	private static readonly float size = 5;

	private BigIntVector2 roomLocation;

	public void Initialise(BigIntVector2 roomLocation)
	{
		this.roomLocation = roomLocation;
		Rotate();
	}

	public void MoveRoom(BigIntVector2 newLocation)
	{
		roomLocation = newLocation;
		transform.position = HexToWorld(roomLocation);
		Rotate();
	}

	bool ShouldRotate()
	{
		return ((roomLocation.x - roomLocation.y) % 3 + 3) % 3 == 2;
	}

	public void Rotate()
	{
		if (ShouldRotate())
		{
			transform.Rotate(0f, 60f, 0f);
		}
	}

	public static Vector3 HexToWorld(BigIntVector2 location)
	{
		float x = size * Mathf.Sqrt(3f) * ((float)location.x + (float)location.y / 2f);
		float z = size * 1.5f * ((float)location.y);

		return new Vector3(x, 0f, z);
	}

	public static BigInteger HexToId(BigIntVector2 location)
	{

        BigInteger d = location.x - location.y;

        // Normalise d mod 3 to {0,1,2}.
        BigInteger mod = ((d % 3) + 3) % 3;

        if (!TileManager.IsOnPattern(location))
            throw new ArgumentException("The coordinates do not satisfy the requirement.");

        BigInteger r;
        BigInteger k;

        if (mod == 0)
        {
            // d = 3k
            r = 0;
            k = d / 3;
        }
        else // mod == 2
        {
            // d = 3k - 1
            r = 1;
            k = (d + 1) / 3;
        }

        // (r, k, y) -> N_0
        return Pair(
            Pair(r, EncodeZ(k)),
            EncodeZ(location.y)
        );
    }

	private static BigInteger EncodeZ(BigInteger n)
    {
        return n >= 0
            ? 2 * n
            : -2 * n - 1;
    }
	
	private static BigInteger Pair(BigInteger a, BigInteger b)
    {
        BigInteger s = a + b;
        return s * (s + 1) / 2 + b;
    }
}
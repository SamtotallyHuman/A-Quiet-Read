using System.Numerics;

public class BigIntVector2
{
	public BigInteger x;
	public BigInteger y;

	public BigIntVector2(BigInteger x, BigInteger y)
	{
		this.x = x;
		this.y = y;
	}

	public static BigIntVector2 operator +(BigIntVector2 a, BigIntVector2 b)
	{
		return new BigIntVector2(a.x + b.x, a.y + b.y);
	}

	public static BigIntVector2 operator -(BigIntVector2 a, BigIntVector2 b)
	{
		return new BigIntVector2(a.x - b.x, a.y - b.y);
	}

	public bool IsEqual(BigIntVector2 a)
	{
		return x == a.x && y == a.y;
	}
}
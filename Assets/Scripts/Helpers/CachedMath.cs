using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CachedMath
{
    public static Vector3 VectorUp = Vector3.up;
    public static Vector3 VectorDown = Vector3.down;
    public static Vector3 VectorRight = Vector3.right;
    public static Vector3 VectorLeft = Vector3.left;
    public static Vector3 VectorForward = Vector3.forward;
    public static Vector3 Vector3Zero = Vector3.zero;
    public static Vector2 Vector2Zero = Vector2.zero;

    public static bool Compare(this Vector3 v1, Vector3 with, float maxDifferenceOn1Axis = 0.01f)
    {
        if (Mathf.Abs(v1.x - with.x) > maxDifferenceOn1Axis ||
            Mathf.Abs(v1.y - with.y) > maxDifferenceOn1Axis ||
            Mathf.Abs(v1.z - with.z) > maxDifferenceOn1Axis)
            return false;

        return true;
    }
    public static bool CompareAngles(this Vector3 v1, Vector3 with, float maxDifferenceOn1Axis = 0.01f)
    {
		v1 = NormalizeAngles(v1);
		with = NormalizeAngles(with);

		if (Mathf.Abs(v1.x - with.x) > maxDifferenceOn1Axis ||
            Mathf.Abs(v1.y - with.y) > maxDifferenceOn1Axis ||
            Mathf.Abs(v1.z - with.z) > maxDifferenceOn1Axis)
            return false;

        return true;
    }
	private static Vector3 NormalizeAngles(this Vector3 vector3)
    {
		if (vector3.x < 0 || vector3.y < 0 || vector3.z < 0)
		{
			while (vector3.x < 0)
				vector3.x += 360;
			while (vector3.y < 0)
				vector3.y += 360;
			while (vector3.z < 0)
				vector3.z += 360;
		}
		if (vector3.x > 360 || vector3.y > 360 || vector3.z > 360)
		{
			while (vector3.x > 360)
				vector3.x -= 360;
			while (vector3.y > 360)
				vector3.y -= 360;
			while (vector3.z > 360)
				vector3.z -= 360;
		}
		return vector3;
	}
	public static bool Compare(this Vector2 v1, Vector2 with, float maxDifferenceOn1Axis = 0.01f)
    {
        if (Mathf.Abs(v1.x - with.x) > maxDifferenceOn1Axis ||
            Mathf.Abs(v1.y - with.y) > maxDifferenceOn1Axis) 
            return false;

        return true;
    }
	public static Vector3 SmoothDampAngle(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed = Mathf.Infinity, float deltaTime = 0.02f)
	{
		return new Vector3(
		  Mathf.SmoothDampAngle(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime),
		  Mathf.SmoothDampAngle(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime),
		  Mathf.SmoothDampAngle(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime)
		);
	}
	public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
	{
		if (Time.deltaTime < Mathf.Epsilon) return rot;
		// account for double-cover
		var Dot = Quaternion.Dot(rot, target);
		var Multi = Dot > 0f ? 1f : -1f;
		target.x *= Multi;
		target.y *= Multi;
		target.z *= Multi;
		target.w *= Multi;
		// smooth damp (nlerp approx)
		var Result = new Vector4(
			Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
			Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
			Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
			Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
		).normalized;

		// ensure deriv is tangent
		var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), Result);
		deriv.x -= derivError.x;
		deriv.y -= derivError.y;
		deriv.z -= derivError.z;
		deriv.w -= derivError.w;

		return new Quaternion(Result.x, Result.y, Result.z, Result.w);
	}
}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
public static class Extensions {
	public static bool IntersectLine(this Rect rectangle, Vector2 lineStart, Vector2 lineEnd, out Vector2 intersectPoint) {
		float minX = lineStart.x < lineEnd.x ? lineStart.x : lineEnd.x;
		float maxX = lineStart.x > lineEnd.x ? lineStart.x : lineEnd.x;
		
		intersectPoint = Vector2.zero;
		
		if(maxX > rectangle.xMax)
		{
			maxX = rectangle.xMax;
		}
		if(minX < rectangle.xMin)
		{
			minX = rectangle.xMin;
		}
		
		if(minX > maxX) 
		{
			return false;
		}
		
		float minY = lineStart.y;
		float maxY = lineEnd.y;
		
		float dx = lineEnd.x - lineStart.x;
		
		if(Mathf.Abs(dx) > 0.0001)
		{
			float a = (lineEnd.y - lineStart.y) / dx;
			float b = lineStart.y - (a * lineStart.x);
			minY = a * minX + b;
			maxY = a * maxX + b;
		}
		
		if(minY > maxY) {
			float tmp = maxY;
			maxY = minY;
			minY = tmp;
		}
		
		if(maxY > rectangle.yMax) {
			maxY = rectangle.yMax;
		}
		
		if(minY < rectangle.yMin) {
			minY = rectangle.yMin;
		}
		if(minY > maxY) {
			return false;
		}
		
		float desiredDistance = 20;
		
		intersectPoint = rectangle.center - ((rectangle.center - lineStart).normalized * desiredDistance);
		
		intersectPoint.x = lineStart.x < lineEnd.x ? minX : maxX;
		intersectPoint.y = lineStart.y < lineEnd.y ? minY : maxY;
		//Debug.Log ("Min: " + new Vector2(minX, minY) + " Max: " + new Vector2(maxX, maxY));
		
		return true;
	}
	
	//public static Vector2 LineIntersection(
	public static Rect OffsetBy(this Rect rectangle, Vector2 offset) {
		Rect retV = new Rect(rectangle);
		retV.x += offset.x;
		retV.y += offset.y;
		
		return retV;
	}
	
	public static Vector3[] GetCorners(this Rect rectangle) {
		return new Vector3[] {
			new Vector3(rectangle.xMin, rectangle.yMin, 0),
			new Vector3(rectangle.xMax, rectangle.yMin, 0),
			new Vector3(rectangle.xMax, rectangle.yMax, 0),
			new Vector3(rectangle.xMin, rectangle.yMax, 0)
		};
	}
	
	public static Vector3 ToWorldCoords(this Vector2 vector) {
		return new Vector3(vector.x, 0, vector.y);
	}
	public static Vector2 ToSaneVector2(this Vector3 input) {
		return new Vector2(input.x, input.z);
	}
	
	public static bool IntersectCircle(this Rect rectangle, Vector2 centre, float radius) {
		// Naive pass if the rectangle contains the centre of the circle:
		if(rectangle.Contains(centre)) {
			return true;
		}
		// Naive fail if the centre of the circle is further away from the edge than radius:
		if(centre.x > rectangle.xMax + radius || centre.x < rectangle.xMin - radius || centre.y > rectangle.yMax + radius || centre.y < rectangle.yMin - radius) {
			return false;
		}
		
		if(centre.x < rectangle.xMax && centre.x > rectangle.xMin) {
			// is within x axis, so passes
			return true;
		}
		if(centre.y < rectangle.yMax && centre.y > rectangle.yMin) {
			// is within y axis, so passes
			return true;
		}
		// Square the radius ahead of time to avoid a sqrt operation
		float sqrRadius = radius * radius;
		// Check if the circle intersects each corner
		foreach(Vector2 corner in rectangle.GetCorners()) {
			if((corner - centre).sqrMagnitude < sqrRadius) {
				return true;
			}
		}
		
		return false;
	}

	public static Vector2 ToCartesian(this Vector2 radial) {
		return new Vector2(Mathf.Sin (radial.x) * radial.y, Mathf.Cos (radial.x) * radial.y);
	}

	public static Vector2 ToPolar(this Vector2 cartesian) {
		return new Vector2(Mathf.Atan2(cartesian.y, cartesian.x), Mathf.Sqrt(Mathf.Pow(cartesian.x, 2) + Mathf.Pow(cartesian.y, 2)));
	}
	
	public static bool IntersectRect(this Rect rectangle, Rect other) {
		foreach(Vector2 corner in rectangle.GetCorners()) {
			if(other.Contains(corner)) {
				return true;
			}
		}
		foreach(Vector2 corner in other.GetCorners()) {
			if(rectangle.Contains(corner)) {
				return true;
			}
		}
		return false;
	}
	
	public static void Shuffle<T>(this IList<T> list)  
	{  
		var provider = new RNGCryptoServiceProvider();
		int n = list.Count;  
		while (n > 1) {
			var box = new byte[1];
			do provider.GetBytes(box);
			while(!(box[0] < n * (byte.MaxValue / n)));
			var k = (box[0] % n);
			n--;  
			var value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
	/// <summary>
	/// Executes a function repeatedly over a stretch of time, with the normalised time (0 - 1)
	/// passed as a float parameter to the function itself.
	/// 
	/// Usage: StartCoroutine(Extensions.InterpolateFunction(delegate(float prop) {
	/// Debug.Log(prop);
	/// }, 10));
	/// 
	/// This will count up to 1 over 10 seconds.
	/// 
	/// </summary>
	/// <returns>A coroutine to be used with MonoBehaviour.StartCoroutine()</returns>
	/// <param name="function">The function to interpolate.</param>
	/// <param name="completionTime">The time over which the function should run.</param>
	public static IEnumerator InterpolateFunction(Interpolator function, float completionTime, bool useFixedUpdate = false) {
		float curTime = 0;
		while(curTime < completionTime) {
			function(curTime / completionTime);
			curTime += useFixedUpdate ? Time.fixedDeltaTime : Time.deltaTime;
			yield return useFixedUpdate ? new WaitForFixedUpdate() : null;
		}
		function(1);
	}

	public static T CopyComponent<T>(T original, GameObject destination) where T : Component
	{
		System.Type type = original.GetType();
		var dst = destination.GetComponent(type) as T;
		if (!dst) dst = destination.AddComponent(type) as T;
		var fields = type.GetFields();
		foreach (var field in fields)
		{
			if (field.IsStatic) continue;
			field.SetValue(dst, field.GetValue(original));
		}
		var props = type.GetProperties();
		foreach (var prop in props)
		{
			if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
			prop.SetValue(dst, prop.GetValue(original, null), null);
		}
		return dst as T;
	}

	public static Component CopyComponent(Component original, GameObject destination)
	{//Needs updating to above to support static variables.
		System.Type type = original.GetType();
		Component copy = destination.AddComponent(type);
		// Copied fields can be restricted with BindingFlags
		System.Reflection.FieldInfo[] fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
		foreach (System.Reflection.FieldInfo field in fields)
		{
			field.SetValue(copy, field.GetValue(original));
		}
		return copy;
	}

	public static void Serialize(this BitStream stream, ref Color col) {
		Vector3 vec = new Vector3(col.r, col.g, col.b);
		float fl = col.a;
		
		stream.Serialize(ref vec);
		stream.Serialize(ref fl);
		if(stream.isReading) {
			col = new Color(vec.x, vec.y, vec.z, fl);
		}
	}

	public static string AddOrdinal(int num)
	{
		if( num <= 0 ) return num.ToString();

		switch(num % 100)
		{
		case 11:
		case 12:
		case 13:
			return num + "th";
		}

		switch(num % 10)
		{
		case 1:
			return num + "st";
		case 2:
			return num + "nd";
		case 3:
			return num + "rd";
		default:
			return num + "th";
		}
	}
}

public delegate void Interpolator(float prop);

public static class LayerMaskExtensions
{
	public static LayerMask Create(params string[] layerNames)
	{
		return NamesToMask(layerNames);
	}
	
	public static LayerMask Create(params int[] layerNumbers)
	{
		return LayerNumbersToMask(layerNumbers);
	}
	
	public static LayerMask NamesToMask(params string[] layerNames)
	{
		LayerMask ret = (LayerMask)0;
		foreach(var name in layerNames)
		{
			ret |= (1 << LayerMask.NameToLayer(name));
		}
		return ret;
	}
	
	public static LayerMask LayerNumbersToMask(params int[] layerNumbers)
	{
		LayerMask ret = (LayerMask)0;
		foreach(var layer in layerNumbers)
		{
			ret |= (1 << layer);
		}
		return ret;
	}
	
	public static LayerMask Inverse(this LayerMask original)
	{
		return ~original;
	}
	
	public static LayerMask AddToMask(this LayerMask original, params string[] layerNames)
	{
		return original | NamesToMask(layerNames);
	}
	
	public static LayerMask RemoveFromMask(this LayerMask original, params string[] layerNames)
	{
		LayerMask invertedOriginal = ~original;
		return ~(invertedOriginal | NamesToMask(layerNames));
	}
	
	public static string[] MaskToNames(this LayerMask original)
	{
		var output = new List<string>();
		
		for (int i = 0; i < 32; ++i)
		{
			int shifted = 1 << i;
			if ((original & shifted) == shifted)
			{
				string layerName = LayerMask.LayerToName(i);
				if (!string.IsNullOrEmpty(layerName))
				{
					output.Add(layerName);
				}
			}
		}
		return output.ToArray();
	}
	
	public static string MaskToString(this LayerMask original)
	{
		return MaskToString(original, ", ");
	}
	
	public static string MaskToString(this LayerMask original, string delimiter)
	{
		return string.Join(delimiter, MaskToNames(original));
	}

	public static Rect OffsetWith(this Rect original, RectOffset offset){
		Vector2 posOffset = new Vector2 (-offset.left, -offset.top);
		Vector2 sizeOffset = new Vector2 (offset.right + offset.left, offset.bottom + offset.top);
		return new Rect (original.position + posOffset, original.size + sizeOffset);
	}
}

public static class ColourUtility {
	
	public static Color GetSpectrum(float point)
	{
		float r = Mathf.Clamp01(Mathf.Sin(point));
		float g = Mathf.Clamp01(Mathf.Cos(point + (0.66f * Mathf.PI)));
		float b = Mathf.Clamp01(Mathf.Sin(point + (1.33f * Mathf.PI)));
		return new Color(r, g, b, 1f);
	}
}

namespace Bowk
{

	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	// static class that has helper functions for building shapes/meshes

	// TODO
	// remove vertCount
	// convert to ref betterlist

	public class UtilShape 
	{
		
		#region 2D

		public static void BuildLine(Vector3 pos, Vector2 a_from, Vector2 a_to, float width,
			ref BetterList<Vector3> verts, ref BetterList<int> tris, bool cap_ends = false)
		{
			int vc = verts.size;

			Vector3 norm = (a_to - a_from).normalized;
			Vector3 perpen = new Vector3(norm.y, -norm.x, 0f);

			float hw = width * 0.5f;
			
			Vector3 from = new Vector3(a_from.x, a_from.y, 0f);
			Vector3 to = new Vector3(a_to.x, a_to.y, 0f);

			if (cap_ends)
			{
				from = from + (-norm * hw);
				to = to + (norm * hw);
			}

			verts.Add(to + (-perpen * hw));
			verts.Add(to + (perpen * hw));
			verts.Add(from + (perpen * hw));
			verts.Add(from + (-perpen * hw));

			tris.Add(vc+0);tris.Add(vc+1);tris.Add(vc+2);
			tris.Add(vc+0);tris.Add(vc+2);tris.Add(vc+3);

			Vector3 v = Vector3.zero;
			for (int i = vc; i < verts.size; ++i)
			{
				v = verts[i];
				v += pos;
				verts[i] = v;
			}
		}

		/// <summary>
		/// Builds a line mesh with alpha fadeout on edges.
		/// </summary>
		public static void BuildLineAA(Vector2 a_From, Vector2 a_To, float a_Width, float a_AAWidth, Color a_Colour,
		                               ref BetterList<Vector3> verts, ref BetterList<int> tris, ref BetterList<Color> colours)
		{
			// 3 quads. 1 center, 2 AA.
			
			// Center pos
			// normal
			// perpendicular
			
			// 8 verts
			// 6 tris - 18 ints

			int vc = verts.size;
			
			Vector2 norm = (a_To - a_From).normalized;
			Vector3 perpen = new Vector3(norm.y, -norm.x, 0f);
			
			float halfWidth = a_Width * 0.5f;
			float outsideW = halfWidth + a_AAWidth;
			
			Vector3 from = new Vector3(a_From.x, a_From.y, 0f);
			Vector3 to = new Vector3(a_To.x, a_To.y, 0f);
			
			verts.Add(to + (-perpen * outsideW));
			verts.Add(to + (-perpen * halfWidth));
			verts.Add(to + (perpen * halfWidth));
			verts.Add(to + (perpen * outsideW));
			
			verts.Add(from + (-perpen * outsideW));
			verts.Add(from + (-perpen * halfWidth));
			verts.Add(from + (perpen * halfWidth));
			verts.Add(from + (perpen * outsideW));
			
			tris.Add(vc+0);tris.Add(vc+1);tris.Add(vc+5);
			tris.Add(vc+0);tris.Add(vc+5);tris.Add(vc+4);
			
			tris.Add(vc+1);tris.Add(vc+2);tris.Add(vc+6);
			tris.Add(vc+1);tris.Add(vc+6);tris.Add(vc+5);
			
			tris.Add(vc+2);tris.Add(vc+3);tris.Add(vc+7);
			tris.Add(vc+2);tris.Add(vc+7);tris.Add(vc+6);
			
			Color col;
			for(int i = 0; i < 8; ++i)
			{
				col = a_Colour;
				// Alpha
				if (i == 0 || i == 3 ||
				    i == 4 || i == 7)
				{
					col.a = 0f;
				}
				colours.Add(col);
			}
		}

		/// <summary>
		/// Builds verts and tris for a triangle.
		/// </summary>
		/// <param name='currVertCount'>
		/// Used to offset the triangles correctly if implementing into exsisting mesh.
		/// </param>
		public static void BuildTriangle(Vector3 position, Quaternion rotation, Vector3 scale, 
		                                 ref BetterList<Vector3> verts, ref BetterList<int> tris)
		{
			
			int vc = verts.size;

			verts.Add(new Vector3(0f, 0.5f, 0f));
			verts.Add(new Vector3(0.4330127f, -0.25f, 0f));
			verts.Add(new Vector3(-0.4330128f, -0.25f, 0f));
			
			tris.Add(vc+0); tris.Add(vc+1); tris.Add(vc+2);
			
			Vector3 v = Vector3.zero;
			for (int i = vc; i < verts.size; ++i)
			{
				v = verts[i];
				v.x *= scale.x;
				v.y *= scale.y;
				v.z *= scale.z;
				v = rotation * v;
				v += position;
				verts[i] = v;
			}
		}
		
		/// <summary>
		/// Builds verts and tris for a quad.
		/// </summary>
		/// <param name='currVertCount'>
		/// Used to offset the triangles correctly if implementing into exsisting mesh.
		/// </param>
		public static void BuildQuad(Vector3 position, Quaternion rotation, Vector3 scale, 
			ref BetterList<Vector3> verts, ref BetterList<int> tris)
		{
			int vc = verts.size;
			int tc = tris.size;

			const float r = 0.5f;
			verts.Add(new Vector3(-r, r, 0f));
			verts.Add(new Vector3(r, r, 0f));
			verts.Add(new Vector3(r, -r, 0f));
			verts.Add(new Vector3(-r, -r, 0f));

			tris.Add(0); tris.Add(1); tris.Add(2);
			tris.Add(2); tris.Add(3); tris.Add(0);

			for(int i = tc; i < tris.size; ++i)
			{
				tris[i] += vc;
			}

			Vector3 v = Vector3.zero;
			for (int i = vc; i < verts.size; ++i)
			{
				v = verts[i];
				v.x *= scale.x;
				v.y *= scale.y;
				v.z *= scale.z;
				v = rotation * v;
				v += position;
				verts[i] = v;
			}
		}

		public static void BuildDiamond(Vector3 position, Quaternion rotation, Vector3 scale, 
			ref BetterList<Vector3> verts, ref BetterList<int> tris)
		{
			int vc = verts.size;
			int tc = tris.size;
			
			const float r = 0.5f;
			verts.Add(new Vector3(0f, r, 0f));
			verts.Add(new Vector3(r, 0f, 0f));
			verts.Add(new Vector3(0f, -r, 0f));
			verts.Add(new Vector3(-r, 0f, 0f));
			
			tris.Add(0); tris.Add(1); tris.Add(2);
			tris.Add(2); tris.Add(3); tris.Add(0);
			
			for(int i = tc; i < tris.size; ++i)
			{
				tris[i] += vc;
			}
			
			Vector3 v = Vector3.zero;
			for (int i = vc; i < verts.size; ++i)
			{
				v = verts[i];
				v.x *= scale.x;
				v.y *= scale.y;
				v.z *= scale.z;
				v = rotation * v;
				v += position;
				verts[i] = v;
			}
		}

		/// <summary>
		/// Builds verts and tris for a quad ring
		/// </summary>
		public static void BuildQuadRing(Vector3 position, Quaternion rotation, Vector3 scale, float ring_width,
		                                 ref BetterList<Vector3> verts, ref BetterList<int> tris)
		{
			int vc = verts.size;
			int tc = tris.size;
			
			const float r = 0.5f;
			verts.Add(new Vector3(-r, r, 0f));
			verts.Add(new Vector3(r, r, 0f));
			verts.Add(new Vector3(r, -r, 0f));
			verts.Add(new Vector3(-r, -r, 0f));

			Vector3 v = Vector3.zero;
			for (int i = vc; i < verts.size; ++i)
			{
				v = verts[i];
				v.x *= scale.x;
				v.y *= scale.y;
				v.z *= scale.z;
				verts[i] = v;
			}

			verts.Add(verts[vc+0] + new Vector3(-ring_width, ring_width, 0f));
			verts.Add(verts[vc+1] + new Vector3(ring_width, ring_width, 0f));
			verts.Add(verts[vc+2] + new Vector3(ring_width, -ring_width, 0f));
			verts.Add(verts[vc+3] + new Vector3(-ring_width, -ring_width, 0f));

			tris.Add(0); tris.Add(5); tris.Add(1);
			tris.Add(0); tris.Add(4); tris.Add(5);

			tris.Add(1); tris.Add(6); tris.Add(2);
			tris.Add(1); tris.Add(5); tris.Add(6);

			tris.Add(2); tris.Add(7); tris.Add(3);
			tris.Add(2); tris.Add(6); tris.Add(7);

			tris.Add(3); tris.Add(4); tris.Add(0);
			tris.Add(3); tris.Add(7); tris.Add(4);

			for(int i = tc; i < tris.size; ++i)
			{
				tris[i] += vc;
			}
			
			v = Vector3.zero;
			for (int i = vc; i < verts.size; ++i)
			{
				v = verts[i];
				v = rotation * v;
				v += position;
				verts[i] = v;
			}
		}

		public static void BuildQuadDiamond(Vector3 position, Quaternion rotation, Vector3 scale, float ring_width,
		                                 ref BetterList<Vector3> verts, ref BetterList<int> tris)
		{
			int vc = verts.size;
			int tc = tris.size;
			
			float r = 0.5f;
			verts.Add(new Vector3(0f, r, 0f));
			verts.Add(new Vector3(r, 0f, 0f));
			verts.Add(new Vector3(0f, -r, 0f));
			verts.Add(new Vector3(-r, 0f, 0f));
			
			Vector3 v = Vector3.zero;
			for (int i = vc; i < verts.size; ++i)
			{
				v = verts[i];
				v.x *= scale.x;
				v.y *= scale.y;
				v.z *= scale.z;
				verts[i] = v;
			}
			
			verts.Add(verts[vc+0] + new Vector3(0f, ring_width, 0f));
			verts.Add(verts[vc+1] + new Vector3(ring_width, 0f, 0f));
			verts.Add(verts[vc+2] + new Vector3(0f, -ring_width, 0f));
			verts.Add(verts[vc+3] + new Vector3(-ring_width, 0f, 0f));
			
			tris.Add(0); tris.Add(5); tris.Add(1);
			tris.Add(0); tris.Add(4); tris.Add(5);
			
			tris.Add(1); tris.Add(6); tris.Add(2);
			tris.Add(1); tris.Add(5); tris.Add(6);
			
			tris.Add(2); tris.Add(7); tris.Add(3);
			tris.Add(2); tris.Add(6); tris.Add(7);
			
			tris.Add(3); tris.Add(4); tris.Add(0);
			tris.Add(3); tris.Add(7); tris.Add(4);
			
			for(int i = tc; i < tris.size; ++i)
			{
				tris[i] += vc;
			}
			
			v = Vector3.zero;
			for (int i = vc; i < verts.size; ++i)
			{
				v = verts[i];
				v = rotation * v;
				v += position;
				verts[i] = v;
			}
		}

		public static void BuildDiamondRing(Vector3 position, Quaternion rotation, Vector3 scale, float ring_width,
			ref BetterList<Vector3> verts, ref BetterList<int> tris)
		{
			int vc = verts.size;
			int tc = tris.size;

			const float r = 0.5f;
			verts.Add(new Vector3(0f, r, 0f));
			verts.Add(new Vector3(r, 0f, 0f));
			verts.Add(new Vector3(0f, -r, 0f));
			verts.Add(new Vector3(-r, 0f, 0f));

			Vector3 v = Vector3.zero;
			for (int i = vc; i < verts.size; ++i)
			{
				v = verts[i];
				v.x *= scale.x;
				v.y *= scale.y;
				v.z *= scale.z;
				verts[i] = v;
			}

			verts.Add(verts[vc+0] + new Vector3(0f, ring_width, 0f));
			verts.Add(verts[vc+1] + new Vector3(ring_width, 0f, 0f));
			verts.Add(verts[vc+2] + new Vector3(0f, -ring_width, 0f));
			verts.Add(verts[vc+3] + new Vector3(-ring_width, 0f, 0f));

			tris.Add(0); tris.Add(5); tris.Add(1);
			tris.Add(0); tris.Add(4); tris.Add(5);

			tris.Add(1); tris.Add(6); tris.Add(2);
			tris.Add(1); tris.Add(5); tris.Add(6);

			tris.Add(2); tris.Add(7); tris.Add(3);
			tris.Add(2); tris.Add(6); tris.Add(7);

			tris.Add(3); tris.Add(4); tris.Add(0);
			tris.Add(3); tris.Add(7); tris.Add(4);

			for(int i = tc; i < tris.size; ++i)
			{
				tris[i] += vc;
			}

			v = Vector3.zero;
			for (int i = vc; i < verts.size; ++i)
			{
				v = verts[i];
				v = rotation * v;
				v += position;
				verts[i] = v;
			}
		}

		public static void BuildQuadLink(Vector3 position, Quaternion rotation, float scale, float ring_width, ref Vector2[] points, ref Vector2[] points_direction,
		                                 ref BetterList<Vector3> verts, ref BetterList<int> tris)
		{
			int vc = verts.size;
			int tc = tris.size;
			
			//------
			for(int i = 0; i < points.Length; ++i)
			{
				int curr = i;
				int next = (i+1 >= points.Length) ? 0 : i+1;

				Vector3 dir1 = -new Vector3(points_direction[curr].x, points_direction[curr].y, 0f);
				Vector3 dir2 = -new Vector3(points_direction[next].x, points_direction[next].y, 0f);

				Vector3 p1 = new Vector3(points[curr].x, points[curr].y, 0f);
				Vector3 p2 = new Vector3(points[next].x, points[next].y, 0f);

				p1 += (-dir1) * scale;
				p2 += (-dir2) * scale;

				verts.Add(p1);
				verts.Add(p1 + (dir1 * ring_width));
				verts.Add(p2 + (dir2 * ring_width));
				verts.Add(p2);

				int vn = i * 4;
				tris.Add(vn+0);tris.Add(vn+1);tris.Add(vn+2);
				tris.Add(vn+2);tris.Add(vn+3);tris.Add(vn+0);
			}

			//------
			
			for(int i = tc; i < tris.size; ++i)
			{
				tris[i] += vc;
			}
			
			Vector3 v = Vector3.zero;
			for (int i = vc; i < verts.size; ++i)
			{
				v = verts[i];
				v = rotation * v;
				v += position;
				verts[i] = v;
			}
		}

		public static void BuildCircle(Vector3 position, Quaternion rotation, Vector3 scale, 
			float radius, int numSides, int currVertCount, out Vector3[] verts, out int[] tris)
		{
			Vector3 forward = rotation * Vector3.forward;
			Vector3 up = rotation * Vector3.up;
			float rotAngle = 360f / numSides;
			Quaternion q;
			// Verts
			verts = new Vector3[numSides+1];
			verts[0] = Vector3.zero;
			for (int i = 1; i < numSides+1; ++i)
			{
				q = Quaternion.AngleAxis(-rotAngle * i, forward);
				verts[i] = position + ((q*up) * radius);
			}
			
			// Tris
			tris = new int[numSides*3];
			for(int i = 0; i < numSides; ++i)
			{
				tris[i*3+0] = 0;
				tris[i*3+1] = ((i+1) % (numSides+1));
				tris[i*3+2] = ((i+1) % (numSides))+1;			
			}
			
			// Increment tris
			for(int i = 0; currVertCount > 0 && i < tris.Length; ++i)
			{
				tris[i] += currVertCount;
			}
			// Apply position/rotation/scale offsets
			for (int i = 0; i < verts.Length; ++i)
			{
				verts[i].x *= scale.x;
				verts[i].y *= scale.y;
				verts[i].z *= scale.z;
				verts[i] = rotation * verts[i];
				verts[i] += position;
			}
		}
		
		/// <summary>
		/// Builds verts and tris for a circle.
		/// </summary>
		/// <param name='position'>
		/// Position of circle
		/// </param>
		/// <param name='radius'>
		/// Radius of circle
		/// </param>
		/// <param name='width'>
		/// Width of circle line
		/// </param>
		/// <param name='numSides'>
		/// Number of sides the circle has
		/// </param>
		/// <param name='currVertCount'>
		/// Used to offset the triangles correctly if implementing into exsisting mesh.
		/// </param>
		public static void BuildCircleRing(Vector3 position, Quaternion rotation, float radius, float width, 
			int numSides, int currVertCount, out Vector3[] verts, out int[] tris)
		{
			// loop through corners creating quads using width
			Vector3 forward = rotation * Vector3.forward;
			Vector3 up = rotation * Vector3.up;
			float rotAngle = 360f / numSides;
			Quaternion q;
			Vector3[] corners = new Vector3[numSides];
			for (int i = 0; i < numSides; ++i)
			{
				q = Quaternion.AngleAxis(rotAngle * i, forward);
				corners[i] = position + ((q*up) * radius);
			}
			
			// Loop through corners creating quads using width
			
			Vector3 v1, v2, perpen1, perpen2;
			Vector3[] p;
			int vn;
			
			List<Vector3> newVerts = new List<Vector3>();
			List<int> newTris = new List<int>();
			
			for (int i = 0; i < corners.Length; ++i)
			{
				// Build quad from this corner to next
				v1 = corners[i];
				v2 = corners[(i+1) % corners.Length];
				
				perpen1 = (v1 - position).normalized;
				perpen2 = (v2 - position).normalized;
				
				p = new Vector3[4]{
					v1 + (perpen1 * width),
					v1 - (perpen1 * width),
					v2 - (perpen2 * width),
					v2 + (perpen2 * width)
				};
				
				vn = newVerts.Count + currVertCount;
				newVerts.AddRange(new Vector3[4]{p[0], p[1], p[2], p[3]});
				newTris.AddRange(new int[6]{
					vn+0, vn+1, vn+2,
					vn+2, vn+3, vn+0
				});
			}
			
			verts = newVerts.ToArray();
			tris = newTris.ToArray();
		}
		
		#endregion
		
		#region 3D

		public static void BuildCube(Vector3 position, Quaternion rotation, Vector3 scale,
			ref BetterList<Vector3> verts, ref BetterList<int> tris)
		{
			int vc = verts.size;
			int tc = tris.size;

			//---
			const float r = 0.5f;
			verts.Add(new Vector3(r, -r, r));
			verts.Add(new Vector3(-r, -r, r));
			verts.Add(new Vector3(r, r, r));
			verts.Add(new Vector3(-r, r, r));
				
			verts.Add(new Vector3(r, r, -r));
			verts.Add(new Vector3(-r, r, -r));
			verts.Add(new Vector3(r, -r, -r));
			verts.Add(new Vector3(-r, -r, -r));
				
			verts.Add(new Vector3(r, r, r));
			verts.Add(new Vector3(-r, r, r));
			verts.Add(new Vector3(r, r, -r));
			verts.Add(new Vector3(-r, r, -r));
				
			verts.Add(new Vector3(r, -r, -r));
			verts.Add(new Vector3(-r, -r, r));
			verts.Add(new Vector3(-r, -r, -r));
			verts.Add(new Vector3(r, -r, r));
				
			verts.Add(new Vector3(-r, -r, r));
			verts.Add(new Vector3(-r, r, -r));
			verts.Add(new Vector3(-r, -r, -r));
			verts.Add(new Vector3(-r, r, r));
				
			verts.Add(new Vector3(r, -r, -r));
			verts.Add(new Vector3(r, r, r));
			verts.Add(new Vector3(r, -r, r));
			verts.Add(new Vector3(r, r, -r));

			tris.Add(0); tris.Add(3); tris.Add(1);
			tris.Add(0); tris.Add(2); tris.Add(3);

			tris.Add(8); tris.Add(5); tris.Add(9);
			tris.Add(8); tris.Add(4); tris.Add(5);

			tris.Add(10); tris.Add(7); tris.Add(11);
			tris.Add(10); tris.Add(6); tris.Add(7);

			tris.Add(12); tris.Add(13); tris.Add(14);
			tris.Add(12); tris.Add(15); tris.Add(13);

			tris.Add(16); tris.Add(17); tris.Add(18);
			tris.Add(16); tris.Add(19); tris.Add(17);

			tris.Add(20); tris.Add(21); tris.Add(22);
			tris.Add(20); tris.Add(23); tris.Add(21);

			for(int i = tc; i < tris.size; ++i)
			{
				tris[i] += vc;
			}

			Vector3 v = Vector3.zero;
			for (int i = vc; i < verts.size; ++i)
			{
				v = verts[i];
				v.x *= scale.x;
				v.y *= scale.y;
				v.z *= scale.z;
				v = rotation * v;
				v += position;
				verts[i] = v;
			}
		}
		
		// Tetrahedron
		/// <summary>
		/// Builds verts and tris for a Tetrahedron.
		/// </summary>
		/// <param name='currVertCount'>
		/// Used to offset the triangles correctly if implementing into exsisting mesh.
		/// </param>
		public static void BuildTetrahedron(Vector3 position, Quaternion rotation, Vector3 scale,
			int currVertCount, out Vector3[] verts, out int[] tris)
		{
			/*
	 		Vertex        coordinate
	    	0,  x= 0.000, y= 0.000, z= 1.000 
	    	1,  x= 0.943, y= 0.000, z=-0.333 
	    	2,  x=-0.471, y= 0.816, z=-0.333 
	    	3,  x=-0.471, y=-0.816, z=-0.333 
			*/
			
			// corners
			Vector3[] c = new Vector3[4];
			c[0] = new Vector3(0f, 0f, 0.5f);
			c[1] = new Vector3(0.4715f, 0f, -0.1665f);
			c[2] = new Vector3(-0.2355f, 0.408f, -0.1665f);
			c[3] = new Vector3(-0.2355f, -0.408f, -0.1665f);
			
			verts = new Vector3[12]
			{
				c[0], c[1], c[2],
				c[0], c[2], c[3],
				c[0], c[3], c[1],
				c[1], c[3], c[2]
			};
			
			tris = new int[12]
			{
				0, 1, 2,
				3, 4, 5,
				6, 7, 8,
				9, 10, 11
			};
			
			for(int i = 0; currVertCount > 0 && i < tris.Length; ++i)
			{
				tris[i] += currVertCount;
			}
			
			for (int i = 0; i < verts.Length; ++i)
			{
				verts[i].x *= scale.x;
				verts[i].y *= scale.y;
				verts[i].z *= scale.z;
				verts[i] = rotation * verts[i];
				verts[i] += position;
			}
		}
		
		// Icosahedron
		/// <summary>
		/// Builds verts and tris for a Icosahedron.
		/// </summary>
		/// <param name='currVertCount'>
		/// Used to offset the triangles correctly if implementing into exsisting mesh.
		/// </param>
		public static void BuildIcosahedron(Vector3 position, Quaternion rotation, Vector3 scale,
			int currVertCount, out Vector3[] verts, out int[] tris)
		{
			// corners
			Vector3[] c = new Vector3[12];		
			c[0] = new Vector3(		0f,			0.5f,			0f);
			c[1] = new Vector3(		0.138f, 	0.2235f,		-0.4255f);
			c[2] = new Vector3(		-0.362f, 	0.2235f,		-0.263f);
			c[3] = new Vector3(		-0.362f, 	0.2235f,		0.263f);
			c[4] = new Vector3(		0.138f, 	0.2235f,		0.4255f);
			c[5] = new Vector3(		0.447f,		0.2235f,		0f);
			c[6] = new Vector3(		0.362f, 	-0.2235f,		-0.263f);
			c[7] = new Vector3(		-0.138f, 	-0.2235f,		-0.4255f);
			c[8] = new Vector3(		-0.447f, 	-0.2235f,		0f);
			c[9] = new Vector3(		-0.138f, 	-0.2235f,		0.4255f);
			c[10] = new Vector3( 	0.362f, 	-0.2235f,		0.263f);
			c[11] = new Vector3( 	0f, 		-0.5f,			0f);
			
			verts = new Vector3[60]
			{
				// Top
				c[0], c[1], c[2],
				c[0], c[2], c[3],
				c[0], c[3], c[4],
				c[0], c[4], c[5],
				c[0], c[5], c[1],
				
				// Mid
				c[2], c[1], c[7],
				c[2], c[7], c[8],
				c[3], c[2], c[8],
				c[3], c[8], c[9],
				c[4], c[3], c[9],
				c[4], c[9], c[10],
				c[5], c[4], c[10],
				c[5], c[10], c[6],
				c[1], c[5], c[6],
				c[1], c[6], c[7],
				
				// Bottom
				c[11], c[7], c[6],
				c[11], c[8], c[7],
				c[11], c[9], c[8],
				c[11], c[10], c[9],
				c[11], c[6], c[10],
			};
			
			tris = new int[60];
			
			for(int i = 0; i < tris.Length; ++i)
			{
				tris[i] = i + currVertCount;
			}
			
			for (int i = 0; i < verts.Length; ++i)
			{
				verts[i].x *= scale.x;
				verts[i].y *= scale.y;
				verts[i].z *= scale.z;
				verts[i] = rotation * verts[i];
				verts[i] += position;
			}
		}
		
		#endregion
		
	}

}





















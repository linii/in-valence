﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pipeline
{
	public class OceanTile
	{
		//==============================================
		// CONSTANTS

		public static Dictionary<Dir, Vector2> DirVecs = new Dictionary<Dir, Vector2> {
			{ Dir.TopLeft, new Vector2 (-1, 1) }, 
			{ Dir.Top, new Vector2 (0, 1) }, 
			{ Dir.TopRight, new Vector2 (1, 1) }, 
			{ Dir.Left, new Vector2 (-1, 0) }, 
			{ Dir.Right, new Vector2 (1, 0) }, 
			{ Dir.BottomLeft, new Vector2 (-1, -1) }, 
			{ Dir.Bottom, new Vector2 (0, -1) }, 
			{ Dir.BottomRight, new Vector2 (1, -1) }
		};

		//==============================================
		// ADMIN & ID
			
		// when this tile was created
		private System.DateTime created = System.DateTime.Now;

		// generate random ID (might be useful later on?)
		private System.Guid id = System.Guid.NewGuid ();

		//==============================================
		// CONSTRUCTOR

		// initialize a new tile, which controls the islands and stuff
		public OceanTile (Vector3 init, float size)
		{
			Coor = init;
			Size = size;
			Scale = new Vector3 (size / 10, 0.1f, size / 10);

			activeNeighbors = new Dictionary<Vector2, OceanTile> ();
			activeIslands = new LinkedList<Island> (); 

			_debug ("Initialized");
		}

		//==============================================
		// MEMBERS

		// its location on global map
		public Vector3 Coor { get; private set; }

		// size of each side of the tile
		public float Size { get; private set; }

		// how much to expand size o ftile by
		public Vector3 Scale { get; private set; }

		// its active islands
		public  LinkedList<Island> activeIslands { get; private set; }

		// its active neighbors
		public Dictionary<Vector2, OceanTile> activeNeighbors { get; private set; }

		//==============================================
		// interfacing with class members

		public bool AddNeighbor (Dir d, OceanTile t)
		{
			Vector2 dirVec = DirVecs [d];
			return AddNeighbor (dirVec, t);
		}

		public bool AddNeighbor (Vector2 dirVec, OceanTile t)
		{
			if (activeNeighbors.ContainsKey (dirVec)) {
				_debug ("[AddNeighbor] Trying to replace a neighbor which already exists.");
				return false;
			}
			activeNeighbors.Add (dirVec, t); 
			return true;
		}

		public void RemoveNeighbor (Dir d)
		{
			RemoveNeighbor (DirVecs [d]);
		}

		public void RemoveNeighbor (Vector2 d)
		{
			_debug ("[RemoveNeighbor] for: " + this.ToString ()); 
			activeNeighbors.Remove (d);
		}

		//==============================================
		// state

		// deactivation for optimization purposes
		public void deactivateTile ()
		{
			_debug ("Deactivating tile: " + this.ToString ());

			// deactivate islands 
			foreach (Island i in activeIslands) {
				i.DestroyIsland ();
			}

			// deactivate ocean thingies? 
		}
		
		//==============================================
		// UTIL

		// checks whether a loc is in tile
		public bool inTile (Vector3 loc)
		{
			return loc.x >= Coor.x && loc.x < (Coor.x + Size) &&
			loc.z >= Coor.z && loc.z < (Coor.z + Size);
		}

		//==============================================
		// DOCUMENTATION AND DEBUGGING

		override
		public string ToString ()
		{
			return "[Tile info]"
			+ "\n[Coor]\t" + this.Coor.ToString ()
			+ "\t[Size]\t" + this.Size.ToString ()
			+ "\t[Scale]\t" + this.Scale.ToString ()
			+ "\t[Created]\t\t" + this.created.ToString ()
			+ "\n[#Neighbors]\t\t" + activeNeighbors.Count.ToString ()
			+ "\t[#Islands]\t\t" + activeIslands.Count.ToString ()
			+ "\n";

		}

		public void _debug (string message)
		{
			Debug.Log ("[Tile log]\t\t" + message);
			Debug.Log (this.ToString ());
		}
	}

}

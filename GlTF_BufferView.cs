﻿using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class GlTF_BufferView : GlTF_Writer  {
	public string buffer;// ": "duck",
	public long byteLength;//": 25272,
	public long byteOffset;//": 0,
	public int target = 34963;
	//	public string target = "ARRAY_BUFFER";
	public int currentOffset = 0;
	public MemoryStream memoryStream = new MemoryStream();

	public GlTF_BufferView (string n) { name = n; }
	public GlTF_BufferView (string n, int t) { name = n; target = t; }

	public void Populate (int[] vs, bool flippedTriangle)
	{
		if (flippedTriangle)
		{
			for (int i = 0; i < vs.Length; i+=3)
			{
				ushort u = (ushort)vs[i];
				memoryStream.Write (BitConverter.GetBytes(u), 0, 2);
				currentOffset += 2;

				u = (ushort)vs[i+2];
				memoryStream.Write (BitConverter.GetBytes(u), 0, 2);
				currentOffset += 2;

				u = (ushort)vs[i+1];
				memoryStream.Write (BitConverter.GetBytes(u), 0, 2);
				currentOffset += 2;
			}
		}
		else
		{
			for (int i = 0; i < vs.Length; i++)
			{
				ushort u = (ushort)vs[i];
				memoryStream.Write (BitConverter.GetBytes(u), 0, 2);
				currentOffset += 2;
			}
		}
		byteLength = currentOffset;
	}

	public void Populate (float[] vs)
	{
		for (int i = 0; i < vs.Length; i++)
		{
			//			memoryStream.Write (vs[i]);
			//			memoryStream.Write ((byte[])vs, 0, vs.Length * sizeof(int));
			float f = vs[i];
			memoryStream.Write (BitConverter.GetBytes(f), 0, 2);
			currentOffset += 4;
		}
		byteLength = currentOffset;
	}

	public void Populate (float v)
	{
		memoryStream.Write (BitConverter.GetBytes(v), 0, 4);
		currentOffset += 4;
		byteLength = currentOffset;
	}

	public override void Write ()
	{
		/*
		"bufferView_4642": {
            "buffer": "vc.bin",
            "byteLength": 630080,
            "byteOffset": 0,
            "target": "ARRAY_BUFFER"
        },
	*/
		Indent();		jsonWriter.Write ("\"" + name + "\": {\n");
		IndentIn();
		Indent();		jsonWriter.Write ("\"buffer\": \"" + Path.GetFileNameWithoutExtension(GlTF_Writer.binFileName)+"\",\n");
		Indent();		jsonWriter.Write ("\"byteLength\": " + byteLength + ",\n");
		Indent();		jsonWriter.Write ("\"byteOffset\": " + byteOffset + ",\n");
		Indent();		jsonWriter.Write ("\"target\": " + target + "\n");
		IndentOut();
		Indent();		jsonWriter.Write ("}");
	}
}
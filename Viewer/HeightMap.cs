using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Viewer
{
	// Token: 0x02000009 RID: 9
	internal class HeightMap
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00004E48 File Offset: 0x00003048
		public HeightMap(IEnumerable<float[,]> planes)
		{
			this.verts.Add(new VertexPositionColor(new Vector3(0f, 1f, 0f), Color.Blue));
			this.verts.Add(new VertexPositionColor(new Vector3(1f, -1f, 0f), Color.Yellow));
			this.verts.Add(new VertexPositionColor(new Vector3(-1f, -1f, 0f), Color.Red));
			this.verts.Add(new VertexPositionColor(new Vector3(1f, 1f, 0f), Color.Red));
		}

		// Token: 0x04000063 RID: 99
		public List<VertexPositionColor> verts = new List<VertexPositionColor>();
	}
}

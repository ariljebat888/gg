using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Viewer
{
	// Token: 0x02000002 RID: 2
	internal class IdentMesh : DrawableGameComponent
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public IdentMesh(Game game)
			: base(game)
		{
			this.Parts = new IdentMesh.Part[2];
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002068 File Offset: 0x00000268
		public override void Initialize()
		{
			base.Initialize();
			for (int i = 0; i < 2; i++)
			{
				this.Parts[i] = new IdentMesh.Part
				{
					effect = new BasicEffect(base.GraphicsDevice)
					{
						VertexColorEnabled = true,
						World = Matrix.Identity
					}
				};
			}
			Color color = Color.Red;
			this.Parts[0].vertexes = new VertexPositionColor[]
			{
				new VertexPositionColor(new Vector3(0f, 0f, 25f), color),
				new VertexPositionColor(new Vector3(200f, 0f, 0f), color),
				new VertexPositionColor(new Vector3(0f, 0f, -25f), color)
			};
			this.Parts[0].indicles = new short[] { 0, 1, 2 };
			color = Color.Blue;
			this.Parts[1].vertexes = new VertexPositionColor[]
			{
				new VertexPositionColor(new Vector3(-25f, 1f, 0f), color),
				new VertexPositionColor(new Vector3(0f, 1f, 200f), color),
				new VertexPositionColor(new Vector3(25f, 1f, 0f), color)
			};
			this.Parts[1].indicles = new short[] { 0, 1, 2 };
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002230 File Offset: 0x00000430
		public override void Draw(GameTime gameTime)
		{
			foreach (IdentMesh.Part part in this.Parts)
			{
				part.effect.View = Camera.DefaultCamera.View;
				part.effect.Projection = Camera.DefaultCamera.Projection;
				foreach (EffectPass effectPass in part.effect.CurrentTechnique.Passes)
				{
					effectPass.Apply();
					base.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, part.vertexes, 0, part.vertexes.Length, part.indicles, 0, part.indicles.Length / 3);
				}
			}
			base.Draw(gameTime);
		}

		// Token: 0x04000001 RID: 1
		public IdentMesh.Part[] Parts;

		// Token: 0x02000003 RID: 3
		public class Part
		{
			// Token: 0x04000002 RID: 2
			public BasicEffect effect;

			// Token: 0x04000003 RID: 3
			public VertexPositionColor[] vertexes;

			// Token: 0x04000004 RID: 4
			public short[] indicles;
		}
	}
}

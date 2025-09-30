using System;
using Microsoft.Xna.Framework;

namespace Viewer
{
	// Token: 0x02000004 RID: 4
	public sealed class Camera
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000232C File Offset: 0x0000052C
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002343 File Offset: 0x00000543
		public static float ElapsedTime
		{
			get
			{
				return Camera._elapsed;
			}
			set
			{
				Camera._elapsed = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000234C File Offset: 0x0000054C
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002363 File Offset: 0x00000563
		public static Camera DefaultCamera
		{
			get
			{
				return Camera._camera;
			}
			set
			{
				Camera._camera = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000236C File Offset: 0x0000056C
		public Matrix View
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002384 File Offset: 0x00000584
		public Matrix Projection
		{
			get
			{
				return this._proj;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000239C File Offset: 0x0000059C
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000023B4 File Offset: 0x000005B4
		public Vector3 Right
		{
			get
			{
				return this._right;
			}
			set
			{
				this._right = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000023C0 File Offset: 0x000005C0
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000023D8 File Offset: 0x000005D8
		public Vector3 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000023E4 File Offset: 0x000005E4
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000023FC File Offset: 0x000005FC
		public Vector3 UpVector
		{
			get
			{
				return this._up;
			}
			set
			{
				this._up = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002408 File Offset: 0x00000608
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002420 File Offset: 0x00000620
		public Vector3 LookAt
		{
			get
			{
				return this._look;
			}
			set
			{
				this._look = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000242C File Offset: 0x0000062C
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002443 File Offset: 0x00000643
		public float AspectRatio { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000244C File Offset: 0x0000064C
		public float Fov
		{
			get
			{
				return 0.7853982f;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002464 File Offset: 0x00000664
		public float NearClip
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000247C File Offset: 0x0000067C
		public float FarClip
		{
			get
			{
				return 1000000f;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002494 File Offset: 0x00000694
		public static Matrix RotationAxis(Vector3 vec, float angle)
		{
			float num = (float)Math.Cos((double)angle);
			float num2 = (float)Math.Sin((double)angle);
			float num3 = 1f - num;
			Matrix matrix = new Matrix(num3 * vec.X * vec.X + num, num3 * vec.X * vec.Y + num2 * vec.Z, num3 * vec.X * vec.Z + num2 * vec.Y, 0f, num3 * vec.X * vec.Y - num2 * vec.Z, num3 * vec.Y * vec.Y + num, num3 * vec.Y * vec.Z + num2 * vec.X, 0f, num3 * vec.X * vec.Z + num2 * vec.Y, num3 * vec.Y * vec.Z - num2 * vec.X, num3 * vec.Z * vec.Z + num, 0f, 0f, 0f, 0f, 1f);
			return Matrix.Transpose(matrix);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000025D0 File Offset: 0x000007D0
		public static void Normalize(Plane p)
		{
			float num = p.Normal.Length();
			p.Normal.X = p.Normal.X * num;
			p.Normal.Y = p.Normal.Y * num;
			p.Normal.Z = p.Normal.Z * num;
			p.D *= num;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002638 File Offset: 0x00000838
		public Camera(Vector3 eye, Vector3 target)
		{
			this.AspectRatio = 1.3333334f;
			this._proj = Matrix.CreatePerspectiveFieldOfView(this.Fov, this.AspectRatio, this.NearClip, this.FarClip);
			this._position = eye;
			this._look = target;
			this._up = Vector3.Up;
			this._right = Vector3.Right;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002709 File Offset: 0x00000909
		public void WindowChanged(int width, int height)
		{
			this.AspectRatio = (float)width / (float)height;
			this._proj = Matrix.CreatePerspectiveFieldOfView(this.Fov, this.AspectRatio, this.NearClip, this.FarClip);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000273B File Offset: 0x0000093B
		public void Strafe(float units)
		{
			this._position += this._right * units * Camera.ElapsedTime;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002765 File Offset: 0x00000965
		public void Fly(float units)
		{
			this._position += this._up * units * Camera.ElapsedTime;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000278F File Offset: 0x0000098F
		public void Land(float units)
		{
			this._position -= this._up * units * Camera.ElapsedTime;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000027B9 File Offset: 0x000009B9
		public void Walk(float units)
		{
			this._position += this._look * units * Camera.ElapsedTime;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000027E4 File Offset: 0x000009E4
		public void Pitch(float angle)
		{
			Matrix matrix = default(Matrix);
			matrix = Camera.RotationAxis(this._right, angle * Camera.ElapsedTime);
			this._up = Vector3.Transform(this._up, matrix);
			this._look = Vector3.Transform(this._look, matrix);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002834 File Offset: 0x00000A34
		public void Yaw(float angle)
		{
			Matrix matrix = default(Matrix);
			matrix = Camera.RotationAxis(this._up, angle * Camera.ElapsedTime);
			this._right = Vector3.Transform(this._right, matrix);
			this._look = Vector3.Transform(this._look, matrix);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002884 File Offset: 0x00000A84
		public void Roll(float angle)
		{
			Matrix matrix = default(Matrix);
			matrix = Camera.RotationAxis(this._look, angle * Camera.ElapsedTime);
			this._right = Vector3.Transform(this._right, matrix);
			this._up = Vector3.Transform(this._up, matrix);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000028D4 File Offset: 0x00000AD4
		public void RotateByMouse(Vector3 pos)
		{
			float num = -pos.X * 0.005f;
			float num2 = -pos.Y * 0.005f;
			Matrix matrix = Camera.RotationAxis(this._right, num2);
			this._look = Vector3.Transform(this._look, matrix);
			this._up = Vector3.Transform(this._up, matrix);
			matrix = Matrix.CreateRotationY(num);
			this._look = Vector3.Transform(this._look, matrix);
			this._right = Vector3.Transform(this._right, matrix);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002960 File Offset: 0x00000B60
		public void Update()
		{
			Matrix matrix = default(Matrix);
			this._look.Normalize();
			this._up = -Vector3.Cross(this._look, this._right);
			this._up.Normalize();
			this._right = -Vector3.Cross(this._up, this._look);
			float num = -Vector3.Dot(this._right, this._position);
			float num2 = -Vector3.Dot(this._up, this._position);
			float num3 = -Vector3.Dot(this._look, this._position);
			matrix.M11 = this._right.X;
			matrix.M12 = this._up.X;
			matrix.M13 = this._look.X;
			matrix.M14 = 0f;
			matrix.M21 = this._right.Y;
			matrix.M22 = this._up.Y;
			matrix.M23 = this._look.Y;
			matrix.M24 = 0f;
			matrix.M31 = this._right.Z;
			matrix.M32 = this._up.Z;
			matrix.M33 = this._look.Z;
			matrix.M34 = 0f;
			matrix.M41 = num;
			matrix.M42 = num2;
			matrix.M43 = num3;
			matrix.M44 = 1f;
			this._view = matrix;
		}

		// Token: 0x04000005 RID: 5
		private static float _elapsed;

		// Token: 0x04000006 RID: 6
		private static Camera _camera;

		// Token: 0x04000007 RID: 7
		private Matrix _proj;

		// Token: 0x04000008 RID: 8
		private Matrix _view;

		// Token: 0x04000009 RID: 9
		private Vector3 _lastMousePosition = new Vector3(0f, 0f, 0f);

		// Token: 0x0400000A RID: 10
		private Vector3 _currentMousePosition = new Vector3(0f, 0f, 0f);

		// Token: 0x0400000B RID: 11
		private Vector3 _right = new Vector3(1f, 0f, 0f);

		// Token: 0x0400000C RID: 12
		private Vector3 _up = new Vector3(0f, 1f, 0f);

		// Token: 0x0400000D RID: 13
		public Vector3 _look;

		// Token: 0x0400000E RID: 14
		private Vector3 _position;
	}
}

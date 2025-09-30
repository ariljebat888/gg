using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Viewer
{
	// Token: 0x0200000A RID: 10
	public class Game : Microsoft.Xna.Framework.Game
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000053 RID: 83 RVA: 0x00004F14 File Offset: 0x00003114
		// (remove) Token: 0x06000054 RID: 84 RVA: 0x00004F50 File Offset: 0x00003150
		public event Action<Ray> OnMouseClick;

		// Token: 0x06000055 RID: 85 RVA: 0x00004F8C File Offset: 0x0000318C
		public void SelectReachProfile(bool reach)
		{
			this._graphics.GraphicsProfile = (reach ? GraphicsProfile.Reach : GraphicsProfile.HiDef);
			this._graphics.ApplyChanges();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004FB0 File Offset: 0x000031B0
		public void Show(IEnumerable<DrawableGameComponent> objs)
		{
			lock (this.switchLock)
			{
				this.Clear();
				foreach (DrawableGameComponent drawableGameComponent in objs)
				{
					if (drawableGameComponent != null)
					{
						base.Components.Add(drawableGameComponent);
					}
				}
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00005058 File Offset: 0x00003258
		public void SyncObjects(IEnumerable<DrawableGameComponent> objs)
		{
			lock (this.switchLock)
			{
				foreach (DrawableGameComponent drawableGameComponent in objs.ToArray<DrawableGameComponent>())
				{
					if (drawableGameComponent != null)
					{
						if (!base.Components.Contains(drawableGameComponent))
						{
							base.Components.Add(drawableGameComponent);
						}
					}
				}
				foreach (DrawableGameComponent drawableGameComponent2 in base.Components.OfType<DrawableGameComponent>().ToArray<DrawableGameComponent>())
				{
					if (!(drawableGameComponent2 is IdentMesh) && !objs.Contains(drawableGameComponent2))
					{
						base.Components.Remove(drawableGameComponent2);
					}
				}
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00005150 File Offset: 0x00003350
		public void Clear()
		{
			lock (this.switchLock)
			{
				this.ClearObjects();
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000519C File Offset: 0x0000339C
		public void SetCamera(BoundingBox box)
		{
			float num = box.Max.X - box.Min.X;
			float num2 = box.Min.Z * 1.2f;
			num /= (float)Math.Tan(0.2617993877991494);
			if (num2 < -num)
			{
				num = -num2;
			}
			Vector3 vector = new Vector3(0f, 0.35f, -0.9f);
			this._camera = new Camera(new Vector3(0f, box.Max.Y, -1f * num), vector);
			this._camera.Update();
			Camera.DefaultCamera = this._camera;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00005250 File Offset: 0x00003450
		public Game()
		{
			this._graphics = new GraphicsDeviceManager(this);
			base.Window.AllowUserResizing = true;
			base.IsMouseVisible = true;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000052C8 File Offset: 0x000034C8
		protected void ClearObjects()
		{
			base.Components.Clear();
			base.Components.Add(new IdentMesh(this));
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000052E9 File Offset: 0x000034E9
		protected override void Initialize()
		{
			base.Window.Title = "rMap ModelViewer : 60 fps";
			base.IsFixedTimeStep = false;
			this.InitializeCamera();
			this.ClearObjects();
			base.Initialize();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000531A File Offset: 0x0000351A
		protected override void LoadContent()
		{
			base.LoadContent();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00005324 File Offset: 0x00003524
		private void InitializeCamera()
		{
			this._camera = new Camera(new Vector3(0f, 0f, 0f), Vector3.Forward);
			Camera.DefaultCamera = this._camera;
			MouseState state = Mouse.GetState();
			this._lastMouseX = state.X;
			this._lastMouseY = state.Y;
			this._camera.Update();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00005390 File Offset: 0x00003590
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				base.Exit();
			}
			Camera.ElapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			KeyboardState state = Keyboard.GetState();
			float num = 5f;
			float num2 = 0.01f;
			float num3 = 100f;
			float num4 = ((state[Keys.LeftShift] == KeyState.Down) ? num3 : ((state[Keys.LeftAlt] == KeyState.Down) ? num2 : num));
			if (state[Keys.W] == KeyState.Down)
			{
				this._camera.Walk(-240f * num4);
			}
			if (state[Keys.S] == KeyState.Down)
			{
				this._camera.Walk(240f * num4);
			}
			if (state[Keys.A] == KeyState.Down)
			{
				this._camera.Strafe(-240f * num4);
			}
			if (state[Keys.D] == KeyState.Down)
			{
				this._camera.Strafe(240f * num4);
			}
			if (state[Keys.E] == KeyState.Down)
			{
				this._camera.Fly(240f * num4);
			}
			if (state[Keys.Q] == KeyState.Down)
			{
				this._camera.Land(240f * num4);
			}
			if (state[Keys.F] == KeyState.Down)
			{
				this._camera.Roll(10f * num4);
			}
			if (state[Keys.G] == KeyState.Down)
			{
				this._camera.Roll(-10f * num4);
			}
			if (state[Keys.Escape] == KeyState.Down)
			{
				base.Exit();
			}
			if (state[Keys.R] == KeyState.Down)
			{
				if (!this._keyTest)
				{
					if (!this._wire)
					{
						this._graphics.GraphicsDevice.RasterizerState = new RasterizerState
						{
							FillMode = FillMode.WireFrame
						};
						this._wire = true;
					}
					else
					{
						this._graphics.GraphicsDevice.RasterizerState = new RasterizerState
						{
							FillMode = FillMode.Solid
						};
						this._wire = false;
					}
				}
				this._keyTest = true;
			}
			else
			{
				this._keyTest = false;
			}
			MouseState state2 = Mouse.GetState();
			if (state2.X >= 0 && state2.X < base.Window.ClientBounds.Width && state2.Y >= 0 && state2.Y < base.Window.ClientBounds.Height)
			{
				if (state2.RightButton == ButtonState.Pressed)
				{
					if (state2.X >= 0 && state2.Y >= 0 && state2.X <= base.Window.ClientBounds.Width && state2.Y <= base.Window.ClientBounds.Height)
					{
						this._camera.RotateByMouse(new Vector3((float)(this._lastMouseX - state2.X), (float)(state2.Y - this._lastMouseY), 0f));
					}
				}
				if (state2.LeftButton == ButtonState.Pressed && !this._leftButtonDown)
				{
					this._leftButtonDown = true;
					if (this.lastLeftMouseClick >= DateTime.Now - new TimeSpan(0, 0, 1))
					{
						if (this.OnMouseClick != null)
						{
							this.OnMouseClick(this.GetMouseRay());
						}
						this.lastLeftMouseClick = DateTime.MinValue;
					}
					else
					{
						this.lastLeftMouseClick = DateTime.Now;
					}
				}
				else if (state2.LeftButton == ButtonState.Released && this._leftButtonDown)
				{
					this._leftButtonDown = false;
				}
			}
			this._lastMouseX = state2.X;
			this._lastMouseY = state2.Y;
			this._camera.Update();
			if ((DateTime.Now - this._last).TotalMilliseconds >= 1000.0)
			{
				base.Window.Title = "rMap ModelViewer : " + this._fps + " fps";
				this._fps = 0;
				this._last = DateTime.Now;
			}
			else
			{
				this._fps++;
			}
			base.Update(gameTime);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00005864 File Offset: 0x00003A64
		private Ray GetMouseRay()
		{
			MouseState state = Mouse.GetState();
			Vector3 vector = new Vector3((float)state.X, (float)state.Y, 0f);
			Vector3 vector2 = new Vector3((float)state.X, (float)state.Y, 1f);
			Vector3 vector3 = base.GraphicsDevice.Viewport.Unproject(vector, this._camera.Projection, this._camera.View, Matrix.Identity);
			Vector3 vector4 = base.GraphicsDevice.Viewport.Unproject(vector2, this._camera.Projection, this._camera.View, Matrix.Identity);
			Vector3 vector5 = vector4 - vector3;
			vector5.Normalize();
			return new Ray(vector3, vector5);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005935 File Offset: 0x00003B35
		protected override void Draw(GameTime gameTime)
		{
			this._graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
			base.Draw(gameTime);
		}

		// Token: 0x04000064 RID: 100
		public List<object> ToDraw = new List<object>();

		// Token: 0x04000065 RID: 101
		private GraphicsDeviceManager _graphics;

		// Token: 0x04000066 RID: 102
		private Camera _camera;

		// Token: 0x04000067 RID: 103
		private int _lastMouseX;

		// Token: 0x04000068 RID: 104
		private int _lastMouseY;

		// Token: 0x04000069 RID: 105
		private bool _keyTest = false;

		// Token: 0x0400006A RID: 106
		private bool _wire = false;

		// Token: 0x0400006B RID: 107
		private DateTime _last = DateTime.Now;

		// Token: 0x0400006C RID: 108
		private int _fps;

		// Token: 0x0400006D RID: 109
		private bool _leftButtonDown = false;

		// Token: 0x0400006E RID: 110
		private DateTime lastLeftMouseClick = DateTime.MinValue;

		// Token: 0x04000070 RID: 112
		private object switchLock = new object();
	}
}

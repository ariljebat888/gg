using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;

namespace Viewer
{
	// Token: 0x02000008 RID: 8
	public class Window : IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000045 RID: 69 RVA: 0x00004C00 File Offset: 0x00002E00
		// (remove) Token: 0x06000046 RID: 70 RVA: 0x00004C3C File Offset: 0x00002E3C
		public event Action<Ray> OnMouseClick;

		// Token: 0x06000047 RID: 71 RVA: 0x00004C78 File Offset: 0x00002E78
		public void Run(bool reachProfile)
		{
			this.runner = new Thread(new ParameterizedThreadStart(this.ThreadStartP));
			this.runner.Start(reachProfile);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004CA4 File Offset: 0x00002EA4
		private void ThreadStartP(object reachProfile)
		{
			using (this.cont = new Game())
			{
				this.cont.SelectReachProfile((bool)reachProfile);
				this.cont.OnMouseClick += this.cont_OnMouseClick;
				this.cont.Run();
			}
			this.cont = null;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00004D24 File Offset: 0x00002F24
		private void cont_OnMouseClick(Ray obj)
		{
			if (this.OnMouseClick != null)
			{
				this.OnMouseClick(obj);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00004D4C File Offset: 0x00002F4C
		public Game Game
		{
			get
			{
				return this.cont;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00004D64 File Offset: 0x00002F64
		public bool IsActive
		{
			get
			{
				return this.cont != null;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004D84 File Offset: 0x00002F84
		public void SetCamera(BoundingBox box)
		{
			if (this.cont != null)
			{
				this.cont.SetCamera(box);
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004DAC File Offset: 0x00002FAC
		public void SyncObjects(IEnumerable<DrawableGameComponent> objs)
		{
			if (this.cont != null)
			{
				this.cont.SyncObjects(objs);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004DD4 File Offset: 0x00002FD4
		public void Show(IEnumerable<DrawableGameComponent> objs)
		{
			if (this.cont != null)
			{
				this.cont.Show(objs);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004E00 File Offset: 0x00003000
		public void Clear()
		{
			if (this.cont != null)
			{
				this.cont.Clear();
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004E27 File Offset: 0x00003027
		public void Dispose()
		{
			this.cont.Exit();
		}

		// Token: 0x04000061 RID: 97
		private Game cont = null;

		// Token: 0x04000062 RID: 98
		private Thread runner;
	}
}

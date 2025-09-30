using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Viewer
{
	// Token: 0x02000005 RID: 5
	public static class DDSLib
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00002AF0 File Offset: 0x00000CF0
		private static bool IsCubemapTest(int ddsCaps1, int ddsCaps2)
		{
			return (ddsCaps1 & 8) != 0 && (ddsCaps2 & 512) != 0;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002B18 File Offset: 0x00000D18
		private static bool IsVolumeTextureTest(int ddsCaps1, int ddsCaps2)
		{
			return (ddsCaps2 & 2097152) != 0;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002B38 File Offset: 0x00000D38
		private static bool IsCompressedTest(uint pfFlags)
		{
			return (pfFlags & 4U) != 0U;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002B54 File Offset: 0x00000D54
		private static bool HasAlphaTest(uint pfFlags)
		{
			return (pfFlags & 1U) != 0U;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002B70 File Offset: 0x00000D70
		private static int MipMapSize(int map, int size)
		{
			for (int i = 0; i < map; i++)
			{
				size >>= 1;
			}
			int num;
			if (size <= 0)
			{
				num = 1;
			}
			else
			{
				num = size;
			}
			return num;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002BA4 File Offset: 0x00000DA4
		private static DDSLib.LoadSurfaceFormat GetLoadSurfaceFormat(uint pixelFlags, uint pixelFourCC, int bitCount, uint rBitMask, uint gBitMask, uint bBitMask, uint aBitMask, DDSLib.FourCC compressionFormat)
		{
			DDSLib.LoadSurfaceFormat loadSurfaceFormat;
			if (pixelFourCC == 36U)
			{
				loadSurfaceFormat = DDSLib.LoadSurfaceFormat.A16B16G16R16;
			}
			else if (pixelFourCC == 115U)
			{
				loadSurfaceFormat = DDSLib.LoadSurfaceFormat.G32R32F;
			}
			else if (pixelFourCC == 112U)
			{
				loadSurfaceFormat = DDSLib.LoadSurfaceFormat.G16R16F;
			}
			else if (pixelFourCC == 63U)
			{
				loadSurfaceFormat = DDSLib.LoadSurfaceFormat.Q8W8V8U8;
			}
			else if (pixelFourCC == 117U)
			{
				loadSurfaceFormat = DDSLib.LoadSurfaceFormat.CxV8U8;
			}
			else if (pixelFourCC == 113U)
			{
				loadSurfaceFormat = DDSLib.LoadSurfaceFormat.A16B16G16R16F;
			}
			else if (pixelFourCC == 116U)
			{
				loadSurfaceFormat = DDSLib.LoadSurfaceFormat.A32B32G32R32F;
			}
			else if (pixelFourCC == 114U)
			{
				loadSurfaceFormat = DDSLib.LoadSurfaceFormat.R32F;
			}
			else if (pixelFourCC == 111U)
			{
				loadSurfaceFormat = DDSLib.LoadSurfaceFormat.R16F;
			}
			else
			{
				if ((pixelFlags & 4U) != 0U)
				{
					if (pixelFourCC == 827611204U)
					{
						return DDSLib.LoadSurfaceFormat.Dxt1;
					}
					if (pixelFourCC == 861165636U || pixelFourCC == 844388420U)
					{
						return DDSLib.LoadSurfaceFormat.Dxt3;
					}
					if (pixelFourCC == 894720068U || pixelFourCC == 877942852U)
					{
						return DDSLib.LoadSurfaceFormat.Dxt5;
					}
				}
				if ((pixelFlags & 64U) != 0U)
				{
					if (pixelFlags == 64U && bitCount == 16 && pixelFourCC == 0U && rBitMask == 31744U && gBitMask == 992U && bBitMask == 31U && aBitMask == 0U)
					{
						return DDSLib.LoadSurfaceFormat.RGB555;
					}
					if (pixelFlags == 65U && bitCount == 32 && pixelFourCC == 0U && rBitMask == 16711680U && gBitMask == 65280U && bBitMask == 255U && aBitMask == 4278190080U)
					{
						return DDSLib.LoadSurfaceFormat.A8R8G8B8;
					}
					if (pixelFlags == 64U && bitCount == 32 && pixelFourCC == 0U && rBitMask == 16711680U && gBitMask == 65280U && bBitMask == 255U && aBitMask == 0U)
					{
						return DDSLib.LoadSurfaceFormat.X8R8G8B8;
					}
					if (pixelFlags == 65U && bitCount == 32 && pixelFourCC == 0U && rBitMask == 255U && gBitMask == 65280U && bBitMask == 16711680U && aBitMask == 4278190080U)
					{
						return DDSLib.LoadSurfaceFormat.A8B8G8R8;
					}
					if (pixelFlags == 64U && bitCount == 32 && pixelFourCC == 0U && rBitMask == 255U && gBitMask == 65280U && bBitMask == 16711680U && aBitMask == 0U)
					{
						return DDSLib.LoadSurfaceFormat.X8B8G8R8;
					}
					if (pixelFlags == 65U && bitCount == 16 && pixelFourCC == 0U && rBitMask == 31744U && gBitMask == 992U && bBitMask == 31U && aBitMask == 32768U)
					{
						return DDSLib.LoadSurfaceFormat.Bgra5551;
					}
					if (pixelFlags == 65U && bitCount == 16 && pixelFourCC == 0U && rBitMask == 3840U && gBitMask == 240U && bBitMask == 15U && aBitMask == 61440U)
					{
						return DDSLib.LoadSurfaceFormat.Bgra4444;
					}
					if (pixelFlags == 64U && bitCount == 24 && pixelFourCC == 0U && rBitMask == 16711680U && gBitMask == 65280U && bBitMask == 255U && aBitMask == 0U)
					{
						return DDSLib.LoadSurfaceFormat.R8G8B8;
					}
					if (pixelFlags == 64U && bitCount == 16 && pixelFourCC == 0U && rBitMask == 63488U && gBitMask == 2016U && bBitMask == 31U && aBitMask == 0U)
					{
						return DDSLib.LoadSurfaceFormat.Bgr565;
					}
					if (pixelFlags == 2U && bitCount == 8 && pixelFourCC == 0U && rBitMask == 0U && gBitMask == 0U && bBitMask == 0U && aBitMask == 255U)
					{
						return DDSLib.LoadSurfaceFormat.Alpha8;
					}
					if (pixelFlags == 64U && bitCount == 32 && pixelFourCC == 0U && rBitMask == 65535U && gBitMask == 4294901760U && bBitMask == 0U && aBitMask == 0U)
					{
						return DDSLib.LoadSurfaceFormat.G16R16;
					}
					if (pixelFlags == 65U && bitCount == 32 && pixelFourCC == 0U && rBitMask == 1072693248U && gBitMask == 1047552U && bBitMask == 1023U && aBitMask == 3221225472U)
					{
						return DDSLib.LoadSurfaceFormat.A2B10G10R10;
					}
				}
				if (pixelFlags == 524288U && bitCount == 32 && (pixelFourCC == 0U || pixelFourCC == 63U) && rBitMask == 255U && gBitMask == 65280U && bBitMask == 16711680U && aBitMask == 4278190080U)
				{
					loadSurfaceFormat = DDSLib.LoadSurfaceFormat.Q8W8V8U8;
				}
				else
				{
					loadSurfaceFormat = DDSLib.LoadSurfaceFormat.Unknown;
				}
			}
			return loadSurfaceFormat;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003018 File Offset: 0x00001218
		private static DDSLib.FourCC GetCompressionFormat(uint pixelFlags, uint pixelFourCC)
		{
			DDSLib.FourCC fourCC;
			if ((pixelFlags & 4U) != 0U)
			{
				fourCC = (DDSLib.FourCC)pixelFourCC;
			}
			else
			{
				fourCC = (DDSLib.FourCC)0U;
			}
			return fourCC;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000303C File Offset: 0x0000123C
		private static int MipMapSizeInBytes(int map, int width, int height, bool isCompressed, DDSLib.FourCC compressionFormat, int depth)
		{
			width = DDSLib.MipMapSize(map, width);
			height = DDSLib.MipMapSize(map, height);
			int num;
			if (compressionFormat == DDSLib.FourCC.D3DFMT_R32F)
			{
				num = width * height * 4;
			}
			else if (compressionFormat == DDSLib.FourCC.D3DFMT_R16F)
			{
				num = width * height * 2;
			}
			else if (compressionFormat == DDSLib.FourCC.D3DFMT_A32B32G32R32F)
			{
				num = width * height * 16;
			}
			else if (compressionFormat == DDSLib.FourCC.D3DFMT_A16B16G16R16F)
			{
				num = width * height * 8;
			}
			else if (compressionFormat == DDSLib.FourCC.D3DFMT_CxV8U8)
			{
				num = width * height * 2;
			}
			else if (compressionFormat == DDSLib.FourCC.D3DFMT_Q8W8V8U8)
			{
				num = width * height * 4;
			}
			else if (compressionFormat == DDSLib.FourCC.D3DFMT_G16R16F)
			{
				num = width * height * 4;
			}
			else if (compressionFormat == DDSLib.FourCC.D3DFMT_G32R32F)
			{
				num = width * height * 8;
			}
			else if (compressionFormat == DDSLib.FourCC.D3DFMT_A16B16G16R16)
			{
				num = width * height * 8;
			}
			else if (isCompressed)
			{
				int num2 = ((compressionFormat == DDSLib.FourCC.D3DFMT_DXT1) ? 8 : 16);
				num = (width + 3) / 4 * ((height + 3) / 4) * num2;
			}
			else
			{
				num = width * height * (depth / 8);
			}
			return num;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003168 File Offset: 0x00001368
		private static void GetMipMaps(int offsetInStream, int map, bool hasMipMaps, int width, int height, bool isCompressed, DDSLib.FourCC compressionFormat, int rgbBitCount, bool partOfCubeMap, BinaryReader reader, DDSLib.LoadSurfaceFormat loadSurfaceFormat, ref byte[] data, out int numBytes)
		{
			int num = 128 + offsetInStream;
			for (int i = 0; i < map; i++)
			{
				num += DDSLib.MipMapSizeInBytes(i, width, height, isCompressed, compressionFormat, rgbBitCount);
			}
			reader.BaseStream.Seek((long)num, SeekOrigin.Begin);
			numBytes = DDSLib.MipMapSizeInBytes(map, width, height, isCompressed, compressionFormat, rgbBitCount);
			if (!isCompressed && rgbBitCount == 24)
			{
				numBytes += numBytes / 3;
			}
			if (data == null || data.Length < numBytes)
			{
				data = new byte[numBytes];
			}
			if (!isCompressed && loadSurfaceFormat == DDSLib.LoadSurfaceFormat.R8G8B8)
			{
				for (int i = 0; i < numBytes; i += 4)
				{
					data[i] = reader.ReadByte();
					data[i + 1] = reader.ReadByte();
					data[i + 2] = reader.ReadByte();
					data[i + 3] = byte.MaxValue;
				}
			}
			else
			{
				reader.Read(data, 0, numBytes);
			}
			if (loadSurfaceFormat == DDSLib.LoadSurfaceFormat.X8R8G8B8 || loadSurfaceFormat == DDSLib.LoadSurfaceFormat.X8B8G8R8)
			{
				for (int i = 0; i < numBytes; i += 4)
				{
					data[i + 3] = byte.MaxValue;
				}
			}
			if (loadSurfaceFormat == DDSLib.LoadSurfaceFormat.A8R8G8B8 || loadSurfaceFormat == DDSLib.LoadSurfaceFormat.X8R8G8B8 || loadSurfaceFormat == DDSLib.LoadSurfaceFormat.R8G8B8)
			{
				int num2 = ((rgbBitCount == 32 || rgbBitCount == 24) ? 4 : 3);
				if (num2 == 3)
				{
					for (int i = 0; i < numBytes - 2; i += 3)
					{
						byte b = data[i];
						byte b2 = data[i + 2];
						data[i] = b2;
						data[i + 2] = b;
					}
				}
				else
				{
					for (int i = 0; i < numBytes - 3; i += 4)
					{
						byte b = data[i];
						byte b2 = data[i + 2];
						data[i] = b2;
						data[i + 2] = b;
					}
				}
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003374 File Offset: 0x00001574
		private static bool CheckFullMipChain(int width, int height, int numMip)
		{
			int i = Math.Max(width, height);
			int num = 0;
			while (i > 1)
			{
				i /= 2;
				num++;
			}
			return num <= numMip;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000033B4 File Offset: 0x000015B4
		public static void DDSFromFile(string fileName, GraphicsDevice device, bool loadMipMap, out Texture2D texture)
		{
			Stream stream = File.OpenRead(fileName);
			Texture texture2;
			DDSLib.InternalDDSFromStream(stream, device, 0, loadMipMap, out texture2);
			stream.Close();
			texture = texture2 as Texture2D;
			if (texture == null)
			{
				throw new Exception("The data in the stream contains a TextureCube not Texture2D");
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000033FC File Offset: 0x000015FC
		public static void DDSFromFile(string fileName, GraphicsDevice device, bool loadMipMap, out TextureCube texture)
		{
			Stream stream = File.OpenRead(fileName);
			Texture texture2;
			DDSLib.InternalDDSFromStream(stream, device, 0, loadMipMap, out texture2);
			stream.Close();
			texture = texture2 as TextureCube;
			if (texture == null)
			{
				throw new Exception("The data in the stream contains a Texture2D not TextureCube");
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003444 File Offset: 0x00001644
		public static void DDSFromFile(string fileName, GraphicsDevice device, bool loadMipMap, out Texture3D texture)
		{
			Stream stream = File.OpenRead(fileName);
			Texture texture2;
			DDSLib.InternalDDSFromStream(stream, device, 0, loadMipMap, out texture2);
			stream.Close();
			texture = texture2 as Texture3D;
			if (texture == null)
			{
				throw new Exception("The data in the stream contains a Texture2D not TextureCube");
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000348C File Offset: 0x0000168C
		public static void DDSFromStream(Stream stream, GraphicsDevice device, int streamOffset, bool loadMipMap, out Texture2D texture)
		{
			Texture texture2;
			DDSLib.InternalDDSFromStream(stream, device, streamOffset, loadMipMap, out texture2);
			texture = texture2 as Texture2D;
			if (texture == null)
			{
				throw new Exception("The data in the stream contains a TextureCube not Texture2D");
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000034C8 File Offset: 0x000016C8
		public static void DDSFromStream(Stream stream, GraphicsDevice device, int streamOffset, bool loadMipMap, out TextureCube texture)
		{
			Texture texture2;
			DDSLib.InternalDDSFromStream(stream, device, streamOffset, loadMipMap, out texture2);
			texture = texture2 as TextureCube;
			if (texture == null)
			{
				throw new Exception("The data in the stream contains a Texture2D not TextureCube");
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003504 File Offset: 0x00001704
		public static void DDSFromStream(Stream stream, GraphicsDevice device, int streamOffset, bool loadMipMap, out Texture3D texture)
		{
			Texture texture2;
			DDSLib.InternalDDSFromStream(stream, device, streamOffset, loadMipMap, out texture2);
			texture = texture2 as Texture3D;
			if (texture == null)
			{
				throw new Exception("The data in the stream contains a Texture2D not TextureCube");
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003540 File Offset: 0x00001740
		private static SurfaceFormat SurfaceFormatFromLoadFormat(DDSLib.LoadSurfaceFormat loadSurfaceFormat, DDSLib.FourCC compressionFormat, uint pixelFlags, int rgbBitCount)
		{
			if (loadSurfaceFormat == DDSLib.LoadSurfaceFormat.Unknown)
			{
				if (compressionFormat <= DDSLib.FourCC.D3DFMT_DXT1)
				{
					if (compressionFormat != (DDSLib.FourCC)0U)
					{
						if (compressionFormat == DDSLib.FourCC.D3DFMT_DXT1)
						{
							return SurfaceFormat.Dxt1;
						}
					}
					else
					{
						if (rgbBitCount == 8)
						{
							return SurfaceFormat.Alpha8;
						}
						if (rgbBitCount == 16)
						{
							if (DDSLib.HasAlphaTest(pixelFlags))
							{
								return SurfaceFormat.Bgr565;
							}
							return SurfaceFormat.Bgra4444;
						}
						else
						{
							if (rgbBitCount == 32 || rgbBitCount == 24)
							{
								return SurfaceFormat.Color;
							}
							throw new Exception("Unsuported format");
						}
					}
				}
				else
				{
					if (compressionFormat == DDSLib.FourCC.D3DFMT_DXT3)
					{
						return SurfaceFormat.Dxt3;
					}
					if (compressionFormat == DDSLib.FourCC.D3DFMT_DXT5)
					{
						return SurfaceFormat.Dxt5;
					}
				}
				throw new Exception("Unsuported format");
			}
			switch (loadSurfaceFormat)
			{
			case DDSLib.LoadSurfaceFormat.Dxt1:
				return SurfaceFormat.Dxt1;
			case DDSLib.LoadSurfaceFormat.Dxt3:
				return SurfaceFormat.Dxt3;
			case DDSLib.LoadSurfaceFormat.Dxt5:
				return SurfaceFormat.Dxt5;
			case DDSLib.LoadSurfaceFormat.R8G8B8:
				return SurfaceFormat.Color;
			case DDSLib.LoadSurfaceFormat.Bgra5551:
				return SurfaceFormat.Bgra5551;
			case DDSLib.LoadSurfaceFormat.Bgra4444:
				return SurfaceFormat.Bgra4444;
			case DDSLib.LoadSurfaceFormat.Bgr565:
				return SurfaceFormat.Bgr565;
			case DDSLib.LoadSurfaceFormat.Alpha8:
				return SurfaceFormat.Alpha8;
			case DDSLib.LoadSurfaceFormat.X8R8G8B8:
				return SurfaceFormat.Color;
			case DDSLib.LoadSurfaceFormat.A8R8G8B8:
				return SurfaceFormat.Color;
			case DDSLib.LoadSurfaceFormat.A8B8G8R8:
				return SurfaceFormat.Color;
			case DDSLib.LoadSurfaceFormat.X8B8G8R8:
				return SurfaceFormat.Color;
			case DDSLib.LoadSurfaceFormat.R32F:
				return SurfaceFormat.Single;
			case DDSLib.LoadSurfaceFormat.R16F:
				return SurfaceFormat.HalfSingle;
			case DDSLib.LoadSurfaceFormat.A32B32G32R32F:
				return SurfaceFormat.Vector4;
			case DDSLib.LoadSurfaceFormat.A16B16G16R16F:
				return SurfaceFormat.HalfVector4;
			case DDSLib.LoadSurfaceFormat.Q8W8V8U8:
				return SurfaceFormat.NormalizedByte4;
			case DDSLib.LoadSurfaceFormat.CxV8U8:
				return SurfaceFormat.NormalizedByte2;
			case DDSLib.LoadSurfaceFormat.G16R16F:
				return SurfaceFormat.HalfVector2;
			case DDSLib.LoadSurfaceFormat.G32R32F:
				return SurfaceFormat.Vector2;
			case DDSLib.LoadSurfaceFormat.G16R16:
				return SurfaceFormat.Rg32;
			case DDSLib.LoadSurfaceFormat.A2B10G10R10:
				return SurfaceFormat.Rgba1010102;
			case DDSLib.LoadSurfaceFormat.A16B16G16R16:
				return SurfaceFormat.Rgba64;
			}
			throw new Exception(loadSurfaceFormat.ToString() + " is an unsuported format");
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003710 File Offset: 0x00001910
		private static TextureCube GenerateNewCubeTexture(DDSLib.LoadSurfaceFormat loadSurfaceFormat, DDSLib.FourCC compressionFormat, GraphicsDevice device, int width, bool hasMipMaps, uint pixelFlags, int rgbBitCount)
		{
			SurfaceFormat surfaceFormat = DDSLib.SurfaceFormatFromLoadFormat(loadSurfaceFormat, compressionFormat, pixelFlags, rgbBitCount);
			TextureCube textureCube = new TextureCube(device, width, hasMipMaps, surfaceFormat);
			if (textureCube.Format != surfaceFormat)
			{
				throw new Exception("Can't generate a " + surfaceFormat.ToString() + " surface.");
			}
			return textureCube;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003768 File Offset: 0x00001968
		private static Texture2D GenerateNewTexture2D(DDSLib.LoadSurfaceFormat loadSurfaceFormat, DDSLib.FourCC compressionFormat, GraphicsDevice device, int width, int height, bool hasMipMaps, uint pixelFlags, int rgbBitCount)
		{
			SurfaceFormat surfaceFormat = DDSLib.SurfaceFormatFromLoadFormat(loadSurfaceFormat, compressionFormat, pixelFlags, rgbBitCount);
			Texture2D texture2D = new Texture2D(device, width, height, hasMipMaps, surfaceFormat);
			if (texture2D.Format != surfaceFormat)
			{
				throw new Exception("Can't generate a " + surfaceFormat.ToString() + " surface.");
			}
			return texture2D;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000037C4 File Offset: 0x000019C4
		private static Texture3D GenerateNewTexture3D(DDSLib.LoadSurfaceFormat loadSurfaceFormat, DDSLib.FourCC compressionFormat, GraphicsDevice device, int width, int height, int depth, bool hasMipMaps, uint pixelFlags, int rgbBitCount)
		{
			SurfaceFormat surfaceFormat = DDSLib.SurfaceFormatFromLoadFormat(loadSurfaceFormat, compressionFormat, pixelFlags, rgbBitCount);
			Texture3D texture3D = new Texture3D(device, width, height, depth, hasMipMaps, surfaceFormat);
			if (texture3D.Format != surfaceFormat)
			{
				throw new Exception("Can't generate a " + surfaceFormat.ToString() + " surface.");
			}
			return texture3D;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003820 File Offset: 0x00001A20
		private static void InternalDDSFromStream(Stream stream, GraphicsDevice device, int streamOffset, bool loadMipMap, out Texture texture)
		{
			if (stream == null)
			{
				throw new Exception("Can't read from a null stream");
			}
			BinaryReader binaryReader = new BinaryReader(stream);
			if ((long)streamOffset > binaryReader.BaseStream.Length)
			{
				throw new Exception("The stream you offered is smaller then the offset you are proposing for it.");
			}
			binaryReader.BaseStream.Seek((long)streamOffset, SeekOrigin.Begin);
			if (binaryReader.ReadUInt32() != 542327876U)
			{
				throw new Exception("Can't open non DDS data.");
			}
			binaryReader.BaseStream.Position += 8L;
			int num = binaryReader.ReadInt32();
			int num2 = binaryReader.ReadInt32();
			binaryReader.BaseStream.Position += 4L;
			int num3 = binaryReader.ReadInt32();
			int num4 = binaryReader.ReadInt32();
			binaryReader.BaseStream.Position += 48L;
			uint num5 = binaryReader.ReadUInt32();
			uint num6 = binaryReader.ReadUInt32();
			int num7 = binaryReader.ReadInt32();
			uint num8 = binaryReader.ReadUInt32();
			uint num9 = binaryReader.ReadUInt32();
			uint num10 = binaryReader.ReadUInt32();
			uint num11 = binaryReader.ReadUInt32();
			int num12 = binaryReader.ReadInt32();
			int num13 = binaryReader.ReadInt32();
			binaryReader.BaseStream.Position += 12L;
			bool flag = DDSLib.IsCubemapTest(num12, num13);
			bool flag2 = DDSLib.IsVolumeTextureTest(num12, num13);
			DDSLib.FourCC compressionFormat = DDSLib.GetCompressionFormat(num5, num6);
			if (compressionFormat == DDSLib.FourCC.DX10)
			{
				throw new Exception("The Dxt 10 header reader is not implemented");
			}
			DDSLib.LoadSurfaceFormat loadSurfaceFormat = DDSLib.GetLoadSurfaceFormat(num5, num6, num7, num8, num9, num10, num11, compressionFormat);
			bool flag3 = DDSLib.IsCompressedTest(num5);
			bool flag4 = DDSLib.CheckFullMipChain(num2, num, num4);
			bool flag5 = num4 > 0;
			flag4 = flag4 && loadMipMap;
			if (flag)
			{
				TextureCube textureCube = DDSLib.GenerateNewCubeTexture(loadSurfaceFormat, compressionFormat, device, num2, flag4, num5, num7);
				int num14 = 0;
				if (num4 == 0)
				{
					num4 = 1;
				}
				if (!flag4)
				{
					for (int i = 0; i < num4; i++)
					{
						num14 += DDSLib.MipMapSizeInBytes(i, num2, num, flag3, compressionFormat, num7);
					}
				}
				for (int i = 0; i < num4; i++)
				{
					int num15 = 0;
					byte[] array = DDSLib.mipData;
					DDSLib.GetMipMaps(streamOffset, i, flag5, num2, num, flag3, compressionFormat, num7, flag, binaryReader, loadSurfaceFormat, ref array, out num15);
					DDSLib.mipData = array;
					if (flag4)
					{
						num14 += num15;
					}
					if (i != 0 && !flag4)
					{
						break;
					}
					textureCube.SetData<byte>(CubeMapFace.PositiveX, i, null, array, 0, num15);
				}
				for (int i = 0; i < num4; i++)
				{
					int num15 = 0;
					byte[] array = DDSLib.mipData;
					DDSLib.GetMipMaps(num14 + streamOffset, i, flag5, num2, num, flag3, compressionFormat, num7, flag, binaryReader, loadSurfaceFormat, ref array, out num15);
					DDSLib.mipData = array;
					if (i != 0 && !flag4)
					{
						break;
					}
					textureCube.SetData<byte>(CubeMapFace.NegativeX, i, null, array, 0, num15);
				}
				for (int i = 0; i < num4; i++)
				{
					int num15 = 0;
					byte[] array = DDSLib.mipData;
					DDSLib.GetMipMaps(num14 * 2 + streamOffset, i, flag5, num2, num, flag3, compressionFormat, num7, flag, binaryReader, loadSurfaceFormat, ref array, out num15);
					DDSLib.mipData = array;
					if (i != 0 && !flag4)
					{
						break;
					}
					textureCube.SetData<byte>(CubeMapFace.PositiveY, i, null, array, 0, num15);
				}
				for (int i = 0; i < num4; i++)
				{
					int num15 = 0;
					byte[] array = DDSLib.mipData;
					DDSLib.GetMipMaps(num14 * 3 + streamOffset, i, flag5, num2, num, flag3, compressionFormat, num7, flag, binaryReader, loadSurfaceFormat, ref array, out num15);
					DDSLib.mipData = array;
					if (i != 0 && !flag4)
					{
						break;
					}
					textureCube.SetData<byte>(CubeMapFace.NegativeY, i, null, array, 0, num15);
				}
				for (int i = 0; i < num4; i++)
				{
					int num15 = 0;
					byte[] array = DDSLib.mipData;
					DDSLib.GetMipMaps(num14 * 4 + streamOffset, i, flag5, num2, num, flag3, compressionFormat, num7, flag, binaryReader, loadSurfaceFormat, ref array, out num15);
					DDSLib.mipData = array;
					if (i != 0 && !flag4)
					{
						break;
					}
					textureCube.SetData<byte>(CubeMapFace.PositiveZ, i, null, array, 0, num15);
				}
				for (int i = 0; i < num4; i++)
				{
					int num15 = 0;
					byte[] array = DDSLib.mipData;
					DDSLib.GetMipMaps(num14 * 5 + streamOffset, i, flag5, num2, num, flag3, compressionFormat, num7, flag, binaryReader, loadSurfaceFormat, ref array, out num15);
					DDSLib.mipData = array;
					if (i != 0 && !flag4)
					{
						break;
					}
					textureCube.SetData<byte>(CubeMapFace.NegativeZ, i, null, array, 0, num15);
				}
				texture = textureCube;
			}
			else if (flag2)
			{
				Texture3D texture3D = DDSLib.GenerateNewTexture3D(loadSurfaceFormat, compressionFormat, device, num2, num, num3, flag4, num5, num7);
				int num16 = streamOffset;
				for (int j = 0; j < texture3D.LevelCount; j++)
				{
					int num17 = DDSLib.MipMapSize(j, num2);
					int num18 = DDSLib.MipMapSize(j, num);
					int num19 = DDSLib.MipMapSize(j, num3);
					for (int i = 0; i < num19; i++)
					{
						int num15 = 0;
						byte[] array = DDSLib.mipData;
						DDSLib.GetMipMaps(num16, 0, flag5, num17, num18, flag3, compressionFormat, num7, flag, binaryReader, loadSurfaceFormat, ref array, out num15);
						num16 += num15;
						DDSLib.mipData = array;
						texture3D.SetData<byte>(j, 0, 0, num17, num18, i, i + 1, array, 0, num15);
					}
				}
				texture = texture3D;
			}
			else
			{
				Texture2D texture2D = DDSLib.GenerateNewTexture2D(loadSurfaceFormat, compressionFormat, device, num2, num, flag4, num5, num7);
				for (int j = 0; j < texture2D.LevelCount; j++)
				{
					int num15 = 0;
					byte[] array = DDSLib.mipData;
					DDSLib.GetMipMaps(streamOffset, j, flag5, num2, num, flag3, compressionFormat, num7, flag, binaryReader, loadSurfaceFormat, ref array, out num15);
					DDSLib.mipData = array;
					texture2D.SetData<byte>(j, null, array, 0, num15);
				}
				texture = texture2D;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003EB0 File Offset: 0x000020B0
		private static bool IsXNATextureCompressed(Texture texture)
		{
			return texture.Format == SurfaceFormat.Dxt1 || texture.Format == SurfaceFormat.Dxt3 || texture.Format == SurfaceFormat.Dxt5;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003EF0 File Offset: 0x000020F0
		private static DDSLib.FourCC XNATextureFourCC(Texture texture)
		{
			DDSLib.FourCC fourCC;
			if (texture.Format == SurfaceFormat.Rgba64)
			{
				fourCC = DDSLib.FourCC.D3DFMT_A16B16G16R16;
			}
			else if (texture.Format == SurfaceFormat.Vector4)
			{
				fourCC = DDSLib.FourCC.D3DFMT_A32B32G32R32F;
			}
			else if (texture.Format == SurfaceFormat.Vector2)
			{
				fourCC = DDSLib.FourCC.D3DFMT_G32R32F;
			}
			else if (texture.Format == SurfaceFormat.HalfVector2)
			{
				fourCC = DDSLib.FourCC.D3DFMT_G16R16F;
			}
			else if (texture.Format == SurfaceFormat.NormalizedByte4)
			{
				fourCC = DDSLib.FourCC.D3DFMT_Q8W8V8U8;
			}
			else if (texture.Format == SurfaceFormat.NormalizedByte2)
			{
				fourCC = DDSLib.FourCC.D3DFMT_CxV8U8;
			}
			else if (texture.Format == SurfaceFormat.HalfVector4)
			{
				fourCC = DDSLib.FourCC.D3DFMT_A16B16G16R16F;
			}
			else if (texture.Format == SurfaceFormat.Single)
			{
				fourCC = DDSLib.FourCC.D3DFMT_R32F;
			}
			else if (texture.Format == SurfaceFormat.HalfSingle)
			{
				fourCC = DDSLib.FourCC.D3DFMT_R16F;
			}
			else if (texture.Format == SurfaceFormat.Dxt1)
			{
				fourCC = DDSLib.FourCC.D3DFMT_DXT1;
			}
			else if (texture.Format == SurfaceFormat.Dxt3)
			{
				fourCC = DDSLib.FourCC.D3DFMT_DXT3;
			}
			else if (texture.Format == SurfaceFormat.Dxt5)
			{
				fourCC = DDSLib.FourCC.D3DFMT_DXT5;
			}
			else
			{
				fourCC = (DDSLib.FourCC)0U;
			}
			return fourCC;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004030 File Offset: 0x00002230
		private static int XNATextureColorDepth(Texture texture)
		{
			return DDSLib.XNATextureNumBytesPerPixel(texture) * 8;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000404C File Offset: 0x0000224C
		private static int XNATextureNumBytesPerPixel(Texture texture)
		{
			int num;
			switch (texture.Format)
			{
			case SurfaceFormat.Color:
			case SurfaceFormat.NormalizedByte4:
			case SurfaceFormat.Rgba1010102:
			case SurfaceFormat.Rg32:
			case SurfaceFormat.Single:
			case SurfaceFormat.HalfVector2:
				num = 4;
				break;
			case SurfaceFormat.Bgr565:
			case SurfaceFormat.Bgra5551:
			case SurfaceFormat.Bgra4444:
			case SurfaceFormat.NormalizedByte2:
			case SurfaceFormat.HalfSingle:
				num = 2;
				break;
			case SurfaceFormat.Dxt1:
			case SurfaceFormat.Dxt3:
			case SurfaceFormat.Dxt5:
				num = 0;
				break;
			case SurfaceFormat.Rgba64:
			case SurfaceFormat.Vector2:
			case SurfaceFormat.HalfVector4:
				num = 8;
				break;
			case SurfaceFormat.Alpha8:
				num = 1;
				break;
			case SurfaceFormat.Vector4:
				num = 16;
				break;
			default:
				throw new Exception(texture.Format + " has no save as DDS support.");
			}
			return num;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000040F0 File Offset: 0x000022F0
		private static void GenerateDdspf(SurfaceFormat fileFormat, out uint flags, out uint rgbBitCount, out uint rBitMask, out uint gBitMask, out uint bBitMask, out uint aBitMask, out uint fourCC)
		{
			switch (fileFormat)
			{
			case SurfaceFormat.Color:
				flags = 65U;
				rgbBitCount = 32U;
				fourCC = 0U;
				rBitMask = 16711680U;
				gBitMask = 65280U;
				bBitMask = 255U;
				aBitMask = 4278190080U;
				break;
			case SurfaceFormat.Bgr565:
				flags = 64U;
				fourCC = 0U;
				rgbBitCount = 16U;
				rBitMask = 63488U;
				gBitMask = 2016U;
				bBitMask = 31U;
				aBitMask = 0U;
				break;
			case SurfaceFormat.Bgra5551:
				flags = 65U;
				rgbBitCount = 16U;
				fourCC = 0U;
				rBitMask = 31744U;
				gBitMask = 992U;
				bBitMask = 31U;
				aBitMask = 32768U;
				break;
			case SurfaceFormat.Bgra4444:
				flags = 65U;
				rgbBitCount = 16U;
				fourCC = 0U;
				rBitMask = 3840U;
				gBitMask = 240U;
				bBitMask = 15U;
				aBitMask = 61440U;
				break;
			case SurfaceFormat.Dxt1:
			case SurfaceFormat.Dxt3:
			case SurfaceFormat.Dxt5:
				flags = 4U;
				rgbBitCount = 0U;
				rBitMask = 0U;
				gBitMask = 0U;
				bBitMask = 0U;
				aBitMask = 0U;
				fourCC = 0U;
				if (fileFormat == SurfaceFormat.Dxt1)
				{
					fourCC = 827611204U;
				}
				if (fileFormat == SurfaceFormat.Dxt3)
				{
					fourCC = 861165636U;
				}
				if (fileFormat == SurfaceFormat.Dxt5)
				{
					fourCC = 894720068U;
				}
				break;
			case SurfaceFormat.NormalizedByte2:
				flags = 4U;
				fourCC = 117U;
				rgbBitCount = 16U;
				rBitMask = 255U;
				gBitMask = 65280U;
				bBitMask = 0U;
				aBitMask = 0U;
				break;
			case SurfaceFormat.NormalizedByte4:
				flags = 524288U;
				fourCC = 63U;
				rgbBitCount = 32U;
				rBitMask = 255U;
				gBitMask = 65280U;
				bBitMask = 16711680U;
				aBitMask = 4278190080U;
				break;
			case SurfaceFormat.Rgba1010102:
				flags = 65U;
				fourCC = 0U;
				rgbBitCount = 32U;
				rBitMask = 1072693248U;
				gBitMask = 1047552U;
				bBitMask = 1023U;
				aBitMask = 3221225472U;
				break;
			case SurfaceFormat.Rg32:
				flags = 64U;
				fourCC = 0U;
				rgbBitCount = 32U;
				rBitMask = 65535U;
				gBitMask = 4294901760U;
				bBitMask = 0U;
				aBitMask = 0U;
				break;
			case SurfaceFormat.Rgba64:
				flags = 4U;
				fourCC = 36U;
				rgbBitCount = 64U;
				rBitMask = 0U;
				gBitMask = 0U;
				bBitMask = 0U;
				aBitMask = 0U;
				break;
			case SurfaceFormat.Alpha8:
				flags = 2U;
				fourCC = 0U;
				rgbBitCount = 8U;
				rBitMask = 0U;
				gBitMask = 0U;
				bBitMask = 0U;
				aBitMask = 255U;
				break;
			case SurfaceFormat.Single:
				flags = 4U;
				fourCC = 114U;
				rgbBitCount = 0U;
				rBitMask = 0U;
				gBitMask = 0U;
				bBitMask = 0U;
				aBitMask = 0U;
				break;
			case SurfaceFormat.Vector2:
				flags = 4U;
				fourCC = 115U;
				rgbBitCount = 0U;
				rBitMask = 0U;
				gBitMask = 0U;
				bBitMask = 0U;
				aBitMask = 0U;
				break;
			case SurfaceFormat.Vector4:
				flags = 4U;
				fourCC = 116U;
				rgbBitCount = 0U;
				rBitMask = 0U;
				gBitMask = 0U;
				bBitMask = 0U;
				aBitMask = 0U;
				break;
			case SurfaceFormat.HalfSingle:
				flags = 4U;
				fourCC = 111U;
				rgbBitCount = 0U;
				rBitMask = 0U;
				gBitMask = 0U;
				bBitMask = 0U;
				aBitMask = 0U;
				break;
			case SurfaceFormat.HalfVector2:
				flags = 4U;
				fourCC = 112U;
				rgbBitCount = 0U;
				rBitMask = 0U;
				gBitMask = 0U;
				bBitMask = 0U;
				aBitMask = 0U;
				break;
			case SurfaceFormat.HalfVector4:
				flags = 4U;
				fourCC = 113U;
				rgbBitCount = 0U;
				rBitMask = 0U;
				gBitMask = 0U;
				bBitMask = 0U;
				aBitMask = 0U;
				break;
			default:
				throw new Exception("Unsuported format");
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00004418 File Offset: 0x00002618
		private static void WriteTexture(BinaryWriter writer, CubeMapFace face, Texture texture, bool saveMipMaps, int width, int height, bool isCompressed, DDSLib.FourCC fourCC, int rgbBitCount)
		{
			int num = texture.LevelCount;
			num = (saveMipMaps ? num : 1);
			for (int i = 0; i < num; i++)
			{
				int num2 = DDSLib.MipMapSizeInBytes(i, width, height, isCompressed, fourCC, rgbBitCount);
				byte[] array = DDSLib.mipData;
				if (array == null || array.Length < num2)
				{
					array = new byte[num2];
				}
				if (texture is TextureCube)
				{
					(texture as TextureCube).GetData<byte>(face, i, null, array, 0, num2);
				}
				if (texture is Texture2D)
				{
					(texture as Texture2D).GetData<byte>(i, null, array, 0, num2);
				}
				if (texture.Format == SurfaceFormat.Color)
				{
					for (int j = 0; j < num2 - 3; j += 4)
					{
						byte b = array[j];
						byte b2 = array[j + 2];
						array[j] = b2;
						array[j + 2] = b;
					}
				}
				writer.Write(array, 0, num2);
				DDSLib.mipData = array;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000453C File Offset: 0x0000273C
		private static void WriteTexture(BinaryWriter writer, CubeMapFace face, Texture texture, int mipLevel, int depth, int width, int height, bool isCompressed, DDSLib.FourCC fourCC, int rgbBitCount)
		{
			int num = DDSLib.MipMapSizeInBytes(mipLevel, width, height, isCompressed, fourCC, rgbBitCount);
			byte[] array = DDSLib.mipData;
			if (array == null || array.Length < num)
			{
				array = new byte[num];
			}
			if (texture is TextureCube)
			{
				(texture as TextureCube).GetData<byte>(face, mipLevel, null, array, 0, num);
			}
			if (texture is Texture2D)
			{
				(texture as Texture2D).GetData<byte>(mipLevel, null, array, 0, num);
			}
			if (texture is Texture3D)
			{
				Texture3D texture3D = texture as Texture3D;
				int num2 = DDSLib.MipMapSize(mipLevel, width);
				int num3 = DDSLib.MipMapSize(mipLevel, height);
				texture3D.GetData<byte>(mipLevel, 0, 0, num2, num3, depth, depth + 1, array, 0, num);
			}
			if (texture.Format == SurfaceFormat.Color)
			{
				for (int i = 0; i < num - 3; i += 4)
				{
					byte b = array[i];
					byte b2 = array[i + 2];
					array[i] = b2;
					array[i + 2] = b;
				}
			}
			writer.Write(array, 0, num);
			DDSLib.mipData = array;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000467C File Offset: 0x0000287C
		public static void DDSToStream(Stream stream, int streamOffset, bool saveMipMaps, Texture texture)
		{
			if (stream == null)
			{
				throw new Exception("Can't write to a null stream");
			}
			if (texture == null || texture.IsDisposed)
			{
				throw new Exception("Can't read from a null texture.");
			}
			Texture2D texture2D = texture as Texture2D;
			Texture3D texture3D = texture as Texture3D;
			TextureCube textureCube = texture as TextureCube;
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			binaryWriter.BaseStream.Seek((long)streamOffset, SeekOrigin.Begin);
			binaryWriter.Write(542327876U);
			binaryWriter.Write(124);
			int num = 4103;
			bool flag = DDSLib.IsXNATextureCompressed(texture);
			if (!flag)
			{
				num |= 8;
			}
			else
			{
				num |= 524288;
			}
			if (texture.LevelCount > 1 && saveMipMaps)
			{
				num |= 131072;
			}
			if (texture3D != null)
			{
				num |= 8388608;
			}
			binaryWriter.Write(num);
			int num2 = 1;
			int num3 = 1;
			if (texture2D != null)
			{
				num2 = texture2D.Width;
				num3 = texture2D.Height;
			}
			if (texture3D != null)
			{
				num2 = texture3D.Width;
				num3 = texture3D.Height;
			}
			if (textureCube != null)
			{
				num2 = textureCube.Size;
				num3 = textureCube.Size;
			}
			binaryWriter.Write(num3);
			binaryWriter.Write(num2);
			uint num6;
			if (flag)
			{
				int num4 = (num2 + 3) / 4 * ((num3 + 3) / 4);
				int num5 = ((texture.Format != SurfaceFormat.Dxt1) ? 8 : 16);
				num6 = (uint)(num4 * num5);
			}
			else
			{
				num6 = (uint)(num2 * DDSLib.XNATextureNumBytesPerPixel(texture));
			}
			binaryWriter.Write(num6);
			if (texture3D != null)
			{
				binaryWriter.Write(texture3D.Depth);
			}
			else
			{
				binaryWriter.Write(0);
			}
			int num7 = ((texture.LevelCount == 1) ? 0 : texture.LevelCount);
			if (!saveMipMaps)
			{
				num7 = 0;
			}
			binaryWriter.Write(num7);
			for (int i = 0; i < 11; i++)
			{
				binaryWriter.Write(0);
			}
			uint num8;
			uint num9;
			uint num10;
			uint num11;
			uint num12;
			uint num13;
			uint num14;
			DDSLib.GenerateDdspf(texture.Format, out num8, out num9, out num10, out num11, out num12, out num13, out num14);
			binaryWriter.Write(32);
			binaryWriter.Write(num8);
			binaryWriter.Write(num14);
			binaryWriter.Write(num9);
			binaryWriter.Write(num10);
			binaryWriter.Write(num11);
			binaryWriter.Write(num12);
			binaryWriter.Write(num13);
			uint num15 = 4096U;
			if (texture.LevelCount > 1 && saveMipMaps)
			{
				num15 |= 4194304U;
				num15 |= 8U;
			}
			if (textureCube != null || texture3D != null)
			{
				num15 |= 8U;
			}
			binaryWriter.Write(num15);
			uint num16 = 0U;
			if (textureCube != null)
			{
				num16 |= 512U;
				num16 |= 2048U;
				num16 |= 8192U;
				num16 |= 32768U;
				num16 |= 1024U;
				num16 |= 4096U;
				num16 |= 16384U;
			}
			if (texture3D != null)
			{
				num16 |= 2097152U;
			}
			binaryWriter.Write(num16);
			for (int i = 0; i < 3; i++)
			{
				binaryWriter.Write(0);
			}
			if (texture2D != null)
			{
				DDSLib.WriteTexture(binaryWriter, CubeMapFace.PositiveX, texture, saveMipMaps, num2, num3, flag, (DDSLib.FourCC)num14, (int)num9);
			}
			if (textureCube != null)
			{
				DDSLib.WriteTexture(binaryWriter, CubeMapFace.PositiveX, texture, saveMipMaps, num2, num3, flag, (DDSLib.FourCC)num14, (int)num9);
				DDSLib.WriteTexture(binaryWriter, CubeMapFace.NegativeX, texture, saveMipMaps, num2, num3, flag, (DDSLib.FourCC)num14, (int)num9);
				DDSLib.WriteTexture(binaryWriter, CubeMapFace.PositiveY, texture, saveMipMaps, num2, num3, flag, (DDSLib.FourCC)num14, (int)num9);
				DDSLib.WriteTexture(binaryWriter, CubeMapFace.NegativeY, texture, saveMipMaps, num2, num3, flag, (DDSLib.FourCC)num14, (int)num9);
				DDSLib.WriteTexture(binaryWriter, CubeMapFace.PositiveZ, texture, saveMipMaps, num2, num3, flag, (DDSLib.FourCC)num14, (int)num9);
				DDSLib.WriteTexture(binaryWriter, CubeMapFace.NegativeZ, texture, saveMipMaps, num2, num3, flag, (DDSLib.FourCC)num14, (int)num9);
			}
			if (texture3D != null)
			{
				for (int i = 0; i < texture3D.LevelCount; i++)
				{
					int num17 = DDSLib.MipMapSize(i, texture3D.Depth);
					for (int j = 0; j < num17; j++)
					{
						DDSLib.WriteTexture(binaryWriter, CubeMapFace.PositiveX, texture, i, j, num2, num3, flag, (DDSLib.FourCC)num14, (int)num9);
					}
				}
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004AF0 File Offset: 0x00002CF0
		public static void DDSToFile(string fileName, bool saveMipMaps, Texture texture, bool throwExceptionIfFileExist)
		{
			if (throwExceptionIfFileExist && File.Exists(fileName))
			{
				throw new Exception("The file allready exists and \"throwExceptionIfFileExist\" is true");
			}
			Stream stream = null;
			try
			{
				stream = File.Create(fileName);
				DDSLib.DDSToStream(stream, 0, saveMipMaps, texture);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
					stream = null;
				}
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004B70 File Offset: 0x00002D70
		public static int GetDataByteSize(Texture2D texture, int mipMapLevel)
		{
			int num;
			if (texture.LevelCount <= mipMapLevel)
			{
				num = -1;
			}
			else
			{
				num = DDSLib.MipMapSizeInBytes(mipMapLevel, texture.Width, texture.Height, DDSLib.IsXNATextureCompressed(texture), DDSLib.XNATextureFourCC(texture), DDSLib.XNATextureColorDepth(texture));
			}
			return num;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004BB8 File Offset: 0x00002DB8
		public static int GetDataByteSize(TextureCube texture, int mipMapLevel)
		{
			int num;
			if (texture.LevelCount <= mipMapLevel)
			{
				num = -1;
			}
			else
			{
				num = DDSLib.MipMapSizeInBytes(mipMapLevel, texture.Size, texture.Size, DDSLib.IsXNATextureCompressed(texture), DDSLib.XNATextureFourCC(texture), DDSLib.XNATextureColorDepth(texture));
			}
			return num;
		}

		// Token: 0x04000010 RID: 16
		private const int DDSD_CAPS = 1;

		// Token: 0x04000011 RID: 17
		private const int DDSD_HEIGHT = 2;

		// Token: 0x04000012 RID: 18
		private const int DDSD_WIDTH = 4;

		// Token: 0x04000013 RID: 19
		private const int DDSD_PITCH = 8;

		// Token: 0x04000014 RID: 20
		private const int DDSD_PIXELFORMAT = 4096;

		// Token: 0x04000015 RID: 21
		private const int DDSD_MIPMAPCOUNT = 131072;

		// Token: 0x04000016 RID: 22
		private const int DDSD_LINEARSIZE = 524288;

		// Token: 0x04000017 RID: 23
		private const int DDSD_DEPTH = 8388608;

		// Token: 0x04000018 RID: 24
		private const int DDPF_ALPHAPIXELS = 1;

		// Token: 0x04000019 RID: 25
		private const int DDPF_ALPHA = 2;

		// Token: 0x0400001A RID: 26
		private const int DDPF_FOURCC = 4;

		// Token: 0x0400001B RID: 27
		private const int DDPF_RGB = 64;

		// Token: 0x0400001C RID: 28
		private const int DDPF_YUV = 512;

		// Token: 0x0400001D RID: 29
		private const int DDPF_LUMINANCE = 8192;

		// Token: 0x0400001E RID: 30
		private const int DDPF_Q8W8V8U8 = 524288;

		// Token: 0x0400001F RID: 31
		private const int DDSCAPS_COMPLEX = 8;

		// Token: 0x04000020 RID: 32
		private const int DDSCAPS_MIPMAP = 4194304;

		// Token: 0x04000021 RID: 33
		private const int DDSCAPS_TEXTURE = 4096;

		// Token: 0x04000022 RID: 34
		private const int DDSCAPS2_CUBEMAP = 512;

		// Token: 0x04000023 RID: 35
		private const int DDSCAPS2_CUBEMAP_POSITIVEX = 1024;

		// Token: 0x04000024 RID: 36
		private const int DDSCAPS2_CUBEMAP_NEGATIVEX = 2048;

		// Token: 0x04000025 RID: 37
		private const int DDSCAPS2_CUBEMAP_POSITIVEY = 4096;

		// Token: 0x04000026 RID: 38
		private const int DDSCAPS2_CUBEMAP_NEGATIVEY = 8192;

		// Token: 0x04000027 RID: 39
		private const int DDSCAPS2_CUBEMAP_POSITIVEZ = 16384;

		// Token: 0x04000028 RID: 40
		private const int DDSCAPS2_CUBEMAP_NEGATIVEZ = 32768;

		// Token: 0x04000029 RID: 41
		private const int DDSCAPS2_VOLUME = 2097152;

		// Token: 0x0400002A RID: 42
		private const uint DDS_MAGIC = 542327876U;

		// Token: 0x0400002B RID: 43
		[ThreadStatic]
		private static byte[] mipData;

		// Token: 0x02000006 RID: 6
		[Flags]
		private enum FourCC : uint
		{
			// Token: 0x0400002D RID: 45
			D3DFMT_DXT1 = 827611204U,
			// Token: 0x0400002E RID: 46
			D3DFMT_DXT2 = 844388420U,
			// Token: 0x0400002F RID: 47
			D3DFMT_DXT3 = 861165636U,
			// Token: 0x04000030 RID: 48
			D3DFMT_DXT4 = 877942852U,
			// Token: 0x04000031 RID: 49
			D3DFMT_DXT5 = 894720068U,
			// Token: 0x04000032 RID: 50
			DX10 = 808540228U,
			// Token: 0x04000033 RID: 51
			DXGI_FORMAT_BC4_UNORM = 1429488450U,
			// Token: 0x04000034 RID: 52
			DXGI_FORMAT_BC4_SNORM = 1395934018U,
			// Token: 0x04000035 RID: 53
			DXGI_FORMAT_BC5_UNORM = 843666497U,
			// Token: 0x04000036 RID: 54
			DXGI_FORMAT_BC5_SNORM = 1395999554U,
			// Token: 0x04000037 RID: 55
			D3DFMT_R8G8_B8G8 = 1195525970U,
			// Token: 0x04000038 RID: 56
			D3DFMT_G8R8_G8B8 = 1111970375U,
			// Token: 0x04000039 RID: 57
			D3DFMT_A16B16G16R16 = 36U,
			// Token: 0x0400003A RID: 58
			D3DFMT_Q16W16V16U16 = 110U,
			// Token: 0x0400003B RID: 59
			D3DFMT_R16F = 111U,
			// Token: 0x0400003C RID: 60
			D3DFMT_G16R16F = 112U,
			// Token: 0x0400003D RID: 61
			D3DFMT_A16B16G16R16F = 113U,
			// Token: 0x0400003E RID: 62
			D3DFMT_R32F = 114U,
			// Token: 0x0400003F RID: 63
			D3DFMT_G32R32F = 115U,
			// Token: 0x04000040 RID: 64
			D3DFMT_A32B32G32R32F = 116U,
			// Token: 0x04000041 RID: 65
			D3DFMT_UYVY = 1498831189U,
			// Token: 0x04000042 RID: 66
			D3DFMT_YUY2 = 844715353U,
			// Token: 0x04000043 RID: 67
			D3DFMT_CxV8U8 = 117U,
			// Token: 0x04000044 RID: 68
			D3DFMT_Q8W8V8U8 = 63U
		}

		// Token: 0x02000007 RID: 7
		private enum LoadSurfaceFormat
		{
			// Token: 0x04000046 RID: 70
			Unknown,
			// Token: 0x04000047 RID: 71
			Dxt1,
			// Token: 0x04000048 RID: 72
			Dxt3,
			// Token: 0x04000049 RID: 73
			Dxt5,
			// Token: 0x0400004A RID: 74
			R8G8B8,
			// Token: 0x0400004B RID: 75
			B8G8R8,
			// Token: 0x0400004C RID: 76
			Bgra5551,
			// Token: 0x0400004D RID: 77
			Bgra4444,
			// Token: 0x0400004E RID: 78
			Bgr565,
			// Token: 0x0400004F RID: 79
			Alpha8,
			// Token: 0x04000050 RID: 80
			X8R8G8B8,
			// Token: 0x04000051 RID: 81
			A8R8G8B8,
			// Token: 0x04000052 RID: 82
			A8B8G8R8,
			// Token: 0x04000053 RID: 83
			X8B8G8R8,
			// Token: 0x04000054 RID: 84
			RGB555,
			// Token: 0x04000055 RID: 85
			R32F,
			// Token: 0x04000056 RID: 86
			R16F,
			// Token: 0x04000057 RID: 87
			A32B32G32R32F,
			// Token: 0x04000058 RID: 88
			A16B16G16R16F,
			// Token: 0x04000059 RID: 89
			Q8W8V8U8,
			// Token: 0x0400005A RID: 90
			CxV8U8,
			// Token: 0x0400005B RID: 91
			G16R16F,
			// Token: 0x0400005C RID: 92
			G32R32F,
			// Token: 0x0400005D RID: 93
			G16R16,
			// Token: 0x0400005E RID: 94
			A2B10G10R10,
			// Token: 0x0400005F RID: 95
			A16B16G16R16
		}
	}
}

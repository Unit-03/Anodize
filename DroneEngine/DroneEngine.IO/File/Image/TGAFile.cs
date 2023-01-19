using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Drone.IO
{
	public class TGAFile : IFileFormat
	{
		#region Enums

		public enum VersionSpec
		{
			VER_1,
			VER_2,
		}

		public enum ImageType
		{
			NO_IMAGE,
			COLOUR_MAPPED,
			TRUE_COLOUR,
			BLACK_AND_WHITE,
		}

		public enum PixelDepth
		{
			/// <summary>Uses 8 bits per pixel to represent a grayscale value</summary>
			GRAYSCALE = 8,
			/// <summary>Uses 15 bits per pixel with 5 bits per colour channel</summary>
			RGB5 = 15,
			/// <summary>Uses 16 bits per pixel with 5 bits per colour channel and 1 bit for the alpha</summary>
			RGB5_A = 16,
			/// <summary>Uses 24 bits per pixel with 8 bits per colour channel</summary>
			RGB8 = 24,
			/// <summary>Uses 32 bits per pixel with 8 bits per colour channel and 8 bits for the alpha</summary>
			RGBA8 = 32,
		}

		#endregion

		#region Types

		private struct Header
		{
			public byte IDLength;
			public byte ColourMapType;
			public byte ImageType;

			public ColourMapSpecification MapSpec;
			public ImageSpecification     ImageSpec;

			public Header(BinaryReader reader)
			{
				IDLength      = reader.ReadByte();
				ColourMapType = reader.ReadByte();
				ImageType     = reader.ReadByte();

				MapSpec   = new(reader);
				ImageSpec = new(reader);
			}
		}

		private struct ColourMapSpecification
		{
			public ushort FirstIndex;
			public ushort Length;
			public byte EntrySize;

			public ColourMapSpecification(BinaryReader reader)
			{
				FirstIndex = reader.ReadUInt16();
				Length     = reader.ReadUInt16();
				EntrySize  = reader.ReadByte();
			}
		}

		private struct ImageSpecification
		{
			public ushort XOrigin;
			public ushort YOrigin;
			public ushort Width;
			public ushort Height;
			public byte PixelDepth;
			public byte ImageDesc;

			public ImageSpecification(BinaryReader reader)
			{
				XOrigin    = reader.ReadUInt16();
				YOrigin    = reader.ReadUInt16();
				Width      = reader.ReadUInt16();
				Height     = reader.ReadUInt16();
				PixelDepth = reader.ReadByte();
				ImageDesc  = reader.ReadByte();
			}
		}

		#endregion

		#region Constants

		private const int HEADER_SIZE = 18; // The number of bytes that comprise a TGA header block
		private const int FOOTER_SIZE = 26; // The number of bytes that comprise a TGA footer block (only appears in a Version 2 TGA file)
		private const int MINIMUM_SIZE = HEADER_SIZE; // A TGA file must at least contain a full header block to be considered valid

		private const int FOOTER_OFFSET    = -26; // The offset of the footer block from the end of the data stream
		private const int SIGNATURE_OFFSET =   8; // The offset of the file signature from the start of the footer block

		private const string EXPECTED_SIGNATURE = "TRUEVISION-XFILE"; // A file signature that's expected to appear in the footer block of a Version 2 TGA file

		#endregion

		#region Properties

		public VersionSpec Version;

		public string Metadata { get; set; }

		public ImageType  Type  { get; set; }
		public PixelDepth Depth { get; set; }
		public byte AlphaDepth { get; set; }

		public bool Compressed { get; set; }

		public ushort XOrigin { get; set; }
		public ushort YOrigin { get; set; }

		public ushort Width  { get; set; }
		public ushort Height { get; set; }

		#endregion

		#region Fields

		#endregion

		#region Constructors

		private TGAFile()
		{
		}

		public TGAFile(string filePath)
		{
			Load(filePath);
		}

		public TGAFile(byte[] data)
		{
			Load(data);
		}

		#endregion

		#region Methods

		public void Load(string filePath)
		{

		}

		public void Load(byte[] data)
		{
			using MemoryStream stream = new(data);
			using BinaryReader reader = new(stream, Encoding.ASCII);

			VersionSpec version = ReadVersion(reader);

			// Header (18 bytes)
			// >> Info (3 bytes)
			byte idLength  = reader.ReadByte();
			bool hasMap    = (reader.ReadByte() & 0b0000_0001) > 0; // While bits 1-127 are reserved for use by Truevision and bits 128-255 are reserved for developers, we have no use for them
			byte imageType = reader.ReadByte();

			// >> Colour Map Specification (5 bytes)
			ushort mapStart  = reader.ReadUInt16();
			ushort mapLength = reader.ReadUInt16();
			byte   mapDepth  = reader.ReadByte();

			// >> Image Specification (10 bytes)
			ushort xOrigin    = reader.ReadUInt16();
			ushort yOrigin    = reader.ReadUInt16();
			ushort width      = reader.ReadUInt16();
			ushort height     = reader.ReadUInt16();
			byte   pixelDepth = reader.ReadByte();

			// >>>> Image Descriptor (1 byte)
			byte descriptor  = reader.ReadByte();
			byte alphaDepth  = (byte)(descriptor & 0b0000_1111);     // Bits 0-3 define the alpha channel depth
			bool rightToLeft =       (descriptor & 0b0001_0000) > 0; // Bit 4 specifies a right-to-left pixel ordering if set, left-to-right otherwise
			bool topToBottom =       (descriptor & 0b0010_0000) > 0; // Bit 5 specifies a top-to-bottom pixel ordering if set, bottom-to-top otherwise
		}

		private VersionSpec ReadVersion(BinaryReader reader)
		{
			// Store the current stream position to return to it later
			long start = reader.BaseStream.Position;

			// Seek to the position of the signature
			reader.BaseStream.Seek(FOOTER_OFFSET,    SeekOrigin.End);
			reader.BaseStream.Seek(SIGNATURE_OFFSET, SeekOrigin.Current);

			string signature = reader.ReadString();

			// Return to the previous stream position
			reader.BaseStream.Seek(start, SeekOrigin.Begin);

			// If the signature matches the expected value then this is probably a Version 2 TGA File, otherwise it's Version 1
			return signature == EXPECTED_SIGNATURE ? VersionSpec.VER_2 : VersionSpec.VER_1;
		}

		#endregion
	}
}

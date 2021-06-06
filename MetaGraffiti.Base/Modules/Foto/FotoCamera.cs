using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Foto
{
	public class FotoCamera
	{
		public string Name { get; set; }

		public string Make { get; set; }
		public string Model { get; set; }

		public int EffectivePixels { get; set; }
		public int MaxResolutionWidth { get; set; }
		public int MaxResolutionHeight { get; set; }
		
		public List<FotoLens> Lenses { get; set; } = new List<FotoLens>();

		public FotoCamera(string make, string model, int effectivePixels = 0)
		{
			Make = make;
			Model = model;
			Name = $"{make} {model}";
			EffectivePixels = effectivePixels;
		}
	}

	public class FotoLens
	{
		public string Make { get; set; }
		public string Model { get; set; }

		public decimal ApertureMin { get; set; }
		public decimal ApertureMax { get; set; }
		
		public decimal FocalLenghtMin { get; set; }
		public decimal FocalLenghtMax { get; set; }

		public bool IsPrime => FocalLenghtMin == FocalLenghtMax;

		public FotoLens() { }
		public FotoLens(string make, string model, decimal apeture = 0, decimal focalLength = 0)
		{
			Make = make;
			Model = model;
			ApertureMin = ApertureMax = apeture;
			FocalLenghtMin = FocalLenghtMax = focalLength;
		}
	}
}

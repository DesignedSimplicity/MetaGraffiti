using MetaGraffiti.Base.Modules.Foto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules
{
	public class FotoCore
	{
		private const string _xiamoi = "Xiamoi";
		private const string _huawei = "Huawei";
		private const string _olympus = "Olympus";
		//motorola Moto G (5) Plus

		public List<FotoCamera> ListCameras()
		{
			var cameras = new List<FotoCamera>();
			
			// Android
			cameras.Add(GetXiamoiNote9S());
			cameras.Add(GetXiamoiNote9Pro());
			cameras.Add(new FotoCamera(_huawei, "RNE-L02")
			{
				Name = "Huawei Mate 10 Lite",
				Lenses = new List<FotoLens>()
				{
					new FotoLens(_huawei, "Wide 16 MP", 2.2M, 27.0M),
					new FotoLens(_huawei, "Front 13 MP", 2.0M, 26.0M),
				}
			});
			
			// Micro Four Thirds
			var em5mk1 = new FotoCamera(_olympus, "EM5 MK1", 16 * 1024 * 1024)
			{
				Name = $"{_olympus} EM5 Mk I",
				Lenses = GetMicro43Lenses(),
			};
			cameras.Add(em5mk1);
			var em5mk2 = new FotoCamera(_olympus, "EM5 MK2", 16 * 1024 * 1024)
			{
				Name = $"{_olympus} EM5 Mk II",
				Lenses = GetMicro43Lenses(),
			};
			cameras.Add(em5mk2);
			return cameras;
		}

		private FotoCamera GetXiamoiNote9S()
		{
			return new FotoCamera(_xiamoi, "Redmi Note 9S")
			{
				Lenses = new List<FotoLens>()
				{
					new FotoLens(_xiamoi, "Wide 48 MP", 1.8M, 26.0M),
					new FotoLens(_xiamoi, "Ultrawide 8 MP", 2.2M, 12.75M),
					new FotoLens(_xiamoi, "Macro 5 MP", 2.4M),
					new FotoLens(_xiamoi, "Front 16 MP", 2.5M),
				}
			};
		}

		private FotoCamera GetXiamoiNote9Pro()
		{
			return new FotoCamera(_xiamoi, "Redmi Note 9 Pro")
			{
				Lenses = new List<FotoLens>()
				{
					new FotoLens(_xiamoi, "Wide 64 MP", 1.9M, 26.0M),
					new FotoLens(_xiamoi, "Ultrawide 8 MP", 2.2M, 12.75M),
					new FotoLens(_xiamoi, "Macro 5 MP", 2.4M),
					new FotoLens(_xiamoi, "Front 16 MP", 2.5M),
				}
			};
		}


		private List<FotoLens> GetMicro43Lenses()
		{
			var lenses = new List<FotoLens>
			{
				new FotoLens()
				{
					Make = "Olympus",
					Model = "14-150",
					FocalLenghtMin = 14,
					FocalLenghtMax = 150,
				},
				new FotoLens()
				{
					Make = "Olympus",
					Model = "12-40 Pro",
					FocalLenghtMin = 12,
					FocalLenghtMax = 40,
				},
			};
			return lenses;
		}
	}
}
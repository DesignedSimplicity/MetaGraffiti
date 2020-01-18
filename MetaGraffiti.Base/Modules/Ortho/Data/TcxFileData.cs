using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	public class TcxFileData
	{
		public string Name { get; set; }

		public string Author { get; set; }

		public DateTime? Timestamp { get; set; }

		public List<TcxActivityData> Activities { get; set; }
	}

	public class TcxActivityData
	{
		public string Id { get; set; }

		public string TrainingType { get; set; }
		public string TrainingName { get; set; }

		public string CreatorName { get; set; }

		public List<TcxLapData> Laps { get; set; }
	}

	public class TcxLapData
	{
		public decimal TotalTimeSeconds { get; set; }
		public decimal DistanceMeters { get; set; }
		public decimal MaximumSpeed { get; set; }
		
		public int Calories { get; set; }
		public int AverageHeartRateBpm { get; set; }
		public int MaximumHeartRateBpm { get; set; }

		public string Intensity { get; set; }

		public List<TcxTrackData> Tracks { get; set; }
	}

	public class TcxTrackData
	{
		public DateTime Timestamp { get; set; }
		public string SensorState { get; set; }
		public int? HeartRateBpm { get; set; }
		public decimal? DistanceMeters { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
	}
}

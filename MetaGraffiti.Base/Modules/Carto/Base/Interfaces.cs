using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaGraffiti.Base.Modules.Carto
{
    public interface ICartoPlaceBoundsData
    {
		double CenterLatitude { get; }
		double CenterLongitude { get; }
		double NorthLatitude { get; }
		double SouthLatitude { get; }
		double WestLongitude { get; }
		double EastLongitude { get; }
	}
}
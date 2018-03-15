using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Ortho.Data
{
	/*
	<rtept lon="9.860624216140083" lat="54.9328621088893">
		<ele>0.0</ele>
		<name>Position 1</name>
	</rtept>
	*/
	public class GpxRouteData : GpxMetaData
	{
		public int Number;
		public IList<GpxPointData> Points;
	}
}

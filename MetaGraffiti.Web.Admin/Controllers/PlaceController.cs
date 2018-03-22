using MetaGraffiti.Base.Modules.Carto.Data;
using MetaGraffiti.Base.Modules.Geo.Info;
using MetaGraffiti.Base.Services;
using MetaGraffiti.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class PlaceController : Controller
    {
		private CartoPlaceService _cartoPlaceService;
		private TripSheetService _tripSheetService;

		public PlaceController()
		{
			_cartoPlaceService = new CartoPlaceService(null);
			_cartoPlaceService.LoadPlaces(AutoConfig.CartoDataUri);

			_tripSheetService = new TripSheetService();
			_tripSheetService.Load(AutoConfig.PlaceDataUri);
		}

		public PlaceViewModel InitModel()
		{
			var model = new PlaceViewModel();

			model.Countries = _tripSheetService.ListCountries();
			model.Years = _tripSheetService.ListYears();

			return model;
		}

		public ActionResult Index()
        {
            return View();
        }

		public ActionResult Report(int? year, string country)
		{
			var model = InitModel();

			model.SelectedYear = year;
			model.SelectedCountry = GeoCountryInfo.Find(country);

			model.Places = new List<PlaceModel>();
			var import = _tripSheetService.ListPlaces(year, country);
			foreach(var data in import)
			{
				var place = new PlaceModel();
				place.Data = data;
				model.Places.Add(place);

				var r = GeoRegionInfo.Find(data.Region);
				if (r != null) place.Place = _cartoPlaceService.FindPlace(r, data.Name, true);

				if (place.Place == null)
				{
					var c = GeoCountryInfo.Find(data.Country);
					if (c != null) place.Place = _cartoPlaceService.FindPlace(c, data.Name, true);
				}				
			}
			model.ImportPlaces = import;

			return View(model);
		}
	}
}
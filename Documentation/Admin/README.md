# MetaGraffiti.Web.Admin

## Web based administration portal to manage geographical metadata

The purpose of this application is to consolidate geographical data from multiple sources to build a simple unified dataset of my personal travel experience from which maps and data visualizations can be created.

The primary objective was to allow me to inventory the numerous hiking trips I have completed in New Zealand in order to tabulate statistics about elevation, distance, time, etc.  Many of the GPX tracks recorded on these adventures have some poor-quality data points which throw off the calculations and look bad when rendered on a map.

## Portal

The landing page provides direct access to each of the 4 major modules of the admin portal.  Each module depends heavily on the data managed by the layer below.

![Admin](./Admin.png)

### Topo

This is the primary workhorse of the application and leverages the data maintained in the additional modules to combine multiple GPX tracks, place data from Google Maps and manually entered metadata.

The initial screen shows a matrix of each country where GPX tracks have been imported from alongside a calendar.  These provide deep links to the Map and Report pages respectively.  

The data for these pages is loaded from a directory of self-contained GPX files which have all the necessary information embedded.  These files are created using the Import processes in the Topo and Ortho modules.  [Download Example GPX File](./Trail.gpx?raw=true)

![Topo](./Topo.png)

#### Map
![Topo Map](./Topo-Map.png)

#### Report
![Topo Report](./Topo-Report.png)

#### Tracks
![Topo Track Import](./Topo-Track-Import.png)
![Topo Track Manage](./Topo-Track-Manage.png)
![Topo Track Modify](./Topo-Track-Modify.png)
![Topo Track Preview](./Topo-Track-Preview.png)
![Topo Track Trail](./Topo-Track-Trail.png)

### Carto

![Carto](./Carto.png)
![Carto Edit](./Carto-Edit.png)
![Carto Map](./Carto-Map.png)
![Carto Report](./Carto-Report.png)
![Carto Search](./Carto-Search.png)

### Ortho

![Ortho](./Ortho.png)
![Ortho Places](./Ortho-Places.png)
![Ortho Tracks](./Ortho-Tracks.png)

### Geo

![Geo](./Geo.png)
![Geo Timezones](./Geo-Timezones.png)
![Geo Countries](./Geo-Countries.png)
![Geo Regions](./Geo-Regions.png)

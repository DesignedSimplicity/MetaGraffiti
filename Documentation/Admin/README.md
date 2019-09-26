# MetaGraffiti Admin

### Web based administration portal to manage geographical metadata

The purpose of this application is to consolidate geographical data from multiple sources to build a simple unified dataset of my personal travel experience from which maps and data visualizations can be created.

The primary objective was to allow me to inventory the numerous hiking trips I have completed in New Zealand in order to tabulate statistics about elevation, distance, time, etc.  Many of the GPX tracks recorded on these adventures have some poor-quality data points which throw off the calculations and look bad when rendered on a map.

## Admin

The landing page provides direct access to each of the 4 major modules of the admin portal.  Each module depends heavily on the data managed by the layer below.

![Admin](./Admin.png)

## Topo

This is the primary workhorse of the application and leverages the data maintained in the additional modules to combine multiple GPX tracks, place data from Google Maps and manually entered metadata.

The initial screen shows a matrix of each country where GPX tracks have been imported from alongside a calendar.  These provide deep links to the Map and Report pages respectively.  

The data for these pages is loaded from a directory of self-contained GPX files which have all the necessary information embedded.  These files are created using the Import processes in the Topo and Ortho modules.  [Download Example File](./Trail.gpx?raw=true)

![Topo](./Topo.png)

### Trail Map
![Topo Map](./Topo-Map.png)

### Trail Report
![Topo Report](./Topo-Report.png)

### Tracks

Trails files are created in a workflow process which allows one or more GPX files to be combined or split to create a multi-segmented file with additional metadata.

#### Track Preview
The first step involves selecting a GPX track file from the Ortho Track Import page and extracting the data into the application cache for manipulation.

![Topo Track Preview](./Topo-Track-Preview.png)

#### Track Modify
Once extracted, the track data can be viewed in extensive detail and manipulated as necessary.  There are tools to remove bad data points, trim the start/end points, add a name or description to the segment, or create Carto Places as necessary.

![Topo Track Modify](./Topo-Track-Modify.png)

#### Track Manage
This process can be repeated for multiple tracks until the entire multi-segment trail has been assembled.

![Topo Track Manage](./Topo-Track-Manage.png)

#### Track Import
Now the trail is ready to be tagged, have meta-data added and finally imported into the master dataset.

![Topo Track Import](./Topo-Track-Import.png)

#### Track Trail
Once imported, the trail is easy recalled and all metadata is displayed alongside the map.

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

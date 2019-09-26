# MetaGraffiti Admin

### Web based administration portal to manage geographical metadata

The purpose of this application is to consolidate geographical data from multiple sources to build a simple unified dataset of my personal travel experience from which maps and data visualizations can be created.

The primary objective was to allow me to inventory the numerous hiking trips I have completed in New Zealand in order to tabulate statistics about elevation, distance, time, etc.  Many of the GPX tracks recorded on these adventures have some poor-quality data points which throw off the calculations and look bad when rendered on a map.

## Admin

The landing page provides direct access to each of the 4 major modules of the admin portal.  Each module depends heavily on the data managed by the layer below.

![Admin](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Admin.png?raw=true)

## Topo

This is the primary workhorse of the application and leverages the data maintained in the additional modules to combine multiple GPX tracks, place data from Google Maps and manually entered metadata.

The initial screen shows a matrix of each country where GPX tracks have been imported from alongside a calendar.  These provide deep links to the Map and Report pages respectively.  

The data for these pages is loaded from a directory of self-contained GPX files which have all the necessary information embedded.  These files are created using the Import processes in the Topo and Ortho modules.  [Download Example File](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Trail.gpx?raw=true)

![Topo](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Topo.png?raw=true)

### Trail Map
![Topo Map](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Topo-Map.png?raw=true)

### Trail Report
![Topo Report](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Topo-Report.png?raw=true)

### Tracks

Trails files are created in a workflow process which allows one or more GPX tracks to be combined to create a multi-segmented file with additional metadata.

#### Track Preview
The first step involves selecting a GPX track file from the Ortho Track Import page and extracting the data into the application cache for manipulation.

![Topo Track Preview](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Topo-Track-Preview.png?raw=true)

#### Track Modify
Once extracted, the track data can be viewed in extensive detail and manipulated as necessary.  There are tools to remove bad data points, trim the start/end points, add a name or description to the segment, or create Carto Places as necessary.

![Topo Track Modify](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Topo-Track-Modify.png?raw=true)

#### Track Manage
This process can be repeated for additional tracks until the entire multi-segment trail has been assembled.

![Topo Track Manage](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Topo-Track-Manage.png?raw=true)

#### Track Import
Now the trail is ready to be tagged, have meta-data added and finally imported into the master dataset.

![Topo Track Import](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Topo-Track-Import.png?raw=true)

#### Track Trail
Once imported, the trail is easy recalled and all metadata is displayed alongside the map.

![Topo Track Trail](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Topo-Track-Trail.png?raw=true)

### Carto

This module maintains the list of geological places and their additional metadata.  Places are added using data from the Google Places API and stored in an Excel file which allows for easy batch editing or other manipulation.

Each place is associated with their own geographic bounds and center point which is used to determine if a track starts, ends or passes through it.  Places also have associated types and additional metadata fields.

The starting page for the Carto module includes multiple starting points for adding new places or querying existing places.  Direct links to the Map and Report pages allow for streamlined navigation.

![Carto](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Carto.png?raw=true)

#### Places Map

Using the Google Maps API, every place documented in the system can be filtered and displayed.

![Carto Map](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Carto-Map.png?raw=true)

#### Places Report

It is easy to toggle between the Map and the detailed Report while retaining selected filter criteria.

![Carto Report](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Carto-Report.png?raw=true)

#### Places Search

The search feature shows you matching places on a map with a direct link to the existing record, if it exists, or a Preview button to easily add a new record.

![Carto Search](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Carto-Search.png?raw=true)

#### Places Edit

Editing an existing or adding a new Place to the repository allows you to modify or augment the metadata and easy change the bounding box and center location used for map view position.  A button to search Wikipedia allows for easy access to additional information that may be needed to complete the form.

![Carto Edit](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Carto-Edit.png?raw=true)

### Ortho

The primary source of data comes from the Ortho module which crawls a local directory to discover GPX track files and text files that contain a list of places visited in a given year.

Linking into either module shows a report of the data and the import status for the record.  The button allows the cache to be reset if the file system has changed since it was loaded and parsed.

![Ortho](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Ortho.png?raw=true)

#### Tracks Source

Every GPX track source is loaded and parsed to display a high-level overview to make identification easy.  Time and distance statistics are calculated along-side a list of inventoried Places that the track passes through.  This data feeds the Topo module.

![Ortho Tracks](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Ortho-Tracks.png?raw=true)

#### Places Source

A list of places visited each year is maintained in a simple text file which is loaded, parsed and matched to existing places in the repository.  This makes it easy to identify and import any new places not yet inventoried. This data feeds the Carto module.

![Ortho Places](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Ortho-Places.png?raw=true)

### Geo

The core module is Geo and based on a previous framework called GeoGraffiti used to build my [Panograffiti.com](https://panograffiti.com) website.  Here the standard reference data of Timezones, Countries and Regions can be viewed and maintained.

Timezone data is used to adjust the GPX track recordings from UTC to local time.  Country and Region data are both stored in the Places repository so that they can be maintained easily and utilize the same core functionality.

![Geo](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Geo.png?raw=true)

#### Geo Timezones

![Geo Timezones](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Geo-Timezones.png?raw=true)

#### Geo Countries

![Geo Countries](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Geo-Countries.png?raw=true)

#### Geo Regions
![Geo Regions](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/Admin/Geo-Regions.png?raw=true)

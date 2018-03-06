# MetaGraffiti
A framework to process geographic and topographic data for map building and visualizations

## Releases

### Alpha
* Timezone, Country and Region metadata
* Geospacial calculations and operations
* Read GPX and save GPX/KML file formats
* Auto identifiy location and timezone
* Display GPX tracks on Google Maps
* Edit, trim and combine GPX tracks

### Beta


## TODO
* Fix regions that are not contained in country bounds (ex: Argentina, Santa Cruz)
* Refactor Gpx and Kml namespaces into Geo/Carto/Topo namespace?
* Move AutoConfig and Services from Web to Base project
* Unit tests for Gpx and Kml writer classes
* Unit tests for additional Geo classes
* Unit tests for service methods
* Finish windows utility to merge/convert gpx files





## Inspiration
**Geography** is a field of science devoted to the study of the lands, the features, the inhabitants, and the phenomena of Earth.
* Seeks an understanding of the Earth and its human and natural complexities—not merely where objects are, but how they have changed and come to be.
* Human geography deals with the study of people and their communities, cultures, economies and interactions with the environment by studying their relations with and across space and place.
* Physical geography deals with the study of processes and patterns in the natural environment like the atmosphere, hydrosphere, biosphere, and geosphere.
* Spatial analyses of natural and the human phenomena.
* Area studies of places and regions.
* Studies of human-land relationships.
* Earth Sciences.

**Topography** is the study of the shape and features of the surface of the Earth
* The recording of relief or terrain, the three-dimensional quality of the surface, and the identification of specific landforms. 
* Generation of elevation data in digital form. 
* Graphic representation of the landform on a map by a variety of techniques, including contour lines, hypsometric tints, and relief shading.

**Cartography** is the study and practice of making maps. 
* Set the map's agenda and select traits of the object to be mapped. This is the concern of map editing. Traits may be physical, such as roads or land masses, or may be abstract, such as toponyms or political boundaries.
* Represent the terrain of the mapped object on flat media. This is the concern of map projections.
* Eliminate characteristics of the mapped object that are not relevant to the map's purpose. This is the concern of generalization.
* Reduce the complexity of the characteristics that will be mapped. This is also the concern of generalization.
* Orchestrate the elements of the map to best convey its message to its audience. This is the concern of map design.


## Architecture

### Data
Simple classes which represent a set of attributes for an entity

### Info 
Objects with state created from a static factory pattern


## Modules

### Geo

* GeoDirection - a direction in degrees or radians
* GeoDistance - distance between two or more points
* GeoHeading - a point and a direction
* GeoLocation - lat/lon with optional elevation and timestamp
* GeoRectangle - a set of points defining a physical region

* GeoTimezoneInfo - a windows or olson set of time conversion rules (ex: Eastern Standard Time)
* GeoCountryInfo - static metadata about a world recognized country (ex: United States)
* GeoRegionInfo - the first level political region within a country (ex: Maryland)


### Topo

* TopoGpx
* TopoKml
* TopoJson


### Carto

* Layer - A group of points related by a type or topic
* Point - A single geospacial point used by Carto entites

* Mark - A single point with additional metadata not bound to a paticular area

* Area - An area defined by a perimeter made of points (ex: Yosemite National Park)
* Place - A single point/radius/bounding box tied directly to an area (ex: Yosemite Valley Visitor Center)

* Tour - A consolidation of multiple trips (ex: South Island 2018)
* Trip - A single component of a tour with multiple segments (ex: Gillespeie's Pass)
* Path - A set of data describing the departure and arrival from an Area or Place (ex: Young Hut to Siberia Hut)
* Stop - A set of data describing a visit to an Area or Place (ex: Gillespeie Peak)

__Range, Space, District, Zone, Transit, Trek, Journey, Odessey, Excursion, Expedition, Voyage, Passage, Travel__



## Utilities

### GpxGraffiti
**A simple windows application to pre-process gpx files to use in Adobe Lightroom and Google Earth**

`GpxGraffiti	-merge		// Combines multiple gpx files into a single file - used for geotagging in Lightroom`

`GpxGraffiti	-fix		// Removes bad data points from a gpx file - cleans up a file with a poor gps signal`

`GpxGraffiti	-kml		// Converts a gpx file to a kml file - prepares file for visualizing in Google Earth`

__Process__
1. Read and parse GPX file to perform distance calculations
2. Remove points with low signial quality or errors
3. Resegment tracks and add metadata/notes
4. Save data back into GPX file
5. Convert to KML file
6. Save as CSV file
7. Save to database
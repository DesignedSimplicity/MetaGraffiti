# MetaGraffiti
A framework to process geographic and topographic data for map building and visualizations

## Alpha

## Beta



### TODO
* Fix regions that are not contained in country bounds (ex: Argentina, Santa Cruz)
* Refactor Gpx and Kml namespaces into Geo/Carto/Topo namespace?
* Move AutoConfig and Services from Web to Base project
* Unit tests for Gpx and Kml writer classes
* Unit tests for additional Geo classes
* Unit tests for service methods
* Finish windows utility to merge/convert gpx files







#### GpxGraffiti
**A simple windows application to pre-process gpx files to use in Adobe Lightroom and Google Earth**
`GpxGraffiti	-merge		Combines multiple gpx files into a single file - used for geotagging in Lightroom`
`GpxGraffiti	-fix		Removes bad data points from a gpx file - cleans up a file with a poor gps signal`
`GpxGraffiti	-kml		Converts a gpx file to a kml file - prepares file for visualizing in Google Earth`

__Process__
1. Read and parse GPX file to perform distance calculations
2. Remove points with low signial quality or errors
3. Resegment tracks and add metadata/notes
4. Save data back into GPX file
5. Convert to KML file
6. Save as CSV file
7. Save to database







Data - Simple classes which represent a set of attributes for an entity
Info - State classes created from a static factory pattern



Geography is a field of science devoted to the study of the lands, the features, the inhabitants, and the phenomena of Earth.
* Seeks an understanding of the Earth and its human and natural complexities—not merely where objects are, but how they have changed and come to be.
* Human geography deals with the study of people and their communities, cultures, economies and interactions with the environment by studying their relations with and across space and place.
* Physical geography deals with the study of processes and patterns in the natural environment like the atmosphere, hydrosphere, biosphere, and geosphere.
* Spatial analyses of natural and the human phenomena.
* Area studies of places and regions.
* Studies of human-land relationships.
* Earth Sciences.

Topography is the study of the shape and features of the surface of the Earth
* The recording of relief or terrain, the three-dimensional quality of the surface, and the identification of specific landforms. 
* Generation of elevation data in digital form. 
* Graphic representation of the landform on a map by a variety of techniques, including contour lines, hypsometric tints, and relief shading.

Cartography is the study and practice of making maps. 
* Set the map's agenda and select traits of the object to be mapped. This is the concern of map editing. Traits may be physical, such as roads or land masses, or may be abstract, such as toponyms or political boundaries.
* Represent the terrain of the mapped object on flat media. This is the concern of map projections.
* Eliminate characteristics of the mapped object that are not relevant to the map's purpose. This is the concern of generalization.
* Reduce the complexity of the characteristics that will be mapped. This is also the concern of generalization.
* Orchestrate the elements of the map to best convey its message to its audience. This is the concern of map design.




Geo

GeoDistance - distance between two or more points
GeoDirection - a direction in degrees or radians
GeoPerimeter - a set of points defining a physical region

GeoLocation - lat/lon with optional elevation
GeoPosition - lat/lon with elevation and optional timestamp

GeoHeading - a point and a direction
GeoVector - a heading established by two points


GeoTimezone - a representation of a standard timezone
GeoTimestamp - a date/time 
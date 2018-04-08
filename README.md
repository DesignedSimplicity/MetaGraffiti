# MetaGraffiti
A 4 dimensional space time framework to process geographic and topographic data for map building and visualizations

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

**Orthography** is a set of conventions for writing a language
* Includes norms of spelling, hyphenation, capitalization, word breaks, emphasis, and punctuation.

## Releases

### Alpha
* Timezone, Country and Region metadata
* Geospacial calculations and operations
* Read GPX and save GPX/KML file formats
* Auto identifiy location and timezone
* Display GPX tracks on Google Maps
* Edit, trim and combine GPX tracks

### Beta


### RC1


### RC2


### RC3



## Backlogs

### Api
* Migrate away from using the new API Controller model or spend time to build it properly


### Carto
* Add ability to create a place from scratch (drop pin, set bounds, etc)
* Optimize and expand FindLocalityPlace functionality
* Build out report page and add filters/sorting
* Add auto-persist warning/functionality and update backup file copy pattern
* Add PlaceTags/IconKey/Created/Update fields and remove PlaceFlags from CartoPlaceInfo/Form/etc
* Add filters to global place map, change url and revert icons back to markers (from types)
* Build simple place read only/display page which includes nearby/containing/contained places
* Update backend to use google place details: https://maps.googleapis.com/maps/api/place/details/json
* Update backend to use google place search: https://developers.google.com/places/web-service/search

### Topo
* Fix existing trail files with incorrect local date filename
* Create options to split import track into multiple edit segments
* Update import new trail to auto-discover country, region, timezone
* Add country, region, timezone autocomplete/lookup to trail form
* Change TopoTrailService to use internal ID in GPX as trail key
* Build keyword autocomplete/selector and select icons for display
* Fix fragile implementation and duplicate code for trail import
* Cleanup interfaces and implementation of TopoTrail/Track/Point
* Consolidate ITopoTrailData, ITopoTrailUpdateRequest, TrackEditModel
* Change track edit manage page to show details using TopoTrackInfo

### Ortho
* Remove usage of GpxFileInfo from browse tracks
* Update GpxExtensionsData to read/write internal ID/key
* Fix implementation of UrlText and UrlLink for GPX v1.1

### Geo
* Base RegionID off of CountryID * (1000 ^ Division)
* Add Local Name data for non-English regions
* Move Region and Country metadata to reference file/array
* Add Japan level 1 and level 2 region data

### Tests
* Unit tests for Gpx and Kml writer classes
* Unit tests for additional Geo classes
* Unit tests for service methods







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

* Layer - A group of points related by a type or topic
* Point - A single geospacial point used by Carto entites
* Track
* Trail


### Carto

* Place - A single point/radius/bounding box tied directly to an area (ex: Yosemite Valley Visitor Center)
* Area - An area defined by a perimeter made of points (ex: Yosemite National Park)
* Mark - A single point with additional metadata not bound to a paticular area

### Chrono

* Tour - A consolidation of multiple trips (ex: South Island 2018)
* Trip - A single component of a tour with multiple segments (ex: Gillespeie's Pass)
* Path - A set of data describing the departure and arrival from an Area or Place (ex: Young Hut to Siberia Hut)
* Stop - A set of data describing a visit to an Area or Place (ex: Gillespeie Peak)

__Range, Space, District, Zone, Transit, Trek, Journey, Odessey, Excursion, Expedition, Voyage, Passage, Travel__

### Crypto



### Ortho

* Gpx
* Kml
* Xls




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








## Ideas

Map Info
~~~~~~~~
Geography
Topography
Cartography

Chronography: an arrangement of past events


File Formats (Gpx, Kml, Xsl)
~~~~~~~~~~~~
Orthography: An orthography is a set of conventions for writing a language. It includes norms of spelling, hyphenation, capitalization, word breaks, emphasis, and punctuation.


Sql Databases
~~~~~~~~~~~~~
Lexicography: The art or craft of compiling, writing and editing dictionaries.
Glossography: The writing of glossaries or glosses; The study of ancient words or languages.



Biography

Iconography
Ideography


Chorography: The study of provinces, regions, cities, etc., as opposed to larger-scale geography.
Diplography: double writing; the writing of something twice or in two forms.
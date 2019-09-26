# MetaGraffiti Framework

### A 4th dimensional space/time framework to process geographic and topographic data for map building and visualizations

## Inspiration

## Framework

## Applications

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
* Topo/Trail/Track functionality complete

### RC2
* Carto/Place functionality complete

### RC3
* Geo/Ortho functionality complete

### V1.0

### V1.1

### V2.0


## Backlogs

### Api
* Migrate away from using the new API Controller model or spend time to build it properly


### Carto
** Build simple place read only/display page which includes nearby/containing/contained places
***** Add toggle to global places map to use markers vs place type icons on map
** Add ability to create a place from scratch (drop pin, set bounds, etc)
* Add sorting to place report/map pages
* Add page to list unsaved changed
* Optimize and expand FindLocalityPlace functionality
* Add auto-persist warning/functionality and update backup file copy pattern


### Topo
** Fix existing trail files with incorrect local date filename
** Add country, region, timezone autocomplete/lookup to trail form
** Create options to split import track into multiple edit segments
** Update import new trail to auto-discover country, region, timezone
* Change TopoTrailService to use internal ID in GPX as trail key
* Build keyword autocomplete/selector and select icons for display
* Fix fragile implementation and duplicate code for trail import
* Cleanup interfaces and implementation of TopoTrail/Track/Point
* Consolidate ITopoTrailData, ITopoTrailUpdateRequest, TrackEditModel
* Change track edit manage page to show details using TopoTrackInfo


### Ortho
** Fix implementation of UrlText and UrlLink for GPX v1.1
* Remove usage of GpxFileInfo from browse tracks
* Update GpxExtensionsData to read/write internal ID/key


### Geo
** Base RegionID off of CountryID * (1000 ^ Division)
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




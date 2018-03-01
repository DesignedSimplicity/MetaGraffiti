# MetaGraffiti
A framework for processing geographic and topographic data for map building and visualizations


Data - Simple classes which represent a set of attributes for an entity
Info - State classes created from a static factory pattern




GpxGraffiti - A simple windows application to pre-process gpx files to use in Adobe Lightroom and Google Earth
	-merge		Combines multiple gpx files into a single file - used for geotagging in Lightroom
	-fix		Removes bad data points from a gpx file - cleans up a file with a poor gps signal
	-kml		Converts a gpx file to a kml file - prepares file for visualizing in Google Earth





Gpx
1. Read and parse GPX file to perform distance calculations
2. Remove points with low signial quality or errors
3. Resegment tracks and add metadata/notes
4. Save data back into GPX file
5. Convert to KML file
6. Save as CSV file
7. Save to database

GpxService

.Load(Uri)
.Load(Stream)
.Save(Info, Uri)
.Save(Info, Stream)

.TrimEnd(Timespan)
.TrimEnd(Timestamp)
.TrimStart(Timespan)
.TrimStart(Timestamp)
.Trim(Timestamp, Timespan)
.Trim(Timestamp1, Timestamp2)

.Clean()
.Clean(DOP)
.Clean(Speed)
.Clean(HDOP, VDOP, PDOP)

.Merge(Gpx1)
.Merge(Gpx1, Gpx2)

.Segment(Timestamp)
.Mark(Timestamp, Metadata)













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
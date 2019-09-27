# MetaGraffiti Framework

## 4th dimensional repository to record moments of space and time

![MetaGraffiti](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/MetaGraffiti.png)

**A software development framework to process geographic and topographic data for map building and visualizations**

## Implementation

## Documentation

### ![Read an overview of the Administration Portal](https://github.com/DesignedSimplicity/MetaGraffiti/blob/master/Documentation/Admin/README.md)
### ![Jump into the wiki for a technical description](https://github.com/DesignedSimplicity/MetaGraffiti/wiki)
### ![Check the project backlog for features in development](https://github.com/DesignedSimplicity/MetaGraffiti/projects/2)

## Inspiration

The inspiration for MetaGraffiti originated from a travel blog I started when I set off on a trip to circumnavigate the planet in 2010.  I wanted to keep track of the places I visited, and more importantly, geotag my photos so they could be displayed on a map and show a progress of my adventures.

![KevinAndEarth.com Circa 2010](https://raw.githubusercontent.com/DesignedSimplicity/MetaGraffiti/master/Documentation/KevinAndEarth-2010.png)

I created a small utility called GeoGraffiti to help manage the geographic data streams.  This grew into a website to display the panoramic photography from my travels.

After my trip, I decided to resettle in New Zealand, which offers so much beautiful scenery.  I started doing a lot of hiking (called tramping here) and decided to continue development to keep track of the hiking.

I wanted to preserve the Graffiti concept but the software framework outgrew the simple Geo nomenclature as it expanded to cover more areas.  Luckily, the suffix *graphy from which Graffiti was derived offers many prefixes to create namespaces from.

## GeoGraffiti
**Geography** is a field of science devoted to the study of the lands, the features, the inhabitants, and the phenomena of Earth.
* Seeks an understanding of the Earth and its human and natural complexities—not merely where objects are, but how they have changed and come to be.
* Human geography deals with the study of people and their communities, cultures, economies and interactions with the environment by studying their relations with and across space and place.
* Physical geography deals with the study of processes and patterns in the natural environment like the atmosphere, hydrosphere, biosphere, and geosphere.
* Spatial analyses of natural and the human phenomena.
* Area studies of places and regions.
* Studies of human-land relationships.
* Earth Sciences.

## TopoGraffiti
**Topography** is the study of the shape and features of the surface of the Earth
* The recording of relief or terrain, the three-dimensional quality of the surface, and the identification of specific landforms. 
* Generation of elevation data in digital form. 
* Graphic representation of the landform on a map by a variety of techniques, including contour lines, hypsometric tints, and relief shading.

## CartoGraffiti
**Cartography** is the study and practice of making maps. 
* Set the map's agenda and select traits of the object to be mapped. This is the concern of map editing. Traits may be physical, such as roads or land masses, or may be abstract, such as toponyms or political boundaries.
* Represent the terrain of the mapped object on flat media. This is the concern of map projections.
* Eliminate characteristics of the mapped object that are not relevant to the map's purpose. This is the concern of generalization.
* Reduce the complexity of the characteristics that will be mapped. This is also the concern of generalization.
* Orchestrate the elements of the map to best convey its message to its audience. This is the concern of map design.

## OrthoGraffiti
**Orthography** is a set of conventions for writing a language
* Includes norms of spelling, hyphenation, capitalization, word breaks, emphasis, and punctuation.

_This is a bit of a stretch, but represents the module responsible for loading and saving the core system data which can be considered the conventions of the MetaGraffiti language_

### Future Development

These are potential prefixes and namespaces for future planned development modules.

* Chronography: an arrangement of past events
* Chorography: The study of provinces, regions, cities, etc., as opposed to larger-scale geography.
* Biography: An account of someone's life written by someone else.
* Iconography: the visual images and symbols used in a work of art or the study or interpretation of these.
* Lexicography: The art or craft of compiling, writing and editing dictionaries.
* Glossography: The writing of glossaries or glosses; The study of ancient words or languages.
* Diplography: double writing; the writing of something twice or in two forms.
* Ideography: a branch of lexicography, specifically creating ideographic or conceptual dictionaries.

### Entity Dictionary

#### Carto

* Place - A single point/radius/bounding box tied directly to an area (ex: Yosemite Valley Visitor Center)
* Area - An area defined by a perimeter made of points (ex: Yosemite National Park)
* Mark - A single point with additional metadata not bound to a particular area

#### Chrono

* Tour - A consolidation of multiple trips (ex: South Island 2018)
* Trip - A single component of a tour with multiple segments (ex: Gillespeie's Pass)
* Path - A set of data describing the departure and arrival from an Area or Place (ex: Young Hut to Siberia Hut)
* Stop - A set of data describing a visit to an Area or Place (ex: Gillespeie Peak)

#### Other
_Range, Space, District, Zone, Transit, Trek, Journey, Odyssey, Excursion, Expedition, Voyage, Passage, Travel_


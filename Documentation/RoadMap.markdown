# SharpFE RoadMap #

A loose specification and list of things still to implement or refactor

## Features

* Finite elements
    * Quad (4-node) membrane
    * Quad (4-node) plate
    * Quad (4-node) shells
    * 3-node beam
    * Quad (9-node)
* Gauss Points
* Result derivatives
    * Strain recovery
    * Stress recovery
    * Internal element force recovery
        * Bending moments
        * Shear forces

## Verifications

* 1D truss
* 2D truss
* 3D truss
* 1D beam
* 3D beam
* Membrane
* Plates
* Shells

## Code

* Clean up unit tests
    * Split verifications from examples. Move to separate folders/projects
    * Split integration tests from verifications and examples
    * Use Assert.That syntax
    * Use Theory attribute where appropriate
    * Include more unit tests for edge conditions and unhappy paths
* Prepare classes for serialization where appropriate
    * Add IFormattable interface
    * Add ISerializable attribute
    * Add ICloneable interface
    * Add IEquatable<T> interface
* KeyedVector and KeyedMatrix should duplicate all Vector and Matrix methods respectively

## Documentation

* Improve Xml classdocs
* Contributors guidance/manual
* QuickStart guide
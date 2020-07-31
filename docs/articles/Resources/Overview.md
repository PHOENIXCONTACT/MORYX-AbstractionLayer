---
uid: ResourceConcept
---
# Resource concept

Everything that is needed to produce a product is a `Resource`. 
It can be a physical device like a Station or a Cell or it can be something non physical like a SerialNumberProvider that calculates a new serial number.
The main advantage of this concept is that the resource interface and implementation structure is the same for all resources. 
So [accessing a resource](ResourceManagement.md) is done in an abstract and general way. 
Processing, configuring and visualization of a resource is done in the same way with the same classes. 
Specialities of a resource can be added in a standard way and extend the [default UI](ResourceManagementUI.md).

## Resource type tree

Look [here](ResourceTypeTree.md) to read what's about with resource types.

## Resource object graph

Look [here](ResourceObjectGraph.md) if you want to know more about how resources are organized.

## Resource relation

Look [here](ResourceRelations.md) to dive deeper into resource relations.

---
uid: Processes
---
# Processes

A [Process](xref:Moryx.AbstractionLayer.IProcess) will be created to produce one instance. Therefore it consists of a series of Activities from a defined workplan. It is managed by the ProcessController.

## Processes in a Resource

It is possible to get the process from an activity inside of a resource. This is sometimes necessary to get more information from the recipe or the product. But if there are information which are necessary for every resource which can handle the activity then consider to provide the information with activity parameters.
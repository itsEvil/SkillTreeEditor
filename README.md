# Skill Tree Editor

This editor was made to speed up the iterative process of creating and managing a skill tree system on my game Forgotten Pantheon.

I am making this public because this project can easily converted for other use cases.

This public repository will be updated with features that I need for my game and not any requests.

# How to run it:

Download Unity and UnityEditor version 2022.3.7f1 or later.
Open the project and build it or click play in the editor.

# How does it work:

## On run

A user gets shown Empty Node.

Once they press the node it gets convered to a Edit node and 
8 new Empty nodes get spawned in at the intercardinal and cardinal directions.

On pressing a Edit node the "Node Maker" UI menu enables.

Information about the node types is below.

## "Node Maker" UI Menu

This menu allows the user to edit information that the nodes store.
Also has a remove and save button.

## UI Panel

The user has 3 buttons located at the top left UI panel.

Here's what happens when you press any of them.

### Save

A new XML file gets created in the StreamingAssets folder 
named "output-{DateTime}.xml" to not overwrite any previous versions by accident.

### Load 

A menu opens to select the file.
Once the file loads all the nodes currently on screen get deleted.
New nodes are spawned in at locations loaded in from the XML file.

### Connections Mode

Pressing any Edit Node will turn it blue indicating that its waiting for a second node to be pressed.
Once a second node gets pressed a line will be rendered between the two nodes.
The Id's of the two nodes get added to both of the nodes.

Currently line cannot spawn on the exact same position as another even if they are connected to two different nodes.

# Known bugs:

-> When removing a node depending on its neighbours it may not remove all/correct Empty nodes.

# Info

### Empty Nodes

Can be identified by the "+" in the centre of them.

On click convered to a Edit node and creates new Empty nodes.
These nodes are skipped when the user saves to a file.

### Edit Nodes

The user can click on these nodes to edit their data.

They can be removed and converted back to Empty Nodes.

They do not need any information in them to be saved.

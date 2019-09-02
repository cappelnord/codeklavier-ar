# codeklavier-ar
AR extension by Patrick Borgeat for [CodeKlavier](https://codeklavier.space/) by Felipe Ignacio Noriega and Anne Veinberg.

https://codeklavier.space/augmentedreality/

## Scenes

Each Scene builds its own app:

- **Visualizer** is a tool used by the pianist for coding/visualizng the structure of the L-Systems
- **ARTestbed** is a pre-vis tool for the final AR visuals
- **TheAR** is the final AR client
- **TheVR** is an experimental client for virtual reality using SteamVR

## Controls

### ARTestbed

Some keys can be used to toggle between different states.

- **S** to toggle between the default sky box and a black background.
- **M** to toggle between a marker visualization, the unity cage (1 cubic meter) and axis

## Tech

Use in conjunction with the websockets server:
https://github.com/narcode/codeklavier-extras

### Unity

Currently this projects uses **Unity 2019.2.3f1**.


### Marker Transforms

Marker transforms are defined in the server configuration and specify the relation of tracking markers to the desired position/rotation/scale of the visuals. Currently there are still issues and it might need a rework.

## Acknowledgments

This repository includes the following third party packages/code
- [SteamVR](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647)
- [Vuforia](https://docs.unity3d.com/Manual/vuforia-sdk-overview.html)
- [TextMesh Pro](https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126)
- [UnityOSC](https://github.com/jorgegarcia/UnityOSC)

[CodeKlavier](https://codeklavier.space/) is supported By Stimuleringsfonds Creatieve Industrie NL and other sponsors.
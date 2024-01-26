# A multiscale model of terrain dynamics for real-time earthmoving simulation

[SpringerOpen: A multiscale model of terrain dynamics for real-time earthmoving simulation](https://amses-journal.springeropen.com/articles/10.1186/s40323-021-00196-3)

## Install

유니티 프로젝트 2022.3.12f1, URP를 이용합니다.

설치해야할 유니티 패키지는 다음과 같습니다.
* [TerrainTools](https://docs.unity3d.com/Packages/com.unity.terrain-tools@5.0/manual/index.html)
* [Terrain Sample Asset Pack](https://assetstore.unity.com/packages/3d/environments/landscapes/terrain-sample-asset-pack-145808)
* [Entities](https://docs.unity3d.com/Packages/com.unity.entities@0.17/manual/index.html)
* [Entities Graphics](https://docs.unity3d.com/Packages/com.unity.entities.graphics@1.0/manual/index.html)
* [Netcode for Entities](https://docs.unity3d.com/Packages/com.unity.netcode@1.0/manual/index.html)

## How to work

지형은 Terrain을 통하여 구현하며, 많은 수의 파티클을 처리하기 위해 ECS를 이용합니다.

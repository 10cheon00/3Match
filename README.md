# 3Match UML

메인씬이나 다른 씬들을 개발하기보단 게임의 주 시스템을 구현해보는 방향으로 개발했다.

게을러서 구현해야할 것을 다 구현해보지 못한 점이 아쉬웠다. 객체에게 책임을 적절히 분배하지 못하여 개발 속도가 느려진 점도 아쉬웠다. 경험이 부족했다.

![uml](https://github.com/10cheon00/3Match/assets/13350111/5d76921b-74a6-404e-b9c3-db27bb26dfa6)

## GameManager

게임매니저에 상태 패턴을 적용하여 각 상태마다 행동을 다르게 구현하였다.

1. `GameManagerIdleState` : 플레이어가 2개의 타일을 선택할 때까지 기다린다.
2. `GameManagerSwapTwoTileState` : 플레이어가 선택한 타일을 교환한다.
3. `GameManagerResolve3MatchState` : 보드에 3Match인 타일이 있는지 검사하고, 만약 있다면 터트리고 새로운 타일을 삽입한다.

게임 진행에 따라 1 -> 2 -> 3 -> 1 순서대로 종속성을 갖게 되었다.

## GameManagerState

상태 패턴이므로 `GameManager`를 참조하여야 하는데, 매번 인자로 넘겨주기 귀찮다고 생각하여 `static` 변수에 한 번만 넘겨주기로 했다. 좋은 판단인지는 모르겠다.

각 상태마다 취해야할 행동들이 있는데, 이런 행동들은 `GameManagerTask`를 `Queue` 자료구조로 관리하여 순차적으로 처리하였다.

각 `GameManagerTask`는 비동기 작업을 나타낸다. 큐에 A, B, C가 들어와 있다면 A 작업이 수행되는 동안 B, C는 A 작업이 끝날 때까지 기다리게 되고, A 작업이 끝나면 바로 B 작업으로 넘어간다.

Unity에서 비동기로 작업을 실행시키는 방법인 Coroutine이 있지만, 작업 순서가 명시적으로 드러나지 않는다고 생각하여 선택하지 않았다.

## GameManagerTask

각 상태에 따라 수행해야할 작업을 나타내는 객체다.

`GameManagerTask`는 상태 패턴을 적용했다. 단, 다른 Task로 전환하는 방법은 갖고 있지 않다. 대신 State에게 Task 내에 있는 모든 작업이 끝났음을 알릴 수 있는 메서드를 통해 다른 Task로 전환한다.

`InitializeTask()` 메서드를 override하여 Task가 처음 실행될 때에 초기화를 진행하고, 매 `Update()` 때마다 State가 호출하는 `RunTask()` 메서드를 override하여 작업을 수행한다.

Task 내에서 할 수 있는 모든 작업이 끝났을 때 `FinishTask()`를 호출하여 State가 현재 Task를 큐에서 제거하여 다음 Task를 수행한다.

## TileBoardManager

타일이 저장된 자료구조인 `TileBoard`와 관련된 작업을 수행하는 책임을 갖는 객체다. 두 타일을 교환한다거나, 3개 이상의 타일이 연속적으로 인접한 상황을 검사, 새로운 타일을 삽입하는 메서드가 구현되어있다.

내부에서 `TileBoardFactory`를 참조하고 있는데, `TileBoardFactory`는 `Tile` 또는 `TileBoard`를 생성하는 역할을 담당한다. (책임이 두 가지라서 분리했어야 하는데 게을러서 안했다)

## TileBoardFactory

`TileBoard`에 추가될 `Tile`, 타일 게임오브젝트를 생성하는 책임을 갖는 객체다. 

직접 `Instantiate()`를 실행하여 게임오브젝트를 생성하고 여기에 추가되어있는 `Tile` 컴포넌트를 수집하여 반환한다.

## Tile

타일 게임오브젝트에 추가된 컴포넌트로, 타일의 색깔과 특정 상황에서 해야할 행동을 명시한 `TileAction` 객체를 관리한다.

## TileAction

`TileAction` 또한 상태 패턴을 적용하여 특정 상황에서 타일의 이펙트를 보여주거나 transform 변경 따위의 작업을 한다.

각 `TileAction` 내에 정의된 행동이 끝났으면 `TileReadyAction` 상태로 돌아가도록 한다. 

## TileSwapHandler

레이캐스트를 통해 두 개의 타일을 저장하는 객체다.

`GameManagerIdleState`에서 참조하고 있다. 두 개의 타일을 선택할 경우 `TilePair` 구조체에 선택한 타일의 참조를 저장하게 된다.

# 느낀 점

디자인 패턴을 공부한 후 게임 개발에 적용해보려 했지만 유니티의 독특한 라이프사이클 때문에 구현하기 어려웠다. 게다가 interface를 활용했어야 하는 아쉬움도 남는다.

그렇지만 책임을 여기저기 분리하여 퍼트려 놓으니 관리하기 편한 점도 있었다.

상태 패턴을 적극적으로 사용하였는데, 굉장히 강력한 도구임을 실감했다.

직접 개발하기 전에 UML이나 다른 도구로 미리 구조를 정했다면 더 좋은 결과가 있었을 것 같다.

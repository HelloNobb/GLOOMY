/*
>assets
    >Scripts
        >Managers : 게임데이터,UI,씬 전환 관리
        >Characters : 캐릭터 관련 스크립트
        >Items
        >UI
        >Scenes : 씬 관련 로직
    >Prefabs : 캐,아이템,UI프리팹
    >Sprites : 게임 이미지 리소스
    >Audio : 사운드 리소스
    >Resources : Json 데이터 파일 저장


** 주요 게임요소를 클래스로 나누기
    -GameManger : 게임 전체 데이터 관리
        -CharacterManager : 글루미 상태 관리
        -ItemManager : 아이템 보유 개수 및 사용기능 관리
        -UIManager : UI업데이트 및 상호작용 관리
        -SceneManager : 씬 전환 관리

** 데이터 저장 -> JSON 활용


** 씬, 흐름 -> SceneLoader.cs


** UI업데이트 -> UIManager.cs 활용

*/
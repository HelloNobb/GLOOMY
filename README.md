# 🎮 GLOOMY - 감성 양육 시뮬레이션

![Unity](https://img.shields.io/badge/Unity-2021+-black?style=flat-square&logo=unity)
![C#](https://img.shields.io/badge/C%23-9.0-purple?style=flat-square&logo=csharp)
![Platform](https://img.shields.io/badge/Platform-Android-green?style=flat-square&logo=android)
![Status](https://img.shields.io/badge/Status-Complete-success?style=flat-square)

> 🌧️ 우울한 반려 캐릭터 **"글루미"**를 돌보며 교감하는 모바일 양육 시뮬레이션 게임

## ✨ 프로젝트 하이라이트

- ✅ **94개 C# 스크립트**, **11개 씬**으로 구성된 완성형 프로젝트
- 🎯 **State Machine 패턴**으로 복잡한 게임 흐름 관리
- 📦 **JSON 기반 데이터 드리븐 설계**로 콘텐츠 확장성 확보
- 🔧 **제네릭 활용** 타입 안전한 저장/로드 시스템 구현
- ⏰ **DateTime 기반 실시간 상태 변화** 시스템 (오프라인 진행 지원)
- 🎨 **ScriptableObject** 활용한 데이터 관리

---

## 📋 프로젝트 정보

| 항목 | 내용 |
|-----|------|
| **플랫폼** | Mobile (Android APK) |
| **개발 엔진** | Unity |
| **개발 언어** | C# |
| **장르** | 양육 시뮬레이션, 힐링 게임 |
| **개발 구조** | 94개 스크립트, 11개 씬 |
| **개발 기간** | 개인 프로젝트 |

## 🎯 핵심 컨셉

플레이어는 "관리인"으로서 연구소에서 배정받은 글루미를 돌봅니다. 글루미는 우울도, 친밀도, 위생도, 건강 등의 상태값을 가지며, 플레이어의 돌봄 활동에 따라 변화합니다.

---

## 📁 프로젝트 디렉토리 구조

```
GLOOMY_0.1.0/
├── Assets/
│   ├── Prefabs/           # 재사용 UI 프리팹
│   ├── Resources/
│   │   ├── Art/           # 이미지 에셋
│   │   ├── Audio/         # 사운드 에셋
│   │   ├── Config/        # 설정 파일
│   │   ├── Flowers/       # 꽃 ScriptableObject 데이터
│   │   ├── Fonts/         # 폰트 에셋
│   │   ├── JSON_Chat/     # 글루미 대화 JSON 데이터
│   │   ├── JSON_Events/   # 이벤트 대화 데이터
│   │   ├── JSON_lab/      # 연구소 대화 데이터
│   │   ├── JSON_TeaTalk/  # 티타임 대화 데이터
│   │   ├── Medicines/     # 약 아이템 데이터
│   │   └── TeaAdds/       # 차 재료 데이터
│   ├── Scenes/
│   │   ├── Scene_Start    # 시작 화면
│   │   ├── Scene_INTRO1   # 첫 인트로 씬
│   │   ├── Scene_INTRO2   # 두 번째 인트로 씬
│   │   ├── Scene_Main     # 메인 게임 화면
│   │   ├── Scene_Garden   # 정원 (꽃 키우기)
│   │   ├── Scene_Bath     # 목욕 씬
│   │   ├── Scene_Lab      # 연구소 (NPC 상점)
│   │   ├── Scene_Kitchen1/2  # 차 만들기 단계
│   │   ├── Scene_Ttime    # 티타임
│   │   └── Scene_Walk     # 산책
│   └── Scripts/
│       ├── BASE/          # 핵심 시스템 스크립트
│       ├── GARDEN/        # 정원 관련 로직
│       ├── GLOOMY/        # 글루미 캐릭터 관리
│       ├── INVENTORY/     # 인벤토리 시스템
│       ├── LAB/           # 연구소 (NPC 대화)
│       ├── MAIN/          # 메인 화면 로직
│       ├── TEA1~TEA3/     # 차 만들기 시스템
│       └── WALK/          # 산책 시스템
├── GLOOMY.apk             # 빌드된 APK 파일
└── ProjectSettings/       # Unity 프로젝트 설정
```

---

## 🎮 주요 시스템 구현

### 1. 게임 상태 관리 (`GameManager`)
- **상태 기반 씬 전환**: `GameState` enum으로 다양한 게임 상태 정의
  - `Intro1Required` / `Intro2Required`: 인트로 시퀀스
  - `MainGame`: 일반 게임 진행
  - `GloomyMeltRequired` / `GloomyFixRequired` / `GloomyleaveRequired`: 글루미 상태에 따른 특별 이벤트
- **싱글톤 패턴**: `DontDestroyOnLoad`로 씬 간 데이터 유지
- **세이브/로드**: `PlayerPrefs` 및 JSON 파일 저장

### 2. 글루미 캐릭터 시스템 (`GloomyManager` + `GloomyData`)
- **상태 시스템**: 
  - `blue` (우울도 0~100), `intimacy` (친밀도 0~100)
  - `hygiene` (위생도), `health` (건강), `battery` (배터리), `sociability` (사회성)
- **활동 메서드**: `ChatGloomy()`, `BathGloomy()`, `ShotGloomy()`, `WalkStartGloomy()`, `TeaDoneGloomy()`
- **시간 기반 변화**: 마지막 목욕 시간, 채팅 시간 등 기록하여 상태 변화 계산

### 3. 저장 시스템 (`SaveSystem`)
- **제네릭 메서드**: `SaveToFile<T>()`, `LoadFromFile<T>()`
- **JSON 직렬화**: `JsonUtility`를 사용한 데이터 저장
- **저장 경로**: `Application.persistentDataPath`

### 4. 대화 시스템 (`DialogueLoader` + `ChatLoader`)
- **JSON 기반 대화**: `Resources.Load<TextAsset>()` 활용
- **노드 기반 구조**: ID로 대화 노드 연결, 분기 지원
- **타이핑 효과**: 코루틴으로 한 글자씩 출력
- **상태 연동 대사**: 글루미 우울도/친밀도에 따른 대사 분기

### 5. 정원 시스템 (`GardenManager` + `FlowerData`)
- **ScriptableObject 활용**: 꽃 데이터를 에셋으로 관리
- **성장 시스템**: 시간 기반 꽃 성장 (Seed → Baby → Child → Adult)
- **수확 시스템**: 더블 탭으로 다 자란 꽃 수확

### 6. 차 만들기 시스템 (`Tea1Manager` → `Tea2Manager` → `Tea3Manager`)
- **재료 선택**: 꽃 + 베이스 + 첨가물 조합
- **레시피 시스템**: 선택한 재료 조합으로 차 제조
- **티타임**: 글루미와 함께 차를 마시는 이벤트

### 7. 눈물 수집 시스템 (`TearManager`)
- **실시간 드롭**: 우울도에 따른 눈물 드롭 주기 변화
- **터치 수집**: 떨어지는 눈물을 터치하여 수집
- **약물 효과**: 눈물약 투여 시 드롭 속도 2배

### 8. 연구소 시스템 (`LabChatController`)
- **NPC 대화**: 플레이어 레벨에 따른 인사말 분기
- **상점**: 약, 아이템 구매/판매
- **글루미 교환/은퇴**: 특수 게임 진행

---

## 🔧 기술 스택

| 카테고리 | 기술 |
|---------|------|
| 게임 엔진 | Unity |
| 언어 | C# |
| 데이터 저장 | JSON (JsonUtility) |
| UI | Unity UI, TextMesh Pro |
| 데이터 구조 | ScriptableObject |
| 애니메이션 | Unity Animator |

---

## 📝 스크립트 담당 역할

| 폴더 | 주요 스크립트 | 역할 |
|-----|-------------|------|
| `BASE` | `GameManager` | 게임 전체 상태 관리 |
| `BASE` | `SaveSystem` | 저장/로드 시스템 |
| `BASE` | `PlayerManager` | 플레이어 데이터 관리 |
| `BASE` | `InventoryManager` | 인벤토리 시스템 |
| `BASE` | `DialogueLoader` | 대화 노드 로드 |
| `GLOOMY` | `GloomyManager` | 글루미 상태/활동 관리 |
| `GARDEN` | `GardenManager` | 정원 기능 |
| `GARDEN` | `FlowerData` | 꽃 ScriptableObject |
| `MAIN` | `ChatManager` | 메인 채팅 시스템 |
| `MAIN` | `TearManager` | 눈물 드롭/수집 |
| `LAB` | `LabChatController` | 연구소 NPC 대화 |
| `TEA1-3` | `TeaManager` 시리즈 | 차 제조 시스템 |

---

## 🎨 리소스 구조

### JSON 대화 파일 (Resources/JSON_Chat)
- **상태별 대사**: `Gloomy_Blue01~04.json`, `Gloomy_Inti01~04.json`
- **이벤트 대사**: `Gloomy_Event_Sick.json`, `Gloomy_Event_Dirty.json`
- **인사말**: `Gloomy_Hello_24h.json`, `Gloomy_Hello_3d.json`, `Gloomy_Hello_7d.json`
- **일상 대사**: `Gloomy_Daily.json`

### ScriptableObject 데이터
- **Flowers/**: 각 꽃의 정보 (이미지, 성장시간, 설명)
- **Medicines/**: 약 아이템 데이터
- **TeaAdds/, TeaBases/**: 차 재료 데이터

---

## 🚀 빌드 및 실행

1. **Android APK**: 루트 폴더의 `GLOOMY.apk` 파일을 설치
2. **Unity 에디터**: Unity로 프로젝트 열어 실행 (빌드 설정: Android, Portrait)

---

## 🎓 기술적 성과

### 구현한 디자인 패턴 & 아키텍처
- **State Machine Pattern**: enum 기반 게임 상태 관리로 복잡한 씬 전환 로직 구현
- **Singleton Pattern**: 씬 간 데이터 유지를 위한 전역 매니저 설계
- **Data-Driven Design**: JSON/ScriptableObject로 콘텐츠와 로직 분리

### 학습 및 적용 기술
- C# 제네릭을 활용한 타입 안전한 직렬화/역직렬화
- Unity 코루틴으로 비동기 게임 루프 구현
- DateTime 연산을 통한 오프라인 진행 시스템 설계
- Resources.Load 동적 로딩으로 메모리 효율화

### 향후 개선 목표
- MVC/MVVM 패턴 적용으로 코드 구조 개선
- 인터페이스 기반 의존성 주입 설계
- 오브젝트 풀링으로 성능 최적화
- 유닛 테스트 추가로 코드 품질 향상

> 📖 더 자세한 기술 구현 내용: [`TECHNICAL_PORTFOLIO.md`](./TECHNICAL_PORTFOLIO.md)

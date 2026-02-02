# 🎮 GLOOMY 프로젝트 - 기술 구현 포트폴리오

## 프로젝트 요약

| 항목 | 내용 |
|-----|------|
| **프로젝트명** | GLOOMY - 감성 양육 시뮬레이션 |
| **개발 기간** | 개인 프로젝트 |
| **플랫폼** | Android (Mobile) |
| **기술 스택** | Unity, C#, JSON, ScriptableObject |
| **역할** | 기획, 설계, 개발 전체 담당 |

---

## 💡 주요 기술 구현 사항

### 1. 상태 기반 게임 흐름 관리 (State Machine Pattern)

**문제 상황**: 글루미의 상태(우울도/친밀도/건강)에 따라 다양한 이벤트 씬으로 분기해야 함

**해결 방안**:
```csharp
public enum GameState {
    Intro1Required,      // 첫 실행
    Intro2Required,      // 글루미 재배정 필요
    MainGame,            // 일반 게임
    GloomyMeltRequired,  // 우울도 100 → 녹는 이벤트
    GloomyFixRequired,   // 우울도 0 → 폐기 이벤트
    GloomyleaveRequired, // 친밀도 100 → 떠남 이벤트
    // ...
}

public void ChangeGameState(GameState gState) {
    currentState = gState;
    ChangeSceneOfCurrentState(); // 상태에 따른 씬 전환
}
```

**기술적 의의**:
- 복잡한 게임 흐름을 enum 상태값으로 명확하게 관리
- 새로운 이벤트 추가 시 GameState enum과 switch문만 확장하면 됨
- 싱글톤 패턴과 결합하여 씬 간 상태 유지

---

### 2. 제네릭 기반 데이터 저장/로드 시스템

**문제 상황**: 글루미 데이터, 정원 데이터, 플레이어 데이터 등 다양한 형식의 데이터를 통일된 방식으로 저장/로드

**해결 방안**:
```csharp
public static class SaveSystem {
    public static void SaveToFile<T>(string filename, T data) {
        string path = Path.Combine(Application.persistentDataPath, filename + ".json");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static T LoadFromFile<T>(string filename) {
        string path = Path.Combine(Application.persistentDataPath, filename + ".json");
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }
        return default;
    }
}
```

**기술적 의의**:
- C# 제네릭을 활용하여 타입 안전한 직렬화/역직렬화 구현
- 코드 재사용성 극대화: 한 번 구현으로 모든 데이터 타입에 적용
- `Application.persistentDataPath` 사용으로 플랫폼 독립적 저장

---

### 3. 시간 기반 실시간 상태 변화 시스템

**문제 상황**: 글루미의 위생도가 시간 경과에 따라 자연스럽게 감소해야 함

**해결 방안**:
```csharp
public void UpdateGHygieneByTime() {
    DateTime lastUpdate = currentGloomy.lastHygieneUpdateTime;
    DateTime now = DateTime.Now;
    TimeSpan elapsed = now - lastUpdate;
    
    // 30분마다 위생도 1씩 감소
    int decreaseAmount = (int)(elapsed.TotalMinutes / 30);
    if (decreaseAmount > 0) {
        SetGHygiene(currentGloomy.hygiene - decreaseAmount);
        currentGloomy.lastHygieneUpdateTime = now;
    }
}
```

**기술적 의의**:
- `DateTime` 연산을 통한 오프라인 진행 지원
- 앱 종료 후 재시작해도 경과 시간만큼 상태 반영
- 마지막 업데이트 시간 저장으로 정확한 계산

---

### 4. JSON 기반 데이터 드리븐 대화 시스템

**문제 상황**: 수십 가지 상황별 대사를 하드코딩 없이 유연하게 관리

**해결 방안**:
```csharp
// JSON 대화 파일 동적 로드
private string GetFileName() {
    if (GloomyManager.instance.IsSick()) {
        return "Gloomy_Event_Sick";
    } 
    else if (GloomyManager.instance.IsDirty()) {
        return Random.value < 0.5f ? "Gloomy_Event_Dirty" : "Gloomy_Daily";
    }
    // 우울도/친밀도 상태에 따른 대사 파일 선택
    // ...
}

public void DisplayChatNode() {
    TextAsset chatJsonFile = Resources.Load<TextAsset>($"JSON_Chat/{GetFileName()}");
    currentNode = chatLoader.GetRandomChatNode(chatJsonFile);
    StartCoroutine(TypeLine(currentNode.text[txtIndex]));
}
```

**기술적 의의**:
- 콘텐츠(대사)와 로직(코드)의 완전한 분리
- JSON 파일만 수정하면 대사 추가/수정 가능 (빌드 불필요)
- `Resources.Load` 동적 로딩으로 메모리 효율적 관리

---

### 5. ScriptableObject 기반 게임 데이터 관리

**문제 상황**: 꽃, 약, 차 재료 등 다양한 아이템 데이터를 효율적으로 관리

**해결 방안**:
```csharp
[CreateAssetMenu(fileName = "FlowerData", menuName = "Garden/Flower")]
public class FlowerData : ScriptableObject {
    public string flowerId;
    public string flowerName;
    public Sprite flowerIcon;
    public Sprite flowerImage_Adult;
    public Sprite flowerImage_Child;
    public Sprite flowerImage_Baby;
    public Sprite flowerImage_Seed;
    public float growthTimeSeconds;
    public string description;
}
```

**기술적 의의**:
- Unity 에디터에서 시각적으로 데이터 편집 가능
- 프리팹이나 씬 독립적인 데이터 에셋
- 기획자도 쉽게 수정 가능한 워크플로우

---

### 6. 코루틴 기반 게임 루프 시스템

**문제 상황**: 눈물 드롭이 우울도에 따라 다른 주기로 발생해야 함

**해결 방안**:
```csharp
IEnumerator TearDropLoop() {
    while (true) {
        if (!isTearDropping) {
            float interval = GetIntervalOfTears(blueLevel);
            yield return new WaitForSeconds(interval);
            StartCoroutine(PlayTearAnimation());
        }
        yield return null;
    }
}

public float GetIntervalOfTears(float level) {
    float interval;
    if (level <= 25.0f) interval = 15f;
    else if (level <= 50.0f) interval = 10f;
    else if (level <= 75.0f) interval = 6f;
    else if (level <= 99.0f) interval = 3f;
    else interval = 100f;
    
    // 약물 효과 적용
    return isTearDrugActive ? interval * 0.5f : interval;
}
```

**기술적 의의**:
- Unity 코루틴을 활용한 비동기 게임 루프 구현
- 상태값(우울도)에 따른 동적 주기 조절
- 외부 요인(약물)에 의한 효과 적용 구조

---

### 7. 싱글톤 매니저 패턴을 통한 전역 데이터 접근

**패턴 적용**:
```csharp
public class GloomyManager : MonoBehaviour {
    public static GloomyManager instance;
    public GloomyData currentGloomy;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
```

**적용된 매니저들**:
- `GameManager` - 게임 상태 관리
- `GloomyManager` - 글루미 데이터/활동
- `PlayerManager` - 플레이어 데이터
- `InventoryManager` - 인벤토리
- `GardenManager` - 정원 시스템

---

## 📊 프로젝트 규모

| 항목 | 수치 |
|-----|------|
| C# 스크립트 | 94개 |
| Unity 씬 | 11개 |
| JSON 대화 파일 | 16개+ |
| ScriptableObject 종류 | 5종 |

---

## 🎯 이 프로젝트에서 배운 점

1. **모듈화의 중요성**: 기능별로 스크립트를 분리하여 유지보수성 향상
2. **데이터 드리븐 설계**: JSON, ScriptableObject 활용으로 콘텐츠 확장 용이
3. **상태 관리**: enum 기반 상태 머신으로 복잡한 게임 흐름 관리
4. **시간 기반 시스템**: DateTime 활용으로 오프라인 진행 구현
5. **씬 간 데이터 유지**: 싱글톤 + DontDestroyOnLoad 패턴 활용

---

## 🔧 향후 개선 방향

- MVC/MVVM 패턴 적용으로 구조 개선
- 인터페이스 기반 의존성 주입
- 오브젝트 풀링으로 메모리 최적화
- 유닛 테스트 추가

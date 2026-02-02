# 🚀 GitHub 업로드 가이드

## 📋 준비 사항

- [x] .gitignore 파일 생성 완료 (자동 생성됨)
- [x] README.md 개선 완료
- [ ] GitHub 계정 준비
- [ ] Git 설치 확인 (`git --version` 명령어로 확인)

---

## 1️⃣ Git 초기화 및 커밋

프로젝트 폴더에서 터미널을 열고 다음 명령어를 실행하세요:

```bash
# 현재 Git 저장소가 있는지 확인
# 이미 .git 폴더가 있으면 이 단계는 생략

# Git 초기화 (처음 한 번만)
git init

# 모든 파일 추가 (gitignore에 의해 필요한 파일만 추가됨)
git add .

# 첫 커밋
git commit -m "Initial commit: GLOOMY 프로젝트 업로드"
```

---

## 2️⃣ GitHub 저장소 생성

1. **GitHub 웹사이트 접속**: https://github.com
2. **New repository 클릭** (우측 상단 + 버튼)
3. **저장소 정보 입력**:
   - Repository name: `GLOOMY` 또는 `GLOOMY-Unity-Game`
   - Description: `🎮 감성 양육 시뮬레이션 게임 - Unity/C# 개인 프로젝트`
   - Public 선택 (포트폴리오용)
   - ⚠️ **README, .gitignore, license 체크 해제** (이미 로컬에 있음)
4. **Create repository 클릭**

---

## 3️⃣ 로컬 저장소와 GitHub 연결

GitHub에서 생성한 저장소 페이지에 나오는 명령어를 따라하세요:

```bash
# GitHub 저장소와 연결 (YOUR_USERNAME을 본인 계정명으로 변경)
git remote add origin https://github.com/YOUR_USERNAME/GLOOMY.git

# 기본 브랜치를 main으로 설정
git branch -M main

# GitHub에 업로드
git push -u origin main
```

### ⚠️ 처음 push할 때 인증 요구 시:

**방법 1: Personal Access Token (권장)**
1. GitHub → Settings → Developer settings → Personal access tokens → Tokens (classic)
2. Generate new token (classic)
3. repo 권한 체크
4. 생성된 토큰을 비밀번호 대신 입력

**방법 2: SSH Key 사용**
```bash
# SSH 키 생성
ssh-keygen -t ed25519 -C "your_email@example.com"

# 공개키 복사
cat ~/.ssh/id_ed25519.pub

# GitHub → Settings → SSH and GPG keys → New SSH key 에 붙여넣기

# 원격 저장소를 SSH 방식으로 변경
git remote set-url origin git@github.com:YOUR_USERNAME/GLOOMY.git
```

---

## 4️⃣ APK 파일 처리 (중요!)

**⚠️ 주의**: GLOOMY.apk 파일은 41MB로 GitHub의 파일 크기 제한(100MB)에는 걸리지 않지만, 저장소 용량을 많이 차지합니다.

### 옵션 A: GitHub Releases 활용 (권장)
```bash
# APK를 gitignore에 이미 추가했으므로, GitHub Releases에 별도 업로드
```

1. GitHub 저장소 → Releases → Create a new release
2. Tag version: `v0.1.0`
3. Release title: `GLOOMY v0.1.0 - Initial Release`
4. APK 파일 드래그 앤 드롭
5. Publish release

### 옵션 B: Git LFS 사용 (대용량 파일 관리)
```bash
# Git LFS 설치 (Mac)
brew install git-lfs

# Git LFS 초기화
git lfs install

# APK 파일을 LFS로 추적
git lfs track "*.apk"

# .gitattributes 커밋
git add .gitattributes
git commit -m "Add Git LFS tracking for APK files"

# APK 파일 추가 및 커밋
git add GLOOMY.apk
git commit -m "Add GLOOMY APK file via Git LFS"
git push
```

### 옵션 C: APK 제외 (가장 간단)
.gitignore에 이미 `*.apk`가 추가되어 있으므로, README에 다운로드 링크만 안내

---

## 5️⃣ README 업데이트 (선택 사항)

README.md 상단에 다운로드 링크 추가:

```markdown
## 📱 다운로드

- [📦 APK 다운로드 (v0.1.0)](https://github.com/YOUR_USERNAME/GLOOMY/releases/download/v0.1.0/GLOOMY.apk)
```

---

## 6️⃣ 업로드 후 확인 사항

✅ **필수 체크리스트**:
- [ ] README.md가 제대로 표시되는지
- [ ] 코드 파일들이 올라갔는지 (Assets, ProjectSettings 등)
- [ ] Library, Temp 폴더가 **제외**되었는지 (용량 확인)
- [ ] .gitignore가 작동하는지
- [ ] APK 다운로드 링크가 작동하는지 (Releases 사용 시)

---

## 💡 추가 팁

### 1. 저장소 주제(Topics) 추가
GitHub 저장소 → About → Topics → 다음 추가:
```
unity, unity3d, csharp, mobile-game, android-game, simulation-game, indie-game, portfolio
```

### 2. 스크린샷 추가
README.md에 게임 플레이 스크린샷 추가:
```markdown
## 📸 스크린샷

![메인 화면](./Screenshots/main.png)
![정원 화면](./Screenshots/garden.png)
```

### 3. 라이선스 추가
```bash
# MIT License 추가 (포트폴리오 프로젝트에 적합)
```
LICENSE 파일 생성: GitHub → Add file → Create new file → `LICENSE` 입력 → Choose a license template → MIT

### 4. GitHub Pages 활용
WebGL로 빌드해서 GitHub Pages에 배포하면 브라우저에서 바로 플레이 가능!

---

## 🆘 문제 해결

### "Repository size too large" 오류
```bash
# Library 폴더가 추가되었는지 확인
git rm -r --cached Library/
git commit -m "Remove Library folder"
git push
```

### Push 실패 시
```bash
# 최신 상태로 업데이트 후 재시도
git pull origin main --allow-unrelated-histories
git push -u origin main
```

### 대용량 파일 경고
```bash
# 해당 파일 제거
git rm --cached <파일명>
git commit -m "Remove large file"
```

---

## 📚 참고 자료

- [Unity GitHub 가이드](https://github.com/github/gitignore/blob/main/Unity.gitignore)
- [GitHub Releases 문서](https://docs.github.com/en/repositories/releasing-projects-on-github)
- [Git LFS 문서](https://git-lfs.github.com/)

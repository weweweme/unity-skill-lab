# **Tower Defense UI 구성**

## 1. **타워 관련 UI**

### 1.1 **타워 상태 패널**
- **역할**: 플레이어가 타워를 클릭했을 때, 해당 타워의 상태와 업그레이드 옵션을 표시합니다.
- **표시 정보**
    - 타워 이름/타입
    - 공격력, 공격 속도, 사거리 등 주요 스탯
    - 업그레이드 가능한 속성 및 비용
    - 타워 제거 버튼
- **예시**
    - [타워 이름: 기본 타워]
    - 공격력: 10
    - 공격 속도: 1.2초
    - 사거리: 3
    - [업그레이드 - 50 자원 필요]
    - [타워 제거 - 10 자원 환급]

### 1.2 **타워 건설 패널**
- **역할**: 플레이어가 타워를 배치할 수 있도록 옵션을 제공합니다.
- **표시 정보**
    - 건설 가능한 타워 목록
    - 각 타워의 건설 비용
    - 타워의 간략한 설명 (예: "단일 대상 고속 공격" 또는 "범위 공격")
- **동작**: 타워를 선택하면 마우스 커서가 타워 아이콘으로 바뀌며, 배치 가능 구역에 따라 초록색/붉은색 표시.
- **예시**
    - [기본 타워 - 20 자원]
    - [고속 타워 - 50 자원]
    - [범위 타워 - 75 자원]


---

## 2. **적 관련 UI**

### 2.1 **적 상태 표시 패널**
- **역할**: 적이 타워의 공격을 받을 때 머리 위에 체력 바를 표시합니다.
- **표시 정보**: 체력바 (슬라이더 형태, 남은 체력을 시각적으로 보여줌)

### 2.2 **적 통계 패널**
- **역할**: 전체 적의 진행 상황을 요약해서 표시.
- **표시 정보**
    - 현재 웨이브 번호
    - 남은 적 수 / 총 적 수
    - 도망친 적 수
    - 처치한 적 수
- **예시**
    - 현재 웨이브: 5
    - 남은 적: 12 / 20
    - 도망친 적: 3
    - 처치한 적: 45


---

## 3. **게임 전반 관련 UI**

### 3.1 **자원 표시 패널**
- **역할**: 현재 플레이어가 보유한 자원 표시.
- **표시 정보**
    - 현재 자원량
    - 자원 획득 방식(적 처치/웨이브 완료)
- **예시**
    - 현재 자원: 150

### 3.2 **게임 난이도 조정 패널**
- **역할**: 플레이어가 적 스폰 난이도(속도, 체력 증가량 등)를 조정.
- **표시 정보**
    - 타워 공격 속도 조정 (슬라이더)
    - 타워 공격력 조정 (슬라이더)
    - 적 팝업 속도 조정
    - 적 체력 조정

### 3.3 **게임 상태 표시 패널**
- **역할**: 현재 웨이브 진행 상태와 다음 웨이브에 대한 정보를 제공합니다.
- **표시 정보**
    - 현재 웨이브 시간 (예: "웨이브 진행 중" 또는 "다음 웨이브까지 남은 시간")
- **예시**
    - 현재 웨이브: 진행 중
    - 다음 웨이브까지: 10초

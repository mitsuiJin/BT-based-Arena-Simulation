## 프로젝트 개요

본 프로젝트는 Unity의 **ML-Agents Toolkit**을 활용하여 공격형(Attacker)과 방어형(Defender) 에이전트가 서로 다른 전략을 수행하도록 학습 및 제어하는 **AI 시뮬레이션 시스템**입니다. 환경 내에서 두 에이전트는 센서 인식 정보를 기반으로 공격·방어·회피·반격 등의 전략적 행동을 수행하며, AI의 의사결정을 **Behavior Tree 구조**로 설계하여 확장성과 재사용성을 높였습니다.

이를 통해 **학습형 에이전트와 규칙 기반 의사결정 로직의 결합 모델**을 구현하고, 각 행동 모듈을 노드 단위로 분리하여 **AI 행동의 트리거·쿨타임·상태 전이**를 유연하게 제어할 수 있도록 하였습니다.

## 담당 역할 및 주요 기여

### 1. Behavior Tree 설계 및 구조 구현

- `Selector`, `Sequence`, `Condition`, `Action` 노드 계층을 직접 설계하여 **트리 기반 의사결정 프레임워크 구축**
- 각 노드가 `Success / Failure / Running` 상태를 반환하도록 설계, **행동 흐름의 평가 루프 자동화**

### 2. 공격/방어형 전략 로직 구현

- **공격형(Attacker) 에이전트**
    - '추격(Chase) → 공격(Attack) → 회피(Evade)' 루틴을 Sequence 노드 기반으로 구성
    - 적의 위치·체력·거리 등의 조건 노드를 통해 **상황별 공격 판단** 로직 구현
- **방어형(Defender) 에이전트**
    - '방어(Defend) → 회피(Evade) → 반격(Counter)' 구조 설계
    - 다발 공격·단발 공격을 구분해 다른 방어 루틴 수행
    - 회피 후 일정 확률로 반격하는 **확률 기반 행동 선택(RandomChanceNode)** 구현

### 3. 상태 관리 및 행동 전이 시스템 개발

- `AgentBlackboard`를 통해 에이전트별 **체력, 쿨타임, 반응 여부, 행동 통계**를 중앙 관리
- `IsCooldownReady`, `SetCooldown` 메서드로 **행동 중복 방지 및 쿨타임 제어 로직** 구축

### Demo
https://www.youtube.com/watch?v=WsmU46M46Gg

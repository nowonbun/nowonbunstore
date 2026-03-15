# skill.md / AGENTS.md / Codex 설정 정리

## 1. 개요

이 문서는 Codex에서 `config.toml`, `AGENTS.md`, fallback 문서, Skills, MCP가 어떤 역할을 하는지 정리한 메모다.
핵심은 다음과 같다.

- `config.toml`은 Codex의 전역/프로젝트 설정이다.
- `AGENTS.md`는 프로젝트 작업 규칙 문서다.
- `README.md`는 프로젝트 설명 문서다.
- `skills/`는 작업 절차와 전문화된 규칙 모음이다.
- `mcp/`는 외부 도구/데이터 연결 계층이다.

---

## 2. Codex 설정 파일 위치

Codex 설정 파일은 보통 아래 위치에 둔다.

```txt
~/.codex/config.toml
```

또는 프로젝트별로:

```txt
project/.codex/config.toml
```

이 파일은 에이전트의 전체 동작 방식을 제어한다.

---

## 3. config.toml의 주요 설정 범주

대표적인 범주는 아래와 같다.

- Core Model Selection
- Reasoning & Verbosity
- Instruction Overrides
- Notifications
- Approval & Sandbox
- Authentication & Login
- Project Documentation Controls
- History & File Opener
- UI / Misc
- Web Search
- Agents

이 중 실무적으로 중요한 건 아래 5개다.

- `model`
- `approval_policy`
- `sandbox_mode`
- `web_search`
- `agents`

---

## 4. 주요 config 항목 정리

### 4.1 모델 설정

```toml
model = "gpt-5.4"
model_provider = "openai"
review_model = "gpt-5.4"
model_context_window = 128000
tool_output_token_limit = 12000
```

의미:

- `model`: 기본 모델
- `model_provider`: 모델 제공자
- `review_model`: `/review` 시 사용할 모델
- `model_context_window`: 최대 context 크기
- `tool_output_token_limit`: 툴 출력 최대 토큰 수

---

### 4.2 추론/출력 설정

```toml
model_reasoning_effort = "medium"
plan_mode_reasoning_effort = "high"
model_verbosity = "medium"
```

대표 옵션:

- reasoning: `minimal`, `low`, `medium`, `high`, `xhigh`
- verbosity: `low`, `medium`, `high`

---

### 4.3 instruction override

```toml
developer_instructions = ""
model_instructions_file = "./instructions.txt"
```

의미:

- `developer_instructions`: 강제 개발자 지침
- `model_instructions_file`: 별도 지침 파일

중요:
- 일반적으로 `developer_instructions`는 `AGENTS.md`보다 우선한다.

---

### 4.4 approval / sandbox

```toml
approval_policy = "on-request"
sandbox_mode = "workspace-write"
```

대표 옵션:

- `approval_policy`
  - `untrusted`
  - `on-request`
  - `never`

- `sandbox_mode`
  - `read-only`
  - `workspace-write`
  - `danger-full-access`

이 항목은 실제 작업 가능 범위를 크게 좌우한다.

---

### 4.5 프로젝트 문서 탐색 설정

```toml
project_doc_max_bytes = 32768
project_doc_fallback_filenames = ["README.md", "docs.md"]
project_root_markers = [".git"]
```

의미:

- `.git`을 기준으로 프로젝트 루트를 판단한다.
- 루트에서 `AGENTS.md`를 먼저 찾는다.
- 없으면 `README.md`, `docs.md` 같은 fallback 문서를 본다.

---

## 5. Codex가 AGENTS.md를 읽는 방식

핵심은 이거다.

- `AGENTS.md`를 config에 직접 경로 지정하는 방식이 아니라
- 프로젝트 루트에서 자동 탐색하는 방식으로 이해하는 게 맞다.

기본 흐름:

1. `.git` 기준으로 프로젝트 루트 탐색
2. 루트에 `AGENTS.md`가 있으면 자동 로드
3. 없으면 fallback 문서 확인

예시:

```txt
project
├─ .git
├─ AGENTS.md
├─ src
└─ README.md
```

이 구조에서 프로젝트 루트에서 Codex를 실행하면 `AGENTS.md`가 자동 적용된다고 이해하면 된다.

---

## 6. AGENTS.md가 적용되지 않는 흔한 이유

### 6.1 루트가 아닌 하위 폴더에만 있는 경우

```txt
project
└─ backend
   └─ AGENTS.md
```

루트에 없으면 기대한 방식으로 적용되지 않을 수 있다.

---

### 6.2 `.git` 위치가 다른 경우

```txt
workspace
├─ .git
└─ project
   └─ AGENTS.md
```

Codex가 `workspace`를 루트로 보면 `project/AGENTS.md`는 무시될 수 있다.

---

### 6.3 파일명이 틀린 경우

정확한 파일명은 아래다.

```txt
AGENTS.md
```

다음은 기대하지 않는 게 맞다.

```txt
agent.md
agents.md
AGENT.md
```

---

## 7. instruction 우선순위

대화 내용을 기준으로 정리하면 보통 아래 순서로 이해하면 된다.

1. CLI prompt
2. `config.toml`의 `developer_instructions`
3. `AGENTS.md`
4. system prompt
5. fallback 문서 (`README.md`, `docs.md`)
6. 기타 프로젝트 파일 (`package.json`, `requirements.txt`, `Dockerfile` 등은 주로 context)

즉 핵심은:

```txt
developer_instructions > AGENTS.md
```

---

## 8. 여러 AGENTS.md를 함께 쓰는 구조

Codex는 계층적으로 여러 `AGENTS.md`를 읽는 구조를 가질 수 있다.

예시:

```txt
project
├─ AGENTS.md
├─ backend
│  └─ AGENTS.md
└─ frontend
   └─ AGENTS.md
```

이 경우 보통:

- 루트 `AGENTS.md`
- 하위 디렉터리 `AGENTS.md`

를 함께 반영하는 구조로 이해하면 된다.

즉, 공통 규칙 + 하위 프로젝트 상세 규칙 형태가 가능하다.

---

## 9. fallback 문서

`AGENTS.md`가 없으면 보통 아래 문서를 fallback으로 본다.

- `README.md`
- `docs.md`

실무적으로는:

- `README.md`: 프로젝트 설명
- `AGENTS.md`: 작업 규칙

이렇게 분리하는 것이 깔끔하다.

---

## 10. 추천 프로젝트 구조

### 10.1 기본 구조

```txt
project
├─ .git
├─ AGENTS.md
├─ .codex
│  └─ config.toml
├─ mcp
└─ src
```

### 10.2 Agent + Skills + MCP 구조

```txt
project
├─ AGENTS.md
├─ README.md
├─ .codex
│  └─ config.toml
├─ skills
│  ├─ coding.md
│  └─ trading.md
├─ mcp
└─ src
```

---

## 11. AGENTS / Skills / MCP 역할 분리

### 11.1 AGENTS.md
- 에이전트 행동 규칙
- 코딩 스타일
- 프로젝트 작업 원칙

### 11.2 Skills
- 특정 작업용 절차
- 도메인별 작업 규칙
- 재사용 가능한 작업 템플릿

### 11.3 MCP
- 도구 연결
- 외부 시스템 접근
- 데이터/기능 확장

즉 실전에서는 아래 3개 축으로 나누는 게 가장 명확하다.

```txt
AGENTS.md
skills/
mcp/
```

---

## 12. 실무적 해석

이 대화의 핵심 해석은 아래와 같다.

- `config.toml`은 전역/프로젝트 설정
- `AGENTS.md`는 프로젝트 행동 규칙
- `README.md`는 프로젝트 설명
- `skills/`는 작업 템플릿/전문화 규칙
- `mcp/`는 도구 연결 계층

역할을 섞지 말고 분리해야 관리가 쉬워진다.

---

## 13. 핵심 요약

반드시 기억할 핵심은 아래 6개다.

1. `AGENTS.md`는 보통 프로젝트 루트에서 자동 로드된다.
2. 별도 경로 지정형보다 루트 탐색형으로 이해하는 게 맞다.
3. `.git` 위치가 루트 판단에 중요하다.
4. `developer_instructions`는 `AGENTS.md`보다 우선할 수 있다.
5. 루트/하위 디렉터리에 여러 `AGENTS.md`를 두는 계층 구조가 가능하다.
6. 실전에서는 `AGENTS.md + skills + MCP` 조합이 가장 유용하다.

---

## 14. 추천 메모

실전에서 Codex 구조를 설계할 때는 아래처럼 생각하면 된다.

- 전역 설정은 `.codex/config.toml`
- 프로젝트 규칙은 `AGENTS.md`
- 프로젝트 설명은 `README.md`
- 작업별 전문 규칙은 `skills/`
- 외부 도구 연결은 `mcp/`

이렇게 나누면 구조가 가장 덜 꼬인다.

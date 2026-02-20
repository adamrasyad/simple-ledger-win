# simple-ledger-win
A lightweight personal finance tracker for rapid expense logging.

# Background
Financial is very important to be maintained, especially for college student. Back then at 2023, when I was learning finite state machine automata, I got inspiration to make financial expense tracker. So I decided to make simple ledger maker stored in CSV with these states.
State : 
    1. Menu,
    2. Add file,
    3. Edit file,
    4. Open file,
    5. Render ledger.

## Features
- CSV-based ledger storage
- Monthly balance grouping
- Tag filtering
- Descriptive statistics

## State Flow
```mermaid
stateDiagram-v2
    [*] --> Empty : App Launch
    
    state Empty {
        [*] --> Initialized
    }

    Empty --> Editing : Row Added /<br/>CSV Imported
    
    state Editing {
        [*] --> Clean : Snapshot Saved
        Clean --> Dirty : Cell Edited
        Dirty --> Clean : Ctrl+S<br/>(Save)
        
        state "Undo/Redo Stack" as UR {
            direction LR
            PrevSnapshot --> CurrentState
            CurrentState --> NextSnapshot
        }
    }

    Editing --> Validating : CellEndEdit
    Validating --> Editing : Valid<br/>(AddSnapshot)
    Validating --> ErrorState : Invalid<br/>Math/Date
    ErrorState --> Editing : User Corrects

    Editing --> ExitPrompt : 'Back' or 'Close'
    ExitPrompt --> [*] : IsClean /<br/>Discard
    ExitPrompt --> Saving : User chooses 'Save'
    Saving --> [*] : Success
```

## Domain Model
```mermaid
classDiagram

class Log {
    +Guid Id
    +DateTime Date
    +decimal Debit
    +decimal Credit
    +string Description
    +string Reference
    +List~string~ Tags
}

class MonthLog {
    +DateTime Month
    +List~Log~ Logs
    +decimal Balance
}

class Ledger {
    +List~MonthLog~ Months
    +decimal TotalBalance
    +GetLogAtMonth(DateTime)
    +GetOrCreateMonth(DateTime)
    +UpdateLogDate(Log, DateTime)
}

Ledger "1" *-- "many" MonthLog
MonthLog "1" *-- "many" Log
```

## Example UI
<p align="center">
  <img src="./screenshot/example.png" alt="Simple Ledger UI" width="700">
</p>

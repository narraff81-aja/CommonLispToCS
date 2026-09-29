# CommonLispToCS (Transpiler da Common Lisp a C#)



**CommonLispToCS** è un transpiler / convertitore di codice scritto in C# con l'obiettivo di analizzare, interpretare e tradurre automaticamente sorgenti legacy in **Common Lisp** verso il linguaggio **C#**.



Il sistema sfrutta un approccio basato su **tokenizzazione**, **analisi ad albero sintattico astratto (AST)** e **ricostruzione strutturata** dei costrutti funzionali di Lisp in costrutti orientati agli oggetti e imperativi di C#.



---



## 🛠️ Caratteristiche Principali



- **Parsing delle S-Expressions**: Gestione completa della sintassi a parentesi annidate di Lisp con un sistema ricorsivo a token (`OpenToken`, `CloseToken`, `ElementToken`, `ListToken`).

- **Conversione dei Tipi e Tipi Personalizzati**:

  - Implementazione della struct `Rational` per la gestione nativa dei numeri razionali Lisp (es. `2345/5675`) e calcolo MCD.

  - Mappatura nativa dei tipi di ritorno e parametri (`dynamic`, `var`, tipi specifici).

- **Mappatura Aritmetica e Condizionale**:

  - Conversione automatica di operatori n-ari (`+`, `-`, `*`, `/`) e unari (`1+`, `1-`).

  - Mappatura di espressioni di confronto (`=`, `/=`, `<`, `>`, `<=`, `>=`) ed espressioni logiche (`and`, `or`, `not`).

- **Traduzione dei Blocchi di Controllo e Macro**:

  - Mappatura di `if`, `cond` (convertito in catene `if / else if / else`), `let` e `let*` (trasformati in contesti di variabili locali C#).

  - Gestione avanzata del blocco `sigma` convertito automaticamente in cicli `for` per il calcolo di sommatorie.

- **Supporto Funzioni Matematiche e Macro Custom**:

  - Traduzione automatica di funzioni Lisp verso System.Math (es. `ceiling` ➔ `Math.Ceiling`, `expt` ➔ `Math.Pow`).

  - Implementazione di macro avanzate come `binary\_search`, `invert\_angular`, `next`, `final` e `sum`.

- **Mantenimento Commenti e Intestazioni**: Preservazione dei commenti in Lisp convertiti in sintassi C# (`//`, `/* */` e markup XML `/// <summary>`).



---



## 📁 Struttura del Progetto



| File | Descrizione |

| :--- | :--- |

| `Program.cs` | Engine principale di lettura sorgente Lisp, tokenizzazione, analisi ed esportazione del codice convertito. |

| `IToken.cs` | Interfaccia ed estensioni dei token (`OpenToken`, `CloseToken`, `ElementToken`, `ListToken`). |

| `IValueTkn.cs` | Mappatura specifica dei token a valore: `OperationTkn`, `ConditionTkn`, `ArrayTkn`, `BlockTkn`, `SigmaBlockTkn`, `FunctionTkn`. |

| `Rational.cs` | Struct C# per la gestione e l'aritmetica dei numeri razionali con semplificazione via MCD. |

| `Macros.cs` | Implementazione delle macro avanzate e funzioni ausiliarie di ricerca/iterazione. |

| `Constant.cs` | Modello e generatore di sintassi C# per le costanti Lisp (`defconstant`). |

| `Fun.cs` | Modello e generatore per le funzioni Lisp (`defun`). |

| `Macro.cs` | Interfaccia ed emettitore per la gestione manuale delle macro Lisp. |



---



## 🚀 Come Funziona



1. **Lettura Sorgente**: Il programma legge il file contenente codice Common Lisp riga per riga.

2. **Tokenizzazione (`TakeToken`)**: Suddivide la stringa di input in token tenendo traccia dei livelli di nidificazione (`Ident`).

3. **Analisi Sintattica (`AnalizeTokens` / `ChangeToken`)**: Ricostruisce le S-Expressions in nodi AST e sostituisce le strutture Lisp nei corrispondenti oggetti C#.

4. **Generazione Codice (`ToCS`)**: Ogni oggetto generato produce il corrispondente blocco di codice C# formattato.



---



## 💻 Requisiti



- **.NET Framework 4.7+** o **.NET Core / .NET 6+**

- Visual Studio 2019 o superiore / Visual Studio Code




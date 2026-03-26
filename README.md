# Дополнительное задание к лабораторной №3

## Постановка задачи

Необходимо реализовать лексический и синтаксический анализ конструкции с помощью программного обеспечения ANTLR

## Вариант задания

Лексический анализатор должен распознавать конструкцию объявления ассоциативного массива с инициализацией на языке C#.

Пример конструкции:

```csharp
Dictionary<int, string> My_dict1 = new Dictionary<int, string> {
    { 1, "one" },
    { 2, "two" },
    { 3, "three" }
};
```

---


## Используемые технологии

-   C#
-   WPF
-   ANTLR 4

------------------------------------------------------------------------

## Грамматика
```text
Dictionary Declaration = "Dictionary", "<", "int", ",", "string", ">",
                         Dictionary Identifier,
                         "=",
                         "new", "Dictionary", "<", "int", ",", "string", ">",
                         "{",
                         Dictionary Element,
                         {",", Dictionary Element},
                         "}", ";";

Dictionary Element = "{", Number, ",", String, "}";
Dictionary Identifier = letter, {letter | digit | "_"};
Number = digit, {digit};
String = "\"", {symbol}, "\"";
```

------------------------------------------------------------------------

## Грамматика ANTLR

``` antlr
grammar DictionaryDeclaration;

dictionaryDeclaration
    : DICTIONARY LT INT COMMA STRING_TYPE GT
      dictionaryIdentifier
      ASSIGN
      NEW DICTIONARY LT INT COMMA STRING_TYPE GT
      LBRACE
      dictionaryElement (SEMI dictionaryElement)*
      RBRACE
      SEMI
      EOF
    ;

dictionaryElement
    : LBRACE number COMMA string RBRACE
    ;

dictionaryIdentifier
    : IDENTIFIER
    ;

number
    : NUMBER
    ;

string
    : STRING
    ;

DICTIONARY  : 'Dictionary';
NEW         : 'new';
INT         : 'int';
STRING_TYPE : 'string';

ASSIGN : '=';
LT     : '<';
GT     : '>';
COMMA  : ',';
SEMI   : ';';
LBRACE : '{';
RBRACE : '}';

IDENTIFIER
    : LETTER (LETTER | DIGIT | '_')*
    ;

NUMBER
    : DIGIT+
    ;

STRING
    : '"' (~["\r\n])* '"'
    ;

fragment LETTER
    : [a-zA-Z]
    ;

fragment DIGIT
    : [0-9]
    ;

WS
    : [ \t\r\n]+ -> skip
    ;
```

------------------------------------------------------------------------
## Тестовые примеры
### Корректная строка
```csharp
Dictionary<int, string> My_dict1 = new Dictionary<int, string> {
    { 1, "one" },
    { 2, "two" },
    { 3, "three" }
};
```
**Результат:**
<img width="712" height="603" alt="image" src="https://github.com/user-attachments/assets/b5718610-43e2-4a1a-9ada-8dda292ae7e4" />

### Несколько ошибок
```csharp
Dictionary<int, string> dict = new Dictionary<int, string>{ {,"one"}; {2,two} };
```
**Результат:**
<img width="708" height="702" alt="image" src="https://github.com/user-attachments/assets/ff3194f5-9c0b-4022-ae57-cc2a92f73d34" />


grammar DictionaryDeclaration;

dictionaryDeclaration
    : dictionaryType IDENTIFIER ASSIGN NEW dictionaryType LBRACE pairList? RBRACE SEMI EOF
    ;

dictionaryType
    : DICTIONARY LT type COMMA type GT
    ;

type
    : INT
    | STRING
    | BOOL
    | DOUBLE
    | FLOAT
    | CHAR
    | LONG
    | SHORT
    | BYTE
    | DECIMAL
    | IDENTIFIER
    ;

pairList
    : pair (COMMA pair)*
    ;

pair
    : LBRACE key COMMA value RBRACE
    ;

key
    : literal
    ;

value
    : literal
    ;

literal
    : INTEGER_LITERAL
    | STRING_LITERAL
    | BOOL_LITERAL
    | FLOAT_LITERAL
    | CHAR_LITERAL
    | NULL_LITERAL
    ;

DICTIONARY : 'Dictionary';
NEW        : 'new';

INT        : 'int';
STRING     : 'string';
BOOL       : 'bool';
DOUBLE     : 'double';
FLOAT      : 'float';
CHAR       : 'char';
LONG       : 'long';
SHORT      : 'short';
BYTE       : 'byte';
DECIMAL    : 'decimal';

BOOL_LITERAL : 'true' | 'false';
NULL_LITERAL : 'null';

ASSIGN : '=';
LT     : '<';
GT     : '>';
COMMA  : ',';
SEMI   : ';';
LBRACE : '{';
RBRACE : '}';

IDENTIFIER
    : [a-zA-Z_][a-zA-Z_0-9]*
    ;

INTEGER_LITERAL
    : [0-9]+
    ;

FLOAT_LITERAL
    : [0-9]+ '.' [0-9]+
    ;

STRING_LITERAL
    : '"' ( '\\' . | ~["\\\r\n] )* '"'
    ;

CHAR_LITERAL
    : '\'' ( '\\' . | ~['\\\r\n] ) '\''
    ;

WS
    : [ \t\r\n]+ -> skip
    ;
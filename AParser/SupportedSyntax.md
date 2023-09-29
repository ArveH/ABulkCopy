# Currently supported syntax

The plan is to create a general SQL parser for parsing default clauses, functions, views, etc. But to start off with, this parser will have an extremely limited scope.

## Expressions Bacus-Naur Form

I'm using the same simple Extended Backus-Naur Form (EBNF) notation as [W3C Extensible Markup Language (XML) 1.0 (Fifth Edition)](https://www.w3.org/TR/xml/#sec-notation)

```ebnf
expression ::= parenteces 
                | function 
                | type 
                | constant

parenteces ::= '(' expression ')'

function ::= functionName '(' expression (',' expression)* ')'
functionName ::= 'convert'
type ::= '[' typeName ']' | typeName
typeName ::= 'bit'

constant ::= numberConstant
numberConstant ::= [0-9]+
```
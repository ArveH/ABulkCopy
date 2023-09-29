# Currently supported syntax

The plan is to create a general SQL parser for parsing default clauses, functions, views, etc. But to start off with, this parser will have an extremely limited scope.

## Expressions Bacus-Naur Form

I'm using Extended Backus-Naur Form (EBNF) notation. I'm not following the spec 100%, but more like this: [W3C Extensible Markup Language (XML) 1.0 (Fifth Edition)](https://www.w3.org/TR/xml/#sec-notation)

NOTE: Currently, whitespace is not specified in the grammar, but it could occur anywhere except in the middle of a token.

## Syntax
``` ebnf
Expression ::= Parentheses 
                | Function 
                | Type 
                | Constant

Parentheses ::= LeftParenthesesToken Expression RightParenthesesToken

Function ::= NameToken LeftParenthesesToken Expression (CommaToken Expression)* RightParenthesesToken
Type ::= SquareLeftParenthesesToken NameToken SquareRightParenthesesToken | NameToken

Constant ::= NumberToken
FunctionName ::= NameToken
```

### Tokens

``` ebnf
Whitespace ::= ' ' | '\t' | '\n' | '\r'
LeftParenthesesToken ::= '('
RightParenthesesToken ::= ')'
SquareLeftParenthesesToken ::= '['
SquareRightParenthesesToken ::= ']'
CommaToken ::= ','

NumberToken ::= [0-9]+
NameToken ::= [a-zA-Z_][a-zA-Z0-9_]*
```

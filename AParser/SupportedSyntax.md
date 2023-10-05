# Currently supported syntax

The plan is to create a general SQL parser for parsing default clauses, functions, views, etc. But to start off with, this parser will have an extremely limited scope.

## Extended Bacus-Naur Form

I'm using Extended Backus-Naur Form (EBNF) notation. I'm not following the spec 100%, but more like this: [W3C Extensible Markup Language (XML) 1.0 (Fifth Edition)](https://www.w3.org/TR/xml/#sec-notation)

NOTE: Currently, whitespace is not specified in the grammar, but it could occur anywhere between tokens.

## Tokens

``` ebnf
CommaToken ::= ','
LeftParenthesesToken ::= '('
NameToken ::= [a-zA-Z_][a-zA-Z0-9_]*
NumberToken ::= [0-9]+
QuotedNameToken ::= '[' [a-zA-Z0-9_ -.]+ ']'
RightParenthesesToken ::= ')'
```

## Parsing

``` ebnf
ConvertFunction ::= 'convert' LeftParenthesesToken Type CommaToken Expression RightParenthesesToken
Expression ::= Parentheses 
                    | Function
                    | Number
Function ::= ConvertFunction
Name ::= NameToken | QuotedNameToken
Number ::= NumberToken
Parentheses ::= LeftParenthesesToken Expression RightParenthesesToken
Type ::= 'bit'
```

### Nodes

``` ebnf
ConvertFunctionNode
CommaNode
LeftParenthesesNode
NameNode
NumberNode
ParenthesesNode
QuotedNameNode
RightParenthesesNode
TypeNode
```



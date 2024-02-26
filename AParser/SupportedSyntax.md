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
NumberToken ::= -?[0-9]*.?[0-9]+ // Optional '-' sign, followed by a number, followed by an optional '.' and a number
QuotedNameToken ::= '[' [a-zA-Z0-9_ -.]+ ']'
RightParenthesesToken ::= ')'
StringToken ::= ['] [.]* ['] // NOTE: The string can contain any character. A single quote is escaped by using two single quotes.
NStringToken ::= 'N'StringToken  // NOTE: No space between the 'N' and the string
```

## Parsing

``` ebnf
ConvertFunction ::= ConvertToNumberFunction | ConvertToDateTimeFunction
ConvertToNumberFunction ::= 'convert' LeftParenthesesToken Type CommaToken Expression RightParenthesesToken
ConvertToDateTimeFunction ::= 'convert' LeftParenthesesToken Type CommaToken Expression [commaToken Expression] RightParenthesesToken
RightParenthesesToken
Expression ::= Parentheses 
                    | Function
                    | Number
                    | String
Function ::= ConvertFunction
Name ::= NameToken | QuotedNameToken
Number ::= NumberToken
String ::= StringToken | NStringToken
Parentheses ::= LeftParenthesesToken Expression RightParenthesesToken
Type ::= 'bit' | 'datetime'
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



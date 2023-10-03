# Currently supported syntax

The plan is to create a general SQL parser for parsing default clauses, functions, views, etc. But to start off with, this parser will have an extremely limited scope.

## Extended Bacus-Naur Form

I'm using Extended Backus-Naur Form (EBNF) notation. I'm not following the spec 100%, but more like this: [W3C Extensible Markup Language (XML) 1.0 (Fifth Edition)](https://www.w3.org/TR/xml/#sec-notation)

NOTE: Currently, whitespace is not specified in the grammar, but it could occur anywhere except in the middle of a token.

## Tokenizer

``` ebnf
CommaToken ::= ','
LeftParenthesesToken ::= '('
NameToken ::= [a-zA-Z_][a-zA-Z0-9_]*
NumberToken ::= [0-9]+
RightParenthesesToken ::= ')'
SquareLeftParenthesesToken ::= '['
SquareRightParenthesesToken ::= ']'
```

## ParseTree

### Nodes
``` ebnf
ExpressionNode ::= ParenthesesNode 
                    | NameNode
                    | QuotedNameNode
                    | FunctionNode 
                    | TypeNode 
                    | ConstantNode

ParenthesesNode ::= LeftParenthesesLeafNode ExpressionNode RightParenthesesLeafNode

FunctionNode ::= ConvertFunctionNode
ConvertFunctionNode ::= NameLeafNode LeftParenthesesLeafNode TypeLeafNode CommaLeafNode ConstantLeafNode RightParenthesesLeafToken
QuotedNameNode ::= LeftParenthesesLeafNode NameLeafNode RightParenthesesLeafNode
```

### LeafNodes

``` ebnf
LeftParenthesesLeafNode ::= LeftParenthesesToken
RightParenthesesLeafNode ::= RightParenthesesToken
SquareLeftParenthesesLeafNode ::= SquareLeftParenthesesToken
SquareRightParenthesesLeafNode ::= SquareRightParenthesesToken
CommaLeafNode ::= CommaToken
NameLeafNode ::= NameToken
TypeLeafNode ::= TypeToken
ConstantLeafNode ::= ConstantToken
```

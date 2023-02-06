using System.Text;

namespace gamification_backend.Stub;

public static class StubGenerator
{
    public enum Language
    {
        Java
    }

    public static string GenerateCode(Language language, StubParser parser)
    {
        var codeTokens = ParseTokens(parser);
        return language switch
        {
            Language.Java => GenerateJava(codeTokens),
            _ => ""
        };
    }

    private static List<CodeToken> ParseTokens(StubParser parser)
    {
        List<CodeToken> codeTokens = new();
        while (!parser.Done())
        {
            var token = parser.Next();
            if (token.IsString())
                switch (token.Value)
                {
                    case "read":
                    {
                        codeTokens.Add(ParseRead(parser));
                        continue;
                    }
                    case "write":
                    {
                        codeTokens.Add(ParseWrite(parser));
                        continue;
                    }
                    case "loopline":
                    case "loop":
                    {
                        var limit = parser.Next().Value;
                        Loop codeToken = new(limit);
                        while (!parser.NextIsNewLine()) codeToken.Variables.Add(ParseVariable(parser));

                        codeTokens.Add(codeToken);
                        continue;
                    }
                }
        }

        return codeTokens;
    }

    private static Write ParseWrite(StubParser parser)
    {
        var value = parser.RestOfLineAsString();
        Write codeToken = new(value);
        return codeToken;
    }

    private static Variable ParseVariable(StubParser parser)
    {
        var name = parser.Next().Value;
        if (!parser.NextIsSeparator())
            throw new Exception("parse error, expected \":\"");
        parser.ConsumeOne();
        var type = GetVariableType(parser.Next().Value);
        Variable variable = new(name, type);
        return variable;
    }


    private static Read ParseRead(StubParser parser)
    {
        Read codeToken = new(ParseVariable(parser));
        return codeToken;
    }

    private static Variable.Type GetVariableType(string input)
    {
        return input switch
        {
            "int" => Variable.Type.Integer,
            "string" => Variable.Type.String,
            "long" => Variable.Type.Long,
            "double" => Variable.Type.Double,
            "float" => Variable.Type.Float,
            "boolean" => Variable.Type.Boolean,
            _ => Variable.Type.Undefined
        };
    }

    private static string Tabs(int amount)
    {
        return string.Concat(Enumerable.Repeat("    ", amount));
    }

    private static string GenerateVariable(Variable variable)
    {
        StringBuilder sb = new();
        switch (variable.VariableType)
        {
            case Variable.Type.Integer:
                sb.AppendLine($"int {variable.Name} = in.nextInt();");
                break;
            case Variable.Type.Float:
                sb.AppendLine($"float {variable.Name} = in.nextFloat();");
                break;
            case Variable.Type.Double:
                sb.AppendLine($"double {variable.Name} = in.nextDouble();");
                break;
            case Variable.Type.Long:
                sb.AppendLine($"long {variable.Name} = in.nextLong();");
                break;
            case Variable.Type.String:
                sb.AppendLine($"String {variable.Name} = in.nextLine();");
                break;
            case Variable.Type.Boolean:
                sb.AppendLine($"boolean {variable.Name} = in.nextBoolean();");
                break;
            case Variable.Type.Undefined:
                sb.AppendLine($"var {variable.Name} = in.nextLine();");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return sb.ToString();
    }

    private static string GenerateJava(List<CodeToken> codeTokens)
    {
        StringBuilder sb = new();
        sb.AppendLine("import java.util.*;");
        sb.AppendLine("import java.io.*;");
        sb.AppendLine("import java.math.*;");
        sb.AppendLine("");
        sb.AppendLine("public class Solution {");
        sb.Append(Tabs(1));
        sb.AppendLine("public static void main(String[] args){");
        sb.Append(Tabs(2));
        sb.AppendLine("Scanner in = new Scanner(System.in);");
        foreach (var token in codeTokens)
        {
            sb.Append(Tabs(2));
            switch (token.Type)
            {
                case CodeTokenType.Read:
                    var read = (Read) token;
                    sb.Append(GenerateVariable(read.Variable));
                    break;
                case CodeTokenType.Write:
                    var write = (Write) token;
                    sb.AppendLine($"System.out.println(\"{write.Value}\");");
                    break;
                case CodeTokenType.Loop:
                case CodeTokenType.Loopline:
                    var loop = (Loop) token;
                    sb.Append($"for(int i = 0; i < {loop.Limit}; i++)");
                    sb.AppendLine(" {");
                    foreach (var loopVariable in loop.Variables)
                    {
                        sb.Append(Tabs(3));
                        sb.Append(GenerateVariable(loopVariable));
                    }

                    sb.Append(Tabs(2));
                    sb.AppendLine("}");
                    break;
                case CodeTokenType.Variable:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        sb.Append(Tabs(1));
        sb.AppendLine("}");
        sb.AppendLine("}");
        return sb.ToString();
    }

    private class CodeToken
    {
        protected CodeToken(CodeTokenType type)
        {
            Type = type;
        }

        public CodeTokenType Type { get; }
    }

    private class Write : CodeToken
    {
        public Write(string value) : base(CodeTokenType.Write)
        {
            Value = value;
        }

        public string Value { get; }
    }

    private class Read : CodeToken
    {
        public Read(Variable variable) : base(CodeTokenType.Read)
        {
            Variable = variable;
        }

        public Variable Variable { get; }
    }

    private class Loop : CodeToken
    {
        public Loop(string limit) : base(CodeTokenType.Loop)
        {
            Limit = limit;
            Variables = new List<Variable>();
        }

        public string Limit { get; }
        public List<Variable> Variables { get; }
    }

    private class Variable : CodeToken
    {
        public Variable(string name, Type type) : base(CodeTokenType.Variable)
        {
            Name = name;
            VariableType = type;
        }

        public string Name { get; }
        public Type VariableType { get; }

        internal new enum Type
        {
            Integer,
            Float,
            Double,
            Long,
            String,
            Boolean,
            Undefined
        }
    }

    private enum CodeTokenType
    {
        Read,
        Write,
        Loop,
        Loopline,
        Variable
    }
}
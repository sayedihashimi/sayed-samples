namespace RazorCSharpSupport {

    public class Demo7 {
        SampleCode scode = new SampleCode();
        public void NRefDemo() {
            ref int refNum = ref scode.GetNumberRef(2);
        }
    }

    public class SampleCode {
        // C# 7.0: Out variable declaration
        public bool TryParseInt(string input, out int result)
        {
            return int.TryParse(input, out result);
        }

        // C# 7.0: Tuples and deconstruction
        public (int sum, int product) Calculate(int a, int b)
        {
            return (a + b, a * b);
        }

        // C# 7.0: Pattern matching with 'is'
        public string TypeCheck(object obj)
        {
            if (obj is int i)
                return $"Integer: {i}";
            if (obj is string s)
                return $"String: {s}";
            return "Unknown type";
        }

        // C# 7.0: Pattern matching in switch
        public string SwitchType(object obj)
        {
            switch (obj)
            {
                case int i:
                    return $"int: {i}";
                case string s:
                    return $"string: {s}";
                default:
                    return "other";
            }
        }

        // C# 7.0: Local functions
        public int Fibonacci(int n)
        {
            int Fib(int x)
            {
                if (x <= 1) return x;
                return Fib(x - 1) + Fib(x - 2);
            }
            return Fib(n);
        }

        // C# 7.0: Expression-bodied members
        public int Square(int x) => x * x;

        // C# 7.0: Throw expressions
        public string GetNonNull(string value) => value ?? throw new System.ArgumentNullException(nameof(value));

        // C# 7.0: Digit separators and binary literals
        public int GetBinaryLiteral() => 0b1010_1010;

        // C# 7.0: Ref returns and locals
        private int[] numbers = { 1, 2, 3, 4, 5 };
        public ref int GetNumberRef(int index) => ref numbers[index];
    }
}

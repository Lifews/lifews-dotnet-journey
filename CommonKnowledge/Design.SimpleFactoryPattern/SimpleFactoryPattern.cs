//简单工厂模式

Operation operation;

operation = OperationFactory.CreateOperation("+");

operation.Operand1 = 10;
operation.Operand2 = 5;

double result = operation.GetResult();
Console.WriteLine(result);

public class OperationFactory
{
    public static Operation CreateOperation(string operationType)
    {
        return operationType switch
        {
            "+" => new OperationAdd(),
            "-" => new OperationSub(),
            "*" => new OperationMul(),
            "/" => new OperationDiv(),
            _ => throw new InvalidOperationException("无效的操作类型"),
        };
    }
}

class OperationAdd : Operation
{
    public override double GetResult()
    {
        return Operand1 + Operand2;
    }
}

class OperationSub : Operation
{
    public override double GetResult()
    {
        return Operand1 - Operand2;
    }
}

class OperationMul : Operation
{
    public override double GetResult()
    {
        return Operand1 * Operand2;
    }
}

class OperationDiv : Operation
{
    public override double GetResult()
    {
        if (Operand2 == 0)
        {
            throw new DivideByZeroException("除数不能为零");
        }
        return Operand1 / Operand2;
    }
}

public class Operation
{
    public double Operand1 { get; set; }
    public double Operand2 { get; set; }

    public virtual double GetResult()
    {
        return 0;
    }
}

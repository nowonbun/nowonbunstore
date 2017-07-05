using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Calculator
{
    public class Calculate
    {
        private String[] OPERATION1 = { "(", ")", "," };
        private String[] OPERATION2 = { "!" };
        private String[] OPERATION3 = { "+", "-", "*", "/", "^", "%" };
        private String[] WORD_OPERATION1 = { "pi", "e" };
        private String[] WORD_OPERATION2 = { "sin", "sinh", "asin", "cos", "cosh", "acos", "tan", "tanh", "atan", "sqrt", "exp", "abs", "log", "ceil", "floor" };
        private String[] WORD_OPERATION3 = { "pow", "round" };

        private static Calculate mInstance = null;
        //싱글톤 패턴
        private Calculate() { }
        private static Calculate getInstance()
        {
            if (mInstance == null)
            {
                mInstance = new Calculate();
            }
            return mInstance;
        }
        public static Decimal calculate(String data)
        {
            return Calculate.getInstance().start(data);
        }
        private Decimal start(String data)
        {
            //문자열을 모두 소문자로 변환한다.
            data = data.ToLower();
            //계산식 안의 빈칸을 없앤다.
            data = data.Replace(" ", "");
            //토큰으로 구분, 즉 구분이 되는 수, 연산자를 분할
            // 예) pow(2+3,1+2) 일 경우, [pow,(,2,+,3,,,1,+,2]로 분할한다.
            IList tokenStack = makeTokens(data);
            //후위 표기식으로 변환
            // 예) [pow,(,2,+,3,,,1,+,2] 일 경우 [2,3,+,(,1,2,+,pow]
            tokenStack = convertPostOrder(tokenStack);
            Stack<Object> calcStack = new Stack<Object>();
            // 후위 표기식 계산
            // List형식으로 tocken이 수가 나오면 스택, 연산자가 나오면 계산을 합니다.
            // 2 3 + 계산 5
            // ( 스택전환
            // 1 2 + 계산 3
            // pow는 (가 나올 때까지 스택 Pop
            // pow 3 5 나오면 pow (5,3)으로 계산 125
            foreach (Object s in tokenStack)
            {
                calcStack.Push(s);
                calcPostOrder(calcStack);
            }
            //값이 없으면 에러
            if (calcStack.Count != 1)
            {
                throw new Exception("계산 에러");
            }
            return (Decimal)calcStack.Pop();
        }
        /// <summary>
        /// 토큰을 만드는 함수
        /// </summary>
        /// <param name="value">입력 문자열</param>
        /// <returns>각 토큰 별로 리스트로 반환한다.</returns>
        private IList makeTokens(String value)
        {
            IList tokenList = new ArrayList();
            StringBuilder numberTokenBuffer = new StringBuilder();
            StringBuilder wordTokenBuffer = new StringBuilder();
            //문자열을 char형으로 쪼갠다.
            char[] arrayToken = value.ToCharArray();
            foreach (char token in arrayToken)
            {
                if (!isOperation(token))
                {
                    //분할 문자가 숫자일 경우
                    //문자 버퍼에 데이터가 있으면 일단 리스트에 집어넣기
                    setWord(tokenList, wordTokenBuffer);
                    numberTokenBuffer.Append(token);
                }
                else
                {
                    //분할 문자가 문자일 경우
                    //숫자 버퍼에 데이터가 있으면 일단 리스트에 집어넣기
                    setNumber(tokenList, numberTokenBuffer);
                    if (setOperation(tokenList, token))
                    {
                        continue;
                    }
                    //문자 연산자일경우 버퍼에 넣는다.
                    wordTokenBuffer.Append(token);
                    //문자 버퍼의 단어가 const에 선언된 연산자에 있는지 체크
                    setWord(tokenList, wordTokenBuffer);
                }
            }
            //남아있는 버퍼에 리스트에 담기
            setNumber(tokenList, numberTokenBuffer);
            setWord(tokenList, wordTokenBuffer);

            return tokenList;
        }
        private Boolean isOperation(String token)
        {
            return containWord(token, OPERATION2) || containWord(token, OPERATION3);
        }
        /// <summary>
        /// 문자열이 숫자인지 문자인지 구분하는 함수
        /// </summary>
        /// <param name="token">char형의 문자</param>
        /// <returns>   
        ///     true - 문자열
        ///     false - 숫자
        /// </returns>
        private Boolean isOperation(char token)
        {
            if (token >= 48 && token <= 57 || token == 46)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 토큰을 Decimal형식으로 변환하는 함수
        /// </summary>
        /// <param name="tokenList">토큰 리스트</param>
        /// <param name="tokenBuffer">토큰 버퍼</param>
        private void setNumber(IList tokenList, StringBuilder tokenBuffer)
        {
            if (tokenBuffer.Length > 0)
            {
                String buffer = tokenBuffer.ToString();
                tokenBuffer.Clear();
                tokenList.Add(Decimal.Parse(buffer));
            }
        }
        /// <summary>
        /// 토큰을 String형식으로 변환하는 함수
        /// </summary>
        /// <param name="tokenList">토큰 리스트</param>
        /// <param name="tokenBuffer">토큰 버퍼</param>
        private void setWord(IList tokenList, StringBuilder tokenBuffer)
        {
            if (tokenBuffer.Length > 0)
            {
                String buffer = tokenBuffer.ToString();
                if (isWordOperation(buffer))
                {
                    tokenBuffer.Clear();
                    tokenList.Add(buffer);
                }
            }
        }
        /// <summary>
        /// 문자 연산자 체크 함수
        /// </summary>
        /// <param name="tokenWord">토큰</param>
        /// <returns>문자 연산자이면 true, 아니면 false</returns>
        private Boolean isWordOperation(String tokenWord)
        {
            if (containWord(tokenWord, WORD_OPERATION1) || 
                containWord(tokenWord, WORD_OPERATION2) || 
                containWord(tokenWord, WORD_OPERATION3))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 문자 연산자 입력 함수
        /// </summary>
        /// <param name="tokenList">토큰 리스트</param>
        /// <param name="token">토큰</param>
        /// <returns>기호연산자 이면 true를 반환</returns>
        private Boolean setOperation(IList tokenList, char token)
        {
            String tokenBuffer = token.ToString();
            if (containWord(tokenBuffer, OPERATION1) || containWord(tokenBuffer, OPERATION2) || containWord(tokenBuffer, OPERATION3))
            {
                tokenList.Add(tokenBuffer);
                return true;
            }
            return false;

        }
        /// <summary>
        /// 연산자 체크 함수
        /// </summary>
        /// <param name="token">토큰 값</param>
        /// <param name="check">검사할 리스트</param>
        /// <returns></returns>
        private Boolean containWord(String token, String[] check)
        {
            if (String.Empty.Equals(token))
            {
                return false;
            }
            foreach (String word in check)
            {
                if (word.Equals(token))
                {
                    return true;
                }
            }
            return false;
        }
        private IList convertPostOrder(IList tokenStack)
        {
            IList postOrderList = new ArrayList();
            Stack<String> exprStack = new Stack<String>();
            Stack<String> wordStack = new Stack<String>();
            foreach (Object token in tokenStack)
            {
                // 숫자인지 문자인지 판별
                if (typeof(Decimal).Equals(token.GetType()))
                {
                    //숫자면 그대로 입력
                    postOrderList.Add(token);
                }
                else
                {
                    //연산자 처리
                    exprAppend(postOrderList, (String)token, exprStack, wordStack);
                }
            }
            String item = null;
            //남은 연산자가 있을 경우 입력하기
            while (exprStack.Count != 0)
            {
                item = exprStack.Pop();
                postOrderList.Add(item);
            }
            while (wordStack.Count != 0)
            {
                item = wordStack.Pop();
                postOrderList.Add(item);
            }
            return postOrderList;
        }
        private void exprAppend(    IList postOrderList,
                                    String token, 
                                    Stack<String> exprStack,
                                    Stack<String> wordStack)
        {
            //문자 연산자인지 확인한다.
            if (isWordOperation(token))
            {
                //상수 연산자이면 연산자의 값을 넣는다.
                Decimal value = converterWordResult(token);
                if (value > 0)
                {
                    //값입력
                    postOrderList.Add(value);
                }
                else
                {
                    //문자 스택에 넣는다.
                    wordStack.Push(token);
                }
                return;
            }
            //기호 연산자 처리
            if (OPERATION1[0].Equals(token))
            {
                //왼쪽 괄호[(] 처리
                exprStack.Push(token);
            }
            else if (OPERATION1[1].Equals(token))
            {
                //오른쪽 괄호[)] 처리
                String opcode = null;
                while (true)
                {
                    //문자 스택이 없을 때까지
                    if (wordStack.Count > 0)
                    {
                        //기호를 스택에서 가져온다.
                        opcode = exprStack.Pop();
                        //왼쪽 괄호 [(]를 만나면 종료
                        if (OPERATION1[0].Equals(opcode))
                        {
                            opcode = wordStack.Pop();
                            postOrderList.Add(opcode);
                            break;
                        }
                        //스택 순서로 후위 계산 리스트에 값을 넣는다.
                        postOrderList.Add(opcode);
                    }
                    else
                    {
                        //연산 스택이 없으면 종료
                        if (exprStack.Count < 1)
                        {
                            break;
                        }
                        opcode = exprStack.Pop();
                        //왼쪽 괄호 [(]를 만나면 종료
                        if (OPERATION1[0].Equals(opcode))
                        {
                            break;
                        }
                        postOrderList.Add(opcode);
                    }
                }
            }
            else if (OPERATION1[2].Equals(token))
            {
                //콤마 [,] 처리
                //콤마는 문자 연산자와 같이 사용하므로 콤마 연산자가 나왔는데 문자 연산자가 없으면 에러
                if (wordStack.Count < 1)
                {
                    throw new Exception("데이터 형식 에러");
                }
                String opcode = null;
                while (true)
                {
                    //연산 스택이 없으면 종료
                    if (exprStack.Count < 1)
                    {
                        break;
                    }
                    String buffer = exprStack.Pop();
                    exprStack.Push(buffer);
                    //왼쪽 괄호 [(]를 만나면 종료
                    if (OPERATION1[0].Equals(buffer))
                    {
                        break;
                    }
                    opcode = exprStack.Pop();
                    postOrderList.Add(opcode);
                }
            }
            else if (isOperation(token))
            {
                //연산자 처리
                String opcode = null;
                while (true)
                {
                    // 연산자 스택에 연산자가 없으면 그대로 등록
                    if (exprStack.Count == 0)
                    {
                        exprStack.Push(token);
                        break;
                    }
                    // 연산자 POP
                    opcode = exprStack.Pop();
                    //연산자 순위 비교
                    //예로 + * 가 만나면 * 계산을 앞으로 넣기(스택에 늦게 들어가는 것이 FIFO원리로 먼저 계산
                    if (exprOrder(opcode) <= exprOrder(token))
                    {
                        exprStack.Push(opcode);
                        exprStack.Push(token);
                        break;
                        
                    }
                    postOrderList.Add(opcode);
                }
            }
        }
        /// <summary>
        /// 계산 우선순위 레벨 반환
        /// </summary>
        /// <param name="s">연산자</param>
        /// <returns></returns>
        private int exprOrder(String s)
        {
            if (s == null)
            {
                throw new NullReferenceException();
            }
            int order = -1;
            if ("-".Equals(s) || "+".Equals(s))
            {
                order = 0;
            }
            else if ("*".Equals(s) || "/".Equals(s) || "%".Equals(s))
            {
                order = 1;
            }
            else if ("^".Equals(s) || "!".Equals(s))
            {
                order = 2;
            }
            return order;
        }
        /// <summary>
        /// 문자 연산자가 계산연산자가 아닌 상수연산자일 경우 값을 반환
        /// </summary>
        /// <param name="token">문자열 토큰</param>
        /// <returns></returns>
        private Decimal converterWordResult(String token)
        {
            if (containWord(token, OPERATION1))
            {
                // pi 연산자
                if(OPERATION1[0].Equals(token)){
                    return new Decimal(Math.PI);
                }
                // e 연산자
                if (OPERATION1[1].Equals(token))
                {
                    return new Decimal(Math.E);
                }
            }
            return 0;
        }
        /// <summary>
        /// 후위 계산 함수
        /// </summary>
        /// <param name="calcStack">계산 스택</param>
        /// <returns></returns>
        private void calcPostOrder(Stack<Object> calcStack)
        {
            //스택 가장 위의 값을 계산
            Object buffer = calcStack.Pop();
            //수일 경우는 스택넘기기
            if (typeof(Decimal).Equals(buffer.GetType()))
            {
                calcStack.Push(buffer);
                return;
            }
            //문자일 경우 계산한다.
            Decimal op1 = 0;
            Decimal op2 = 0;
            String opcode = null;
            //연산자 포함 스택에 최소 2개 이상
            if (calcStack.Count >= 2)
            {
                //버퍼는 연산자
                opcode = (String)buffer;
                //그 다음값은 수
                op1 = (Decimal)calcStack.Pop();
                if (!opCodeCheck(opcode))
                {
                    //연산자가 수가 두개 필요하기에 하나 더 가져오기
                    op2 = (Decimal)calcStack.Pop();
                }
                Decimal result = calculateByOpCode(op1, op2, opcode);
                calcStack.Push(result);
            }
        }
        /// <summary>
        /// 연산자에 필요한 수의 개수
        /// </summary>
        /// <param name="opcode">연산자</param>
        /// <returns>연산자가 수를 하나 필요하면 true, 두개 필요하면 false</returns>
        private Boolean opCodeCheck(String opcode)
        {
            return containWord(opcode, WORD_OPERATION2) || containWord(opcode, OPERATION2);
        }
        /// <summary>
        /// 계산함수
        /// </summary>
        /// <param name="op1">수1</param>
        /// <param name="op2">수2</param>
        /// <param name="opcode">연산자</param>
        /// <returns>계산 후 값</returns>
        private Decimal calculateByOpCode(Decimal op1, Decimal op2, String opcode)
        {
            if (opcode == null)
            {
                throw new NullReferenceException("OPCode null Error");
            }
            //더하기
            if (OPERATION3[0].Equals(opcode))
            {
                return op1 + op2;
            }
            //빼기
            if (OPERATION3[1].Equals(opcode))
            {
                return op1 - op2;
            }
            //곱하기
            if (OPERATION3[2].Equals(opcode))
            {
                return op1 * op2;
            }
            //나누기
            if (OPERATION3[3].Equals(opcode))
            {
                return op2 / op1;
            }
            //제곱
            if (OPERATION3[4].Equals(opcode))
            {
                return new Decimal(Math.Pow(Decimal.ToDouble(op2), Decimal.ToDouble(op1)));
            }
            //나머지
            if (OPERATION3[5].Equals(opcode))
            {
                return op2 % op1;
            }
            //팩토리얼
            if (OPERATION2[0].Equals(opcode))
            {
                return Factorial(op1);
            }
            //sin
            if (WORD_OPERATION2[0].Equals(opcode))
            {
                return new Decimal(Math.Sin(Decimal.ToDouble(op1)));
            }
            //sinh
            if (WORD_OPERATION2[1].Equals(opcode))
            {
                return new Decimal(Math.Sinh(Decimal.ToDouble(op1)));
            }
            //asin
            if (WORD_OPERATION2[2].Equals(opcode))
            {
                return new Decimal(Math.Asin(Decimal.ToDouble(op1)));
            }
            //cos
            if (WORD_OPERATION2[3].Equals(opcode))
            {
                return new Decimal(Math.Cos(Decimal.ToDouble(op1)));
            }
            //cosh
            if (WORD_OPERATION2[4].Equals(opcode))
            {
                return new Decimal(Math.Cosh(Decimal.ToDouble(op1)));
            }
            //acos
            if (WORD_OPERATION2[5].Equals(opcode))
            {
                return new Decimal(Math.Acos(Decimal.ToDouble(op1)));
            }
            //tan
            if (WORD_OPERATION2[6].Equals(opcode))
            {
                return new Decimal(Math.Tan(Decimal.ToDouble(op1)));
            }
            //tanh
            if (WORD_OPERATION2[7].Equals(opcode))
            {
                return new Decimal(Math.Tanh(Decimal.ToDouble(op1)));
            }
            //atan
            if (WORD_OPERATION2[8].Equals(opcode))
            {
                return new Decimal(Math.Atan(Decimal.ToDouble(op1)));
            }
            //sqrt
            if (WORD_OPERATION2[9].Equals(opcode))
            {
                return new Decimal(Math.Sqrt(Decimal.ToDouble(op1)));
            }
            //exp
            if (WORD_OPERATION2[10].Equals(opcode))
            {
                return new Decimal(Math.Exp(Decimal.ToDouble(op1)));
            }
            //abs
            if (WORD_OPERATION2[11].Equals(opcode))
            {
                return new Decimal(Math.Abs(Decimal.ToDouble(op1)));
            }
            //log
            if (WORD_OPERATION2[12].Equals(opcode))
            {
                return new Decimal(Math.Log(Decimal.ToDouble(op1)));
            }
            //pow
            if (WORD_OPERATION3[0].Equals(opcode))
            {
                return calculateByOpCode(op1, op2, OPERATION3[4]);
            }
            //ceil
            if (WORD_OPERATION2[13].Equals(opcode))
            {
                return new Decimal(Math.Ceiling(Decimal.ToDouble(op1)));
            }
            //floor
            if (WORD_OPERATION2[14].Equals(opcode))
            {
                return new Decimal(Math.Floor(Decimal.ToDouble(op1)));
            }
            //round
            if (WORD_OPERATION3[1].Equals(opcode))
            {
                return new Decimal(Math.Round(Decimal.ToDouble(op2), Decimal.ToInt32(op1)));
            }
            throw new Exception("계산식이 없습니다.");
        }
        //팩토리얼 계산
        private Decimal Factorial(Decimal op)
        {
            if (op == 1)
            {
                return 1;
            }
            return Factorial(op - 1) * op;
        }

    }
}

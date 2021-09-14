# IO_Stream

- 스트림은 `데이터 통로`를 의미한다.
- 즉, 스트림을 생성하고 나서 데이터의 송수신이 이루어진다는 의미다.

- Stream 클래스는 그 자체로 입력 스트림, 출력 스트림의 역할을 모두 할수 있다.
- 파일을 읽고 쓰는 방식 역시 순차 접근 방식과 임의 접근 방식을 모드 지원한다.
- 그러나 Stream 자체는 추상 클래스이기 때문에 파생 클래스의 인스턴스를 이용해야함
  - 파일의 형식에 따라 Stream의 용도가 달라지기 때문에 이런식으로 설계되었음



```c#
// 간단한 파일 읽고 쓰기

Stream stream1 = new FileStream("a.dat", FileMode.Create); // 파일 생성
Stream stream2 = new FileStream("b.dat", FileMode.Open); // 파일 열기
Stream stream3 = new FileStream("c.dat", FileMode.OpenOrCreate); // 파일이 없으면 새로 생성
Stream stream4 = new FileStream("d.dat", FileMode.Truncate); // 파일을 비워서 열기
Stream stream5 = new FileStream("e.dat", FileMode.Append);// 덧붙이기 모드로 열기
```



```c#
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO2_Stream
{
    class BasicIO
    {
        static void Main(string[] args)
        {
            long someValue = 0x123456789ABCDEF0; // 16진수
            Console.WriteLine("{0, -1} : 0x{1:X16}", "Original Data", someValue);
			
            // 파일 생성
            Stream outStream = new FileStream("a.dat", FileMode.Create);
            byte[] wBytes = BitConverter.GetBytes(someValue);
	
            Console.Write("{0} : ", "Byte Array");

            foreach (byte b in wBytes)
                Console.Write("{0:X2} ", b);
            Console.WriteLine();

            outStream.Write(wBytes, 0, wBytes.Length);
            outStream.Close();

            Stream inStream = new FileStream("a.dat", FileMode.Open);
            byte[] rbytes = new byte[8];

            int i = 0;
            while (inStream.Position < inStream.Length)
            {
                rbytes[i++] = (byte)inStream.ReadByte();
            }

            inStream.Close();

            long readValue = BitConverter.ToInt64(rbytes, 0);

            Console.WriteLine("{0} : 0x{1:X16}", "Read Data", readValue);


        }
    }
}

```





```
//result

Original Data : 0x123456789ABCDEF0
Byte Array : F0 DE BC 9A 78 56 34 12  ----> ????
Read Data : 0x123456789ABCDEF0
```



- Byte Array가 역순으로 출력 되는것이 확인되었다.
- 하지만 파일에 저장된 것은 Original Data와 달라지지 않았다.
- CLR이 지원하는 byte order가 데이터의 낮은 주소부터 기록하는 리틀 엔디안(Little Endian) 방식이라 나타난 현상이다.





## Position 프로퍼티 활용

- Position 프로퍼티는 현재 스트림의 읽는 위치 또는 쓰는 위치를 나타냄
  - Position이 3이라면 파일의 3번째 바이트에서 쓰거나 읽을 준비가 된것



- Seek() 메소드를 활용하면 임의 위치로 접근할 수 있고 바로 적으면 데이터는 바로 뒤에 붙는다.



```c#
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO3_Position
{
    class Program
    {
        static void Main(string[] args)
        {
            Stream outStream = new FileStream("a.dat", FileMode.Create);
            Console.WriteLine($"Position : {outStream.Position}"); // position 0 시작

            outStream.WriteByte(0x01);
            Console.WriteLine($"Position : {outStream.Position}");

            outStream.WriteByte(0x02);
            Console.WriteLine($"Position : {outStream.Position}");

            outStream.WriteByte(0x03);
            Console.WriteLine($"Position : {outStream.Position}");

            outStream.Seek(5, SeekOrigin.Current);
            Console.WriteLine($"Position : {outStream.Position}");

            outStream.WriteByte(0x04);
            Console.WriteLine($"Position : {outStream.Position}");

            outStream.Close();
        }
    }
}

```

```
// result

Position : 0
Position : 1
Position : 2
Position : 3
Position : 8
Position : 9

//바이너리 뷰어

01 02 03 00 00 00 00 00 04
```





## BynaryWriter / BinaryReader

- 지금 까지의 방식은 Byte 형식을 계속 지정해줘야 하는 불편함이 있었음
- BynaryWriter / BinaryReader를 이용해 이진데이터를 편리하게 읽어드릴수 있다.



```c#
// 기본 형식
BinaryWriter bw = new BinaryWriter( new FileStream("a.dat", FileMode.Create));
										// 스트림을 매개변수로 넣어줘야함
bw.Write(32);
bw.Write("Good Morning");
bw.Write(3. 14);

bw. close();
```

```c#
BinaryReader br = new BinaryReader(new FileStream("a.dat", FileMode.Open));

int a = br.ReadInt32(0);
string b = br.ReadString();
double c = br.ReadDouble();

br.Close();
```







## StreamWriter / StreamReader

- 텍스트 파일을 읽고 쓰기 위한 도우미 클래스



```c#
// 기본 형식

StreamWriter sw = new StreamWriter( new FileStream("a.dat", FileMode.Create));

sw.Wrtie(32);
sw.Write("Good Morning");
sw.Write(3.14);

sw.Close();
```

```c#
StreamReader sr = new StreamReader( new FileStream("a.dat", FileMode.Open));

while (sr.EndOfStream == false)
{
    Console.WriteLine(sr.ReadLine());
}

sr.Close();
```





### 직렬화

- BinaryWrite/Reader, StreamWriter/Reader로 기본 데이터 형식을 입출력 할 수 있지만 프로그래머가 직접 정의한 클래스나 구조체 같은 복합 데이터 형식은 지원하지 않는다.
- 이런 복합 데이터 형식을 스트림에 읽고 쓰게하기 위해 직렬화(Serialization) 메커니즘을 제공한다.
- 직렬화는 객체의 상태를 메모리나 영구 저장 장치에 저장이 가능한 0과 1의 순서로 바꾸는 것이다.



```c#
//기본 형식

[Serializable]
class MyClass
{
    ....
}

Stream ws = new FilStream("a.dat", FileMode.Create);
BinaryFormaater serializer = new BinaryFormatter();

MyClass obj = new MyClass();
// obj의 필드에 값 저장

serializer.Serialize(ws, obj); // 직렬화
ws.Close();
```

```c#
Stream rs = new FileStream("a.dat", FileMode.Open);
BinaryFormatter deserializer = new BinaryFormater();

MyClass obj = (MyClass)deserializer.Deserialize(rs); // 역직렬화
rs.Close();
```



- [NonSerialized] 어트리뷰션을 사용하면 직렬화 대상에서 제외할 수 있음

```c#
[Serializable]
class MyClass
{
    public int myField1;
    public int myField2;
    
    public NonserializableClass myField3;
    
    public int myField4;
}
```





- 예제

```c#
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace IO4_Stream
{
    [Serializable]
    class NameCard
    {
        public string Name;
        public string Phone;
        public int Age;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stream ws = new FileStream("a.dat", FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();

            NameCard nc = new NameCard();
            nc.Name = "박상현";
            nc.Phone = "010-1234-5678";
            nc.Age = 33;

            serializer.Serialize(ws, nc);
            ws.Close();

            Stream rs = new FileStream("a.dat", FileMode.Open);
            BinaryFormatter deserializer = new BinaryFormatter();

            NameCard nc2 = (NameCard)deserializer.Deserialize(rs);
            rs.Close();

            Console.WriteLine(nc2.Name);
            Console.WriteLine(nc2.Age);
            Console.WriteLine(nc2.Phone);
        }
    }
}

```

```
//result

박상현
33
010-1234-5678
```




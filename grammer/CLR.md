# C# CLR

- JVM과 비슷한 역할을 한다고 보면 된다.



![CLR](https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdn%2F2g60y%2FbtqDL3ZgCmo%2FHfh88r4e6PE65kkFvHJPxk%2Fimg.png)

- **Base Class Library Support:** The Common Language Runtime provides support for the base class library. The BCL contains multiple libraries that provide various features such as *Collections*, *I/O*, *XML*, *DataType definitions*, etc. for the multiple *.NET* programming languages.



- **Thread Support:** The CLR provides thread support for managing the parallel execution of multiple threads. The *System.Threading class* is used as the base class for this.



- **COM Marshaller:** Communication with the COM (Component Object Model) component in the .NET application is provided using the COM marshaller. This provides the COM interoperability support.



- **Type Checker:** Type safety is provided by the type checker by using the Common Type System (CTS) and the Common Language Specification (CLS) that are provided in the CLR to verify the types that are used in an application.



- **Exception Manager:** The exception manager in the CLR handles the exceptions regardless of the *.NET Language* that created them. For a particular application, the catch block of the exceptions are executed in case they occur and if there is no catch block then the application is terminated.



- **Security Engine:** The security engine in the CLR handles the security permissions at various levels such as the code level, folder level, and machine level. This is done using the various tools that are provided in the *.NET* framework.



- **Debug Engine:** An application can be debugged during the run-time using the debug engine. There are various ICorDebug interfaces that are used to track the managed code of the application that is being debugged.



- **JIT Compiler:** The [JIT compiler](https://www.geeksforgeeks.org/what-is-just-in-time-jit-compiler-in-dot-net/) in the CLR converts the Microsoft Intermediate Language (MSIL) into the machine code that is specific to the computer environment that the JIT compiler runs on. The compiled MSIL is stored so that it is available for subsequent calls if required.



- **Code Manager:** The code manager in CLR manages the code developed in the .NET framework i.e. the managed code. The managed code is converted to intermediate language by a language-specific compiler and then the intermediate language is converted into the machine code by the Just-In-Time (JIT) compiler.



- **Garbage Collector:** Automatic memory management is made possible using the garbage collector in CLR. The garbage collector automatically releases the memory space after it is no longer required so that it can be reallocated.



- **CLR Loader:** Various modules, resources, assemblies, etc. are loaded by the CLR loader. Also, this loader loads the modules on demand if they are actually required so that the program initialization time is faster and the resources consumed are lesser.





- 사실 여기서 가장 중요하게 보고 넘어가야할 것은 GC가 아닐까 생각한다.
- JVM에서 마찬가지로 메모리 누수의 주요 원인이기도 하다.
  - GC를 맹신하지 말라는 말을 자주 찾아볼 수 있다.





- GC는 Heap에 쌓여있는 불필요한 메모리를 청소해주는 역할을 한다.

- 할당된 메모리의 위치를 참조하는 객체를 루트(Root)라고 한다. (object c = new object() 인경우 c가 루트)

  - JIT 컴파일러가 이 루트들을 목록으로 만든다.
  - CLR이 루트 목록을 관리한다.

  - GC는 루트 목록을 순회하며 루트가 참조하고 있는 힙 객체와의 관계 여부를 조사한다.
  - 어떤루트와도 관계가 없다면 쓰레기라 판단하고 삭제한다.
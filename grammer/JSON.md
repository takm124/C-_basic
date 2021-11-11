# Json



## 환경설정

프로젝트 -> NuGet 패키지 관리 -> Newtonsoft.Json 설치



## object to JSON

### Account 클래스

```c#
public class Account
    {
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<string> Roles { get; set; }
    }
```



### JSON 형태로 변환

```c#
Account account = new Account
{
    Email = "james@example.com",
    Active = true,
    CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
    Roles = new List<string>
    {
        "User",
        "Admin"
    }
};

string json = JsonConvert.SerializeObject(account, Formatting.Indented);

// DeSerialize
Account account = JsonConvert.DeserializeObject<Account>(json);

// {
//   "Email": "james@example.com",
//   "Active": true,
//   "CreatedDate": "2013-01-20T00:00:00Z",
//   "Roles": [
//     "User",
//     "Admin"
//   ]
// }
```



## Dictionary to JSON

```c#
Dictionary<string, int> points = new Dictionary<string, int>
{
    { "James", 9001 },
    { "Jo", 3474 },
    { "Jess", 11926 }
};

string json = JsonConvert.SerializeObject(points, Formatting.Indented);

// {
//   "James": 9001,
//   "Jo": 3474,
//   "Jess": 11926
// }
```



## 서버로 JSON 보내고 JSON 받아오기

- 서버는 스프링부트, 자바 구현



### Student 클래스

```c#
public class Student
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
```

- JsonProperty는 key값을 설정해주는 부분
- 만약 멤버변수가 대문자로 시작한다면 소문자로 바꿔줘야 서버에서 받을 때 null로 받는 경우가 없어짐





### 구현 부분

```c#
//요청 url
string url = "http://localhost:8080/JsonTest";

// HTTP Request
HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

request.Method = "POST";
request.ContentType = "application/json; charset=utf-8";

Student stu = new Student
{
    Id = 1234,
    Name = "홍길동"
};

string json = JsonConvert.SerializeObject(stu, Formatting.Indented);

// 데이터를 바이너리 형태로 변경
byte[] byteData = Encoding.UTF8.GetBytes(json);

// Stream 생성 및 request 전송
Stream dataStream = request.GetRequestStream();
dataStream.Write(byteData, 0, byteData.Length);
dataStream.Close();


// Response
WebResponse response = request.GetResponse();

// response 상태코드
string status = ((HttpWebResponse)response).StatusDescription.ToString();
Console.WriteLine("응답 상태 코드 : " + status);

// response data 수신
using (dataStream = response.GetResponseStream())
{
    StreamReader sr = new StreamReader(dataStream);
    string Ret = sr.ReadToEnd();
    Student result = JsonConvert.DeserializeObject<Student>(Ret);
    lblResult.Text = result.Name;
}

response.Close();
```



### Java 서버 부분

```java
@PostMapping("/JsonTest")
public StudentVO JsonTest(@RequestBody StudentVO stu) {

    System.out.println("result : " + stu.toString());
    // result : StudentVO(id=1234, name=홍길동)

    return stu;
}
```



### HTTP 메시지 정보

```http
POST /JsonTest HTTP/1.1
Content-Type: application/json; charset=utf-8
Host: localhost:8080
Content-Length: 42
Expect: 100-continue
Connection: Keep-Alive

{
  "id": 1234,
  "name": "홍길동"
}
```


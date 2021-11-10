# C# HTTP GET / POST 요청 처리

```
c# winForm에서 로그인 기능을 만들어보고 싶었다.

winForm은 UI 역할을 해주고 데이터 기능을 받아
서버로 아이디/패스워드를 보내 처리를 받아야한다.

그럼 일단 c#에서 서버로 Get/Post요청을 하는 법을 알아야 겠다 싶어서 정리한다.
```



- 서버는 스프링부트 환경(java 8, gradle)에서 구동시킴

- DB는 따로 사용하지 않았고 Repository 클래스를 하나 만들어서 Map에 데이터 저장함



## Get 요청

### **C#**

```c#
private void btnServerConnect_Click(object sender, EventArgs e)
        {
            // 요청을 보낼 URL
            string url = "http://localhost:8080/login";
            string responseText = string.Empty;

            // HTTP 요청 생성
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
                {
                    // 상태코드
                    HttpStatusCode status = resp.StatusCode;
                    lblServerStatus.Text = status.ToString(); // OK

                    // 데이터
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                        lblData.Text = responseText;
                    }
                }
            }
            catch (WebException err)
            {
                MessageBox.Show(err.ToString());
            }
        }
```



### **JAVA**

```java
@RestController
public class LoginController {
    StaffRepository store = new StaffRepository();

    @GetMapping("/login")
    public String LoginTest(){
        return "connect completed";
    }
}
```



- 결과 : 서버에서 return 해준 connect completed가 c# 쪽으로 전송됨





## Post 요청

### C#

```c#
private void btnLogin_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:8080/login";
            Staff staff1 = new Staff("1111", "2222");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            var settings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            string jsonUserinfo = JsonConvert.SerializeObject(staff1, settings);
            try
            {
                string result = string.Empty;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonUserinfo);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)request.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                        lblLoginResult.Text = result;
                    }
                }
            }
            catch (WebException err)
            {
                MessageBox.Show(err.ToString());
            }
        }
```



### **JAVA**

```java
@RestController
public class LoginController {

    StaffRepository store = new StaffRepository();

    @GetMapping("/login")
    public String LoginTest(){
        return "connect completed";
    }

    @PostMapping("/login")
    public String Login(@RequestBody StaffVO staffVO){
        System.out.println(staffVO.toString());
        return "login success";
    }

}
```


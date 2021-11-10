# c# multipart 전송

- HTTP Request 요청시 body에 데이터를 담아 전송 할 경우 header에는 body에 담긴 데이터가 무엇인지 표시해주어야 한다. (Content-Type, MIME)
- 즉, 이 말은 한 번 전송시 하나의 데이터 타입만을 담아서 보낼 수 있다는 의미이다.
- 그러나 우리가 이미지와 텍스트를 한번에 보내고 싶다면 어떻게 해야 할까?



- multipart는 이러한 의문증을 풀어줄 수 있는 타입이다.





- 먼저 일반적인 방식의 POST 방식의 request이다.

![POST](https://media.vlpt.us/post-images/rosewwross/6fc65770-4b39-11ea-abce-67c155f8f58a/image.png)



- Header 부분에 POST 방식이라는 명시와 함께 기타 헤더들이 포함되어 있다.
  - 포인트는 Content-Type으로 body에 포함된 데이터가 html임을 명시해주고 있다.





- multipart/form-data 일때의 형식이다.

![multipart](https://community.smartbear.com/t5/image/serverpage/image-id/7139i88A183EE3571D0F7/image-size/large?v=1.0&px=999)

- POST일 때 와 비슷한 구조로 되어있으며 이번에 Content-Type은 multipart/form-data로 되어있다.
  - 눈여겨 볼 점은 Content-Type 옆에 있는 boundary인데 저 boundary를 기점으로 데이터가 구분됨을 나타낸다.
  - 실제로 빨간박스에 있는 데이터들은 boundary로 구분되어 있다.





## multipart/form-data 데이터 전송 방식

- 그렇다면 이제 C#에서 멀티파트 전송을 하는 법을 알아보자

   https://spirit32.tistory.com/21님의 블로그를 참고해서 구현해보았습니다



### boundary 설정

```c#
string boundary = string.Format("-------------------{0:N}", Guid.NewGuid());
byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
```

- boundary는 처음 request header 부분에 넣을 데이터이고

  boundaryBytes는 body에 담는 데이터들을 구분하기 위해 만들어준 byte array다.

  (stream에 바로 꽂아줄거라서 바이너리 형태로 변환한 것이다.)

- Guid는 전역 고유 식별자로 고유 구분자 역할을 해줄 것이다.



#### request header 설정

```c#
// 요청 URL
string url = "http://localhost:8080/multipart";

// Request 생성
HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

// HTTP Method 설정
request.Method = "POST"; // 필수

// header
request.ContentType = "multipart/form-data; boundary=" + boundary; // 필수
request.KeepAlive = true;
request.Credentials = CredentialCache.DefaultCredentials;
```

- Method와 ContentType까지만 제대로 명시해주면 되고 기타 다른 설정은 필요하면 건들면 된다.

  KeepAlive는 커넥션 유지 관련 헤더이고

  Credentials은 CORS 관련하여 origin과 관련된 속성인데 필요하다면 추가 조사를 해보시면 될 것같다.



### 전송할 데이터

- C#에서 주로 데이터를 전송할 때 Dictionary 형태로 데이터를 구분하여 전송하기 때문에 비슷한 방식으로 구현해 보았다.

```c#
public class FileForm
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string FilePath { get; set; }
        public Stream Stream { get; set; }
    }
```

```c#
// 전송 할 데이터
Dictionary<string, Object> dic = new Dictionary<string, object>();
FileForm fileData = new FileForm();
fileData.Name = "00000000.tif";
fileData.ContentType = "application/tif";
fileData.FilePath = lblMultipart.Text; // 이미지 파일 경로가 담겨져있다.
string key = "uploadFile";
dic.Add(key, fileData);
```



- 전송할 데이터의 정보를 가지고 있는 FileForm을 만들어주고 (전송 할 데이터가 file 형식일 때만 사용, 아닌 경우 JSON으로 전송하면 된다.)

  값을 하나하나 개별 설정하여 dictionary에 넣어주었다.



### 전송 부분

```c#
Stream requestStream = request.GetRequestStream();

// 전송 과정
foreach (KeyValuePair<string, object> pair in dic)
{
	requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
	if (pair.Value is FileForm)
	{
		FileForm file = pair.Value as FileForm;
		string header = "Content-Disposition: form-data; name=\"" + "paramName" + "\"; filename=\"" + file.Name + "\"\r\nContent-Type: " + file.ContentType + "\r\n\r\n";
		byte[] bytes = Encoding.UTF8.GetBytes(header);
		requestStream.Write(bytes, 0, bytes.Length);
        
		byte[] buffer = new byte[32768];
		int bytesRead;  
		// upload from file
		using (FileStream fileStream = File.OpenRead(file.FilePath))
        {
			while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
				requestStream.Write(buffer, 0, bytesRead);
				fileStream.Close();
            }
        }
    }

	byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
	requestStream.Write(trailer, 0, trailer.Length);
	requestStream.Close();
}
```

- 순서는 다음과 같다.
  - 1. Stream 생성
    2. dictionary에 있는 데이터를 하나씩 뽑아서 boundary로 감싸주어야 하기 때문에 foreach 사용
    3. 처음에 만들어 뒀던 boundarybyte로 첫 줄을 그어주고
    4. 담겨진 데이터의 정보를 담아준다 (Content-Disposition)
    5. Content-Disposition의 name으로 설정 해 준 것이 파라미터 이름이다.
    6. 필자의 경우 이미지 파일 전송을 시도했고 FileStream으로 buffer에 이미지 데이터를 담아 스트림에 추가 시켜줬다.
    7. 마찬가지로 끝나는 라인을 바운더리로 표시해주면서 마무리 했다.





### Reponse

```c#
using (WebResponse response = request.GetResponse())
{
	using (Stream responseStream = response.GetResponseStream())
	using (StreamReader reader = new StreamReader(responseStream))
	result = reader.ReadToEnd();
}
```

- 응답 값은 서버쪽에서 일단 String만 보낸다고 가정했고

  데이터를 받아오거나 한다면 과정이 좀 더 복잡해질 것이다.





### 서버쪽 처리

- 서버는 spring boot (java 8, gradle) 환경에서 구현했다.

- 이제 서버에서 데이터를 받아 임의의 장소에 사진파일을 저장해볼 것이다.

  데이터 저장 과정의 경우 https://takeknowledge.tistory.com/61님의 블로그를 참고했다.

- 사실 밑의 방식으로 저장하는 것은 매우 귀찮고 번거로운 작업이다.
  MultipartFile을 사용할 경우 file.transferTo(new File(저장 경로)) 메소드를 사용하면 쉽게 한번에 저장 가능하다.
  

```java
@PostMapping("/multipart")
    public String multipartTest(@RequestPart("paramName") MultipartFile file) {
        try (
            // 윈도우일 경우
            FileOutputStream fos = new FileOutputStream("C:\\java_project\\multipart\\ecm\\" + file.getOriginalFilename());
            // 파일 저장할 경로 + 파일명을 파라미터로 넣고 fileOutputStream 객체 생성하고
            InputStream is = file.getInputStream();) {
            // file로 부터 inputStream을 가져온다.
            int readCount = 0;
            byte[] buffer = new byte[1024];
            while ((readCount = is.read(buffer)) != -1) {
                fos.write(buffer, 0, readCount);
            }
        } catch (Exception ex) {
            throw new RuntimeException("file Save Error");
        }

        return "multipart 요청이 성공적으로 이루어졌습니다.";
    }
```



- 여기서 가져가야할 포인트는 스프링 부트 환경에서 multipart 데이터를 어떻게 받아오냐다
- 나같은 경우 보내준 사진이 하나뿐이고 파라미터의 이름을 알고 있기 때문에 @RequestPart를 사용해서 직접적으로 파일을 받아왔다.
- 그렇지 않은 경우 MultipartHttpServletRequest에서 request를 받아온 뒤 반복문으로 하나씩 받아와줘야한다.



```java
Iterator<String> fileNames = request.getFileNames();
while (fileNames.hasNext()) {
	System.out.println(fileNames.next());
}

MultipartFile file = request.getFile("paramName");
```

- 이런식으로 이름을 하나하나 받아와서 추출해주어도 된다.

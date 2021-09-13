[TOC]



# LINQ

- '컬렉션'을 편리하게 다루기 위한 목적으로 만들어진 질의(query) 언어



## 기본 문법

- From : 어떤 데이터 집합에서 찾나 (from <범위변수> in <데이터원본>)
- Where : 어떤 값의 데이터를 찾을 것인가
- Select : 어떤 항목을 추출할 것인가



```c#
namespace LINQ
{

    class MainApp
    {
        static void Main(string[] args)
        {
            int[] numbers = { 9, 2, 6, 4, 5, 3, 7, 8, 1, 10 };

            var result = from n in numbers 
                         where n % 2 == 0
                         orderby n descending // default는 ascending
                         select n;

            foreach (int num in result)
            {
                Console.WriteLine($" 짝수 : {num}");
            }
        }
    }
```

```
Result

짝수 : 2
짝수 : 4
짝수 : 6
짝수 : 8
짝수 : 10
```



- result의 타입을 getType으로 확인해보면 System.Linq.OrderedEnumerable`2[System.Int32,System.Int32] 라고 나온다.

  이런 경우에는 타입을 다음과 같이 지정해주면 var을 사용하지 않아도 된다.

  ```c#
  IOrderedEnumerable<int> result
  ```

  



## class 사용

```c#
class Profile
    {
        public string Name { get; set; }
        public int Height { get; set; }
    }

    class MainApp
    {
        static void Main(string[] args)
        {
            Profile[] arrProfile =
            {
                new Profile() { Name="정우성", Height=186},
                new Profile() { Name="김태희", Height=158},
                new Profile() { Name="고현정", Height=172},
                new Profile() { Name="이문세", Height=178},
                new Profile() { Name="하하", Height=171}
            };

            var profiles = from profile in arrProfile
                           where profile.Height > 175
                           orderby profile.Height
                           select new
                           {
                               Name = profile.Name,
                               InchHeight = profile.Height * 0.393
                           };


            foreach (var profile in profiles)
            {
               Console.WriteLine($"{profile.Name}, {profile.InchHeight}");
            }

        }
    }
}
```





### class 멤버변수가 배열인 경우

```c#
namespace LINQ2
{
    class Class
    {
        public string Name { get; set; }
        public int[] Score { get; set; }
    }

    class MainApp
    {
        static void Main(string[] args)
        {
            Class[] arrClass =
            {
                new Class() { Name = "연두반", Score = new int[] { 99, 80, 70, 24} },
                new Class() { Name = "분홍반", Score = new int[] { 60, 45, 87, 72} },
                new Class() { Name = "파랑반", Score = new int[] { 92, 30, 85, 94} },
                new Class() { Name = "노랑반", Score = new int[] { 90, 88, 0, 17} }
            };

            var classes = from c in arrClass // 첫번째 데이터 원본
                          from s in c.Score // 두번째 데이터 원본
                          where s < 60
                          orderby s
                          select new { c.Name, Lowest = s };

            foreach (var c in classes)
            {
                Console.WriteLine($"낙제 : {c.Name} ({c.Lowest})");
            }
        }
    }
}
```

```
result

낙제 : 노랑반 (0)
낙제 : 노랑반 (17)
낙제 : 연두반 (24)
낙제 : 파랑반 (30)
낙제 : 분홍반 (45)
```



## group by

- **group** A **by** B **into** c
  - A에는 from 절에서 뽑아낸 범위 변수를
  - B는 분류 기준
  - c는 그룹변수 명 지정

```c#
namespace LINQ
{

    class Profile
    {
        public string Name { get; set; }
        public int Height { get; set; }
    }

    class MainApp
    {
        static void Main(string[] args)
        {
            Profile[] arrProfile =
            {
                new Profile() { Name="정우성", Height=186},
                new Profile() { Name="김태희", Height=158},
                new Profile() { Name="고현정", Height=172},
                new Profile() { Name="이문세", Height=178},
                new Profile() { Name="하하", Height=171}
            };

            var listProfile = from profile in arrProfile
                              orderby profile.Height
                              group profile by profile.Height < 175 into g
                              select new { GroupKey = g.Key, Profiles = g };

            foreach (var Group in listProfile)
            {
                Console.WriteLine($"- 175 미만 / : {Group.GroupKey}");

                foreach (var profile in Group.Profiles)
                {
                    Console.WriteLine($"    {profile.Name} : {profile.Height}");
                }
            }
        }
    }
}
```

```
result

- 175 미만 / : True
    김태희 : 158
    하하 : 171
    고현정 : 172
- 175 미만 / : False
    이문세 : 178
    정우성 : 186
```





## Join

- 두 데이터 원본을 연결함



### 내부조인 (inner join)

```c#
namespace LINQ3_JOIN_
{
    class Profile
    {
        public string Name { get; set; }
        public int Height { get; set; }
    }

    class Product
    {
        public string Title { get; set; }
        public string Star { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Profile[] arrProfile =
            {
                new Profile() { Name="정우성", Height=186},
                new Profile() { Name="김태희", Height=158},
                new Profile() { Name="고현정", Height=172},
                new Profile() { Name="이문세", Height=178},
                new Profile() { Name="하하", Height=171}
            };

            Product[] arrProduct =
            {
                new Product() { Title = "비트", Star = "정우성"},
                new Product() { Title = "CF 다수", Star = "김태희"},
                new Product() { Title = "아이리스", Star = "김태희"},
                new Product() { Title = "모래시계", Star = "고현정"},
                new Product() { Title = "Solo 예찬", Star = "이문세"}
            };

            var listProfile =
                from profile in arrProfile
                join product in arrProduct on profile.Name equals product.Star
                select new
                {
                    Name = profile.Name,
                    Work = product.Title,
                    Height = profile.Height
                };

            Console.WriteLine("---내부 조인 결과---");
            foreach (var profile in listProfile)
            {
                Console.WriteLine($"이름 : {profile.Name}, 작품 : {profile.Work}, 키 : {profile.Height}");
            }
            
        }
    }
}
```

```
---내부 조인 결과---
이름 : 정우성, 작품 : 비트, 키 : 186
이름 : 김태희, 작품 : CF 다수, 키 : 158
이름 : 김태희, 작품 : 아이리스, 키 : 158
이름 : 고현정, 작품 : 모래시계, 키 : 172
이름 : 이문세, 작품 : Solo 예찬, 키 : 178
```





### 외부 조인 (outer join)

```c#
namespace LINQ3_JOIN_
{
    class Profile
    {
        public string Name { get; set; }
        public int Height { get; set; }
    }

    class Product
    {
        public string Title { get; set; }
        public string Star { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Profile[] arrProfile =
            {
                new Profile() { Name="정우성", Height=186},
                new Profile() { Name="김태희", Height=158},
                new Profile() { Name="고현정", Height=172},
                new Profile() { Name="이문세", Height=178},
                new Profile() { Name="하하", Height=171}
            };

            Product[] arrProduct =
            {
                new Product() { Title = "비트", Star = "정우성"},
                new Product() { Title = "CF 다수", Star = "김태희"},
                new Product() { Title = "아이리스", Star = "김태희"},
                new Product() { Title = "모래시계", Star = "고현정"},
                new Product() { Title = "Solo 예찬", Star = "이문세"}
            };

            var listProfile =
                from profile in arrProfile
                join product in arrProduct on profile.Name equals product.Star into ps
                from product in ps.DefaultIfEmpty(new Product() { Title = "없음" })
                select new
                {
                    Name = profile.Name,
                    Work = product.Title,
                    Height = profile.Height
                };

            Console.WriteLine();
            Console.WriteLine("--- 외부 조인 결과 ---");
            foreach (var profile in listProfile)
            {
                Console.WriteLine($"이름 : {profile.Name}, 작품 : {profile.Work}, 키 : {profile.Height}");
            }
        }
    }
}
```

```
--- 외부 조인 결과 ---
이름 : 정우성, 작품 : 비트, 키 : 186
이름 : 김태희, 작품 : CF 다수, 키 : 158
이름 : 김태희, 작품 : 아이리스, 키 : 158
이름 : 고현정, 작품 : 모래시계, 키 : 172
이름 : 이문세, 작품 : Solo 예찬, 키 : 178
이름 : 하하, 작품 : 없음, 키 : 171
```





## 연습문제

```c#
namespace LINQ_practice
{
    class Car
    {
        public int Cost { get; set; }
        public int MaxSpeed { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Car[] cars =
            {
                new Car() { Cost = 56, MaxSpeed = 120},
                new Car() { Cost = 70, MaxSpeed = 150},
                new Car() { Cost = 45, MaxSpeed = 180},
                new Car() { Cost = 32, MaxSpeed = 200},
                new Car() { Cost = 82, MaxSpeed = 280}
            };

            // Cost 50이상 MaxSpeed 150 이상

            var selected = from car in cars
                           where car.Cost >= 50
                           where car.MaxSpeed >= 150
                           orderby car.Cost
                           select car;

            foreach (var car in selected)
            {
                Console.WriteLine($"가격 : {car.Cost}, 최대속도 : {car.MaxSpeed}");
            }
                

        }
    }
}

```


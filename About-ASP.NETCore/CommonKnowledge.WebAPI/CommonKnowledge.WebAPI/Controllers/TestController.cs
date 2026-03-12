using Microsoft.AspNetCore.Mvc;

namespace CommonKnowledge.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public Person GetPerson()
        {
            return new Person("Alice", 1, 18);
        }

        [HttpPost]
        public string[] SaveNote(SaveNoteRequest request)
        {
            System.IO.File.WriteAllText(request.Title + ".txt", request.Content);
            return new string[] { "ok", request.Title };
        }

        [HttpGet]
        public async Task<string> GetRectraction()
        {
            string s = await System.IO.File.ReadAllTextAsync("C:/Users/tsuki/Downloads/log.txt");
            return s.Substring(0, s.Length - 1);
        }

        // 对比下面两个方法
        // 通用原则：在 ASP.NET Core Web API 开发中，优先使用 ActionResult<T> 来表示有固定成功数据类型的动作
        // 它结合了灵活性（可返回 ActionResult 错误）和类型安全性。
        // 仅当成功返回类型多变时，才退回到 IActionResult。
        [HttpGet]
        public IActionResult GetAction1(int id)
        {
            if (id == 1)
                return Ok(88);
            else if (id == 2)
                return Ok(99);
            else
                return NotFound("id错误");
        }

        [HttpGet]
        public ActionResult<int> GetAction2(int id)
        {
            if (id == 1)
                return 88;
            else if (id == 2)
                return 99;
            else
                return NotFound("id错误");
        }

        // 比较RESTful的写法
        // {i1} {i2}称为占位符
        [HttpGet("{i1}/{i2}")]
        public int Multiply1(int i1, int i2)
        {
            return i1 * i2;
        }

        // 比较RESTful的写法
        // 当参数名字不一致时，需要使用 [FromRoute] 特性来显式指定占位符名称
        [HttpGet("{i1}/{i2}")] // 路由模板中的占位符是 i1 和 i2
        public int Multiply2([FromRoute(Name = "i1")] int a, [FromRoute(Name = "i2")] int b)
        {
            return a * b;
        }

        [HttpGet("multiply")] // 没有路由参数，从查询字符串读取
        public int Multiply3([FromQuery(Name = "x")] int a, [FromQuery(Name = "y")] int b)
        {
            return a * b;
            // eg：GET /api/calc/multiply?x=5&y=3
        }

        [HttpPost]
        public string AddPerson1(Person p1)
        {
            return "保存成功，" + p1.Name;
        }

        [HttpPut("{id}")]
        public string UpdatePerson1([FromRoute] int id, Person p2)
        {
            return "更新id: " + id + " 成功" + p2.Name;
        }
    }
}

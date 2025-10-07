using Microsoft.AspNetCore.Mvc;

namespace veeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PrimitiividController : ControllerBase
    {
        private readonly Random rand = new Random();

        // GET: primitiivid/hello-world
        [HttpGet("hello-world")]
        public string HelloWorld()
        {
            return "Hello world at " + DateTime.Now;
        }

        // GET: primitiivid/hello-variable/mari
        [HttpGet("hello-variable/{nimi}")]
        public string HelloVariable(string nimi)
        {
            return "Hello " + nimi;
        }

        // GET: primitiivid/add/5/6
        [HttpGet("add/{nr1}/{nr2}")]
        public int AddNumbers(int nr1, int nr2)
        {
            return nr1 + nr2;
        }

        // GET: primitiivid/multiply/5/6
        [HttpGet("multiply/{nr1}/{nr2}")]
        public int Multiply(int nr1, int nr2)
        {
            return nr1 * nr2;
        }

        // GET: primitiivid/do-logs/5
        [HttpGet("do-logs/{arv}")]
        public void DoLogs(int arv)
        {
            for (int i = 0; i < arv; i++)
            {
                Console.WriteLine("See on logi nr " + i);
            }
        }
        // GET: primitiivid/random/1/10
        [HttpGet("random/{min}/{max}")]
        public int GetRandomNumber(int min, int max)
        {
            if (min > max) (min, max) = (max, min); // vahetab ümber, kui kasutaja annab valepidi
            return rand.Next(min, max + 1); // +1, et max oleks kaasatud
        }

        // GET: primitiivid/age/2005/05/10
        [HttpGet("age/{year}/{month}/{day}")]
        public string GetAge(int year, int month, int day)
        {
            var birthDate = new DateTime(year, month, day);
            var today = DateTime.Today;

            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--; // kui sünnipäev pole veel olnud, lahutame ühe

            return $"Oled {age} aastat vana.";
        }
    }
}
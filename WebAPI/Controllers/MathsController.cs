using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("maths")]
    public class MathsController : ControllerBase
    {
        private readonly ILogger<MathsController> _logger;

        public MathsController(ILogger<MathsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("fibonacci")]
        public IEnumerable<int> GetFibonacciSequence(int max)
        {
            List<int> results = new List<int>() { 0, 1 };

            int lastNumber = results.LastOrDefault();

            while(max > lastNumber)
            {
                int firstIndex = results.Count - 2;
                int firstNumber = results[firstIndex];
                int result = firstNumber + lastNumber;
                if (result < max)
                    results.Add(result);

                lastNumber = result;
            }

            return results;
        }

        [HttpGet]
        [Route("even-numbers")]
        public IEnumerable<int> EvenNumbers(int max)
        {
            List<int> numbers = new List<int>();
            for (int i = 1; i <= max; i++)
            {
                numbers.Add(i);
            }
            return numbers.Where(x => x % 2 == 0);
        }

        [HttpGet]
        [Route("odd-numbers")]
        public IEnumerable<int> OddNumbers(int max)
        {
            List<int> numbers = new List<int>();
            for (int i = 1; i <= max; i++)
            {
                numbers.Add(i);
            }
            return numbers.Where(x => x % 2 != 0);
        }
    }
}
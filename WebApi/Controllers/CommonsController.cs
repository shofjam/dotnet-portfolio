using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("commons")]
    public class CommonsController : ControllerBase
    {
        private readonly ILogger<CommonsController> _logger;

        public CommonsController(ILogger<CommonsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("bubble-sort")]
        public IEnumerable<int> BubbleSort(List<int> arr)
        {
            bool swapped = true;
            while(swapped)
            {
                swapped = false;
                for (int j = 1; j < arr.Count; j++)
                {
                    if (arr[j - 1] > arr[j])
                    {
                        int temp = arr[j - 1];
                        arr[j - 1] = arr[j];
                        arr[j] = temp;
                        swapped = true;

                        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(arr));
                    }
                }

                if (!swapped)
                    break;
            }

            return arr;
        }
    }
}
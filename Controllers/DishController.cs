using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/{restaurantId}/dish")]
    [ApiController]
    //walidajcja modeli dla kazdej z akcji 
    public class DishController : ControllerBase
    {
        private readonly IDishService _disService;

        public DishController(IDishService dishService)
        {
                _disService = dishService;

        }
        [HttpPost]
        public ActionResult Post([FromRoute] int restaurantId, CreateDishDto dto)
        {
            var newDishId = _disService.Create(restaurantId, dto);
            return Created($"api/{restaurantId}/dish/{newDishId}", null);
        }
    }
}

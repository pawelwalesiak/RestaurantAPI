﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        
        private readonly IRestaurantService _restaurantService; 

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService=restaurantService;
        }
        [HttpGet]
        public ActionResult <IEnumerable<RestaurantDto>> GetAll()
        {
           var restaurantsDtos = _restaurantService.GetAll();

            return Ok(restaurantsDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);

            if (restaurant is null)
            {
                return  NotFound();
            }
            return Ok(restaurant);
        }

        // mapowwanie dto ktore przyjdzie od klineta do encji restauracji z jej adresem a nastepenie dodac ja do db z entity framework
        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var id = _restaurantService.Create(dto);
            _restaurantService.Create(dto);
            return Created($"/api/restaurant/{id}", null);
        }


    }
}

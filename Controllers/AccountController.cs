﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using static RestaurantAPI.Services.AccountService;

namespace RestaurantAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    //walidacja modelu kontrolera - automat 
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto) 
        {
            //dodajemy model wysylany przez usera walidacje zalatwia appi controller 
            _accountService.RegisterUser(dto);
            return Ok();    
        }
        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }

    }
}

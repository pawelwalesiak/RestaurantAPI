using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {

        public int MinimumAge { get;  } 
        // set; usuniete bo wartosc jest dodawana przez konstruktor 


        public MinimumAgeRequirement( int minimumAge) 
        {
        MinimumAge = minimumAge;
        }
        
    
    
    }
}

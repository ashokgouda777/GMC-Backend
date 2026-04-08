
using GMC.Data;
using GMC.Model;
using GMC.Model.GMC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace GMC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AddressController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveAddress([FromBody] AddressDto m)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingAddress = await _context.Address
             .FirstOrDefaultAsync(x =>
                 x.ClientID == m.ClientID &&
                 x.AddressType == m.AddressType);

            if (existingAddress == null)
            {
                string addressid = await GeneratePractitionerUserIdAsync(m.AddressType);

                var add = new Address
                {
                    AddressID = addressid,                  // Generated ID
                    ClientID = m.ClientID,
                    AddressType = m.AddressType,

                    CountryId = "1",
                    StateId = "1",
                    CouncilId = "1",

                    Address1 = m.Address1,
                    Address2 = m.Address2,
                    City = m.City,
                    State = m.State,
                    Zip = m.Zip,
                    Country = m.Country,

                    Phone1 = m.phone1,
                    Phone2 = m.phone2,
                    PlaceType = m.PlaceType,
                    District = m.District,

                    CreatedBy = m.CreatedBy,
                    CreatedOn = DateTime.Now
                };


                _context.Address.Add(add);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Address Inserted Successfully", data = m });
            }
            else
            {
                // ✅ UPDATE
                existingAddress.CountryId = "1";
                existingAddress.StateId = "1";
                existingAddress.CouncilId = "1";
                existingAddress.AddressType = m.AddressType;
                existingAddress.Address1 = m.Address1;
                existingAddress.Address2 = m.Address2;
                existingAddress.City = m.City;
                existingAddress.State = m.State;
                existingAddress.Zip = m.Zip;
                existingAddress.Country = m.Country;
                existingAddress.Phone1 = m.phone1;
                existingAddress.Phone2 = m.phone2;
                existingAddress.PlaceType = m.PlaceType;
                existingAddress.District = m.District;
                existingAddress.UpdatedOn = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Address Updated Successfully", data = existingAddress });
            }
        }



        private async Task<string> GeneratePractitionerUserIdAsync(string type)
        {
            string id;
            var rnd = new Random();

            do
            {
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                int random = rnd.Next(100, 999);
                id = $"A{type}{timestamp}{random}";

            } while (await _context.Users.AnyAsync(x => x.userId == id));

            return id;
        }
        [AllowAnonymous]
        [HttpGet("getaddress")]
        public async Task<IActionResult> GetAddress(string clientId, string addressType)
        {
            if (string.IsNullOrEmpty(clientId))
                return BadRequest("ClientID is required");

            if (string.IsNullOrEmpty(addressType) ||
               (addressType != "R" && addressType != "P"))
                return BadRequest("AddressType must be 'R' or 'P'");

            var existingAddress = await (
       from a in _context.Address

           // State LEFT JOIN
       join s in _context.StateMaster
           on a.State equals s.StateId into sGroup
       from s in sGroup.DefaultIfEmpty()

           // Country LEFT JOIN
       join c in _context.CountryMaster
           on a.Country equals c.CountryId into cGroup
       from c in cGroup.DefaultIfEmpty()

           // District LEFT JOIN
       join d in _context.DistrictMaster
           on a.District equals d.DistrictId into dGroup
       from d in dGroup.DefaultIfEmpty()

       where a.ClientID == clientId
             && a.AddressType == addressType
       select new
     {
         a.ClientID,
         a.AddressType,
         a.Address1,
         a.Address2,
         a.City,
         s.StateName,   
         d.DistrictName,
         a.Zip,
         c.CountryName,
         a.Phone1,
         a.Phone2,
         a.PlaceType
     }
 ).FirstOrDefaultAsync();


            if (existingAddress == null)
                return NotFound("Address not found");

            return Ok(existingAddress);
        }


    }
}

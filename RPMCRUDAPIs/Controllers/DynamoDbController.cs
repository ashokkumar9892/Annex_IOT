using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPMCRUDAPIs.Models;

namespace RPMCRUDAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamoDbController : ControllerBase
    {
            private readonly ICreateTable _createTable;

            public DynamoDbController(ICreateTable createTable)
            {
                _createTable = createTable;
            }

            [Route("createtable")]
            public IActionResult CreateDynamoDbTable()
            {
                _createTable.CreateDynamoDbTable();

                return Ok();
            }
        

    }
}
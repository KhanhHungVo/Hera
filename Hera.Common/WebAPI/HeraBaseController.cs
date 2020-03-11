using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Common.WebAPI
{
    public abstract class HeraBaseController : ControllerBase
    {
        public HeraBaseController()
        {

        }

        protected object EncryptRequest(object data)
        {
            return data;
        }

        [NonAction]
        public IActionResult HeraOk(object data)
        {
            return Ok(data);
        }

        [NonAction]
        public IActionResult HeraBadReques(object error)
        {
            return BadRequest(error);
        }
    }
}

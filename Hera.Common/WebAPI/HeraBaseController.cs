using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult HeraOk()
        {
            return Ok();
        }

        [NonAction]
        public IActionResult HeraOk(object data)
        {
            return Ok(data);
        }

        [NonAction]
        public IActionResult HeraCreated(object data)
        {
            return Created("", data);
        }

        [NonAction]
        public IActionResult HeraBadRequest(object error)
        {
            return BadRequest(error);
        }

        [NonAction]
        public IActionResult HeraBadRequest()
        {
            string messages = string.Join("; ", ModelState.Values
                                                          .SelectMany(x => x.Errors)
                                                          .Select(x => x.ErrorMessage));
            return BadRequest(messages);
        }
    }
}

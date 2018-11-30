using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entities;

namespace WebUI.Models
{
    public class CardModelsBinder : IModelBinder
    {
        private const string sessionKey = "Card";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            
            Card card = null;
            card = (Card)controllerContext.HttpContext.Session[sessionKey];

            if (card == null)
            {
                card = new Card();
                controllerContext.HttpContext.Session[sessionKey] = card;
            }

            return card;
        }
    }
}
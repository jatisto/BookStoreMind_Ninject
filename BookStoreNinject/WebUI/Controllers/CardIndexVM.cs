using Domain.Entities;

namespace WebUI.Controllers
{
    public class CardIndexVM
    {
        public Card Card { get; set; }
        public string ReturnUrl { get; set; }
    }
}
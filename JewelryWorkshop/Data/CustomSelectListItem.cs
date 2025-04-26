using Microsoft.AspNetCore.Mvc.Rendering;

namespace JewelryWorkshop.Data
{
    public class CustomSelectListItem 
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public decimal AdditionalValue { get; set; }
    }
}

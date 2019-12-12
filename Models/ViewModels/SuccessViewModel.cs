
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.ViewModels
{
    public class SuccessViewModel
    {
        public string Message { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double Value { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double Balance { get; set; }
    }
}

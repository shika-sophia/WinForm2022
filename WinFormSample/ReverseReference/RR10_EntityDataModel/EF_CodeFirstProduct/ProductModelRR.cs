using System.ComponentModel.DataAnnotations;

namespace WinFormGUI.WinFormSample.ReverseReference.RR10_EntityDataModel.EF_CodeFirstProduct
{
    class ProductModelRR
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; } = 0;

        public override string ToString()
        {
            return $"ProductId: {ProductId}\n" +
                $"Name: {Name}\n" +
                $"Price: {Price}\n";
        }//ToString()
    }//class
}

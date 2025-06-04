﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebCriptomonedas.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CryptoCode { get; set; } = string.Empty;

        [Required]
        public string Action { get; set; } = string.Empty; // "purchase" o "sale"

        [Required]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client? Client { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public decimal CryptoAmount { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public decimal Money { get; set; }

        [Required]
        public DateTime Datetime { get; set; }
    }
}

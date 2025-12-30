using Rakna.DAL.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ParkingSession
{
    [Key]
    public int ParkingSessionId { get; set; }
    public bool IsRegistered { get; set; }
    public bool IsReserved { get; set; }
    public double Cost { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    public string PlateNumbers { get; set; }
    public string PlateLetters { get; set; }

    // Nullable Foreign Key for Vehicle
    // This allows for the parking session to potentially not be linked to a registered vehicle
    public int? VehicleID { get; set; }

    [ForeignKey("VehicleID")]
    public virtual Vehicle? Vehicle { get; set; } // Navigation property for Vehicle

    // Non-Nullable Foreign Key for Garage
    // Every parking session must be associated with a Garage
    public int GarageID { get; set; }

    [ForeignKey("GarageID")]
    public virtual Garage Garage { get; set; } // Navigation property for Garage
}

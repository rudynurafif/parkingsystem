using System;
using System.Collections.Generic;
using System.Linq;

class ParkingLot
{
    private int totalSlots;
    private List<ParkingSlot> slots;

    public ParkingLot(int totalSlots)
    {
        this.totalSlots = totalSlots;
        this.slots = new List<ParkingSlot>();
        for (int i = 1; i <= totalSlots; i++)
        {
            this.slots.Add(new ParkingSlot(i));
        }
    }

    public int TotalSlots => totalSlots;

    public void CheckIn(string vehicleType, string registrationNumber, string color)
    {
        if (IsParkingFull())
        {
            Console.WriteLine("Sorry, parking lot is full");
            return;
        }

        if (IsVehicleAllowed(vehicleType))
        {
            ParkingSlot availableSlot = slots.FirstOrDefault(slot => !slot.IsOccupied);
            if (availableSlot != null)
            {
                availableSlot.CheckIn(new Vehicle(vehicleType, registrationNumber, color));
                Console.WriteLine($"Allocated slot number: {availableSlot.SlotNumber}");
            }
        }
        else
        {
            Console.WriteLine("Invalid vehicle type. Only SmallCar and Motorbike are allowed.");
        }
    }

    public void CheckOut(int slotNumber)
    {
        ParkingSlot slot = GetSlotByNumber(slotNumber);
        if (slot != null && slot.IsOccupied)
        {
            slot.CheckOut();
            Console.WriteLine($"Slot number {slotNumber} is free");
        }
        else
        {
            Console.WriteLine($"Slot number {slotNumber} is already empty");
        }
    }

    public void GenerateStatusReport()
    {
        Console.WriteLine("Slot\tNo.\tType\tRegistration No\tColour");
        foreach (ParkingSlot slot in slots)
        {
            if (slot.IsOccupied)
            {
                Vehicle vehicle = slot.ParkedVehicle;
                Console.WriteLine($"{slot.SlotNumber}\t{vehicle.RegistrationNumber}\t{vehicle.Type}\t{vehicle.Color}");
            }
        }
    }

    public void GenerateVehicleCountByTypeReport()
    {
        var vehicleCountByType = slots
            .Where(slot => slot.IsOccupied)
            .GroupBy(slot => slot.ParkedVehicle.Type)
            .Select(group => new { Type = group.Key, Count = group.Count() });

        foreach (var item in vehicleCountByType)
        {
            Console.WriteLine($"{item.Type}: {item.Count}");
        }
    }

    public void GenerateVehicleCountByColorReport(string color)
    {
        var vehicleCountByColor = slots
            .Where(slot => slot.IsOccupied)
            .Where(slot => string.Equals(slot.ParkedVehicle.Color, color, StringComparison.OrdinalIgnoreCase))
            .Count();

        Console.WriteLine($"Total {color} vehicles: {vehicleCountByColor}");
    }

    private bool IsParkingFull()
    {
        return slots.All(slot => slot.IsOccupied);
    }

    private bool IsVehicleAllowed(string vehicleType)
    {
        return string.Equals(vehicleType, "SmallCar", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(vehicleType, "Motorbike", StringComparison.OrdinalIgnoreCase);
    }

    private ParkingSlot GetSlotByNumber(int slotNumber)
    {
        return slots.FirstOrDefault(slot => slot.SlotNumber == slotNumber);
    }
}

class ParkingSlot
{
    public int SlotNumber { get; }
    public Vehicle ParkedVehicle { get; private set; }
    public bool IsOccupied => ParkedVehicle != null;

    public ParkingSlot(int slotNumber)
    {
        SlotNumber = slotNumber;
    }

    public void CheckIn(Vehicle vehicle)
    {
        ParkedVehicle = vehicle;
    }

    public void CheckOut()
    {
        ParkedVehicle = null;
    }
}

class Vehicle
{
    public string Type { get; }
    public string RegistrationNumber { get; }
    public string Color { get; }

    public Vehicle(string type, string registrationNumber, string color)
    {
        Type = type;
        RegistrationNumber = registrationNumber;
        Color = color;
    }
}

class Program
{
    static void Main(string[] args)
    {
        ParkingLot parkingLot = null;

        while (true)
        {
            Console.WriteLine("Available Commands:");
            Console.WriteLine("create_parking_lot [total_slots]");
            Console.WriteLine("park [registration_number] [color] [vehicle_type]");
            Console.WriteLine("leave [slot_number]");
            Console.WriteLine("status");
            Console.WriteLine("type_of_vehicles [vehicle_type]");
            Console.WriteLine("registration_numbers_for_vehicles_with_ood_plate");
            Console.WriteLine("registration_numbers_for_vehicles_with_event_plate");
            Console.WriteLine("registration_numbers_for_vehicles_with_colour [color]");
            Console.WriteLine("exit");
            Console.Write("Enter command: ");
            string input = Console.ReadLine();

            string[] commandParts = input.Split(' ');
            string action = commandParts[0];

            switch (action)
            {
                case "create_parking_lot":
                    int totalSlots = int.Parse(commandParts[1]);
                    parkingLot = new ParkingLot(totalSlots);
                    Console.WriteLine($"Created a parking lot with {totalSlots} slots");
                    break;

                case "park":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                    }
                    else
                    {
                        string registrationNumber = commandParts[1];
                        string color = commandParts[2];
                        string vehicleType = commandParts[3];
                        parkingLot.CheckIn(vehicleType, registrationNumber, color);
                    }
                    break;

                case "leave":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                    }
                    else
                    {
                        int slotNumber = int.Parse(commandParts[1]);
                        parkingLot.CheckOut(slotNumber);
                    }
                    break;

                case "status":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                    }
                    else
                    {
                        parkingLot.GenerateStatusReport();
                    }
                    break;

                case "type_of_vehicles":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                    }
                    else
                    {
                        string vehicleType = commandParts[1];
                        parkingLot.GenerateVehicleCountByTypeReport();
                    }
                    break;

                case "registration_numbers_for_vehicles_with_ood_plate":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                    }
                    else
                    {
                        // Implement this command if needed.
                    }
                    break;

                case "registration_numbers_for_vehicles_with_event_plate":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                    }
                    else
                    {
                        // Implement this command if needed.
                    }
                    break;

                case "registration_numbers_for_vehicles_with_colour":
                    if (parkingLot == null)
                    {
                        Console.WriteLine("Please create a parking lot first.");
                    }
                    else
                    {
                        string color = commandParts[1];
                        parkingLot.GenerateVehicleCountByColorReport(color);
                    }
                    break;

                case "exit":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }
    }
}

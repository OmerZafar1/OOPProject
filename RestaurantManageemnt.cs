using System;

namespace ConsoleApp18
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    enum ReservationStatus
    {
        Canceled,
        Confirmed,
        Completed,
        NoShow
    }

    enum SeatType
    {
        Regular, Kid, Accessible, Other
    }

    enum OrderStatus
    {
        Received, Preparation, Completed, Canceled, None
    }

    enum TableStatus
    {
        Free, Reserved, Occupied, Other
    }

    enum PaymentStatus
    {
        Unpaid, Pending, Completed, Filled, Declined, Cancelled, Abandoned, Refunded
    }

    class Address
    {
        public string StreetAddress { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }
        public string Country { get; private set; }

        public Address(string streetAddress, string city, string state, string zipCode, string country)
        {
            StreetAddress = streetAddress;
            City = city;
            State = state;
            ZipCode = zipCode;
            Country = country;
        }

        public override string ToString()
        {
            return $"{StreetAddress}, {City}, {State} {ZipCode}, {Country}";
        }
    }

    abstract class Person
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public abstract string GetRole();

        public override string ToString()
        {
            return $"{Name} - {GetRole()}";
        }
    }

    class Employee : Person
    {
        public string SocialSecurityNumber { get; set; }

        public override string GetRole()
        {
            return "Employee";
        }
    }

    class Customer : Person
    {
        public int CustomerId { get; private set; }

        private static int nextCustomerId = 1;

        public Customer(string name, Address address, string email, string phone)
        {
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
            CustomerId = nextCustomerId++;
        }

        public override string GetRole()
        {
            return "Customer";
        }

        public override string ToString()
        {
            return $"{base.ToString()} - ID: {CustomerId}";
        }
    }

    class Waiter : Employee
    {
        public string EmployeeId { get; private set; }
        private List<Order> orders;

        public Waiter(string employeeId, string name, Address address, string email, string phone)
        {
            EmployeeId = employeeId;
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
            orders = new List<Order>();
        }

        public void TakeOrder(Customer customer, List<MenuItem> menuItems)
        {
            Console.WriteLine($"{Name} is taking an order.");
            Order order = new Order(customer);
            foreach (var item in menuItems)
            {
                order.AddItem(item);
            }
            orders.Add(order);
            customer.AddOrder(order);
        }

        public void ServeOrder(Order order)
        {
            if (order.Status == OrderStatus.Preparation)
            {
                order.Status = OrderStatus.Completed;
                Console.WriteLine($"{Name} served order #{order.OrderId}.");
            }
            else
            {
                Console.WriteLine($"Order #{order.OrderId} is not ready for serving.");
            }
        }

        public override string GetRole()
        {
            return "Waiter";
        }

        public void ListOrders()
        {
            Console.WriteLine($"{Name}'s Orders:");
            foreach (var order in orders)
            {
                Console.WriteLine($"Order #{order.OrderId} - Status: {order.Status}");
            }
        }
    }

    class Chef : Employee
    {
        public string EmployeeId { get; private set; }

        public Chef(string employeeId, string name, Address address, string email, string phone)
        {
            EmployeeId = employeeId;
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
        }

        public void CookOrder(Order order)
        {
            if (order.Status == OrderStatus.Received)
            {
                order.Status = OrderStatus.Preparation;
                Console.WriteLine($"{Name} is cooking order #{order.OrderId}.");
            }
            else
            {
                Console.WriteLine($"Order #{order.OrderId} cannot be cooked at the moment.");
            }
        }

        public override string GetRole()
        {
            return "Chef";
        }
    }

    class Table
    {
        public int TableId { get; private set; }
        public int MaxCapacity { get; private set; }
        public string LocationIdentifier { get; private set; }
        public TableStatus Status { get; private set; }
        private List<Seat> seats;

        public Table(int tableId, int maxCapacity, string locationIdentifier)
        {
            TableId = tableId;
            MaxCapacity = maxCapacity;
            LocationIdentifier = locationIdentifier;
            Status = TableStatus.Free;
            seats = new List<Seat>();
            for (int i = 0; i < maxCapacity; i++)
            {
                seats.Add(new Seat(i + 1));
            }
        }

        public bool IsTableFree()
        {
            return Status == TableStatus.Free;
        }

        public void OccupyTable()
        {
            if (Status == TableStatus.Free)
            {
                Status = TableStatus.Occupied;
                Console.WriteLine($"Table {TableId} is now occupied.");
            }
            else
            {
                Console.WriteLine($"Table {TableId} is already occupied.");
            }
        }

        public void ClearTable()
        {
            if (Status == TableStatus.Occupied)
            {
                Status = TableStatus.Free;
                Console.WriteLine($"Table {TableId} has been cleared and is now free.");
            }
            else
            {
                Console.WriteLine($"Table {TableId} is not occupied.");
            }
        }

        public void PrintSeats()
        {
            Console.WriteLine($"Seats at Table {TableId}:");
            foreach (var seat in seats)
            {
                Console.WriteLine(seat);
            }
        }
    }

    class Seat
    {
        public int SeatNumber { get; private set; }
        public SeatType Type { get; private set; }
        public bool IsOccupied { get; private set; }

        public Seat(int seatNumber)
        {
            SeatNumber = seatNumber;
            Type = SeatType.Regular;
            IsOccupied = false;
        }

        public void OccupySeat()
        {
            IsOccupied = true;
            Console.WriteLine($"Seat {SeatNumber} is now occupied.");
        }

        public void ClearSeat()
        {
            IsOccupied = false;
            Console.WriteLine($"Seat {SeatNumber} has been cleared.");
        }

        public override string ToString()
        {
            return $"Seat {SeatNumber} - {Type} - {(IsOccupied ? "Occupied" : "Vacant")}";
        }
    }

    class MenuItem
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<string> Ingredients { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; }

        public MenuItem(int id, string name, string description, decimal price, List<string> ingredients, string category)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Ingredients = ingredients;
            Category = category;
            IsAvailable = true; // By default, menu items are available
        }

        public void AddIngredient(string ingredient)
        {
            Ingredients.Add(ingredient);
        }

        public void RemoveIngredient(string ingredient)
        {
            if (Ingredients.Contains(ingredient))
            {
                Ingredients.Remove(ingredient);
                Console.WriteLine($"Ingredient '{ingredient}' removed.");
            }
            else
            {
                Console.WriteLine($"Ingredient '{ingredient}' not found in the menu item.");
            }
        }

        public void SetAvailability(bool availability)
        {
            IsAvailable = availability;
            Console.WriteLine($"{Name} availability has been set to {(availability ? "available" : "unavailable")}.");
        }

        public void CheckAvailability()
        {
            if (IsAvailable)
            {
                Console.WriteLine($"{Name} is available.");
            }
            else
            {
                Console.WriteLine($"{Name} is not available.");
            }
        }

        public void UpdatePrice(decimal newPrice)
        {
            Price = newPrice;
            Console.WriteLine($"{Name} price has been updated to {newPrice:C}.");
        }

        public override string ToString()
        {
            return $"{Name} - {Price:C}";
        }
    }

    class Menu
    {
        private List<MenuItem> items;

        public Menu()
        {
            items = new List<MenuItem>();
        }

        public void AddItem(MenuItem item)
        {
            items.Add(item);
            Console.WriteLine($"{item.Name} has been added to the menu.");
        }

        public void RemoveItem(MenuItem item)
        {
            if (items.Contains(item))
            {
                items.Remove(item);
                Console.WriteLine($"{item.Name} has been removed from the menu.");
            }
            else
            {
                Console.WriteLine($"{item.Name} not found in the menu.");
            }
        }

        public void ListMenu()
        {
            Console.WriteLine("Menu Items:");
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }

        public MenuItem FindMenuItemById(int itemId)
        {
            return items.Find(item => item.Id == itemId);
        }
    }

    class Order
    {
        public int OrderId { get; private set; }
        public Customer Customer { get; private set; }
        public List<MenuItem> OrderedItems { get; private set; }
        public OrderStatus Status { get; set; }

        private static int nextOrderId = 1;

        public Order(Customer customer)
        {
            OrderId = nextOrderId++;
            Customer = customer;
            OrderedItems = new List<MenuItem>();
            Status = OrderStatus.Received;
        }

        public void AddItem(MenuItem item)
        {
            OrderedItems.Add(item);
            Console.WriteLine($"{item.Name} added to order #{OrderId}.");
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
            Console.WriteLine($"Order #{OrderId} status updated to {Status}.");
        }

        public decimal CalculateTotal()
        {
            return OrderedItems.Sum(item => item.Price);
        }

        public override string ToString()
        {
            return $"Order #{OrderId} - Status: {Status}, Total: {CalculateTotal():C}";
        }
    }

    class Kitchen
    {
        private List<Chef> chefs;

        public Kitchen()
        {
            chefs = new List<Chef>();
        }

        public void AddChef(Chef chef)
        {
            chefs.Add(chef);
            Console.WriteLine($"{chef.Name} has been added to the kitchen staff.");
        }

        public void RemoveChef(Chef chef)
        {
            if (chefs.Contains(chef))
            {
                chefs.Remove(chef);
                Console.WriteLine($"{chef.Name} has been removed from the kitchen staff.");
            }
            else
            {
                Console.WriteLine($"{chef.Name} not found in the kitchen staff.");
            }
        }

        public void ListChefs()
        {
            Console.WriteLine("Kitchen Staff:");
            foreach (var chef in chefs)
            {
                Console.WriteLine(chef);
            }
        }
    }

    class Reservation
    {
        public int ReservationId { get; private set; }
        public Customer Customer { get; private set; }
        public Table ReservedTable { get; private set; }
        public DateTime ReservationDate { get; private set; }
        public TimeSpan ReservationTime { get; private set; }
        public ReservationStatus Status { get; private set; }
        public int PeopleCount { get; private set; }

        private static int nextReservationId = 1;

        public Reservation(Customer customer, Table table, DateTime reservationDate, TimeSpan reservationTime, int peopleCount)
        {
            ReservationId = nextReservationId++;
            Customer = customer;
            ReservedTable = table;
            ReservationDate = reservationDate.Date;
            ReservationTime = reservationTime;
            Status = ReservationStatus.Confirmed;
            PeopleCount = peopleCount;
        }

        public void UpdateDate(DateTime newDate)
        {
            ReservationDate = newDate.Date;
        }

        public void UpdateTime(TimeSpan newTime)
        {
            ReservationTime = newTime;
        }

        public void CancelReservation()
        {
            if (Status == ReservationStatus.Confirmed)
            {
                Status = ReservationStatus.Canceled;
                Console.WriteLine($"Reservation #{ReservationId} has been canceled.");
            }
            else
            {
                Console.WriteLine($"Cannot cancel reservation #{ReservationId}.");
            }
        }

        public void PrintReservationDetails()
        {
            Console.WriteLine($"Reservation #{ReservationId}:");
            Console.WriteLine($"Date: {ReservationDate}, Time: {ReservationTime}");
            Console.WriteLine($"Table: {ReservedTable.TableId}, Status: {Status}");
            Console.WriteLine($"Customer: {Customer.Name}, People: {PeopleCount}");
        }
    }

    class Restaurant
    {
        private List<Branch> branches;

        public Restaurant()
        {
            branches = new List<Branch>();
        }

        public void AddBranch(Branch branch)
        {
            branches.Add(branch);
            Console.WriteLine($"Branch '{branch.Name}' has been added.");
        }

        public void ListBranches()
        {
            Console.WriteLine("Branches:");
            foreach (var branch in branches)
            {
                Console.WriteLine(branch);
            }
        }
    }

    class Branch
    {
        public string Name { get; private set; }
        public Address Location { get; private set; }
        private Kitchen kitchen;
        private Manager manager;
        private List<Table> tables;
        private List<Reservation> reservations;

        public Branch(string name, Address location)
        {
            Name = name;
            Location = location;
            kitchen = new Kitchen();
            tables = new List<Table>();
            reservations = new List<Reservation>();
        }

        public void AssignManager(Manager newManager)
        {
            if (manager == null)
            {
                manager = newManager;
                Console.WriteLine($"{newManager.Name} has been assigned as the manager of {Name} branch.");
            }
            else
            {
                Console.WriteLine($"{Name} branch already has a manager.");
            }
        }

        public void AddTable(Table table)
        {
            if (!tables.Contains(table))
            {
                tables.Add(table);
                Console.WriteLine($"Table {table.TableId} has been added to {Name} branch.");
            }
            else
            {
                Console.WriteLine($"Table {table.TableId} already exists in {Name} branch.");
            }
        }

        public void ListTables()
        {
            Console.WriteLine($"Tables in {Name} branch:");
            foreach (var table in tables)
            {
                Console.WriteLine(table);
            }
        }

        public void ListReservations()
        {
            Console.WriteLine($"Reservations in {Name} branch:");
            foreach (var reservation in reservations)
            {
                reservation.PrintReservationDetails();
            }
        }

        public void AddReservation(Reservation reservation)
        {
            reservations.Add(reservation);
            Console.WriteLine($"Reservation #{reservation.ReservationId} has been added to {Name} branch.");
        }

        public void RemoveReservation(Reservation reservation)
        {
            if (reservations.Contains(reservation))
            {
                reservations.Remove(reservation);
                Console.WriteLine($"Reservation #{reservation.ReservationId} has been removed from {Name} branch.");
            }
            else
            {
                Console.WriteLine($"Reservation #{reservation.ReservationId} not found in {Name} branch.");
            }
        }

        public override string ToString()
        {
            return $"Branch: {Name}, Location: {Location}";
        }
    }

    class Manager : Employee
    {
        private List<Waiter> waiters;
        private List<Chef> chefs;

        public Manager(string name, Address address, string email, string phone, string socialSecurityNumber)
        {
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
            SocialSecurityNumber = socialSecurityNumber;
            waiters = new List<Waiter>();
            chefs = new List<Chef>();
        }

        public void AddWaiter(Waiter waiter)
        {
            waiters.Add(waiter);
            Console.WriteLine($"{waiter.Name} has been added to {Name}'s waiter team.");
        }

        public void AddChef(Chef chef)
        {
            chefs.Add(chef);
            Console.WriteLine($"{chef.Name} has been added to {Name}'s chef team.");
        }

        public void RemoveWaiter(Waiter waiter)
        {
            if (waiters.Contains(waiter))
            {
                waiters.Remove(waiter);
                Console.WriteLine($"{waiter.Name} has been removed from {Name}'s waiter team.");
            }
            else
            {
                Console.WriteLine($"{waiter.Name} is not part of {Name}'s waiter team.");
            }
        }

        public void RemoveChef(Chef chef)
        {
            if (chefs.Contains(chef))
            {
                chefs.Remove(chef);
                Console.WriteLine($"{chef.Name} has been removed from {Name}'s chef team.");
            }
            else
            {
                Console.WriteLine($"{chef.Name} is not part of {Name}'s chef team.");
            }
        }

        public void ListWaiters()
        {
            Console.WriteLine($"Waiter Team of {Name}:");
            foreach (var waiter in waiters)
            {
                Console.WriteLine(waiter);
            }
        }

        public void ListChefs()
        {
            Console.WriteLine($"Chef Team of {Name}:");
            foreach (var chef in chefs)
            {
                Console.WriteLine(chef);
            }
        }

        public override string GetRole()
        {
            return "Manager";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create a restaurant
            Restaurant myRestaurant = new Restaurant();

            // Create a branch
            Address branchAddress = new Address("123 Main St", "Cityville", "12345", "Countryland");
            Branch mainBranch = new Branch("Main Branch", branchAddress);
            myRestaurant.AddBranch(mainBranch);

            // Create a manager for the branch
            Address managerAddress = new Address("456 Elm St", "Villagetown", "54321", "Countryland");
            Manager branchManager = new Manager("John Doe", managerAddress, "john@example.com", "123-456-7890", "123-45-6789");
            mainBranch.AssignManager(branchManager);

            // Create a chef and waiter
            Chef chef1 = new Chef("Chef Gordon", managerAddress, "gordon@example.com", "111-222-3333", "987-65-4321");
            Waiter waiter1 = new Waiter("Jane Smith", managerAddress, "jane@example.com", "444-555-6666", "543-21-9876");
            branchManager.AddChef(chef1);
            branchManager.AddWaiter(waiter1);

            // Create a kitchen
            Kitchen kitchen = new Kitchen();
            kitchen.AddChef(chef1);

            // Create menu items
            MenuItem item1 = new MenuItem(1, "Burger", "Delicious beef burger", 10.99M, new List<string> { "Beef patty", "Lettuce", "Tomato" }, "Main Course");
            MenuItem item2 = new MenuItem(2, "Salad", "Healthy garden salad", 7.99M, new List<string> { "Lettuce", "Tomato", "Cucumber" }, "Appetizer");
            MenuItem item3 = new MenuItem(3, "Cheesecake", "Rich and creamy cheesecake", 6.99M, new List<string> { "Cream cheese", "Graham cracker crust" }, "Dessert");

            // Create a menu and add items
            Menu menu = new Menu();
            menu.AddItem(item1);
            menu.AddItem(item2);
            menu.AddItem(item3);

            // Create a customer
            Customer customer = new Customer("Alice Johnson", "alice@example.com", "555-666-7777");

            // Create a reservation
            Table reservationTable = new Table(1, 4, "A1");
            DateTime reservationDate = DateTime.Now.AddDays(1);
            Reservation reservation = new Reservation(customer, reservationTable, reservationDate, TimeSpan.FromHours(18), 4);
            mainBranch.AddReservation(reservation);

            // Create an order
            Order order = new Order(customer);
            order.AddItem(item1);
            order.AddItem(item2);

            // Display information
            Console.WriteLine($"Welcome to {mainBranch.Name}!");
            Console.WriteLine($"Manager: {branchManager.Name}");
            branchManager.ListWaiters();
            branchManager.ListChefs();
            Console.WriteLine($"Available Menu Items:");
            menu.ListMenu();

            Console.WriteLine($"Reservation Details:");
            mainBranch.ListReservations();

            Console.WriteLine($"Order Details:");
            Console.WriteLine(order);
            Console.WriteLine($"Total: {order.CalculateTotal():C}");

            // Additional interactions and testing can be added here

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

}
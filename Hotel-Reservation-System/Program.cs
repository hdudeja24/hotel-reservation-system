﻿using System;
using System.IO;
using System.Data.SqlClient;

namespace Hotel_Reservation_System
{


    class Program
    {
        //This class allows the program to use the base rate as a global variable
        //because C# does not support global variables
        //Whenever base rate is referenced in another class, it must be done as
        //              Program.BaseRate.baseRate;
        //this also conatains the function that retrieves the connection strings
        public static class GlobalClass
        {
            public static double baseRate = 100.0;

            public static string ConnectionStr()
            {
                //you two will need to create a text file that contains your connection string to the database
                //then you will need to create a string conatining the file path on your computer to the text
                //file containing  your connection string, we will need to change the variable in the 
                //File.OpenRead function below to the name of the string containing our filepath each time
                //we want to run the code on our computer

                string HarrishFilePath;
                string HitsFilePath;
                string HunterFilePath = "C:\\Users\\hhowa\\Source\\Repos\\hdudeja24\\Hotel-Reservation-System\\Hotel-Reservation-System\\HunterConnectionString.txt";
                using FileStream file = File.OpenRead(HunterFilePath); //change this variable to the string of your filepath
                using var stream = new StreamReader(file);
                return stream.ReadLine();
            }
        }


        static void Main(string[] args)
        {
        //here is the login, the user will enter the role they want to login as, must be a guest, employee, or manager
        Login:
            string login;
            Console.WriteLine("Welcome. Are you signing in as a 'Guest', 'Employee', or 'Manager'?");

            //only allow a login if they enter a valid account type
            while (true)
            {
                login = Console.ReadLine();
                if ((login == "Guest") || (login == "Employee") || (login == "Manager"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Login. Type 'Guest', 'Employee', or 'Manager' to login as your role.");
                }
            }

            //actions that can be performed as a guest:
            //Making a reservation, cancelling a reservation, rescheduling a reservation, or providing credit card info
            //log out
            if (login == "Guest")
            {
                while (true)
                {
                    string GuestAction;
                    Console.WriteLine("\nWould you like to 'M'ake, 'C'ancel, or 'R'eschedule a reservation?");
                    Console.WriteLine("Or, if you've already made a 60-Day reservation, 'A'dd your credit card info?");
                    Console.WriteLine("Enter 'L' to log out.");

                    while (true) // only let users enter a valid command
                    {
                        GuestAction = Console.ReadLine();

                        if ((GuestAction == "M") || (GuestAction == "C") || (GuestAction == "R") || (GuestAction == "A") || (GuestAction == "L"))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Action. Enter 'M'ake, 'C'ancel, 'R'eschedule, 'A'dd, 'L'ogout.");
                        }
                    }
                    //once the user enters a valid command, perform command

                    //add any more guest actions here

                    if (GuestAction == "M") //Making a reservation
                    {
                        Reservation.MakeReservation();
                    }
                    if (GuestAction == "C") //Cancelling a reservation
                    {
                        //Reservation.CancelReserve();
                    }
                    if (GuestAction == "R") //Rescheduling a reservation
                    {
                        //Reservation.RescheduleReserve();
                    }
                    if (GuestAction == "A") //adding credit card info to a 60-day
                    {
                        Reservation.Add_CC_Info();
                    }
                    if (GuestAction == "L") // logging out
                    {
                        Console.WriteLine("Logged Out.\n");
                        goto Login;
                    }
                }
            }

            //actions that can be performed as an employee
            //
            if (login == "Employee")
            {
                string EmployeeAction;
                Console.WriteLine("\nWhat function would you like to perform?");
                Console.WriteLine("Enter 'L' to log out.");

                while (true) //only let users enter a valid command
                {
                    EmployeeAction = Console.ReadLine();
                    if ((EmployeeAction == "L"))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Enter a valid action.'L' - Logout");
                    }
                }
                //once the manager enters a valid command, perform it

                //add other employee actions here

                if (EmployeeAction == "L")
                {
                    Console.WriteLine("Logged out.\n");
                    goto Login;
                }
            }

            //actions that can be performed as a manager
            //Changing base rate, loging out
            if (login == "Manager")
            {
                while (true)
                {
                    string ManagerAction;
                    Console.WriteLine("\nWhat function would you like to perform?");
                    Console.WriteLine("'C' - Change Base Rate; 'L' - Log Out");

                    while (true) //only let users enter a valid command
                    {
                        ManagerAction = Console.ReadLine();
                        if ((ManagerAction == "C") || (ManagerAction == "L") || (ManagerAction == "O"))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Enter a valid action. 'C' - Change Rate 'L' - Logout 'O' - Expected Occupany Report");
                        }
                    }
                    //once the manager enters a valid command, perform it

                    //add all manager actions here

                    if (ManagerAction == "C")
                    {
                        Hotel.ChangeBaseRate();
                    }
                    if (ManagerAction == "O")
                    {
                        //need to add here a filepath and a todays date
                        //Report.expectedOccupancy();
                    }
                    if (ManagerAction == "L")
                    {
                        Console.WriteLine("Logged out.\n");
                        goto Login;
                    }
                }
            }



        }
    }

    public class Hotel
    {
        public static void ChangeBaseRate()
        {
            Console.WriteLine("Current base rate is: " + Program.GlobalClass.baseRate);
            Console.WriteLine("Enter the new base rate:");
            while (true)
            {
                string baseRateStr = Console.ReadLine();
                if (IsDouble(baseRateStr) == true)
                {
                    Program.GlobalClass.baseRate = Convert.ToDouble(baseRateStr);
                    break;
                }
                else
                {
                    Console.WriteLine("Enter a valid rate.");
                }
            }
            Console.WriteLine("New base rate: " + Program.GlobalClass.baseRate);
        }

        //This function accepts a string meant to represent a money value
        //it first checks to make sure that the length is at most 16
        //any larger wouldnt fit into the database, it then uses TryParse to 
        //check that the string is a valid double, returns true if so, false if not
        public static bool IsDouble(string price)
        {
            bool isGoodDouble;
            if (price.Length > 16)
            {
                isGoodDouble = false;
                return isGoodDouble;
            }
            else
            {
                isGoodDouble = double.TryParse(price, out _);
                return isGoodDouble;
            }
        }
    }

    public class Report
    {
        // Generates the Expected Occupancy Report given the current date and file path. (Only management can access this function)
        public static void expectedOccupancy(string filePath, DateTime date)
        {
            int i = 1;
            double occupancy_rate = 0;
            DateTime currentDate;
            using StreamWriter sw = File.CreateText(filePath);

            sw.WriteLine("Date       " + "Prepaid Reservation  " + "60-Day Reservation  " + "Conventional Reservation  " + "Incentive Reservation  " + "Total Number of Rooms Reserved");
            while (i <= 30)
            {
                currentDate = date.AddDays(i);
                sw.Write(currentDate.ToString("yyyy-MM-dd") + " ");
                sw.Write(Reservation.num_PrepaidReservation[i].ToString() + "                   ");
                sw.Write(Reservation.num_60DayReservation[i].ToString() + "                  ");
                sw.Write(Reservation.num_ConventionalReservation[i].ToString() + "                        ");
                sw.Write(Reservation.num_IncentiveReservation[i].ToString() + "                     ");
                sw.WriteLine(Reservation.num_OccupiedRooms[i].ToString());
                occupancy_rate = ((Reservation.num_OccupiedRooms[i]) / 45)*100 + occupancy_rate;
                i++;
            }
            sw.WriteLine("Average Expected Occupancy Rate: " + (occupancy_rate / 30) + "%");
            Console.WriteLine("Expected Occupancy Report successfully created in desired directory.");
        }

        public static void roomIncomeReport(string filePath, DateTime date) 
        {
            
        }
        
    }

    public class Reservation
    {
        // Arrays store the number of occupied rooms and reservation type for each day from current day.
        // The elements in the arrays are updated accordingly every time a guest checks in or checks out.
        public static int[] num_OccupiedRooms = new int[365];
        public static int[] num_PrepaidReservation = new int[365];
        public static int[] num_ConventionalReservation = new int[365];
        public static int[] num_IncentiveReservation = new int[365];
        public static int[] num_60DayReservation = new int[365];

        public static void MakeReservation()
        {
            Console.WriteLine("\nYou are now about to make a reservation.");
            Console.WriteLine("Which type of reservation would you like to make?");
            Console.WriteLine("-'P' for Prepaid  -'S' for 60 Day  -'C' for Conventional    -'I' for Incentive");
            string selectReservation;
            while (true)
            {
                selectReservation = Console.ReadLine();
                if ((selectReservation == "P") || (selectReservation == "S") || (selectReservation == "C") || (selectReservation == "I"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid reservation type.");
                    Console.WriteLine("Enter 'P' for Prepaid, 'S' for 60 Day, 'C' for Conventional, or 'I' Incentive");
                }
            }

            if (selectReservation == "P")
            {
                PrepaidReserve();
            }
            if (selectReservation == "S")
            {
                SixtyDayReserve();
            }
            if (selectReservation == "C")
            {
                ConvReserve();
            }
            if (selectReservation == "I")
            {
                //IncentReserve();
            }
        }

        //This function is called by makeReservation() when the guest selects to make a prepaid(90-day)
        //reservation, it will take the guests first and last name, then a valid credit card number
        //Then, it will call selectDays so that the guest can choose the duration of their stay
        //it will then calculate the number of nights the guest is staying and the total price
        //of their stay, it then uploads all of this information to the database
        public static void PrepaidReserve()
        {
            string reserveType = "prepaid";
            string FName, LName, CCNum, startDate, endDate;
            string[] days;
            int dayDiff;
            double totalPrice;

            //get first and last name and cc number
            Console.WriteLine("Enter your first name:");
            FName = Console.ReadLine();
            Console.WriteLine("Enter your last name:");
            LName = Console.ReadLine();
            Console.WriteLine("Enter your credit card number:");
            while (true) //we need a valid credit card number
            {
                CCNum = Console.ReadLine();
                if (CC_payment(CCNum) == true)
                {
                    break;
                }
                else
                    Console.WriteLine("Invalid credit card number. Please re-enter your CC Number:");
            }
            //selecting start and end days
            days = SelectDays(reserveType);
            startDate = days[0];
            endDate = days[1];

            //calculating the number of nights for the total price of the stay
            DateTime start = DateTime.ParseExact(startDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            dayDiff = (end.Date - start.Date).Days;
            totalPrice = dayDiff * (Program.GlobalClass.baseRate * 0.75);

            //the sql insert command for all of the reservation info
            string ConnectionStr = Program.GlobalClass.ConnectionStr();
            using SqlConnection newConnection = new(ConnectionStr);
            SqlCommand InsertTest = new("INSERT INTO Reservations(Fname, Lname, CCNum, reserveType, " +
                                     "startDate, endDate, checkedIn, baseRate, totalPrice) VALUES ('" + FName 
                                     + "','" + LName+ "','"+ CCNum+ "','"+ reserveType+"','"+ startDate+ "','" 
                                     + endDate + "'," + 0 + "," + Program.GlobalClass.baseRate + ","+ totalPrice 
                                     +")", newConnection);
            InsertTest.Connection.Open();
            try
            {
                if (InsertTest.ExecuteNonQuery() > 0)
                    Console.WriteLine("Prepaid reservation made."); //reservation is in database
                else
                    Console.WriteLine("Unable to make reservation.");
            }
            catch
            {
                Console.WriteLine("Error occurred while making Prepaid Reservation.");
            }
            InsertTest.Connection.Close();
        }

        public static void SixtyDayReserve()
        {
            string reserveType = "sixty";
            string FName, LName, Email, startDate, endDate;
            string[] days;
            int dayDiff;
            double totalPrice;

            //get first and last name and cc number
            Console.WriteLine("Enter your first name:");
            FName = Console.ReadLine();
            Console.WriteLine("Enter your last name:");
            LName = Console.ReadLine();
            Console.WriteLine("Enter your email.");
            Email = Console.ReadLine();
            //selecting start and end days
            days = SelectDays(reserveType);
            startDate = days[0];
            endDate = days[1];

            //calculating the number of nights for the total price of the stay
            DateTime start = DateTime.ParseExact(startDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            dayDiff = (end.Date - start.Date).Days;
            totalPrice = dayDiff * (Program.GlobalClass.baseRate * 0.85);

            //the sql insert command for all of the reservation info
            string ConnectionStr = Program.GlobalClass.ConnectionStr();
            using SqlConnection newConnection = new(ConnectionStr);
            SqlCommand InsertTest = new("INSERT INTO Reservations(Fname, Lname, Email, reserveType, " +
                                     "startDate, endDate, checkedIn, baseRate, totalPrice) VALUES ('" + FName
                                     + "','" + LName + "','" + Email + "','" + reserveType + "','" + startDate + "','"
                                     + endDate + "'," + 0 + "," + Program.GlobalClass.baseRate + "," + totalPrice
                                     + ")", newConnection);
            InsertTest.Connection.Open();
            try
            {
                if (InsertTest.ExecuteNonQuery() > 0)
                    Console.WriteLine("60-Day reservation made."); //reservation is in database
                else
                    Console.WriteLine("Unable to make reservation.");
            }
            catch
            {
                Console.WriteLine("Error occurred while making 60-Day Reservation.");
            }
            InsertTest.Connection.Close();
        }

        public static void ConvReserve()
        {
            string reserveType = "convent";
            string FName, LName, CCNum, startDate, endDate;
            string[] days;
            int dayDiff;
            double totalPrice;

            //get first and last name and cc number
            Console.WriteLine("Enter your first name:");
            FName = Console.ReadLine();
            Console.WriteLine("Enter your last name:");
            LName = Console.ReadLine();
            Console.WriteLine("Enter your credit card number:");
            while (true) //we need a valid credit card number
            {
                CCNum = Console.ReadLine();
                if (CC_payment(CCNum) == true)
                {
                    break;
                }
                else
                    Console.WriteLine("Invalid credit card number. Please re-enter your CC Number:");
            }
            //selecting start and end days
            days = SelectDays(reserveType);
            startDate = days[0];
            endDate = days[1];

            //calculating the number of nights for the total price of the stay
            DateTime start = DateTime.ParseExact(startDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            dayDiff = (end.Date - start.Date).Days;
            totalPrice = dayDiff * (Program.GlobalClass.baseRate);

            //the sql insert command for all of the reservation info
            string ConnectionStr = Program.GlobalClass.ConnectionStr();
            using SqlConnection newConnection = new(ConnectionStr);
            SqlCommand InsertTest = new("INSERT INTO Reservations(Fname, Lname, CCNum, reserveType, " +
                                     "startDate, endDate, checkedIn, baseRate, totalPrice) VALUES ('" + FName
                                     + "','" + LName + "','" + CCNum + "','" + reserveType + "','" + startDate + "','"
                                     + endDate + "'," + 0 + "," + Program.GlobalClass.baseRate + "," + totalPrice
                                     + ")", newConnection);
            InsertTest.Connection.Open();
            try
            {
                if (InsertTest.ExecuteNonQuery() > 0)
                    Console.WriteLine("Conventional reservation made."); //reservation is in database
                else
                    Console.WriteLine("Unable to make reservation.");
            }
            catch
            {
                Console.WriteLine("Error occurred while making Conventional Reservation.");
            }
            InsertTest.Connection.Close();

        }

        public static void IncentReserve()
        {
            string reserveType = "incent";
            string FName, LName, CCNum;
        }

        //string reserveType will let this function know what days to let the guest select from, it will then take the dates 
        //as a string and check that they are in valid format with the ValidDate() function. once ValidDate() accepts the two
        //strings entered by the user, this function will call CheckDays() to see if those days are open, if CheckDays() returns
        //True, then the days selected by the user are valid and SelectDays() will return an array containing two strings representing
        //the start date in index 0 and the end date in index 1
        public static string[] SelectDays(string reserveType)
        {
            string [] days = new string[2];
            string startDay, endDay;

            if (reserveType == "prepaid")
            {
                string prepaidDate = DateTime.Now.AddDays(90).ToString("yyyy-MM-dd"); //90 days from today
                Console.WriteLine("Select a day at or after " + prepaidDate);
                SelectPrepaidDays:
                while(true) //we need a date in the correct format that is at least 90 days from today
                {
                    Console.WriteLine("Enter a starting day for your reservation in format yyyy-MM-dd");
                    startDay = Console.ReadLine();
                    if(ValidDate(startDay) == true) //if were in the correct format
                    {
                        int a = String.Compare(startDay, prepaidDate);
                        if(a < 0) //if start day is before prepaid day it is invalid
                        {
                            Console.WriteLine("Start date must be at least " + prepaidDate);
                            continue;
                        }
                        break;
                    }
                }
                while (true) //end date must be a valid number
                {
                    Console.WriteLine("Enter an end date in yyyy-MM-dd format.");
                    endDay = Console.ReadLine();
                    if (ValidDate(endDay) == true)
                    {
                        int a = String.Compare(startDay, endDay); //it must be after the start date
                        if(a >= 0)
                        {
                            Console.WriteLine("End date must be after start date");
                            continue;
                        }
                        break;
                    }
                }
                //make sure there is an occupancy for the duration
                bool goodDuration = CheckDays(startDay, endDay, reserveType);

                if(goodDuration == true)
                {
                    days[0] = startDay;
                    days[1] = endDay;
                    return days; //return the array storing our start and end days
                }
                Console.WriteLine("There is an occupancy conflict for that duration.");
                Console.WriteLine("Please choose a different duration of stay.");
                goto SelectPrepaidDays;
            }

            /*
             * 
             * 60 day reservation
             * 
             */
            if (reserveType == "sixty")
            {
                //get todays date and add 60 to it, user must choose days >= 60 days
                string sixtyDate = DateTime.Now.AddDays(60).ToString("yyyy-MM-dd"); //60 days from today
                Console.WriteLine("Select a day at or after " + sixtyDate);
            SelectSixtyDays:
                while (true) //we need a date in the correct format that is at least 60 days from today
                {
                    Console.WriteLine("Enter a starting day for your reservation in format yyyy-MM-dd");
                    startDay = Console.ReadLine();
                    if (ValidDate(startDay) == true) //if were in the correct format
                    {
                        int a = String.Compare(startDay, sixtyDate);
                        if (a < 0) //if start day is before prepaid day it is invalid
                        {
                            Console.WriteLine("Start date must be at least " + sixtyDate);
                            continue;
                        }
                        break;
                    }
                }
                while (true) //end date must be a valid number
                {
                    Console.WriteLine("Enter an end date in yyyy-MM-dd format.");
                    endDay = Console.ReadLine();
                    if (ValidDate(endDay) == true)
                    {
                        int a = String.Compare(startDay, endDay); //it must be after the start date
                        if (a >= 0)
                        {
                            Console.WriteLine("End date must be after start date");
                            continue;
                        }
                        break;
                    }
                }
                //make sure there is an occupancy for the duration
                bool goodDuration = CheckDays(startDay, endDay, reserveType);

                if (goodDuration == true)
                {
                    days[0] = startDay;
                    days[1] = endDay;
                    return days; //return the array storing our start and end days
                }
                Console.WriteLine("There is an occupancy conflict for that duration.");
                Console.WriteLine("Please choose a different duration of stay.");
                goto SelectSixtyDays;
            }
            /*
             * 
             * Conventional
             * 
             */
            if (reserveType == "convent")
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd"); //today
                Console.WriteLine("Select a day at or after " + today);
            SelectConventionalDays:
                while (true) //we need a date in the correct format that is at least today
                {
                    Console.WriteLine("Enter a starting day for your reservation in format yyyy-MM-dd");
                    startDay = Console.ReadLine();
                    if (ValidDate(startDay) == true) //if were in the correct format
                    {
                        int a = String.Compare(startDay, today);
                        if (a < 0) //if start day is before prepaid day it is invalid
                        {
                            Console.WriteLine("Start date must be at least " + today);
                            continue;
                        }
                        break;
                    }
                }
                while (true) //end date must be a valid number
                {
                    Console.WriteLine("Enter an end date in yyyy-MM-dd format.");
                    endDay = Console.ReadLine();
                    if (ValidDate(endDay) == true)
                    {
                        int a = String.Compare(startDay, endDay); //it must be after the start date
                        if (a >= 0)
                        {
                            Console.WriteLine("End date must be after start date");
                            continue;
                        }
                        break;
                    }
                }
                //make sure there is an occupancy for the duration
                bool goodDuration = CheckDays(startDay, endDay, reserveType);

                if (goodDuration == true)
                {
                    days[0] = startDay;
                    days[1] = endDay;
                    return days; //return the array storing our start and end days
                }
                Console.WriteLine("There is an occupancy conflict for that duration.");
                Console.WriteLine("Please choose a different duration of stay.");
                goto SelectConventionalDays;
            }
            if (reserveType == "incent")
            {
                return days;
            }
            return days;
        }

        //Once the user has entered the start and end date the function accept the two strings and calculates
        //the range between the two days. From there, for every day in the range it will use the SELECT COUNT
        //query to see how many rows (reservations) exist on each day in the range, if any of the days checked
        //returns a value equal to 45, then that day is full and the function will return false
        //if every day is good, update the occupancy array for each day, then return true
        public static bool CheckDays(string startDate, string endDate, string reserveType)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd"); //get todays date
            //turn todays date, the start date, and the end date into DateTime objects so we can do math on them
            DateTime today = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime start = DateTime.ParseExact(startDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            int startDif = (start.Date - today.Date).Days; //how far away is the start date from today
            int endDif = (end.Date - today.Date).Days;  //how far away is the end date from today

            for(int i = startDif; i < endDif; i++) //check the occupancy array over the duration of the stay
            {
                if(num_OccupiedRooms[i] >= 45) //if we have a day that is fully occupied
                {
                    return false; //duration is not acceptable
                }
            }
            for (int i = startDif; i < endDif; i++) //if the duration is good, update the occupancy array
            {                                       //and array corresponding to reservation type
                num_OccupiedRooms[i]++;
                if(reserveType == "prepaid")
                    num_PrepaidReservation[i]++;
                else if(reserveType == "sixty")
                    num_60DayReservation[i]++;
                else if(reserveType == "convent")
                    num_ConventionalReservation[i]++;
                else if(reserveType == "incent")
                    num_IncentiveReservation[i]++;
            }
            return true; //otherwise, accept duration
        }

        public static void CancelReserve()
        {

        }

        public static void RescheduleReserve()
        {

        }

        //get Fname and Lname, check that they have a 60-day reservation
        //update their reservation otherwise give message saying 'no
        //reservation for that name' or 'already have credit card info for you'
        public static void Add_CC_Info()
        {
            string FName, LName, CCNum;
            Console.WriteLine("Enter your first name:");
            FName = Console.ReadLine();
            Console.WriteLine("Enter your last name:");
            LName = Console.ReadLine();
            Console.WriteLine("Enter your credit card information.");
            while(true)
            {
                CCNum = Console.ReadLine();
                if(CC_payment(CCNum) == true)
                {
                    break;
                }
                Console.WriteLine("Enter a valid credit card number.");
            }

            using SqlConnection newConnection = new(Program.GlobalClass.ConnectionStr());
            SqlCommand UpdateTest = new("UPDATE Reservations SET CCNum = " + CCNum + "WHERE FName = '" + FName + "' AND LName = '" + LName + "' AND reserveType = 'sixty'", newConnection);
            UpdateTest.Connection.Open();
            try
            {
                if (UpdateTest.ExecuteNonQuery() > 0)
                    Console.WriteLine("Credit card number updated successfully.");
                else
                    Console.WriteLine("No 60-Day reservation exists for that name.");
            }
            catch
            {
                Console.WriteLine("Error occurred while attempting to update credit card info.");
            }
            UpdateTest.Connection.Close();
        }


        //This function accepts a string meant to represent a credit card number
        //It first checks that the string has a length of 16, then it uses the
        //TryParse function to make sure the entire string is numerical
        //If the string is a valid credit card number, the function returns true
        //Otherwise, the function will return false
        public static bool CC_payment(string ccNum)
        {
            bool isGoodNum;
            if (ccNum.Length != 16)
            {
                isGoodNum = false;
                return isGoodNum;
            }
            else
            {
                isGoodNum = long.TryParse(ccNum, out _);
                return isGoodNum;
            }
        }


        //This function is passed a string meant to represent a date in the format yyyy-MM-dd
        //It first checks that the string os of length 10 and that the dashes are in the correct 
        //spots, it then makes sure that all of the other characters are numbers. Then, it takes
        //the month and day values and checks to make sure that they are valid values. If so,
        //the function will return true, otherwise it returns false
        public static bool ValidDate(string Date)
        {
            bool isGoodDate = true;
            if (Date.Length != 10)
            {
                isGoodDate = false;
                return isGoodDate;
            }
            if ((Date[4] != '-') || (Date[7] != '-'))
            {
                isGoodDate = false;
                return isGoodDate;
            }
            if (!Char.IsDigit(Date[0]) || !Char.IsDigit(Date[1]) || !Char.IsDigit(Date[2]) || !Char.IsDigit(Date[3])
            || !Char.IsDigit(Date[5]) || !Char.IsDigit(Date[6]) || !Char.IsDigit(Date[8]) || !Char.IsDigit(Date[9]))
            {
                isGoodDate = false;
                return isGoodDate;
            }

            string monthStr = Date.Substring(5, 2);
            string dayStr = Date.Substring(8, 2);
            int month = int.Parse(monthStr);
            int day = int.Parse(dayStr);

            if ((month < 1) || (month > 12))
            {
                isGoodDate = false;
                return isGoodDate;
            }
            if ((month == 1) || (month == 3) || (month == 5) || (month == 7) || (month == 8) || (month == 10) || (month == 12))
            {
                if ((day < 1) || (day > 31))
                {
                    isGoodDate = false;
                    return isGoodDate;
                }
            }
            else if ((month == 4) || (month == 6) || (month == 9) || (month == 11))
            {
                if ((day < 1) || (day > 30))
                {
                    isGoodDate = false;
                    return isGoodDate;
                }
            }
            else if (month == 2)
            {
                if ((day < 1) || (day > 28))
                {
                    isGoodDate = false;
                    return isGoodDate;
                }
            }
            return isGoodDate;
        }
    }

}

﻿using System;
using System.IO;
using System.Data.SqlClient;

namespace Hotel_Reservation_System
{

   
    class Program
    {
        //This class allows the program to use the base rate as a global variable
        //because C# does not support global variables
        //Whener base rate is referenced in another class, it must be done as
        //              Program.BaseRate.baseRate;
        public static class BaseRate
        {
            public static double baseRate = 100.0;
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
                if((login == "Guest") || (login == "Employee") || (login == "Manager"))
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
                    if (GuestAction == "A")
                    {
                        //Reservation.Add_CC_Info();
                    }
                    if (GuestAction == "L")
                    {
                        Console.WriteLine("Logged Out.\n");
                        goto Login;
                    }
                }
            }

            //actions that can be performed as an employee
            //
            if(login == "Employee")
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
                        Console.WriteLine("Enter a valid action. 'C' - Change Rate 'L' - Logout");
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
            if(login == "Manager")
            {
                while (true)
                {
                    string ManagerAction;
                    Console.WriteLine("\nWhat function would you like to perform?");
                    Console.WriteLine("'C' - Change Base Rate; 'L' - Log Out");

                    while (true) //only let users enter a valid command
                    {
                        ManagerAction = Console.ReadLine();
                        if((ManagerAction == "C") || (ManagerAction == "L"))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Enter a valid action. 'C' - Change Rate 'L' - Logout");
                        }
                    }
                    //once the manager enters a valid command, perform it

                    //add all manager actions here

                    if (ManagerAction == "C")
                    {
                        Hotel.ChangeBaseRate();
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
            Console.WriteLine("Current base rate is: " + Program.BaseRate.baseRate);
            Console.WriteLine("Enter the new base rate:");
            while(true)
            {
                string baseRateStr = Console.ReadLine();
                if (IsDouble(baseRateStr) == true)
                {
                    Program.BaseRate.baseRate = Convert.ToDouble(baseRateStr);
                    break;
                }
                else
                {
                    Console.WriteLine("Enter a valid rate.");
                }
            }
            Console.WriteLine("New base rate: " + Program.BaseRate.baseRate);



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

    }

    public class Reservation
    {

        public static void MakeReservation()
        {
            Console.WriteLine("\nYou are now about to make a reservation.");
            Console.WriteLine("Which type of reservation would you like to make?");
            Console.WriteLine("-'P' for Prepaid  -'S' for 60 Day  -'C' for Conventional    -'I' for Incentive");
            string selectReservation;
            while(true)
            {
                selectReservation = Console.ReadLine();
                if((selectReservation == "P") || (selectReservation == "S") || (selectReservation == "C") || (selectReservation == "I"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid reservation type.");
                     Console.WriteLine("Enter 'P' for Prepaid, 'S' for 60 Day, 'C' for Conventional, or 'I' Incentive");
                }
            }

            if(selectReservation == "P")
            {
                //prepaidReserve();
            }
            if(selectReservation == "S")
            {
                //SixtyDayReserve();
            }
            if(selectReservation == "C")
            {
                //ConvReserve();
            }
            if(selectReservation == "I")
            {
                //IncentReserve();
            }
            Console.WriteLine("Reservation Complete.");
        }

        public static void PrepaidReserve()
        {
            string reserveType = "prepaid";
            string FName, LName, CCNum;
        }

        public static void SixtyDayReserve()
        {
            string reserveType = "sixty";
            string Email;
        }

        public static void ConvReserve()
        {
            string reserveType = "conventional";
            string FName, LName, CCNum;
        }

        public static void IncentReserve()
        {
            string reserveType = "incentive";
            string FName, LName, CCNum;
        }

        //string reserveType will let this function know what days to let the guest select from, it will then take the dates 
        //as a string and check that they are in valid format with the ValidDate() function. once ValidDate() accepts the two
        //strings entered by the user, this function will call CheckDays() to see if those days are open, if CheckDays() returns
        //True, then the days selected by the user are valid and SelectDays() will return true to let the reservation functions
        //know the user's reservation can be made
        public static bool SelectDays(string reserveType)
        {

            if(reserveType == "prepaid")
            {
                //get todays date and add 90 days to it, user must choose days >= 90 days


                return true;
            }
            if(reserveType == "sixty")
            {
                //get todays date and add 60 to it, user must choose days >= 60 days


                return true;
            }
            if(reserveType == "conventional")
            {
                return true;
            }
            if(reserveType == "incentive")
            {
                return true;
            }
            return true;
        }

        //Once the user has entered the start and end date the function accept the two strings and calculates
        //the range between the two days. From there, for every day in the range it will use the SELECT COUNT
        //query to see how many rows (reservations) exist on each day in the range, if any of the days checked
        //returns a value equal to 45, then that day is full and the function will return false
        //if every day is good, the return true
        public static bool CheckDays(string startDate, string endDate)
        {
            return true;
        }

        public static void CancelReserve()
        {

        }

        public static void RescheduleReserve()
        {

        }

        //get Fname and Lname, check that they have a 60-day reservation
        //and that credit card info is null, if so, update their reservation
        //otherwise give message saying 'no reservation for that name' or
        //'already have credit card info for you'
        public static void Add_CC_Info()
        {

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
            if(Date.Length != 10)
            {
                isGoodDate = false;
                return isGoodDate;
            }
            if((Date[4] != '-') || (Date[7] != '-'))
            {
                isGoodDate = false;
                return isGoodDate;
            }
            if(!Char.IsDigit(Date[0]) || !Char.IsDigit(Date[1]) || !Char.IsDigit(Date[2]) || !Char.IsDigit(Date[3]) 
            || !Char.IsDigit(Date[5]) || !Char.IsDigit(Date[6]) || !Char.IsDigit(Date[8]) || !Char.IsDigit(Date[9]))
            {
                isGoodDate = false;
                return isGoodDate;
            }
        
            string monthStr = Date.Substring(5, 2);
            string dayStr = Date.Substring(8, 2);
            int month = int.Parse(monthStr);
            int day = int.Parse(dayStr);

            if((month < 1) || (month > 12))
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
            else if((month == 4) || (month == 6) || (month == 9) || (month == 11))
            {
                if ((day < 1) || (day > 30))
                {
                    isGoodDate = false;
                    return isGoodDate;
                }
            }
            else if(month == 2)
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

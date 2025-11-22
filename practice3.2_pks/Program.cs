namespace practice3._2_pks
{
    class MainProgram
    {
        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
                Console.WriteLine("Выберите функцию:");
                Console.WriteLine("1 - Управление столами");
                Console.WriteLine("2 - Бронирование");
                Console.WriteLine("3 - Тест (Program3)");
                Console.WriteLine("4 - Выход");

                Console.Write("Введите номер задания: ");

                while (true)
                {
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Table.table_management();
                            break;
                        case "2":
                            Reservation.reservation_manager();
                            break;
                        case "3":
                            break;
                        case "4":
                            return;
                        default:
                            Console.WriteLine("Неверный выбор");
                            continue;
                    }
                    break;

                }
            }
        }

        public static int checking_int_number()
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int number) && number > 0)
                {
                    return number;
                }
                Console.Write("Ошибка! Введите целое число больше 0:");
            }
        }

        public static int checking_start_hour()
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int hour) && hour >= 9 && hour <= 17)
                {
                    return hour;
                }
                Console.WriteLine("Ошибка! Введите целое число от 9 до 17:");
            }
        }

        public static int checking_end_hour(int startHour)
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int hour) && hour >= startHour + 1 && hour <= 18)
                {
                    return hour;
                }

                Console.WriteLine($"Ошибка! Введите целое число от {startHour + 1} до 18:");
            }
        }

        public static string checking_phone()
        {
            while (true)
            {
                string phone = Console.ReadLine();
                if (phone.Length == 11 && phone.All(char.IsDigit))
                {
                    return phone;
                }
                Console.Write("Ошибка! Номер телефона должен содержать ровно 11 цифр. Попробуйте еще раз: ");
            }
        }

        public static string checking_phone_last4number ()
        {
            while (true)
            {
                string phone = Console.ReadLine();
                if (phone.Length == 4 && phone.All(char.IsDigit))
                {
                    return phone;
                }
                Console.Write("Ошибка! Номер телефона должен содержать ровно 4 цифр. Попробуйте еще раз: ");
            }
        }

        public static string checking_name()
        {
            while (true)
            {
                string name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name) && !name.Any(char.IsDigit))
                {
                    return name;
                }
                Console.Write("Ошибка! Имя не может быть пустым и не должно содержать цифр. Попробуйте еще раз: ");
            }
        }
    }
}


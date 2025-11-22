using System;
namespace practice3._2_pks
{
	public class Table
	{
        public static void table_management()
        {
            while (true)
            {
                Console.WriteLine("\n=== УПРАВЛЕНИЕ СТОЛАМИ ===");
                Console.WriteLine("1 - Добавить стол");
                Console.WriteLine("2 - Вывести информацию о столе");
                Console.WriteLine("3 - Изменения информации стола");
                Console.WriteLine("4 - Вывод перечня всех доступных для бронирования столов по фильтру");
                Console.WriteLine("5 - Вернуться в главное меню");
                Console.WriteLine("6 - Выход");

                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        creating_table();
                        break;
                    case "2":
                        output_information_about_table();
                        break;
                    case "3":
                        changing_table_information();
                        break;
                    case "4":
                        search_table_for_reservation_with_filter();
                        break;
                    case "5":
                        return;
                    case "6":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        continue;
                }
            }
        }

        public int id { get; set; }
        public string location { get; set; }
        public int seats_count { get; set; }
        public Dictionary<string, string> timetable = new Dictionary<string, string>()
        {
            {"9.00-10.00", null},
            {"10.00-11.00", null},
            {"11.00-12.00", null},
            {"12.00-13.00", null},
            {"13.00-14.00", null},
            {"14.00-15.00", null},
            {"15.00-16.00", null},
            {"16.00-17.00", null},
            {"17.00-18.00", null},
            {"18.00-19.00", null},
        };

       


        public static List<Table> table_list = new List<Table>();

        public Table(int id, string location, int seats_count)
        {
            this.id = id;
            this.location = location;
            this.seats_count = seats_count;
        }

        static void creating_table()
        {
            int id;
            if (table_list.Count == 0) { id = 1; }
            else { id = table_list[^1].id + 1; }
            Console.WriteLine($"Стол будет иметь id = {id}");

            Console.WriteLine("Введите насположение стола: 1 - у окна, 2 - у прохода, 3 - у выхода, 4 - в глубине");

            string table_location = "";

            while (true)
            {
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        table_location = "у окна";
                        break;
                    case "2":
                        table_location = "у прохода";
                        break;
                    case "3":
                        table_location = "у выхода";
                        break;
                    case "4":
                        table_location = "в глубине";
                        break;
                    default:
                        Console.WriteLine("Неверный выбор");
                        continue;
                }
                break;
            }

            Console.WriteLine("Введите количество мест");
            int number_of_seats = MainProgram.checking_int_number();


            Table newTable = new Table(id, table_location, number_of_seats);
            table_list.Add(newTable);

        }

        static void changing_table_information()
        {
            if (table_list.Count == 0)
            {
                Console.WriteLine("Нет созданных столов");
                return;
            }

            Console.WriteLine("Введите id стола, информацию которого хотите изменить");
            int id = MainProgram.checking_int_number();

            Table found_table = null;
            foreach (Table table in table_list)
            {
                if (id == table.id)
                {
                    found_table = table;
                    break;
                }
            }
            if (found_table == null)
            {
                Console.WriteLine("Ошибка, нет стола с таким id");
                return;
            }


            bool active_reservation = false;
            foreach (var time_slot in found_table.timetable)
            {
                if (time_slot.Value != null)
                {
                    active_reservation = true;
                    break;
                }
            }

            if (active_reservation)
            {
                Console.WriteLine("Ошибка! Нельзя изменять стол с активными бронями.");
                Console.WriteLine("Сначала отмените все брони на этот стол.");
                return;
            }

            Console.WriteLine($"ID:============ {found_table.id}");
            Console.WriteLine($"Расположение:====== {found_table.location}");
            Console.WriteLine($"Количество мест:==== {found_table.seats_count}");

            Console.WriteLine("\nКакую информацию желаете изменить? \nid - 1, \nРасположение - 2, \nКоличество мест - 3");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Введите новый id: ");
                    int new_id = MainProgram.checking_int_number();
                    bool check_table_id = true;
                    foreach (Table table in table_list)
                    {
                        if (new_id == table.id)
                        {
                            check_table_id = false;
                            Console.WriteLine($"Стол с id - {new_id}, уже существует. Ошибка!");
                        }
                    }
                    if (check_table_id == true)
                    {
                        found_table.id = new_id;   
                    }
                    break;
                case "2":
                    Console.WriteLine("Введите насположение стола: 1 - у окна, 2 - у прохода, 3 - у выхода, 4 - в глубине");
                    string table_location_choice = "";
                    while (true)
                    {
                        table_location_choice = Console.ReadLine();

                        switch (table_location_choice)
                        {
                            case "1":
                                found_table.location = "у окна";
                                break;
                            case "2":
                                found_table.location = "у прохода";
                                break;
                            case "3":
                                found_table.location = "у выхода";
                                break;
                            case "4":
                                found_table.location = "в глубине";
                                break;
                            default:
                                Console.WriteLine("Неверный выбор");
                                continue;
                        }
                        break;
                    }
                    break;
                case "3":
                    Console.WriteLine("Введите новое количество мест: ");
                    int new_seats_count = MainProgram.checking_int_number();
                    found_table.seats_count = new_seats_count;
                    break;
                default:
                    Console.WriteLine("Неверный выбор");
                    Console.ReadKey();
                    break;
            }

            Console.WriteLine("Обновлённая информация о столе");
            Console.WriteLine($"ID:================= {found_table.id}");
            Console.WriteLine($"Расположение:======= {found_table.location}");
            Console.WriteLine($"Количество мест:==== {found_table.seats_count}");
        }

        static void output_information_about_table()
        {
            if (table_list.Count == 0)
            {
                Console.WriteLine("Нет созданных столов");
                return;
            }

            Console.WriteLine("Введите id стола, информацию о котором хотите вывести");
            int id = MainProgram.checking_int_number();

            Table found_table = null;

            foreach (Table table in table_list)
            {
                if (id == table.id)
                {
                    found_table = table;
                    break;
                }
            }

            if (found_table == null)
            {
                Console.WriteLine("Нет стола с таким id!");
                return;
            }

            Console.WriteLine($"ID:================= {found_table.id}");
            Console.WriteLine($"Расположение:======= {found_table.location}");
            Console.WriteLine($"Количество мест:==== {found_table.seats_count}");
            Console.WriteLine("Расписание:");

            foreach (var time_slot in found_table.timetable)
            {
                string status;
                if (time_slot.Value == null)
                {
                    status = "Свободно";
                }
                else
                {
                    string[] reservation_data = time_slot.Value.Split(';');
                    string res_id = reservation_data[0];

                    Reservation reservation = Reservation.reservation_list.Find(r => r.id.ToString() == res_id);

                    if (reservation != null)
                    {
                        string reservation_end = reservation.end_reservation;
                        string slot_start = time_slot.Key.Split('-')[0];

                        if (slot_start == reservation_end)
                        {
                            status = "Буфер (уборка)";
                        }
                        else
                        {
                            status = $"Занято: {reservation.name}";
                        }
                    }
                    else
                    {
                        status = "Занято (бронь не найдена)";
                    }
                }
                Console.WriteLine($"  {time_slot.Key}: {status}");
            }
        }

        public static void search_table_for_reservation_with_filter()
        {
            if (table_list.Count == 0)
            {
                Console.WriteLine("Нет созданных столов");
                return;
            }

            string filter_table_location = "";

            Console.WriteLine("Введите фильтры:");
            Console.WriteLine("Введите насположение стола: 1 - у окна, 2 - у прохода, 3 - у выхода, 4 - в глубине");

            while (true)
            {
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        filter_table_location = "у окна";
                        break;
                    case "2":
                        filter_table_location = "у прохода";
                        break;
                    case "3":
                        filter_table_location = "у выхода";
                        break;
                    case "4":
                        filter_table_location = "в глубине";
                        break;
                    default:
                        Console.WriteLine("Неверный выбор");
                        continue;
                }
                break;
            }

            Console.Write("Введите количество мест:");
            int filter_seats_count = MainProgram.checking_int_number();

            int start_hour = 0;
            int end_hour = 0;

            Console.WriteLine("Время брони с: \nВведите целое число от 9 до 18");
            start_hour = MainProgram.checking_start_hour();

            Console.WriteLine($"Время брони до: \nВведите целое число от {start_hour + 1} до 19");
            end_hour = MainProgram.checking_end_hour(start_hour);

            bool found_any_table = false;



            foreach (Table table in table_list)
            {

                if (filter_table_location == table.location && filter_seats_count == table.seats_count)
                {
                    string real_hour = "";
                    bool table_free = true;
                    for (int hour = start_hour; hour < end_hour + 1; hour++)
                    {
                        real_hour = hour.ToString() + ".00-" + (hour + 1).ToString() + ".00";
                        if (table.timetable[real_hour] != null)
                        {
                            table_free = false;
                            break;
                        }
                    }

                    if (table_free)
                    {
                        found_any_table = true;
                        Console.WriteLine($"ID:================= {table.id}");
                        Console.WriteLine($"Расположение:======= {table.location}");
                        Console.WriteLine($"Количество мест:==== {table.seats_count}");
                        Console.WriteLine("Расписание:");
                        foreach (var time_slot in table.timetable)
                        {
                            string status;
                            if (time_slot.Value == null)
                            {
                                status = "Свободно";
                            }
                            else
                            {
                                status = "Занято: " + time_slot.Value;
                            }
                            Console.WriteLine($"  {time_slot.Key}: {status}");
                        }
                    }
                }
            }


            if (!found_any_table)
            {
                Console.WriteLine("К сожалению столов по данным фильтрам нет");
            }
        }

        public static Table choose_table_for_reservation_with_filter(int start_hour, int end_hour)
        {
            

            Console.Write("Введите количество мест:");
            int filter_seats_count = MainProgram.checking_int_number();

            bool found_any_table = false;
            List<Table> available_tables = new List<Table>();

            foreach (Table table in table_list)
            {
                if (filter_seats_count <= table.seats_count)
                {
                    string real_hour = "";
                    bool table_free = true;
                    for (int hour = start_hour; hour < end_hour + 1; hour++)
                    {
                        real_hour = hour.ToString() + ".00-" + (hour + 1).ToString() + ".00";
                        if (table.timetable[real_hour] != null)
                        {
                            table_free = false;
                            break;
                        }
                    }

                    if (table_free)
                    {
                        found_any_table = true;
                        available_tables.Add(table);
                        Console.WriteLine($"\nID:================= {table.id}");
                        Console.WriteLine($"Расположение:======= {table.location}");
                        Console.WriteLine($"Количество мест:==== {table.seats_count}");
                    }
                }
            }

            if (!found_any_table)
            {
                Console.WriteLine("К сожалению столов по данным фильтрам нет");
                return null;
            }

            while (true)
            {
                Console.WriteLine("\nВыберите ID стола для бронирования:");
                int chosen_table_id = MainProgram.checking_int_number();

                Table selected_table = available_tables.Find(t => t.id == chosen_table_id);
                if (selected_table != null)
                {
                    return selected_table;
                }
                else
                {

                    Console.WriteLine("Ошибка! Стол с таким ID не найден в списке доступных.");
                }
            }
            
        }
    }
}


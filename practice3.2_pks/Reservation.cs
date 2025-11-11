using System;
namespace practice3._2_pks
{
	public class Reservation
	{
        public static void reservation_manager()
        {
            while (true)
            {
                Console.WriteLine("\n=== УПРАВЛЕНИЕ БРОНЯМИ ===");
                Console.WriteLine("1 - Добавить бронь");
                Console.WriteLine("2 - Изненение брони");
                Console.WriteLine("3 - Отмена брони");
                Console.WriteLine("4 - Вернуться в главное меню");
                Console.WriteLine("5 - Вывод перечня всех бронирований");
                Console.WriteLine("6 - Поиск информации о бронировании");
                Console.WriteLine("7 - Выход");

                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        create_reservation();
                        break;
                    case "2":
                        changing_reservation_information();
                        break;
                    case "3":
                        cancel_reservation();
                        break;
                    case "4":
                        return;
                    case "5":
                        output_all_reservation();
                        break;
                    case "6":
                        search_reservation();
                        break;
                    case "7":
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        continue;
                }
            }
        }


        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string start_reservation { get; set; }
        public string end_reservation { get; set; }
        public string comment { get; set; }
        public int table_id { get; set; }

        public static List<Reservation> reservation_list = new List<Reservation>();

        public Reservation(int id, string name, string phone, string start_reservation, string end_reservation, string comment, int table_id)
        {
            this.id = id;
            this.name = name;
            this.phone = phone;
            this.start_reservation = start_reservation;
            this.end_reservation = end_reservation;
            this.comment = comment;
            this.table_id = table_id;
        }

        public static void create_reservation()
        {
            string start_hour_real = "";
            string end_hour_real = "";
            string start_hour = "";
            string end_hour = "";

            Table selected_table = null;

            Console.WriteLine("Для бронирования столика придёстя внести информацию.");

            Console.WriteLine("На какое количество часов желаете забронивароть столик? \n1 - один час \n2 - более одного часа");
            int res_table_id = 0;
            string need_hour = "";

            string chose = Console.ReadLine();
            if (chose == "1")
            {
                bool table_found = false;

                Console.WriteLine("На какое время желаете заброниваровать столик? \nВведите целое число от 9 до 18");
                start_hour = Console.ReadLine();
                end_hour = (int.Parse(start_hour) + 1).ToString();
                string hour_real = start_hour + ".00-" + (int.Parse(start_hour) + 1).ToString() + ".00";


                Console.WriteLine($"Ищем на время: {hour_real}"); // +-отладка

                foreach (Table table in Table.table_list)
                {
                    if (table.timetable.ContainsKey(hour_real) && table.timetable[hour_real] == null)
                    {
                        Console.WriteLine($"Стол {table.id} свободен в {hour_real}"); // отладка
                        Console.WriteLine($"Стол найден и забронирован за вами.");
                        res_table_id = table.id;
                        need_hour = hour_real;
                        selected_table = table;
                        table_found = true;
                        break;
                    }
                }
                if (!table_found)
                {
                    Console.WriteLine("Извините, не найдет столик под ваше время");
                    return;
                }
            }
            else if (chose == "2")
            {
                bool table_found = false;

                Console.WriteLine("На c какого времини желаете заброниваровать столик? \nВведите целое число от 9 до 18");
                start_hour = Console.ReadLine();
                start_hour_real = start_hour + ".00";

                Console.WriteLine("До какого времини желаете заброниваровать столик? \nВведите целое число от 9 до 19");
                end_hour = Console.ReadLine();
                end_hour_real = end_hour + ".00";

                Console.WriteLine($"Ищем на время: {start_hour_real}-{end_hour_real}"); // +-отладка


                foreach (Table table in Table.table_list)
                {
                    bool table_free = true;

                    foreach (var slot in table.timetable)
                    {
                        string slot_start = slot.Key.Split('-')[0];
                        string slot_end = slot.Key.Split('-')[1];

                        bool slot_in_range = (CompareTime(slot_start, start_hour_real) >= 0 && CompareTime(slot_end, end_hour_real) <= 0);

                        if (slot_in_range && slot.Value != null)
                        {
                            table_free = false;
                            break;
                        }

                    }
                    if (table_free)
                    {
                        //Console.WriteLine($"{table.id} - id подходящего стола");
                        Console.WriteLine($"Стол {table.id} свободен в {start_hour_real}-{end_hour_real}"); // отладка
                        Console.WriteLine($"Стол найден и забронирован за вами.");
                        need_hour = $"{start_hour_real} до {end_hour_real}";
                        res_table_id = table.id;
                        selected_table = table;
                        table_found = true;
                        break;
                    }
                }
                if (!table_found)
                {
                    Console.WriteLine("Извините, не найдет столик под ваше время");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Ошибка ввода");
            }


            int id;
            if (reservation_list.Count == 0) { id = 1; }
            else { id = reservation_list[^1].id + 1; }
            Console.WriteLine($"Ваш id будет {id}"); // отладка

            Console.WriteLine("Введите ваше имя: ");
            string name = Console.ReadLine();

            Console.WriteLine("Введите номер телефона: ");
            string phone = Console.ReadLine();


            Console.WriteLine("Введите свой комментарий или оставте поле пустым: ");
            string comment = Console.ReadLine();

            Reservation newReservation = new Reservation(id, name, phone, start_hour_real, end_hour_real, comment, res_table_id);
            reservation_list.Add(newReservation);


            for (int hour = int.Parse(start_hour); hour < int.Parse(end_hour); hour++)
            {
                string slot = $"{hour}.00-{hour + 1}.00";
                selected_table.timetable[slot] = $"{newReservation.id};{newReservation.name};{newReservation.phone};{newReservation.comment}";
                Console.WriteLine("Данные успешно добавлены"); // отдалка
            }


        }
        public static int CompareTime(string time1, string time2)
        {
            double t1 = double.Parse(time1.Replace(".", ""));
            double t2 = double.Parse(time2.Replace(".", ""));
            return t1.CompareTo(t2);
        }

        public static void changing_reservation_information()
        {
            if (reservation_list.Count == 0)
            {
                Console.WriteLine("Активных бронирований нет.");
                return;
            }

            string name = "";
            string four_num_phone = "";

            Console.WriteLine("Введите ваше имя");
            name = Console.ReadLine();

            Console.WriteLine("Введите 4 цифры вашего номера");
            four_num_phone = Console.ReadLine();


            foreach (Reservation reservation in reservation_list)
            {
                string res_phone = reservation.phone.Substring(reservation.phone.Length - 4);
                Table found_table = null;
                foreach (Table table in Table.table_list)
                {
                    if (reservation.name == name && res_phone.EndsWith(four_num_phone))
                    {
                        found_table = table;
                        break;
                    }
                }
                Table current_table = found_table;
                if (current_table == null) continue;

                List<string> all_time_slots = new List<string>();

                foreach (var time_slot in current_table.timetable)
                {
                    if (time_slot.Value != null && time_slot.Value.StartsWith($"{reservation.id};") && reservation.name == name && res_phone.EndsWith(four_num_phone))
                    {
                        all_time_slots.Add(time_slot.Key);
                    }
                }
                Console.WriteLine($"\n=== БРОНИРОВАНИЕ ===");
                Console.WriteLine($"ID брони: {reservation.id}");
                Console.WriteLine($"Стол: {reservation.table_id}");
                Console.WriteLine($"Имя: {reservation.name}");
                Console.WriteLine($"Телефон: {reservation.phone}");
                Console.WriteLine($"Комментарий: {reservation.comment}");
                Console.WriteLine($"Время брони: {string.Join("; ", all_time_slots)}");
                Console.WriteLine("=====================");


                while (true)
                {
                    Console.WriteLine("\nКакую информацию желаете изменить? \nИмя - 1, \nТелефон - 2, \nКомментарий - 3 \nВремя - 4 \nВыйти из редактора - 5");
                    while (true)
                    {
                        string choice = Console.ReadLine();
                        switch (choice)
                        {
                            case "1":
                                Console.Write("Введите новое имя: ");
                                string new_name = Console.ReadLine();

                                reservation.name = new_name;

                                foreach (string slot in all_time_slots)
                                {
                                    if (current_table.timetable.ContainsKey(slot))
                                    {
                                        current_table.timetable[slot] = $"{reservation.id};{new_name};{reservation.phone};{reservation.comment}";
                                    }
                                }

                                Console.WriteLine("Имя успешно изменено!");
                                break;
                            case "2":
                                Console.Write("Введите новый телефон: ");
                                string new_phone = Console.ReadLine();

                                reservation.phone = new_phone;

                                foreach (string slot in all_time_slots)
                                {
                                    if (current_table.timetable.ContainsKey(slot))
                                    {
                                        current_table.timetable[slot] = $"{reservation.id};{reservation.name};{new_phone};{reservation.comment}";
                                    }
                                }

                                Console.WriteLine("Телефон успешно изменен!");
                                break;
                            case "3":
                                Console.Write("Введите новый комментарий: ");
                                string new_comment = Console.ReadLine();

                                reservation.comment = new_comment;

                                foreach (string slot in all_time_slots)
                                {
                                    if (current_table.timetable.ContainsKey(slot))
                                    {
                                        current_table.timetable[slot] = $"{reservation.id};{reservation.name};{reservation.phone};{new_comment}";
                                    }
                                }

                                Console.WriteLine("Комментарий успешно изменен!");
                                break;
                            case "4":
                                Console.WriteLine($"Время брони: {string.Join("; ", all_time_slots)}");
                                foreach (string slot in all_time_slots)
                                {
                                    if (current_table.timetable.ContainsKey(slot))
                                    {
                                        current_table.timetable[slot] = null;
                                    }
                                }

                                string start_hour_real = "";
                                string end_hour_real = "";
                                string start_hour = "";
                                string end_hour = "";
                                int res_table_id = 0;
                                string need_hour = "";
                                Table selected_table = null;
                                bool table_found = false;

                                Console.WriteLine("На c какого времини желаете заброниваровать столик? \nВведите целое число от 9 до 18");
                                start_hour = Console.ReadLine();
                                start_hour_real = start_hour + ".00";

                                Console.WriteLine("До какого времини желаете заброниваровать столик? \nВведите целое число от 9 до 18");
                                end_hour = Console.ReadLine();
                                end_hour_real = end_hour + ".00";

                                Console.WriteLine($"Ищем на время: {start_hour_real}-{end_hour_real}"); // +-отладка


                                foreach (Table table in Table.table_list)
                                {
                                    bool table_free = true;

                                    foreach (var slot in table.timetable)
                                    {
                                        string slot_start = slot.Key.Split('-')[0];
                                        string slot_end = slot.Key.Split('-')[1];

                                        bool slot_in_range = (CompareTime(slot_start, start_hour_real) >= 0 && CompareTime(slot_end, end_hour_real) <= 0);

                                        if (slot_in_range && slot.Value != null)
                                        {
                                            table_free = false;
                                            break;
                                        }

                                    }
                                    if (table_free)
                                    {
                                        //Console.WriteLine($"{table.id} - id подходящего стола");
                                        Console.WriteLine($"Стол {table.id} свободен в {start_hour_real}-{end_hour_real}"); // отладка
                                        Console.WriteLine($"Стол найден и забронирован за вами.");
                                        need_hour = $"{start_hour_real} до {end_hour_real}";
                                        res_table_id = table.id;
                                        selected_table = table;
                                        table_found = true;
                                        break;
                                    }
                                }
                                if (!table_found)
                                {
                                    Console.WriteLine("Извините, не найдет столик под ваше время");

                                    return;
                                }

                                for (int hour = int.Parse(start_hour); hour < int.Parse(end_hour); hour++)
                                {
                                    string slot = $"{hour}.00-{hour + 1}.00";
                                    selected_table.timetable[slot] = $"{reservation.id};{reservation.name};{reservation.phone};{reservation.comment}";
                                    Console.WriteLine("Данные успешно добавлены"); // отдалка
                                }

                                break;
                            case "5":
                                Console.WriteLine($"\n=== БРОНИРОВАНИЕ ПОСЛЕ ИЗМЕНЕНИЙ ===");
                                Console.WriteLine($"ID брони: {reservation.id}");
                                Console.WriteLine($"Стол: {reservation.table_id}");
                                Console.WriteLine($"Имя: {reservation.name}");
                                Console.WriteLine($"Телефон: {reservation.phone}");
                                Console.WriteLine($"Комментарий: {reservation.comment}");
                                Console.WriteLine($"Время брони: {string.Join("; ", all_time_slots)}");
                                Console.WriteLine("=====================");
                                return;
                            default:
                                Console.WriteLine("Неверный выбор");
                                Console.ReadKey();
                                break;
                        }
                        break;
                    }
                }

            }
            Console.WriteLine("Стол не найден!");

        }

        public static void cancel_reservation()
        {
            if (reservation_list.Count == 0)
            {
                Console.WriteLine("Активных бронирований нет.");
                return;
            }

            string name = "";
            string four_num_phone = "";
            int id_to_remove = 0;

            Console.WriteLine("Введите ваше имя");
            name = Console.ReadLine();

            Console.WriteLine("Введите 4 цифры вашего номера");
            four_num_phone = Console.ReadLine();

            foreach (Reservation reservation in reservation_list)
            {
                string res_phone = reservation.phone.Substring(reservation.phone.Length - 4);
                Table found_table = null;
                foreach (Table table in Table.table_list)
                {
                    if (table.id == reservation.table_id)
                    {
                        found_table = table;
                        break;
                    }
                }
                Table current_table = found_table;
                if (current_table == null) continue;

                List<string> all_time_slots = new List<string>();

                foreach (var time_slot in current_table.timetable)
                {
                    if (time_slot.Value != null && time_slot.Value.StartsWith($"{reservation.id};") && reservation.name == name && res_phone.EndsWith(four_num_phone))
                    {
                        current_table.timetable[time_slot.Key] = null;
                    }

                }

                Console.WriteLine($"\n=== БРОНИРОВАНИЕ ===");
                Console.WriteLine($"ID брони: {reservation.id}");
                Console.WriteLine($"Стол: {reservation.table_id}");
                Console.WriteLine($"Имя: {reservation.name}");
                Console.WriteLine($"Телефон: {reservation.phone}");
                Console.WriteLine($"Комментарий: {reservation.comment}");
                Console.WriteLine($"Время брони: {string.Join("; ", all_time_slots)}");
                Console.WriteLine("=====================");

                id_to_remove = reservation.id;
                Console.WriteLine("Всё время стерто");
                break;
            }
            if (id_to_remove != 0)
            {
                for (int i = 0; i < reservation_list.Count(); i++)
                {
                    if (reservation_list[i].id == id_to_remove)
                    {
                        reservation_list.Remove(reservation_list[i]);
                        Console.WriteLine("Бронь полностью удалена!"); // отдалка
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Бронь не найдена!");
            }
        }

        public static void output_all_reservation()
        {
            if (reservation_list.Count == 0)
            {
                Console.WriteLine("Активных бронирований нет.");
                return;
            }

            foreach (Reservation reservation in reservation_list)
            {
                Table found_table = null;
                foreach (Table table in Table.table_list)
                {
                    if (table.id == reservation.table_id)
                    {
                        found_table = table;
                        break;
                    }
                }
                Table current_table = found_table;
                if (current_table == null) continue;

                List<string> all_time_slots = new List<string>();

                foreach (var time_slot in current_table.timetable)
                {
                    if (time_slot.Value != null && time_slot.Value.StartsWith($"{reservation.id};"))
                    {
                        all_time_slots.Add(time_slot.Key);
                    }
                }

                Console.WriteLine($"\n=== БРОНИРОВАНИЕ ===");
                Console.WriteLine($"ID брони: {reservation.id}");
                Console.WriteLine($"Стол: {reservation.table_id}");
                Console.WriteLine($"Имя: {reservation.name}");
                Console.WriteLine($"Телефон: {reservation.phone}");
                Console.WriteLine($"Комментарий: {reservation.comment}");
                Console.WriteLine($"Время брони: {string.Join("; ", all_time_slots)}");
                Console.WriteLine("=====================");
            }
        }

        public static void search_reservation()
        {
            if (reservation_list.Count == 0)
            {
                Console.WriteLine("Активных бронирований нет.");
                return;
            }
            string name = "";
            string four_num_phone = "";

            Console.WriteLine("Введите ваше имя");
            name = Console.ReadLine();

            Console.WriteLine("Введите 4 цифры вашего номера");
            four_num_phone = Console.ReadLine();



            foreach (Reservation reservation in reservation_list)
            {
                Table found_table = null;
                string res_phone = reservation.phone.Substring(reservation.phone.Length - 4);
                foreach (Table table in Table.table_list)
                {
                    if (reservation.name == name && res_phone.EndsWith(four_num_phone))
                    {
                        found_table = table;
                        break;
                    }
                }
                Table current_table = found_table;
                if (current_table == null) continue;

                List<string> all_time_slots = new List<string>();

                foreach (var time_slot in current_table.timetable)
                {
                    if (time_slot.Value != null && time_slot.Value.StartsWith($"{reservation.id};") && reservation.name == name && res_phone.EndsWith(four_num_phone))
                    {
                        all_time_slots.Add(time_slot.Key);
                    }
                }
                Console.WriteLine($"\n=== БРОНИРОВАНИЕ ===");
                Console.WriteLine($"ID брони: {reservation.id}");
                Console.WriteLine($"Стол: {reservation.table_id}");
                Console.WriteLine($"Имя: {reservation.name}");
                Console.WriteLine($"Телефон: {reservation.phone}");
                Console.WriteLine($"Комментарий: {reservation.comment}");
                Console.WriteLine($"Время брони: {string.Join("; ", all_time_slots)}");
                Console.WriteLine("=====================");
                break;
            }
        }
    }
}


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
                        break;
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
            if (Table.table_list.Count == 0)
            {
                Console.WriteLine("Нет созданных столов");
                return;
            }

            Console.WriteLine("Для бронирования столика придётся внести информацию.");


            int start_hour = 0;
            int end_hour = 0;

            Console.WriteLine("Время брони с: \nВведите целое число от 9 до 17");
            start_hour = MainProgram.checking_start_hour();

            Console.WriteLine($"Время брони до: \nВведите целое число от {start_hour + 1} до 18");
            end_hour = MainProgram.checking_end_hour(start_hour);


            Table selected_table = Table.choose_table_for_reservation_with_filter(start_hour, end_hour);
            if (selected_table == null)
            {
                return;
            }


            string start_hour_real = start_hour + ".00";
            string end_hour_real = end_hour + ".00";

            int res_table_id = selected_table.id;
            string need_hour = $"{start_hour_real} до {end_hour_real}";



            int id;
            if (reservation_list.Count == 0) { id = 1; }
            else { id = reservation_list[^1].id + 1; }
            Console.WriteLine($"Ваш id будет {id}"); // отладка

            Console.WriteLine("Введите ваше имя: ");
            string name = MainProgram.checking_name();

            Console.WriteLine("Введите номер телефона: ");
            string phone = MainProgram.checking_phone();


            Console.WriteLine("Введите свой комментарий или оставте поле пустым: ");
            string comment = Console.ReadLine();

            Reservation newReservation = new Reservation(id, name, phone, start_hour_real, end_hour_real, comment, res_table_id);
            reservation_list.Add(newReservation);


            for (int hour = start_hour; hour < end_hour + 1; hour++)
            {
                string slot = $"{hour}.00-{hour + 1}.00";
                selected_table.timetable[slot] = $"{newReservation.id};{newReservation.name};{newReservation.phone};{newReservation.comment}";
                Console.WriteLine("Данные успешно добавлены"); // отладка
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
            string phone = "";

            Console.Write("Введите ваше имя: ");
            name = MainProgram.checking_name();

            Console.Write("Введите номера телефона: ");
            phone = MainProgram.checking_phone();

            List<Reservation> user_reservations = new List<Reservation>();

            foreach (Reservation res in reservation_list)
            {
                if (res.name == name && res.phone == phone)
                {
                    user_reservations.Add(res);
                }
            }

            if (user_reservations.Count == 0)
            {
                Console.WriteLine("Брони не найдены!");
                return;
            }

            Console.WriteLine("\n=== ВАШИ БРОНИ ===");
            for (int i = 0; i < user_reservations.Count; i++)
            {
                Console.WriteLine($"{i + 1}. ID: {user_reservations[i].id}, Стол: {user_reservations[i].table_id}, Время: {user_reservations[i].start_reservation}-{user_reservations[i].end_reservation}");
            }

            Console.WriteLine($"\nВыберите бронь для изменения (1-{user_reservations.Count}):");
            int choice_index = MainProgram.checking_int_number() - 1;

            if (choice_index < 0 || choice_index >= user_reservations.Count)
            {
                Console.WriteLine("Неверный выбор!");
                return;
            }

            Reservation reservation = user_reservations[choice_index];

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
                            string new_name = MainProgram.checking_name();

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
                            string new_phone = MainProgram.checking_phone();

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

                            foreach (var time_slot in current_table.timetable.ToList())
                            {
                                if (time_slot.Value != null && time_slot.Value.StartsWith($"{reservation.id};"))
                                {
                                    current_table.timetable[time_slot.Key] = null;
                                }
                            }

                            int start_hour = 0;
                            int end_hour = 0;

                            Console.WriteLine("На c какого времени желаете забронировать столик? \nВведите целое число от 9 до 18");
                            start_hour = MainProgram.checking_start_hour();
                            string start_hour_real = start_hour + ".00";

                            Console.WriteLine($"До какого времени желаете забронировать столик? \nВведите целое число от {start_hour + 1} до 18");
                            end_hour = MainProgram.checking_end_hour(start_hour);
                            string end_hour_real = end_hour + ".00";

                            Console.WriteLine($"Ищем на время: {start_hour_real}-{end_hour_real}");

                            Table selected_table = null;
                            bool table_found = false;

                            foreach (Table table in Table.table_list)
                            {
                                bool table_free = true;

                                for (int hour = start_hour; hour < end_hour; hour++)
                                {
                                    string slot = $"{hour}.00-{hour + 1}.00";

                                    if (table.timetable.ContainsKey(slot) && table.timetable[slot] != null)
                                    {
                                        table_free = false;
                                        break;
                                    }
                                }

                                if (table_free && end_hour <= 18)
                                {
                                    string buffer_slot = $"{end_hour}.00-{end_hour + 1}.00";
                                    if (table.timetable.ContainsKey(buffer_slot) && table.timetable[buffer_slot] != null)
                                    {
                                        table_free = false;
                                    }
                                }

                                if (table_free)
                                {
                                    Console.WriteLine($"Стол {table.id} свободен в {start_hour_real}-{end_hour_real}");
                                    Console.WriteLine($"Стол найден и забронирован за вами.");
                                    selected_table = table;
                                    table_found = true;
                                    break;
                                }
                            }

                            if (!table_found)
                            {
                                Console.WriteLine("Извините, не найден столик под ваше время");
                                foreach (string slot in all_time_slots)
                                {
                                    current_table.timetable[slot] = $"{reservation.id};{reservation.name};{reservation.phone};{reservation.comment}";
                                }
                                return;
                            }

                            reservation.table_id = selected_table.id;
                            reservation.start_reservation = start_hour_real;
                            reservation.end_reservation = end_hour_real;

                            for (int hour = start_hour; hour < end_hour; hour++)
                            {
                                string slot = $"{hour}.00-{hour + 1}.00";
                                selected_table.timetable[slot] = $"{reservation.id};{reservation.name};{reservation.phone};{reservation.comment}";
                            }

                            if (end_hour <= 18)
                            {
                                string buffer_slot = $"{end_hour}.00-{end_hour + 1}.00";
                                selected_table.timetable[buffer_slot] = $"{reservation.id};{reservation.name};{reservation.phone};{reservation.comment}";
                            }

                            Console.WriteLine("Время брони успешно изменено!");
                            break;
                        case "5":
                            return;
                        default:
                            Console.WriteLine("Неверный выбор");
                            break;
                    }
                    break;
                }
            }
        }

        public static void cancel_reservation()
        {
            if (reservation_list.Count == 0)
            {
                Console.WriteLine("Активных бронирований нет.");
                return;
            }

            string name = "";
            string phone = "";

            Console.Write("Введите ваше имя: ");
            name = MainProgram.checking_name();

            Console.Write("Введите номера телефона: ");
            phone = MainProgram.checking_phone();

            List<Reservation> user_reservations = new List<Reservation>();

            foreach (Reservation res in reservation_list)
            {
                if (res.name == name && res.phone == phone)
                {
                    user_reservations.Add(res);
                }
            }

            if (user_reservations.Count == 0)
            {
                Console.WriteLine("Брони не найдены!");
                return;
            }

            Console.WriteLine("\n=== ВАШИ БРОНИ ===");
            for (int i = 0; i < user_reservations.Count; i++)
            {
                Console.WriteLine($"{i + 1}. ID: {user_reservations[i].id}, Стол: {user_reservations[i].table_id}, Время: {user_reservations[i].start_reservation}-{user_reservations[i].end_reservation}");
            }

            Console.WriteLine($"\nВыберите бронь для изменения (1-{user_reservations.Count}):");
            int choice_index = MainProgram.checking_int_number() - 1;

            if (choice_index < 0 || choice_index >= user_reservations.Count)
            {
                Console.WriteLine("Неверный выбор!");
                return;
            }

            Reservation reservation = user_reservations[choice_index];

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


            int id_to_remove = 0;

            foreach (var time_slot in current_table.timetable)
            {
                if (time_slot.Value != null && time_slot.Value.StartsWith($"{reservation.id};"))
                {
                    current_table.timetable[time_slot.Key] = null;
                }
            }

            Console.WriteLine("Всё время стерто");
            id_to_remove = reservation.id;

            foreach (var time_slot in current_table.timetable)
            {
                if (time_slot.Value != null && time_slot.Value.StartsWith($"{reservation.id};") && reservation.name == name && reservation.phone == phone)
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
            Console.WriteLine("Всё время стерто"); // отдалка

            if (id_to_remove != 0)
            {
                for (int i = 0; i < reservation_list.Count(); i++)
                {
                    if (reservation_list[i].id == id_to_remove)
                    {
                        reservation_list.Remove(reservation_list[i]);
                        Console.WriteLine("Бронь полностью удалена!");
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

            Console.Write("Введите ваше имя: ");
            name = MainProgram.checking_name();

            Console.Write("Введите последние 4 цифры вашего номера телефона: ");
            four_num_phone = MainProgram.checking_phone_last4number();



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
            }
        }
    }
}


namespace APICeomedAplicacoes.Uteis
{
    public static class ExtensionsDateTime
    {
        public static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new Exception("A Data Final deve ser maior que a Data Inicial.");

            while (startDate <= endDate)
            {
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }

        public static string CalculaIdade(this DateTime Bday)
        {
            string ta = "0 Ano ", tm = "0 Mês ", td = "0 Dia";
            int Years, Months, Days;
            DateTime Cday = DateTime.Now;

            if ((Cday.Year - Bday.Year) > 0 ||
               (((Cday.Year - Bday.Year) == 0) &&
               ((Bday.Month < Cday.Month) ||
               ((Bday.Month == Cday.Month) &&
               (Bday.Day <= Cday.Day)))))
            {

                int DaysInBdayMonth = DateTime.DaysInMonth(Bday.Year, Bday.Month);
                int DaysRemain = Cday.Day + (DaysInBdayMonth - Bday.Day);

                if (Cday.Month > Bday.Month)
                {
                    Years = Cday.Year - Bday.Year;
                    Months = Cday.Month - (Bday.Month + 1) + Math.Abs(DaysRemain / DaysInBdayMonth);
                    Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                }
                else if (Cday.Month == Bday.Month)
                {
                    if (Cday.Day >= Bday.Day)
                    {
                        Years = Cday.Year - Bday.Year;
                        Months = 0;
                        Days = Cday.Day - Bday.Day;
                    }
                    else
                    {
                        Years = (Cday.Year - 1) - Bday.Year;
                        Months = 11;
                        Days = DateTime.DaysInMonth(Bday.Year, Bday.Month) - (Bday.Day - Cday.Day);

                    }
                }
                else
                {
                    Years = (Cday.Year - 1) - Bday.Year;
                    Months = Cday.Month + (11 - Bday.Month) + Math.Abs(DaysRemain / DaysInBdayMonth);
                    Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                }
            }
            else
            {
                return "Data de nascimento inválida.";
            }

            if (Years > 1)
                ta = Years + " Anos ";
            else if (Years == 1)
                ta = Years + " Ano ";
            if (Months > 1)
                tm = Months + " Meses ";
            else if (Months == 1)
                tm = Months + " Mês ";
            if (Days > 1)
                td = Days + " Dias ";
            else if (Days == 1)
                td = Days + " Dia ";

            return ta + tm + td;
        }

        public static string ToExtence(this DateTime date)
        {
            string mes = "";

            switch (date.Month)
            {
                case 1:
                    {
                        mes = "Janeiro";
                        break;
                    }
                case 2:
                    {
                        mes = "Fevereiro";
                        break;
                    }
                case 3:
                    {
                        mes = "Março";
                        break;
                    }
                case 4:
                    {
                        mes = "Abril";
                        break;
                    }
                case 5:
                    {
                        mes = "Maio";
                        break;
                    }
                case 6:
                    {
                        mes = "Junho";
                        break;
                    }
                case 7:
                    {
                        mes = "Julho";
                        break;
                    }
                case 8:
                    {
                        mes = "Agosto";
                        break;
                    }
                case 9:
                    {
                        mes = "Setembro";
                        break;
                    }
                case 10:
                    {
                        mes = "Outubro";
                        break;
                    }
                case 11:
                    {
                        mes = "Novembro";
                        break;
                    }
                case 12:
                    {
                        mes = "Dezembro";
                        break;
                    }







            }

            return date.Day + " de " + mes + " de " + date.Year;
        }

        public static string ToIsoString(this DateTime date)
        {
            //2011-10-05T14:48:00.000Z

            string ano, mes, dia, hora, minuto, segundo = "";

            ano = date.Year.ToString();
            mes = date.Month <= 9 ? "0" + date.Month.ToString() : date.Month.ToString();
            dia = date.Day <= 9 ? "0" + date.Day.ToString() : date.Day.ToString();
            //hora = date.Hour <= 9 ? "0" + date.Hour.ToString() : date.Hour.ToString();
            //minuto = date.Minute <= 9 ? "0" + date.Minute.ToString() : date.Minute.ToString();
            //segundo = date.Second <= 9 ? "0" + date.Second.ToString() : date.Second.ToString();

            string iso = String.Format("{0}-{1}-{2}T03:00:00.000Z", ano, mes, dia);

            return iso;
        }

        public static string ToHourMinuteTimeString(this TimeSpan time)
        {
            string hour = "", minute = "";

            if (time.Hours < 10)
                hour = "0";
            if (time.Minutes < 10)
                minute = "0";

            hour += time.Hours.ToString();
            minute += time.Minutes.ToString();
            return hour + ":" + minute;
        }
    }
}

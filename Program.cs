using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CommonLispToCS {
    class Program {
        static void Main(string[] args) {

            //Console.ReadLine();
            //return;

            List<string> righe = new List<string>();
            righe.AddRange(File.ReadAllLines("prova.txt"));
            for (int i = 0; i < righe.Count; i++)
                righe[i] = righe[i].Trim();


            //righe.ForEach(s => s = s.Trim());
            //List<string> righeF = righe.FindAll(s => s.Trim().StartsWith("(def") || s.Trim().StartsWith(";; TYPE"));
            //Array.ForEach(righe, Console.WriteLine);
            //List<string> righeF = righe.FindAll(s =>  (!s.StartsWith(";")) && s.Contains(";"));
            //File.WriteAllLines("con commenti.txt", righeF);
            //righeF = righe.FindAll(s => s.Trim().StartsWith("(def"));
            // File.WriteAllLines("def.txt", righeF);



            //*
            Constant cnst;
            //Macro mcr; //tutto a mano
            Fun fun;
            List<string> ris = new List<string>();
            while (righe.Count > 0) {
                string temp = righe[0];
                if (temp.StartsWith(";;;; Section: ")) {
                    //region                    
                    if (ris.Count > 0)
                        ris.Add("#endregion");
                    ris.Add($"#region \"{temp.Replace(";;;; Section: ", "")}\"");
                    righe.RemoveAt(0);
                } else if (temp.StartsWith("(defconstant ")) {
                    //Costant
                    cnst = new Constant(temp.Replace("(defconstant ", "").Replace("-", "_"));
                    //ris.Add(cnst.Name);
                    Console.WriteLine(cnst.Name);
                    righe.RemoveAt(0);
                    List<IToken> tokens = null;
                    bool isEndBody = false;
                    while (!isEndBody) {
                        temp = righe[0];
                        if (temp.StartsWith(";; TYPE ")) {
                            cnst.AddComment(temp.Replace(";; ", ""));//per sicurezza
                            cnst.Type = temp.Replace(";; TYPE ", "");
                        } else if (temp.StartsWith(";; ")) {
                            cnst.AddComment(temp.Replace(";; ", ""));
                        } else {
                            //Body
                            if (tokens == null) {
                                tokens = new List<IToken>();
                                tokens.Add(new OpenToken(0, "("));
                            }
                            /*es "(fixed-from-julian (julian-date (ce 622) july 16)))"                                     
                                 "(- mayan-epoch
                                   (mayan-haab-ordinal (mayan-haab-date 18 8))))"
                                "(- aztec-correlation
                                  (aztec-tonalpohualli-ordinal
                                  (aztec-tonalpohualli-date 1 5))))" */
                            isEndBody = TakeToken(tokens, temp);
                            if (isEndBody) {
                                //ris.Add("ok");
                                //ris.AddRange();
                                AnalizeTokens(tokens);
                                if ((tokens[0] as ListToken).Items.Count == 1) {
                                    IToken body = (tokens[0] as ListToken).Items[0];
                                    if (body is ElementToken || body is IValueTkn) {
                                        cnst.Value = body.Text;
                                        cnst.AddComment(body.Comment);
                                    }
                                    ris.AddRange(cnst.ToCS());
                                }
                            }
                        }
                        righe.RemoveAt(0);
                    }
                } else if (temp.StartsWith("(defun ")) {
                    //Fun
                    //(defun tibetan-leap-day? (t-year t-month t-day)
                    temp = temp.Replace("(defun ", "").Replace("-", "_");
                    //tibetan_leap_day? (t_year t_month t_day)
                    fun = new Fun();
                    int spazio = temp.IndexOf(' ');
                    fun.Name = temp.Remove(spazio);
                    //ris.Add(fun.Name);
                    Console.WriteLine(fun.Name);
                    temp = temp.Substring(spazio + 1);
                    //(t-year t-month t-day)                        
                    fun.Params = StringtoParams(temp);
                    righe.RemoveAt(0);
                    string stringa = "";
                    List<IToken> tokens = null;
                    bool isEndBody = false;
                    while (!isEndBody) {
                        temp = righe[0];
                        if (temp.StartsWith(";; TYPE ")) {
                            fun.AddComment(temp.Replace(";; ", ""));//per sicurezza
                            stringa += temp.Replace(";; TYPE", "").Replace("-", "_");
                            if (stringa.Contains("->")) {
                                /* es: 
                                moment -> hindu_lunar_day
                                {1_60} -> {0_10}
                                fixed_date -> 1_9
                                (hebrew_year hebrew_month hebrew_day) -> hebrew_date */
                                string ret;
                                fun.ParamsType = StringtoParamsType(stringa.Trim(), out ret);
                                fun.TypeReturn = ret;
                            }
                        } else if (temp.StartsWith(";; ")) {
                            fun.AddComment(temp.Replace(";; ", ""));
                        } else {
                            //Body
                            if (tokens == null) {
                                tokens = new List<IToken>();
                                tokens.Add(new OpenToken(0, "("));
                            }
                            isEndBody = TakeToken(tokens, temp);
                            if (isEndBody) {
                                //ris.Add("ok");
                                //ris.AddRange();
                                AnalizeTokens(tokens);
                                if ((tokens[0] as ListToken).Items.Count == 1) {
                                    IToken body = (tokens[0] as ListToken).Items[0];
                                    if (body is ElementToken || body is IValueTkn) {
                                        fun.Body = body.Text;
                                        fun.AddComment(body.Comment);
                                    }
                                    ris.AddRange(fun.ToCS());
                                }                                
                            }
                        }
                        righe.RemoveAt(0);
                    }
                } /*else if (temp.StartsWith("(defmacro ")) {
                    //Macro (li ho dovuti eliminare per fare a mano, colpa del Do
                    temp = temp.Replace("(defmacro ", "").Replace("-", "_");
                    mcr = new Macro();
                    int spazio = temp.IndexOf(' ');
                    mcr.Name = temp.Remove(spazio);
                    //ris.Add(mcr.Name);
                    Console.WriteLine(mcr.Name);
                    temp = temp.Substring(spazio + 1);
                    mcr.Params = StringtoParams(temp);
                    righe.RemoveAt(0);
                    List<IToken> tokens = null;
                    bool isEndBody = false;
                    while (!isEndBody) {
                        temp = righe[0];
                        //niente ";; TYPE "
                        if (temp.StartsWith(";; ")) {
                            mcr.AddComment(temp.Replace(";; ", ""));
                        } else {
                            //Body
                            if (tokens == null) {
                                tokens = new List<IToken>();
                                tokens.Add(new OpenToken(0, "("));
                            }
                            isEndBody = TakeToken(tokens, temp);
                            if (isEndBody) {
                                //ris.Add("ok");
                                //ris.AddRange(AnalizeTokens(tokens));
                            }
                        }
                        righe.RemoveAt(0);
                    }
                }*/

            }
            ris.Add("#endregion");

            //ris.ForEach(Console.WriteLine);//*/
            //File.WriteAllLines("def analisi.txt", ris);
            //File.WriteAllLines("altre funzioni.txt", ris);
            File.WriteAllLines("Convertito.txt", ris);

            Console.WriteLine("Convertito!");
            Console.ReadLine();
        }

        private static List<string> StringtoParams(string testo) {
            List<string> ris = new List<string>();
            if (testo.StartsWith("(") && testo.EndsWith(")")) {
                testo = testo.Substring(1, testo.Length - 2);
                ris.AddRange(testo.Split(' '));
                return ris;
            }
            return null;
        }
        private static List<string> StringtoParamsType(string testo, out string typeFun) {
            List<string> ris = new List<string>();
            /* es: 
            moment -> hindu-lunar-day
            {1-60} -> {0-10}
            fixed-date -> 1-9
            (hebrew-year hebrew-month hebrew-day) -> hebrew-date  */
            if (!testo.Contains("->")) {
                typeFun = "dynamic";
                testo = testo.Trim();
            } else {
                typeFun = testo.Split(new string[] { "->" }, StringSplitOptions.None)[1].Trim();
                testo = testo.Split(new string[] { "->" }, StringSplitOptions.None)[0].Trim();
            }

            /* es: 
            moment
            {1-60}
            fixed-date
            (hebrew-year hebrew-month hebrew-day)    */
            if (testo.Contains("(") ^ testo.Contains(")")) {
                throw new Exception("Ha solo '(' oppure ')'");
            } else if (testo.Contains("(")) {
                testo = testo.Substring(1, testo.Length - 2);
                ris.AddRange(testo.Split(' '));
            } else {
                ris.Add(testo);
            }
            return ris;
        }
        /// <summary>
        /// True se ha trovato CloseTaken con Ident==0, altrimenti False
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="testo"></param>
        /// <returns></returns>
        private static bool TakeToken(List<IToken> tokens, string testo) {
            //Attenzione fallisce in "variabile; commento" va aggiunto a mano uno spazio "variabile ; commento"
            /*es "(fixed-from-julian (julian-date (ce 622) july 16)))"                                     
                "(- mayan-epoch
                (mayan-haab-ordinal (mayan-haab-date 18 8))))"
            "(- aztec-correlation
                (aztec-tonalpohualli-ordinal
                (aztec-tonalpohualli-date 1 5))))"*/
            int identCurrent = tokens.Last().Ident;
            if (tokens.Last() is OpenToken)
                identCurrent++;
            int current = 0;
            while (current < testo.Length) {
                switch (testo[current]) {
                    case '(':
                        OpenToken open = new OpenToken(identCurrent++, testo.Substring(0, current + 1));
                        tokens.Add(open);
                        testo = testo.Substring(current + 1);
                        break;
                    case ')':
                        if (current > 0) {
                            string elText = testo.Substring(0, current);
                            IToken element;
                            if (elText.Contains(".")) {
                                //double
                                element = new ValueTkn(identCurrent, elText.Replace("L", "E").Replace("D", "E"));
                            } else if (elText.Contains("/") && (!elText.StartsWith("/"))) {
                                //rational
                                element = new ValueTkn(identCurrent, "new Rational(" + elText.Replace("/", ",") + ")");
                            } else {
                                //.Replace("-", "_")
                                element = new ElementToken(identCurrent, ReplaceWithUnderscore(elText));
                            }
                            tokens.Add(element);
                            testo = testo.Substring(current);
                            current = 0;
                        }
                        CloseToken close = new CloseToken(--identCurrent, ")");
                        tokens.Add(close);
                        testo = testo.Substring(current + 1);
                        current = 0;
                        break;
                    case ' ':
                        if (current > 0) {
                            string elText = testo.Substring(0, current);
                            IToken element;
                            if (elText.Contains(".")) {
                                //double
                                element = new ValueTkn(identCurrent, elText.Replace("L", "E").Replace("D", "E"));
                            } else if (elText.Contains("/") && (!elText.StartsWith("/"))) {
                                //rational
                                element = new ValueTkn(identCurrent, "new Rational(" + elText.Replace("/", ",") + ")");
                            } else {
                                element = new ElementToken(identCurrent, ReplaceWithUnderscore(elText));
                            }
                            tokens.Add(element);
                        }
                        testo = testo.Substring(current + 1);
                        current = 0;
                        break;
                    case ';':
                        while (testo.StartsWith(";") || testo.StartsWith(" "))
                            testo = testo.Substring(1);
                        tokens.Last().Comment += " " + testo;
                        //current = testo.Length;//NO per l'if successivo
                        testo = "";
                        current = 0;
                        break;
                    default:
                        current++;
                        break;
                }

            }
            if (!String.IsNullOrWhiteSpace(testo)) {
                //capita quando finisce con solo testo
                IToken element;
                if (testo.Contains(".")) {
                    //double
                    element = new ValueTkn(identCurrent, testo.Replace("L", "E").Replace("D", "E"));
                } else if (testo.Contains("/") && (!testo.StartsWith("/"))) {
                    //rational
                    element = new ValueTkn(identCurrent, "new Rational(" + testo.Replace("/", ",") + ")");
                } else {
                    element = new ElementToken(identCurrent, ReplaceWithUnderscore(testo));
                }
                tokens.Add(element);
            }
            if (tokens.Last().Ident == 0) {
                return true;
            }
            return false;
        }

        private static string[] ArrayConstant = { "adar",
            "adarii",
            "april",
            "armenian_epoch",
            "arya_jovian_period",
            "arya_lunar_day",
            "arya_lunar_month",
            "arya_solar_month",
            "arya_solar_year",
            "august",
            "autumn",
            "av",
            "ayyam_i_ha",
            "aztec_correlation",
            "aztec_tonalpohualli_correlation",
            "aztec_xihuitl_correlation",
            "bahai_epoch",
            "bali_epoch",
            "bogus",
            "chinese_day_name_epoch",
            "chinese_epoch",
            "chinese_month_name_epoch",
            "coptic_epoch",
            "december",
            "egyptian_epoch",
            "elul",
            "ethiopic_epoch",
            "evening",
            "false",
            "february",
            "first_quarter",
            "french_epoch",
            "friday",
            "full",
            "gregorian_epoch",
            "haifa",
            "hebrew_epoch",
            "hebrew_location",
            "hindu_anomalistic_month",
            "hindu_anomalistic_year",
            "hindu_creation",
            "hindu_epoch",
            "hindu_location",
            "hindu_lunar_era",
            "hindu_sidereal_month",
            "hindu_sidereal_year",
            "hindu_solar_era",
            "hindu_synodic_month",
            "ides",
            "islamic_epoch",
            "islamic_location",
            "iyyar",
            "j2000",
            "jaffa",
            "january",
            "jd_epoch",
            "jerusalem",
            "julian_epoch",
            "july",
            "june",
            "kalends",
            "kislev",
            "last_quarter",
            "march",
            "marheshvan",
            "may",
            "mayan_epoch",
            "mayan_haab_epoch",
            "mayan_tzolkin_epoch",
            "mean_sidereal_year",
            "mean_synodic_month",
            "mean_tropical_year",
            "mecca",
            "mjd_epoch",
            "monday",
            "morning",
            "new",
            "nisan",
            "nones",
            "november",
            "october",
            "paris",
            "persian_epoch",
            "saturday",
            "september",
            "shevat",
            "sidereal_start",
            "sivan",
            "spring",
            "summer",
            "sunday",
            "tammuz",
            "tehran",
            "tevet",
            "thursday",
            "tibetan_epoch",
            "tishri",
            "true",
            "tuesday",
            "ujjain",
            "wednesday",
            "winter",
            "year_rome_founded"};
        private static string[] ArrayMacro = { "binary_search",
            "final",
            "invert_angular",
            "next",
            //"sigma", // gestito a parte
            "sum" };
        private static string[] ArrayFunc = {"aberration",
            "adda_season_in_gregorian",
            "advent",
            "alt_fixed_from_gregorian",
            "alt_fixed_from_observational_hebrew",
            "alt_fixed_from_observational_islamic",
            "alt_gregorian_from_fixed",
            "alt_gregorian_year_from_fixed",
            "alt_hindu_sunrise",
            "alt_observational_hebrew_from_fixed",
            "alt_observational_islamic_from_fixed",
            "alt_orthodox_easter",
            "amod",
            "angle",
            "angle_from_degrees",
            "apparent_from_local",
            "approx_moment_of_depression",
            "arccos_degrees",
            "arcsin_degrees",
            "arctan_degrees",
            "arithmetic_french_from_fixed",
            "arithmetic_french_leap_year?",
            "arithmetic_persian_from_fixed",
            "arithmetic_persian_leap_year?",
            "arithmetic_persian_year_from_fixed",
            "armenian_date",
            "armenian_from_fixed",
            "asr",
            "astro_hindu_calendar_year",
            "astro_hindu_lunar_from_fixed",
            "astro_hindu_solar_from_fixed",
            "astro_hindu_sunset",
            "astro_lunar_day_from_moment",
            "astronomical_easter",
            "auc_year_from_julian_year",
            "ayanamsha",
            "aztec_tonalpohualli_date",
            "aztec_tonalpohualli_from_fixed",
            "aztec_tonalpohualli_name",
            "aztec_tonalpohualli_number",
            "aztec_tonalpohualli_on_or_before",
            "aztec_tonalpohualli_ordinal",
            "aztec_xihuitl_date",
            "aztec_xihuitl_day",
            "aztec_xihuitl_from_fixed",
            "aztec_xihuitl_month",
            "aztec_xihuitl_on_or_before",
            "aztec_xihuitl_ordinal",
            "aztec_xihuitl_tonalpohualli_on_or_before",
            "aztec_xiuhmolpilli_designation",
            "aztec_xiuhmolpilli_from_fixed",
            "aztec_xiuhmolpilli_name",
            "aztec_xiuhmolpilli_number",
            "bahai_cycle",
            "bahai_date",
            "bahai_day",
            "bahai_from_fixed",
            "bahai_major",
            "bahai_month",
            "bahai_new_year",
            "bahai_year",
            "bali_asatawara",
            "bali_asatawara_from_fixed",
            "bali_caturwara",
            "bali_caturwara_from_fixed",
            "bali_dasawara",
            "bali_dasawara_from_fixed",
            "bali_day_from_fixed",
            "bali_dwiwara",
            "bali_dwiwara_from_fixed",
            "bali_luang",
            "bali_luang_from_fixed",
            "balinese_date",
            "bali_on_or_before",
            "bali_pancawara",
            "bali_pancawara_from_fixed",
            "bali_pawukon_from_fixed",
            "bali_sadwara",
            "bali_sadwara_from_fixed",
            "bali_sangawara",
            "bali_sangawara_from_fixed",
            "bali_saptawara",
            "bali_saptawara_from_fixed",
            "bali_triwara",
            "bali_triwara_from_fixed",
            "bali_week_from_fixed",
            "bce",
            "birkath_ha_hama",
            "birkath_ha_hama_old",
            "ce",
            "chinese_age",
            "chinese_branch",
            "chinese_cycle",
            "chinese_date",
            "chinese_day",
            "chinese_day_name",
            "chinese_day_name_on_or_before",
            "chinese_from_fixed",
            "chinese_leap",
            "chinese_location",
            "chinese_month",
            "chinese_month_name",
            "chinese_name",
            "chinese_name_difference",
            "chinese_new_moon_before",
            "chinese_new_moon_on_or_after",
            "chinese_new_year",
            "chinese_new_year_in_sui",
            "chinese_new_year_on_or_before",
            "chinese_no_major_solar_term?",
            "chinese_prior_leap_month?",
            "chinese_sexagesimal_name",
            "chinese_solar_longitude_on_or_after",
            "chinese_stem",
            "chinese_winter_solstice_on_or_before",
            "chinese_year",
            "chinese_year_marriage_augury",
            "chinese_year_name",
            "christmas",
            "classical_passover_eve",
            "clock_from_moment",
            "coptic_christmas",
            "coptic_date",
            "coptic_from_fixed",
            "coptic_in_gregorian",
            "coptic_leap_year?",
            "cosine_degrees",
            "current_major_solar_term",
            "current_minor_solar_term",
            "dawn",
            "daylight_saving_end",
            "daylight_saving_start",
            "day_number",
            "day_of_week_from_fixed",
            "days_in_hebrew_year",
            "days_remaining",
            "daytime_temporal_hour",
            "declination",
            "deg",
            "degrees",
            "degrees_from_radians",
            "degrees_minutes_seconds",
            "direction",
            "diwali",
            "dragon_festival",
            "dusk",
            "dynamical_from_universal",
            "easter",
            "eastern_orthodox_christmas",
            "egyptian_date",
            "egyptian_from_fixed",
            "election_day",
            "elevation",
            "end",
            "ephemeris_correction",
            "epiphany",
            "equation_of_time",
            "estimate_prior_solar_longitude",
            "ethiopic_date",
            "ethiopic_from_fixed",
            "feast_of_ridvan",
            "first_kday",
            "fixed_from_arithmetic_french",
            "fixed_from_arithmetic_persian",
            "fixed_from_armenian",
            "fixed_from_astro_hindu_lunar",
            "fixed_from_astro_hindu_solar",
            "fixed_from_bahai",
            "fixed_from_chinese",
            "fixed_from_coptic",
            "fixed_from_egyptian",
            "fixed_from_ethiopic",
            "fixed_from_french",
            "fixed_from_future_bahai",
            "fixed_from_gregorian",
            "fixed_from_hebrew",
            "fixed_from_hindu_fullmoon",
            "fixed_from_hindu_lunar",
            "fixed_from_hindu_solar",
            "fixed_from_islamic",
            "fixed_from_iso",
            "fixed_from_jd",
            "fixed_from_julian",
            "fixed_from_list_of_moments",
            "fixed_from_mayan_long_count",
            "fixed_from_mjd",
            "fixed_from_moment",
            "fixed_from_observational_hebrew",
            "fixed_from_observational_islamic",
            "fixed_from_old_hindu_lunar",
            "fixed_from_old_hindu_solar",
            "fixed_from_persian",
            "fixed_from_roman",
            "fixed_from_tibetan",
            "french_date",
            "french_from_fixed",
            "french_new_year_on_or_before",
            "future_bahai_from_fixed",
            "future_bahai_new_year_on_or_before",
            "gregorian_date",
            "gregorian_date_difference",
            "gregorian_from_fixed",
            "gregorian_leap_year?",
            "gregorian_new_year",
            "gregorian_year_end",
            "gregorian_year_from_fixed",
            "gregorian_year_range",
            "hebrew_birthday",
            "hebrew_birthday_in_gregorian",
            "hebrew_calendar_elapsed_days",
            "hebrew_date",
            "hebrew_from_fixed",
            "hebrew_leap_year?",
            "hebrew_new_year",
            "hebrew_sabbatical_year?",
            "hebrew_year_length_correction",
            "hindu_arcsin",
            "hindu_ascensional_difference",
            "hindu_calendar_year",
            "hindu_daily_motion",
            "hindu_date_occur",
            "hindu_day_count",
            "hindu_equation_of_time",
            "hindu_expunged?",
            "hindu_fullmoon_from_fixed",
            "hindu_lunar_date",
            "hindu_lunar_day",
            "hindu_lunar_day_at_or_after",
            "hindu_lunar_day_from_moment",
            "hindu_lunar_event",
            "hindu_lunar_from_fixed",
            "hindu_lunar_holiday",
            "hindu_lunar_leap_day",
            "hindu_lunar_leap_month",
            "hindu_lunar_longitude",
            "hindu_lunar_month",
            "hindu_lunar_new_year",
            "hindu_lunar_on_or_before?",
            "hindu_lunar_phase",
            "hindu_lunar_station",
            "hindu_lunar_year",
            "hindu_mean_position",
            "hindu_new_moon_before",
            "hindu_rising_sign",
            "hindu_sine",
            "hindu_sine_table",
            "hindu_solar_date",
            "hindu_solar_from_fixed",
            "hindu_solar_longitude",
            "hindu_solar_longitude_at_or_after",
            "hindu_solar_sidereal_difference",
            "hindu_sundial_time",
            "hindu_sunrise",
            "hindu_sunset",
            "hindu_tithi_occur",
            "hindu_tropical_longitude",
            "hindu_tropical_longitude_old",
            "hindu_true_position",
            "hindu_zodiac",
            "hour",
            "hr",
            "ides_of_month",
            "independence_day",
            "in_range?",
            "interval",
            "islamic_date",
            "islamic_from_fixed",
            "islamic_in_gregorian",
            "islamic_leap_year?",
            "iso_date",
            "iso_day",
            "iso_from_fixed",
            "iso_long_year?",
            "iso_week",
            "iso_year",
            "japanese_location",
            "jd_from_fixed",
            "jd_from_moment",
            "jewish_dusk",
            "jewish_morning_end",
            "jewish_sabbath_ends",
            "jovian_year",
            "julian_centuries",
            "julian_date",
            "julian_from_fixed",
            "julian_in_gregorian",
            "julian_leap_year?",
            "julian_season_in_gregorian",
            "julian_year_from_auc_year",
            "kajeng_keliwon",
            "karana",
            "kday_after",
            "kday_before",
            "kday_nearest",
            "kday_on_or_after",
            "kday_on_or_before",
            "korean_location",
            "korean_year",
            "labor_day",
            "last_day_of_hebrew_month",
            "last_kday",
            "last_month_of_hebrew_year",
            "latitude",
            "list_range",
            "local_from_apparent",
            "local_from_standard",
            "local_from_universal",
            "location",
            "longitude",
            "long_marheshvan?",
            "losar",
            "lunar_altitude",
            "lunar_anomaly",
            "lunar_diameter",
            "lunar_distance",
            "lunar_elongation",
            "lunar_latitude",
            "lunar_longitude",
            "lunar_node",
            "lunar_parallax",
            "lunar_phase",
            "lunar_phase_at_or_after",
            "lunar_phase_at_or_before",
            "major_solar_term_on_or_after",
            "mawlid_an_nabi",
            "mayan_baktun",
            "mayan_calendar_round_on_or_before",
            "mayan_haab_date",
            "mayan_haab_day",
            "mayan_haab_from_fixed",
            "mayan_haab_month",
            "mayan_haab_on_or_before",
            "mayan_haab_ordinal",
            "mayan_katun",
            "mayan_kin",
            "mayan_long_count_date",
            "mayan_long_count_from_fixed",
            "mayan_tun",
            "mayan_tzolkin_date",
            "mayan_tzolkin_from_fixed",
            "mayan_tzolkin_name",
            "mayan_tzolkin_number",
            "mayan_tzolkin_on_or_before",
            "mayan_tzolkin_ordinal",
            "mayan_uinal",
            "mayan_year_bearer_from_fixed",
            "mean_lunar_longitude",
            "memorial_day",
            "mesha_samkranti",
            "midday",
            "midday_in_tehran",
            "midnight",
            "midnight_in_china",
            "midnight_in_paris",
            "minor_solar_term_on_or_after",
            "minute",
            "mjd_from_fixed",
            "molad",
            "moment_from_jd",
            "moment_of_depression",
            "month_length",
            "moon_node",
            "moonrise",
            "mt",
            "multiples_in_range",
            "naw_ruz",
            "new_moon_at_or_after",
            "new_moon_before",
            "nighttime_temporal_hour",
            "nones_of_month",
            "nth_kday",
            "nth_new_moon",
            "nutation",
            "obliquity",
            "observational_hebrew_from_fixed",
            "observational_hebrew_new_year",
            "observational_islamic_from_fixed",
            "observed_lunar_altitude",
            "old_hindu_lunar_date",
            "old_hindu_lunar_day",
            "old_hindu_lunar_from_fixed",
            "old_hindu_lunar_leap",
            "old_hindu_lunar_leap_year?",
            "old_hindu_lunar_month",
            "old_hindu_lunar_year",
            "old_hindu_solar_from_fixed",
            "omer",
            "orthodox_easter",
            "passover",
            "pentecost",
            "persian_date",
            "persian_from_fixed",
            "persian_new_year_on_or_before",
            "phasis_on_or_after",
            "phasis_on_or_before",
            "poly",
            "positions_in_range",
            "possible_hebrew_days",
            "possible_hebrew_days_old",
            "precession",
            "purim",
            "qing_ming",
            "quotient",
            "radians_from_degrees",
            "rama",
            "rd",
            "refraction",
            "right_ascension",
            "roman_count",
            "roman_date",
            "roman_event",
            "roman_from_fixed",
            "roman_leap",
            "roman_month",
            "roman_year",
            "sacred_wednesdays",
            "sacred_wednesdays_in_range",
            "samuel_season_in_gregorian",
            "season_in_gregorian",
            "sec",
            "seconds",
            "secs",
            "sh_ela",
            "sh_ela_old",
            "shift_days",
            "shiva",
            "short_kislev?",
            "sidereal_from_moment",
            "sidereal_lunar_longitude",
            "sidereal_solar_longitude",
            "sidereal_zodiac",
            "sin_degrees",
            "sine_offset",
            "solar_anomaly",
            "solar_longitude",
            "solar_longitude_after",
            "standard_day",
            "standard_from_local",
            "standard_from_sundial",
            "standard_from_universal",
            "standard_month",
            "standard_year",
            "start",
            "sunrise",
            "sunset",
            "sunset_in_haifa",
            "ta_anit_esther",
            "tangent_degrees",
            "tibetan_date",
            "tibetan_day",
            "tibetan_from_fixed",
            "tibetan_leap_day",
            "tibetan_leap_day?",
            "tibetan_leap_month",
            "tibetan_leap_month?",
            "tibetan_month",
            "tibetan_moon_equation",
            "tibetan_new_year",
            "tibetan_sun_equation",
            "tibetan_year",
            "time_from_clock",
            "time_from_moment",
            "time_of_day",
            "tishah_be_av",
            "topocentric_lunar_altitude",
            "tumpek",
            "universal_from_dynamical",
            "universal_from_local",
            "universal_from_standard",
            "unlucky_fridays_in_range",
            "vietnamese_location",
            "visible_crescent",
            "yahrzeit",
            "yahrzeit_in_gregorian",
            "yoga",
            "yom_ha_zikkaron",
            "yom_kippur",
            "zone",
            "zone_from_longitude" };

        private static string ReplaceWithUnderscore(string testo) {
            if (testo == "1-")
                return testo;
            else if (testo == "-")
                return testo;
            else if (testo.StartsWith("-"))
                return testo;
            else if (testo.Contains("-"))// si suppone che !StartWith
                return testo.Replace("-", "_");
            else// casomai
                return testo;
        }
        private static List<string> AnalizeTokens(List<IToken> tokens) {
            List<string> ris = new List<string>();
            while (tokens.Count > 1) {
                int idOpen = 0, idClose = 0;
                for (int i = 0; i < tokens.Count; i++) {
                    if (tokens[i] is CloseToken) {
                        idClose = i;
                        break;
                    }
                }
                for (int i = idClose; i >= 0; i--) {
                    if (tokens[i] is OpenToken) {
                        idOpen = i;
                        break;
                    }
                }
                if (tokens[idOpen].Ident != tokens[idClose].Ident)
                    throw new Exception("errore nella struttura agli ID: open " + idOpen + " e close " + idClose);
                ListToken list = new ListToken(tokens[idOpen].Ident);
                list.Open = tokens[idOpen].Text;
                if (!String.IsNullOrWhiteSpace(tokens[idOpen].Comment))
                    list.Comment = tokens[idOpen].Comment;
                if (!String.IsNullOrWhiteSpace(tokens[idClose].Comment)) {
                    list.Comment = String.IsNullOrWhiteSpace(list.Comment) ? tokens[idClose].Comment
                        : list.Comment + "\n" + tokens[idClose].Comment;
                }
                for (int i = idOpen + 1; i < idClose; i++) {
                    if (tokens[i].Ident != tokens[idOpen].Ident + 1)
                        throw new Exception("errore nella struttura agli ID: open " + idOpen + " e elem " + i);
                    list.Items.Add(tokens[i]);
                }

                // ris.Add(list.Items[0].Text + "|" + list.Items.Count);
                tokens.RemoveRange(idOpen, idClose - idOpen + 1);
                IToken tkn = ChangeToken(list);
                if (tkn is IValueTkn) {
                } else if (tkn is ElementToken) {
                } else {
                    //ris.Add(list.Items[0].Text + "|" + list.Items.Count);
                }
                tokens.Insert(idOpen, tkn);
            }
            return ris;
        }
        private static List<string> AnalizeTokens2(List<IToken> tokens) {
            List<string> ris = new List<string>();
            for (int i = 1; i < tokens.Count; i++) {
                if (tokens[i] is ElementToken) {
                    //if (tokens[i - 1] is OpenToken && tokens[i - 1].Text != "(")
                    //    ris.Add(tokens[i - 1].Text);
                    if (tokens[i].Text != "1-" && tokens[i].Text != "-" && !tokens[i].Text.StartsWith("-") && tokens[i].Text.Contains("-"))
                        ris.Add(tokens[i].Text + "|" + tokens[i].ToString());
                }
            }
            return ris;
        }

        private static IToken ChangeToken(ListToken list) {
            switch (list.Items[0].Text) {
                case "+":
                case "-":
                case "*":
                case "/":
                case "1+":
                case "1-":
                    return new OperationTkn(list);
                case "=":
                case "/=":
                case "<":
                case ">":
                case "<=":
                case ">=":
                case "and":
                case "or":
                case "not":
                case "equal":
                    return new ConditionTkn(list);
                case "list":
                case "nth":
                case "first":
                case "second":
                case "third":
                case "fourth":
                case "fifth":
                case "sixth":
                case "seventh":
                case "eighth":
                case "ninth":
                case "tenth":
                    return new ArrayTkn(list);
                case "if":
                case "cond":
                case "let":
                case "let*":
                    return new BlockTkn(list);
                case "sigma":
                    return new SigmaBlockTkn(list);
                case "gensym":
                case "mod":
                case "member"://funzione list
                case "append"://funzione list
                case "rest"://funzione list
                case "early_month?":
                //xxx da definire a mano "errata20130806.pdf" pag 13
                //case ",f"://funzione (macro) lambda
                //case "apply"://funzione (solo in macro)
                //case "function"://funzione (solo in macro)
                case "integerp"://isInteger
                    return new FunctionTkn(list);
                case "ceiling":
                    return new FunctionTkn(list, "Math.Ceiling");
                case "floor":
                    return new FunctionTkn(list, "Math.Floor");
                case "abs":
                    return new FunctionTkn(list, "Math.Abs");
                case "acos":
                    return new FunctionTkn(list, "Math.Acos");
                case "asin":
                    return new FunctionTkn(list, "Math.Asin");
                case "atan":
                    return new FunctionTkn(list, "Math.Atan");
                case "expt":
                    return new FunctionTkn(list, "Math.Pow");
                case "cos":
                    return new FunctionTkn(list, "Math.Cos");
                case "round":
                    return new FunctionTkn(list, "Math.Round");
                case "signum":
                    return new FunctionTkn(list, "Math.Sign");
                case "max":
                    return new FunctionTkn(list, "Math.Max");
                case "min":
                    return new FunctionTkn(list, "Math.Min");
                case "sin":
                    return new FunctionTkn(list, "Math.Sin");
                case "sqrt":
                    return new FunctionTkn(list, "Math.Sqrt");
                case "tan":
                    return new FunctionTkn(list, "Math.Tan");
                default:
                    if (ArrayMacro.Contains(list.Items[0].Text))
                        return new FunctionTkn(list);
                    else if (ArrayFunc.Contains(list.Items[0].Text))
                        return new FunctionTkn(list);
                    return list;
            }
        }
    }
}

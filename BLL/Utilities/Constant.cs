using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiCampusConcierge.BLL.Utilities
{
    public class Constant
    {
        public static String AppName = "MyCc";

        public static String OS_iOS = "iOS";
        public static String OS_Android = "Android";

        public static String CertificatesDevelopement = "D";
        public static String CertificatesProduction = "P";

        public static String CaseTypePAP = "PAP";
        public static String CaseTypeRSL = "RSL";

        public static String TerritoryRole_OAM = "OAM";
        public static String TerritoryRole_CAM = "CAM";

        public static String CycleType_Daily = "Daily";
        public static String CycleType_Weekly = "Weekly";

        public static String GoalType_Territory = "Territory";
        public static String GoalType_Region = "Region";
        public static String GoalType_Nation = "Nation";

        public static String CalendarType_445Calendar = "445Calendar";
        public static String CalendarType_CalendarMonth = "CalendarMonth";

        public static String FileOperationCode_Output = "112";

        public static DateTime SQLMinDate = new DateTime(1900, 1, 1);
        public static DateTime SQLMaxDate = new DateTime(2079, 6, 6);

        public static String FlagYes = "Y";
        public static String FlagNo = "N";

        public static String Active = "A";
        public static String DeActive = "D";

        public static String TimerCode = "101";
        public static int DefaultTimerInterval = 10;
        public static int DefaultLogsDay = 7;
        public static String DateTimeFormat = "dd-MMM-yyyy hh:mm:ss tt";
        public static int NoOfDaysInWeek = 5;

       
    }

}
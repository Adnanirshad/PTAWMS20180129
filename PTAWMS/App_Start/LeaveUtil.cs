using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
// using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using System.Data.SqlClient;

/// <summary>
/// Summary description for LeaveUtil
/// </summary>
/// 
namespace PTAWMS.App_Start
{
    public class LeaveUtil
    {
        static SQLDBAccess dbAccess = new SQLDBAccess();
        static string strQuery = "";
        static DataSet dsGrid = new DataSet();
        static SqlDataReader reader = null;

        public LeaveUtil()
        {
            dbAccess = new SQLDBAccess();
            CultureInfo.GetCultureInfo("en-GB");
            //
            // TODO: Add constructor logic here
            //
        }
        public static double GetCasualLeaveBalance(string EmpID, string JoiningDate, string AppointmentMode)
        {
            double TotalMonths = 0; double LeaveBalance = 0.0;

            //DateTime DateOfJoining = DateTime.ParseExact(JoiningDate, "dd/MM/yyyy", null);
            DateTime DateOfJoining = Convert.ToDateTime(JoiningDate);//, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (DateOfJoining.Year == DateTime.Now.Year)
            {
                TotalMonths = (12 - DateOfJoining.Month + 1);
                TotalMonths = ((AppointmentMode == "T" || AppointmentMode == "U") ? TotalMonths : TotalMonths = TotalMonths * 1.7);
            }
            // incase of current year joining
            if (TotalMonths <= 0)
            {
                TotalMonths = ((AppointmentMode == "T" || AppointmentMode == "U") ? 12 : 20);
            }

            strQuery = "SELECT isnull(SUM(DUELEAVE),0) AS DUELEAVE FROM HR_LeaveReq  with (nolock) WHERE EMPLOYEEID='" + EmpID + "' And LEAVETYPE = 'CASUAL' AND Status='Approved' And  Year(FROMDATE)='" + DateTime.Now.Year + "'";
            reader = dbAccess.FillDataReader(strQuery);

            if (reader != null && reader.Read())
            {
                LeaveBalance = ((reader[0] != null && !reader[0].ToString().Equals("")) ? TotalMonths - Double.Parse(reader[0].ToString()) : TotalMonths);
            }

            dbAccess.CloseDataReader(ref reader);
            return LeaveBalance;
        }
        public static string GetEmployeeJoiningDate(string EmpID)
        {
            string JoinigDate = "";
            strQuery = "SELECT Convert(varchar,DOJ,103) as ORGJOINDATE FROM ViewUserEmp with(nolock) WHERE EmpNo ='" + EmpID + "'";
            reader = dbAccess.FillDataReader(strQuery);
            if (reader != null && reader.Read())
            {
                JoinigDate = reader["ORGJOINDATE"].ToString();
            }
            dbAccess.CloseDataReader(ref reader);
            return JoinigDate;
        }
        public static double GetPendingEarnedLeaves(string EmpID)
        {
            double PendingLeaves = 0;
            strQuery = "SELECT isnull(SUM(DUELEAVE),0) as PENDING FROM HR_LEAVEREQ WHERE  LEAVETYPE IN (SELECT DESCR FROM HR_LEAVETYPE WHERE ISDUTY =1 AND ISEARNED=1) AND STATUS IN ('Recommended','Pending') and EMPLOYEEID = '" + EmpID + "'";
            reader = dbAccess.FillDataReader(strQuery);
            if (reader != null && reader.Read())
            {
                PendingLeaves = Convert.ToDouble(reader["PENDING"].ToString());
            }
            dbAccess.CloseDataReader(ref reader);
            return PendingLeaves;
        }
        public static void GetLeaveJoiningDate(string EmpID, ref string JoinigDate)
        {
            //strQuery = "SELECT to_char(ORGJOINDATE,'dd/mm/yyyy') as ORGJOINDATE FROM EMPLEAVEDATE WHERE EMP ='" + EmpID + "'";
            strQuery = "select DOJ as ORGJOINDATE from ViewUserEmp  with (nolock) where EmpStatus = 'Resigned' and EmpNo ='" + EmpID + "'";

            reader = dbAccess.FillDataReader(strQuery);
            if (reader != null && reader.Read())
            {
                JoinigDate = reader["ORGJOINDATE"].ToString();
            }
            dbAccess.CloseDataReader(ref reader);
        }
        public static double GetEarnedLeaves(string EmpID, String BPSScale, DateTime DutyFrom, DateTime DutyTo)
        {
            double LeaveEarned = 0;
            DateDifference Age = null;
            DateTime EnchasmentDate = DateTime.ParseExact("01/01/2007", "dd/MM/yyyy", null);
            DateTime Regulation2016 = DateTime.ParseExact("01/01/2030", "dd/MM/yyyy", null);
            // if duty period is before 2007 then multiple by 4
            if (DutyFrom < DutyTo)
            {
                if (DutyFrom <= EnchasmentDate)
                {
                    if (DutyTo <= EnchasmentDate)
                    {
                        Age = new DateDifference(DutyTo, DutyFrom);
                        LeaveEarned = (Age.Years * 48) + (Age.Months * 4) + (Age.Days > 15 ? 4 : 0);
                    }
                    else
                    {
                        Age = new DateDifference(EnchasmentDate.AddDays(-1), DutyFrom); //new DateTime((EnchasmentDate - DutyFrom).Ticks).AddDays(1);
                        LeaveEarned = (Age.Years) * 48 + (Age.Months) * 4 + (Age.Days > 15 ? 4 : 0);
                        Age = new DateDifference(DutyTo, EnchasmentDate); //new DateTime((DutyTo - EnchasmentDate).Ticks).AddDays(1);
                        double MultiplyFactor = 0.0;
                        MultiplyFactor = (BPSScale.Equals("21") || BPSScale.Equals("22") ? 3 : 2.5);
                        LeaveEarned += (Age.Years) * (MultiplyFactor * 12) + (Age.Months) * MultiplyFactor + (Age.Days > 15 ? MultiplyFactor : 0);
                    }
                }
                else
                {
                    if (DutyFrom <= Regulation2016)
                    {
                        if (DutyTo <= Regulation2016)
                        {

                            Age = new DateDifference(DutyTo, DutyFrom);
                            double MultiplyFactor = 0.0;
                            MultiplyFactor = (BPSScale.Equals("21") || BPSScale.Equals("22") ? 3 : 2.5);
                            LeaveEarned += (Age.Years) * (MultiplyFactor * 12) + (Age.Months) * MultiplyFactor + (Age.Days > 15 ? MultiplyFactor : 0);
                        }
                        else
                        {
                            Age = new DateDifference(Regulation2016.AddDays(-1), DutyFrom); //new DateTime((EnchasmentDate - DutyFrom).Ticks).AddDays(1);
                            LeaveEarned = (Age.Years * 30) + (Age.Months * 2.5) + (Age.Days > 15 ? 2.5 : 0);

                            Age = new DateDifference(DutyTo, Regulation2016);
                            LeaveEarned += (Age.Years) * 48 + (Age.Months) * 4 + (Age.Days > 15 ? 4 : 0);
                        }

                    }
                    else
                    {
                        Age = new DateDifference(DutyTo, DutyFrom);
                        LeaveEarned = (Age.Years) * 48 + (Age.Months) * 4 + (Age.Days > 15 ? 4 : 0);
                    }

                }
            }
            // if duty period is falls between
            return LeaveEarned;
        }


        public static DataSet GetCasualLeaveDataSet(String EmployeeID)
        {
            //strQuery = @"Select LEAVEID,TO_CHAR(APPDATE,'dd/mm/yyyy') as APPDATE,TO_CHAR(FROMDATE,'dd/mm/yyyy') as FROMDATE,TO_CHAR(TODATE,'dd/mm/yyyy') as TODATE,((TODATE - FROMDATE) + 1) as total,
            //            ( SELECT   SUM(DECODE( TO_CHAR(  (TO_DATE(FROMDATE))+ROWNUM-1, 'DY' ),'SUN',0,'SAT',0,1)   ) FROM ALL_OBJECTS AO 
            //                WHERE   ROWNUM <= TO_DATE(TODATE) - (TO_DATE(FROMDATE)-1) )DAYS 
            //            FROM EMPLEAVEREQ 
            //            WHERE
            //                LEAVETYPE = 'CASUAL' AND 
            //                Status='Approved' And  
            //                TO_CHAR(FROMDATE,'yyyy')='" + DateTime.Now.Year + @"' AND
            //                EMPLOYEEID='" + EmployeeID + @"'
            //            Order By  TO_DATE(FROMDATE,'DD/MM/YYYY') DESC";

            strQuery = @"Select LEAVEID,convert(varchar,APPDATE,103) as APPDATE,convert(varchar,FROMDATE,103) as FROMDATE,convert(varchar,TODATE,103) as TODATE,((TODATE - FROMDATE) + 1) as total
					,DUELEAVE
                    as DAYS
                    FROM HR_LEAVEREQ with (nolock)
                    WHERE
                        LEAVETYPE = 'CASUAL' AND 
                        Status='Approved' And  
                        year(FROMDATE)='" + DateTime.Now.Year + @"' AND
                        EMPLOYEEID='" + EmployeeID + @"'
                    Order By  convert(date,FROMDATE,103) DESC";
            return dbAccess.FillDataset(strQuery, "EMPLEAVEREQ");
        }
        public static DataSet GetELDataSet(string EmployeeID, String BPSScale, string JoiningDate, string MofAppointment)
        {

            strQuery = @"SELECT                    
                    HR_LEAVEREQ.EMPLOYEEID,                        
                    HR_LEAVEREQ.LEAVEID,
                    'N/A' AS DUTYFROM,                         
                    'N/A' AS DUTYTO,                        
                    '0' AS YEAR,                        
                    '0' AS MONTH,                        
                    '0' AS DAYS,                        
                    '0' AS LEAVESEARNED,                    
                    convert(varchar,HR_LEAVEREQ.APPDATE,103) AS APPDATE,                        
                    convert(varchar,HR_LEAVEREQ.FROMDATE,103) AS FROMDATE,                        
                    convert(varchar,HR_LEAVEREQ.TODATE,103) AS TODATE,                                                     
                    dbo.INITCAP(HR_LEAVEREQ.LEAVETYPE) AS LEAVETYPE,                    
                    HR_LEAVEREQ.DUELEAVE,                    
                    HR_LEAVETYPE.ISEARNED,                    
                    HR_LEAVETYPE.ISDUTY,                    
                    HR_LEAVEREQ.STATUS,                        
                    '0' AS BALANACE                     
                FROM                    
                    HR_LEAVETYPE with (nolock),HR_LEAVEREQ  with (nolock)                 
                WHERE                          
                    HR_LEAVETYPE.DESCR = HR_LEAVEREQ.LEAVETYPE AND                         
                    HR_LEAVEREQ.LEAVETYPE <> 'CASUAL' AND                         
                    UPPER(HR_LEAVEREQ.STATUS) = 'APPROVED' AND                          
                    HR_LEAVEREQ.EMPLOYEEID = '" + EmployeeID + @"'                     
                    ORDER BY convert(date,TODATE,103) ASC";


            dsGrid = dbAccess.FillDataset(strQuery, "EMPLEAVEREQ");


            DateTime DutyFrom = DateTime.ParseExact(JoiningDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                     DutyTo = System.DateTime.Now,
                     RunningDutyFrom = System.DateTime.Now,
                     RunningDutyTo = System.DateTime.Now,
                     NewRegulationDate = DateTime.ParseExact("30/09/2007", "dd/MM/yyyy", CultureInfo.InvariantCulture);// current date 

            DateDifference Age = null;


            string DutyPeriod = string.Empty, Year = String.Empty, Month = String.Empty, Days = String.Empty;
            double LeavesEarned = 0.0; double balance = 0.0;

            foreach (DataRow row in dsGrid.Tables["EMPLEAVEREQ"].Rows)
            {
                string LeaveType = row["LEAVETYPE"].ToString().ToUpper();
                DutyTo = DateTime.ParseExact(row["TODATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                RunningDutyFrom = DateTime.ParseExact(row["FROMDATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(-1);
                RunningDutyTo = (DutyTo < NewRegulationDate ? (LeaveType.Equals("LEAVE ENCASHMENT") ? DutyTo : RunningDutyFrom) : (Convert.ToInt32(row["ISDUTY"]) == 1 ? DutyTo : RunningDutyFrom));

                // update duty period for only duty leaves

                /************ in case of earned leaves or on duty leaves the duty period end at leave end date  *********/
                if (LeaveType.Equals("LEAVE ENCASHMENT") || LeaveType.Equals("DELETED"))
                    row["FROMDATE"] = row["TODATE"] = String.Empty;

                row["DUTYFROM"] = String.Format("{0:dd/MM/yyyy}", DutyFrom);
                RunningDutyTo = (RunningDutyTo > System.DateTime.Now ? System.DateTime.Now : RunningDutyTo);
                row["DUTYTO"] = String.Format("{0:dd/MM/yyyy}", (DutyFrom < RunningDutyTo ? RunningDutyTo : RunningDutyTo.AddDays(1)));
                // Calculating Duty Period

                LeavesEarned = LeaveUtil.GetEarnedLeaves(EmployeeID, BPSScale, DutyFrom, RunningDutyTo);
                row["LEAVESEARNED"] = LeavesEarned + (balance.Equals(0) ? "" : " + " + balance);  // displaying balance


                balance += LeavesEarned - (row["ISEARNED"].ToString().Equals("1") ? (LeaveType.Equals("HALF AVERAGE PAY") ? Convert.ToDouble(row["DUELEAVE"].ToString()) / 2 : (LeaveType.Equals("ABSENT") ? Convert.ToDouble(row["DUELEAVE"].ToString()) * 2 : Convert.ToDouble(row["DUELEAVE"].ToString()))) : 0);  // calculating new balance
                row["BALANACE"] = balance;

                // displaying Duty Period        
                if (DutyFrom < RunningDutyTo)
                {
                    if (RunningDutyTo > System.DateTime.Now)
                        RunningDutyTo = System.DateTime.Now;
                    Age = new DateDifference(RunningDutyTo, DutyFrom); //new DateTime((RunningDutyTo - DutyFrom).Ticks).AddDays(1);
                    Year = (Age.Years).ToString(); row["YEAR"] = Year;
                    Month = (Age.Months).ToString(); row["MONTH"] = Month;
                    Days = (Age.Days).ToString(); row["DAYS"] = Days;
                }

                DutyFrom = DutyTo.AddDays(1);
            }

            /************************ Creating Last Row and Calulating balance **********************/
            if (DutyFrom < DateTime.Now)
            {
                LeavesEarned = LeaveUtil.GetEarnedLeaves(EmployeeID, BPSScale, DutyFrom, System.DateTime.Now); //Getting last Earned Leaves
                string LastBalance = LeavesEarned + (balance.Equals(0) ? "" : " + " + balance); // Displaying Balance
                balance += LeavesEarned; // Getting new Balance
                Age = new DateDifference(System.DateTime.Now, DutyFrom);//new DateTime((System.DateTime.Now - DutyFrom).Ticks);            // Calulating Duty Period
                string[] values = { EmployeeID, "0", String.Format("{0:dd/MM/yyyy}", DutyFrom.Date), String.Format("{0:dd/MM/yyyy}", System.DateTime.Now), (Age.Years).ToString(), (Age.Months).ToString(), (Age.Days).ToString(), LastBalance, "", "", "", "", "0", "0", "0", "0", balance.ToString() };
                dsGrid.Tables["EMPLEAVEREQ"].Rows.Add(values); // adding last created row
            }
            /**********************************/

            return dsGrid; // binding grid

        }
        public static double GetELBalance(string EmployeeID, string BPSScale, string JoiningDate, string MofAppointment)
        {
            string DutyPeriod = string.Empty, Year = String.Empty, Month = String.Empty, Days = String.Empty;
            double LeavesEarned = 0.0; double balance = 0.0;
            
            // for Consultants
            if ((MofAppointment == "T" || MofAppointment == "U"))
            {
                balance = 0;
            }
            else
            {
                strQuery = @"SELECT                                        
                    HR_LEAVEREQ.LEAVEID,                                                                   
                    convert(varchar,HR_LEAVEREQ.FROMDATE,103) AS FROMDATE,                        
                    convert(varchar,HR_LEAVEREQ.TODATE,103) AS TODATE,                                                     
                    dbo.INITCAP(HR_LEAVEREQ.LEAVETYPE) AS LEAVETYPE,                    
                    HR_LEAVEREQ.DUELEAVE,                    
                    HR_LEAVETYPE.ISEARNED,                    
                    HR_LEAVETYPE.ISDUTY                                                                          
                FROM                    
                    HR_LEAVETYPE  with (nolock),HR_LEAVEREQ  with (nolock)                    
                WHERE                          
                    HR_LEAVETYPE.DESCR = HR_LEAVEREQ.LEAVETYPE AND                         
                    HR_LEAVEREQ.LEAVETYPE <> 'CASUAL' AND                         
                    UPPER(HR_LEAVEREQ.STATUS) = 'APPROVED' AND                       
                    HR_LEAVEREQ.EMPLOYEEID  = '" + EmployeeID + @"'                     
                    ORDER BY Convert(date,TODATE,103) ASC";

                dsGrid = dbAccess.FillDataset(strQuery, "EMPLEAVEREQ");
                
                DateTime DutyFrom = Convert.ToDateTime(JoiningDate);// DateTime.ParseExact(JoiningDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime DutyTo = DateTime.Now; // current date
                //DateDifference Age = null;
                DateTime RunningDutyTo = System.DateTime.Now; // current date
                DateTime NewRegulationDate = DateTime.ParseExact("30/09/2007", "dd/MM/yyyy", null);

                
                foreach (DataRow row in dsGrid.Tables["EMPLEAVEREQ"].Rows)
                {
                    string LeaveType = row["LEAVETYPE"].ToString().ToUpper();
                    DutyTo = DateTime.ParseExact(row["TODATE"].ToString(), "dd/MM/yyyy", null);

                    DateTime RunningDutyFrom = DateTime.ParseExact(row["FROMDATE"].ToString(), "dd/MM/yyyy", null).AddDays(-1);
                    // RunningDutyTo = (RunningDutyTo > System.DateTime.Now ? System.DateTime.Now : RunningDutyTo);
                    RunningDutyTo = (DutyTo < NewRegulationDate ? (LeaveType.Equals("LEAVE ENCASHMENT") ? DutyTo : RunningDutyFrom) : (Convert.ToInt32(row["ISDUTY"]) == 1 ? (DutyTo > System.DateTime.Now ? System.DateTime.Now : DutyTo) : RunningDutyFrom));

                    // update duty period for only duty leaves
                    // if (row["ISDUTY"].ToString().Equals("1"))
                    // {               
                    // Calculating Duty Period
                    LeavesEarned = LeaveUtil.GetEarnedLeaves(EmployeeID, BPSScale, DutyFrom, RunningDutyTo);
                    balance += LeavesEarned - (row["ISEARNED"].ToString().Equals("1") ? (LeaveType.Equals("HALF AVERAGE PAY") ? Convert.ToDouble(row["DUELEAVE"].ToString()) / 2 : (LeaveType.Equals("ABSENT") ? Convert.ToDouble(row["DUELEAVE"].ToString()) * 2 : Convert.ToDouble(row["DUELEAVE"].ToString()))) : 0);  // calculating new balance
                                                                                                                                                                                                                                                                                                                          // balance += LeavesEarned - (row["ISEARNED"].ToString().Equals("1") ? Convert.ToDouble(row["DUELEAVE"].ToString()) : 0);  // calculating new balance                                              
                                                                                                                                                                                                                                                                                                                          // }
                    DutyFrom = DutyTo.AddDays(1);
                }
                
                /************************ Creating Last Row and Calulating balance **********************/
                if (DutyFrom < DateTime.Now)
                {
                    LeavesEarned = LeaveUtil.GetEarnedLeaves(EmployeeID, BPSScale, DutyFrom, System.DateTime.Now); //Getting last Earned Leaves            
                    balance += LeavesEarned; // Getting new Balance                   
                }
               
                /**********************************/
            }
            return balance; // binding grid

        }
        public static double GetELBalanceEncashment(string EmployeeID, string BPSScale, string JoiningDate)
        {
            
            strQuery = @"SELECT                                        
                    EMPLEAVEREQ.LEAVEID,                                                                   
                    Convert(varchar,EMPLEAVEREQ.FROMDATE,103) AS FROMDATE,                        
                    Convert(varchar,EMPLEAVEREQ.TODATE,103) AS TODATE,                                                     
                    dbo.INITCAP(EMPLEAVEREQ.LEAVETYPE) AS LEAVETYPE,                    
                    EMPLEAVEREQ.DUELEAVE,                    
                    EMSLEAVETYPE.ISEARNED,                    
                    EMSLEAVETYPE.ISDUTY                                                                          
                FROM                    
                    HR_LEAVETYPE as EMSLEAVETYPE with(nolock),HR_LEAVEREQ as EMPLEAVEREQ with(nolock)                    
                WHERE                          
                    EMSLEAVETYPE.DESCR = EMPLEAVEREQ.LEAVETYPE AND                         
                    EMPLEAVEREQ.LEAVETYPE <> 'CASUAL' AND                         
                    UPPER(EMPLEAVEREQ.STATUS) = 'APPROVED' AND                          
                    EMPLEAVEREQ.EMPLOYEEID = '" + EmployeeID + @"'                     
                    ORDER BY convert(date,TODATE,103) ASC";


            dsGrid = dbAccess.FillDataset(strQuery, "EMPLEAVEREQ");

            DateTime EncashmentDate = DateTime.ParseExact(SQLDBAccess.GetEncashmentDate(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime DutyFrom = DateTime.ParseExact(JoiningDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime DutyTo = EncashmentDate;
            DateDifference Age = null;
            DateTime RunningDutyTo = EncashmentDate;
            DateTime NewRegulationDate = DateTime.ParseExact("30/09/2007", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            string DutyPeriod = string.Empty, Year = String.Empty, Month = String.Empty, Days = String.Empty;
            double LeavesEarned = 0.0; double balance = 0.0;
            foreach (DataRow row in dsGrid.Tables["EMPLEAVEREQ"].Rows)
            {
                string LeaveType = row["LEAVETYPE"].ToString().ToUpper();
                DutyTo = DateTime.ParseExact(row["TODATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime RunningDutyFrom = DateTime.ParseExact(row["FROMDATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(-1);
                // RunningDutyTo = (RunningDutyTo > System.DateTime.Now ? System.DateTime.Now : RunningDutyTo);
                RunningDutyTo = (DutyTo < NewRegulationDate ? (LeaveType.Equals("LEAVE ENCASHMENT") ? DutyTo : RunningDutyFrom) : (Convert.ToInt32(row["ISDUTY"]) == 1 ? (DutyTo > System.DateTime.Now ? System.DateTime.Now : DutyTo) : RunningDutyFrom));

                // update duty period for only duty leaves
                // if (row["ISDUTY"].ToString().Equals("1"))
                // {               
                // Calculating Duty Period
                LeavesEarned = LeaveUtil.GetEarnedLeaves(EmployeeID, BPSScale, DutyFrom, RunningDutyTo);
                balance += LeavesEarned - (row["ISEARNED"].ToString().Equals("1") ? (LeaveType.Equals("HALF AVERAGE PAY") ? Convert.ToDouble(row["DUELEAVE"].ToString()) / 2 : (LeaveType.Equals("ABSENT") ? Convert.ToDouble(row["DUELEAVE"].ToString()) * 2 : Convert.ToDouble(row["DUELEAVE"].ToString()))) : 0);  // calculating new balance
                                                                                                                                                                                                                                                                                                                      // balance += LeavesEarned - (row["ISEARNED"].ToString().Equals("1") ? Convert.ToDouble(row["DUELEAVE"].ToString()) : 0);  // calculating new balance                                              
                                                                                                                                                                                                                                                                                                                      // }
                DutyFrom = DutyTo.AddDays(1);
            }

            /************************ Creating Last Row and Calulating balance **********************/
            if (DutyFrom < EncashmentDate)
            {
                LeavesEarned = LeaveUtil.GetEarnedLeaves(EmployeeID, BPSScale, DutyFrom, EncashmentDate); //Getting last Earned Leaves            
                balance += LeavesEarned; // Getting new Balance                   
            }
            /**********************************/
            return balance; // binding grid

        }
        public static double GetELBalance(string EmployeeID, string BPSScale, string JoiningDate, string AppointmentMode, string TillDate)
        {
            strQuery = @"SELECT                                        
                    EMPLEAVEREQ.LEAVEID,                                                                   
                    Convert(varchar,EMPLEAVEREQ.FROMDATE,103) AS FROMDATE,                        
                    Convert(varchar,EMPLEAVEREQ.TODATE,103) AS TODATE,                                                     
                    dbo.INITCAP(EMPLEAVEREQ.LEAVETYPE) AS LEAVETYPE,                    
                    EMPLEAVEREQ.DUELEAVE,                    
                    EMSLEAVETYPE.ISEARNED,                    
                    EMSLEAVETYPE.ISDUTY                                                                          
                FROM                    
                    HR_LEAVETYPE as EMSLEAVETYPE with(nolock),HR_LEAVEREQ as EMPLEAVEREQ with(nolock)                   
                WHERE                          
                    EMSLEAVETYPE.DESCR = EMPLEAVEREQ.LEAVETYPE AND                         
                    EMPLEAVEREQ.LEAVETYPE <> 'CASUAL' AND                         
                    UPPER(EMPLEAVEREQ.STATUS) = 'APPROVED' AND                          
                    EMPLEAVEREQ.EMPLOYEEID = '" + EmployeeID + @"' and fromdate < Convert(date,'" + TillDate + "',103) " +
                        "ORDER BY TODATE ASC";


            dsGrid = dbAccess.FillDataset(strQuery, "EMPLEAVEREQ");
            DateTime Till = DateTime.ParseExact(TillDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime DutyFrom = DateTime.ParseExact(JoiningDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime DutyTo = Till; // Provided date

            //DateTime Age = System.DateTime.Now;
            DateDifference Age = null;
            DateTime RunningDutyTo = Till; // current date

            DateTime NewRegulationDate = DateTime.ParseExact("30/09/2007", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            string DutyPeriod = string.Empty, Year = String.Empty, Month = String.Empty, Days = String.Empty;
            double LeavesEarned = 0.0; double balance = 0.0;
            foreach (DataRow row in dsGrid.Tables["EMPLEAVEREQ"].Rows)
            {
                string LeaveType = row["LEAVETYPE"].ToString().ToUpper();
                DutyTo = DateTime.ParseExact(row["TODATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime RunningDutyFrom = DateTime.ParseExact(row["FROMDATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(-1);

                RunningDutyTo = (DutyTo < NewRegulationDate ? (LeaveType.Equals("LEAVE ENCASHMENT") ? DutyTo : RunningDutyFrom) : (Convert.ToInt32(row["ISDUTY"]) == 1 ? DutyTo : RunningDutyFrom));

                // update duty period for only duty leaves
                // if (row["ISDUTY"].ToString().Equals("1"))
                // {               
                // Calculating Duty Period
                RunningDutyTo = (RunningDutyTo > Till ? Till : RunningDutyTo);
                LeavesEarned = LeaveUtil.GetEarnedLeaves(EmployeeID, BPSScale, DutyFrom, RunningDutyTo);
                // balance += LeavesEarned - (row["ISEARNED"].ToString().Equals("1") ? Convert.ToDouble(row["DUELEAVE"].ToString()) : 0);  // calculating new balance                                              
                balance += LeavesEarned - (row["ISEARNED"].ToString().Equals("1") ? (LeaveType.Equals("HALF AVERAGE PAY") ? Convert.ToDouble(row["DUELEAVE"].ToString()) / 2 : Convert.ToDouble(row["DUELEAVE"].ToString())) : 0);  // calculating new balance
                                                                                                                                                                                                                                    // }
                DutyFrom = DutyTo.AddDays(1);
            }

            /************************ Creating Last Row and Calulating balance **********************/
            if (DutyFrom < Till)
            {
                LeavesEarned = LeaveUtil.GetEarnedLeaves(EmployeeID, BPSScale, DutyFrom, Till); //Getting last Earned Leaves            
                balance += LeavesEarned; // Getting new Balance                   
            }
            /**********************************/
            return balance; // binding grid

        }
        public static double GetEarnedLeaveBalance(String EmpID, string DateOfJoining, string AppointmentMode)
        {
            double LeaveEarned = 0.0;
            //DateTime Age = System.DateTime.Now;
            DateDifference Age = null;

            // DateTime JoiningDate = DateTime.ParseExact(ViewState["JoiningDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);                      
            // Get total earned leave before 30/09/2007
            DateTime JoiningDate = DateTime.ParseExact(DateOfJoining, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime EnchasmentDate = DateTime.ParseExact("01/01/2007", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (JoiningDate < EnchasmentDate)
            {
                Age = new DateDifference(EnchasmentDate.AddDays(-1), JoiningDate);
                LeaveEarned = (Age.Years * 48) + (Age.Months * 4) + (Age.Days > 16 ? 4 : 0);
                JoiningDate = EnchasmentDate; // for next calculation
            }
            // after 1/1/2007
            Age = new DateDifference(System.DateTime.Now, JoiningDate);
            LeaveEarned += (Age.Years * 30) + (Age.Months * 2.5) + (Age.Days > 16 ? 2.5 : 0);

            // Get Earned , Encashed , delete leave balance.
            return LeaveEarned - GetEarnedAvailed(EmpID) - GetUnEarnedPeriod(EmpID);


        }
        public static double GetEarnedAvailed(string EmpID)
        {
            double availedLeaves = 0.0;
            strQuery = @"select isnull(SUM(DUELEAVE),0) AS AVAILED from hr_leavereq as empleavereq with(nolock),HR_LEAVETYPE as EMSLEAVETYPE with(nolock) where  
                    empleavereq.LEAVETYPE = emsleavetype.DESCR and STATUS = 'Approved' AND
                    EMSLEAVETYPE.ISEARNED = 1 AND EMSLEAVETYPE.ISDUTY = 1 AND EMPLOYEEID='" + EmpID + "'";
            reader = dbAccess.FillDataReader(strQuery);
            if (reader != null && reader.Read())
            {
                availedLeaves = Convert.ToDouble(reader["AVAILED"].ToString());
            }
            dbAccess.CloseDataReader(ref reader);
            return availedLeaves;
        }
        public static double GetUnEarnedPeriod(string EmpID)
        {
            double UnearnedPeriod = 0.0;
            double LeaveBalance = 0.0;
            //DateTime Age = System.DateTime.Now;
            DateDifference Age = null;
            DateTime EnchasmentDate = DateTime.ParseExact("01/01/2007", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            strQuery = @"SELECT EMPLEAVEREQ.EMPLOYEEID,Convert(varchar,FROMDATE,103) AS FROMDATE,Convert(varchar,TODATE,103) AS TODATE,EMPLEAVEREQ.LEAVETYPE,DUELEAVE 
                    from HR_LEAVEREQ as EMPLEAVEREQ with(nolock),HR_LEAVETYPE as EMSLEAVETYPE with(nolock) where  
                        EMPLEAVEREQ.LEAVETYPE = EMSLEAVETYPE.DESCR and STATUS = 'Approved' AND 
                        EMSLEAVETYPE.ISEARNED = 0 AND EMSLEAVETYPE.ISDUTY = 0 AND EMPLOYEEID='" + EmpID + "'";
            dsGrid = dbAccess.FillDataset(strQuery, "EMSLEAVEREQ");
            for (int i = 0; i < dsGrid.Tables["EMSLEAVEREQ"].Rows.Count; i++)
            {
                DateTime FromDate = DateTime.ParseExact(dsGrid.Tables["EMSLEAVEREQ"].Rows[i]["FROMDATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime ToDate = DateTime.ParseExact(dsGrid.Tables["EMSLEAVEREQ"].Rows[i]["TODATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                Age = new DateDifference(ToDate, FromDate);

                UnearnedPeriod = (ToDate < EnchasmentDate ?
                    (Age.Years * 48) + (Age.Months * 4) + (Age.Days > 16 ? 4 : 0) :
                    (Age.Years * 30) + (Age.Months * 2.5) + (Age.Days > 16 ? 2.5 : 0));
                //JoiningDate = JoiningDate.AddDays(1); // for next calculation
                LeaveBalance += UnearnedPeriod;
            }
            return LeaveBalance;
        }
        public static bool LeavesOverLapping(string EmployeeID, string FromDate, string ToDate)
        {
            
            bool retFlg = false;
            strQuery = @"SELECT *
                    FROM 
                            HR_LEAVEREQ with (nolock),  
                            (select convert(date,'" + FromDate + "',103) a , convert(date,'" + ToDate + @"',103) b) X 
                    WHERE            
                            (UPPER(HR_LEAVEREQ.STATUS) like 'APPROVED' OR  
                            UPPER(HR_LEAVEREQ.STATUS) like 'PENDING' ) AND      
                            ( HR_LEAVEREQ.FROMDATE BETWEEN X.a AND X.b AND  HR_LEAVEREQ.TODATE BETWEEN X.a AND X.b or HR_LEAVEREQ.FROMDATE < X.a and  X.b < HR_LEAVEREQ.TODATE ) AND
                             HR_LEAVEREQ.LEAVETYPE <> 'LEAVE ENCASHMENT' AND HR_LEAVEREQ.EMPLOYEEID = '" + EmployeeID + "'";
            
            reader = dbAccess.FillDataReader(strQuery);
            if (reader != null && reader.Read())
            {
                retFlg = true;
            }
            dbAccess.CloseDataReader(ref reader);
            return retFlg;


        }
        public static bool LeavesOverLapping(string EmployeeID, string FromDate, string ToDate, string LeaveID)
        {
            bool retFlg = false;
            strQuery = @"SELECT *
                    FROM 
                            HR_LEAVEREQ with (nolock),  
                            (select convert(date,'" + FromDate + "',103) a , convert(date,'" + ToDate + @"',103) b) X 
                    WHERE            
                            (UPPER(HR_LEAVEREQ.STATUS) like 'APPROVED' OR  
                            UPPER(HR_LEAVEREQ.STATUS) like 'PENDING' ) AND      
                            ( HR_LEAVEREQ.FROMDATE BETWEEN X.a AND X.b AND  HR_LEAVEREQ.TODATE BETWEEN X.a AND X.b or HR_LEAVEREQ.FROMDATE < X.a and  X.b < HR_LEAVEREQ.TODATE ) AND
                             HR_LEAVEREQ.LEAVETYPE <> 'LEAVE ENCASHMENT' AND HR_LEAVEREQ.EMPLOYEEID = '" + EmployeeID + "' AND LEAVEID <>'" + LeaveID + "'";
            reader = dbAccess.FillDataReader(strQuery);
            if (reader != null && reader.Read())
            {
                retFlg = true;
            }
            dbAccess.CloseDataReader(ref reader);
            return retFlg;


        }
        public static string GetEmail(string EmployeeID)
        {
            string Email = "";
            strQuery = "SELECT EmailID as EmailAddress FROM  ViewUserEmp with(nolock) WHERE EmployeeID='" + EmployeeID + "'";
            reader = dbAccess.FillDataReader(strQuery);
            if (reader != null && reader.Read())
            {
                Email = reader["EmailAddress"].ToString();
            }
            dbAccess.CloseDataReader(ref reader);
            return Email;
        }
        public static string GetEmpName(string EmployeeID)
        {
            string Name = "";
            strQuery = "SELECT FullName as NAME FROM ViewUserEmp with(nolock) WHERE EmployeeID='" + EmployeeID + "'";
            reader = dbAccess.FillDataReader(strQuery);
            if (reader != null && reader.Read())
            {
                Name = reader["NAME"].ToString();
            }
            dbAccess.CloseDataReader(ref reader);
            return Name;
        }
        public static string GetRefName(string LeaveID)
        {
            String Name = "";
            strQuery = "SELECT DISTINCT FullName as EMPLOYEENAME FROM HR_LEAVEREC with(nolock),ViewUserEmp with(nolock) WHERE EMAILID = REFEREMAIL AND STATUS='1'  AND LEAVEID ='" + LeaveID + "' AND RECID = (SELECT isnull(MAX(RECID),0) FROM HR_LEAVEREC WHERE LEAVEID ='" + LeaveID + "')";
            reader = dbAccess.FillDataReader(strQuery);
            if (reader != null && reader.Read())
            {
                Name = reader["EMPLOYEENAME"].ToString();
            }
            dbAccess.CloseDataReader(ref reader);
            return Name;
        }
        public static string GetEmployeeEmail(string EmployeeID)
        {
            string Email = "";
            strQuery = "SELECT EMAILID  as EMAIL FROM EmpView with(nolock) WHERE EMPLOYEEID='" + EmployeeID + "'";
            reader = dbAccess.FillDataReader(strQuery);
            if (reader != null && reader.Read())
            {
                Email = reader["EMAIL"].ToString();
            }
            dbAccess.CloseDataReader(ref reader);
            return Email;
        }
    }
}
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ReportingEngine.Master" CodeBehind="StepFive.aspx.cs" Inherits="PTAWMS.Reports.Filters.StepFive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <section class="container" style="margin-left:0;margin-right:0;">
        <div class="col-sm-3 col-md-3 col-lg-3" >
            <!-- Sidebar -->
             <div id="Div2">
                <ul class="sidebar-nav">
                    <li class="sidebar-brand">
                        <h4>Filters Navigation</h4>
                    </li>                    
                     <% { %> <% PTAWMS.Models.ViewUserEmp user = (PTAWMS.Models.ViewUserEmp)HttpContext.Current.Session["LoggedUser"];%>                 
                    <% if(user.UserType=="A" || user.UserType=="H" ||user.UserType=="E") %> 
                    <% {%>    
                        <li >
                            <asp:LinkButton ID="btnStepOne" runat="server" CssClass="inactive-link" OnClick="btnStepOne_Click" >Step One<p>Division, Zone</p></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="btnStepTwo" runat="server" CssClass="inactive-link" OnClick="btnStepTwo_Click" >Step Two<p>Employee Type, Shifts</p></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="btnStepThree" runat="server"  CssClass="inactive-link" OnClick="btnStepThree_Click" >Step Three<p>Department, Designation</p></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="btnStepFour" runat="server" CssClass="inactive-link" OnClick="btnStepFour_Click" >Step Four<p>Employee</p></asp:LinkButton>
                        </li>                  
                        <li>
                            <asp:LinkButton ID="btnStepSix" runat="server" CssClass="active-link" OnClick="btnStepSix_Click" >Finish<p>Generate Report</p></asp:LinkButton>

                        </li>
                    <% } else
                       {%>
                        
                        <%--<li>
                            <asp:LinkButton ID="LinkButton3" runat="server"  CssClass="inactive-link" OnClick="btnStepThree_Click" >Step One<p>Department, Designation</p></asp:LinkButton>
                        </li>--%>
                        <li>
                            <asp:LinkButton ID="LinkButton4" runat="server" CssClass="inactive-link" OnClick="btnStepFour_Click" >Step Two<p>Employee</p></asp:LinkButton>
                        </li>                  
                        <li>
                            <asp:LinkButton ID="LinkButton5" runat="server" CssClass="active-link" OnClick="btnStepSix_Click" >Finish<p>Generate Report</p></asp:LinkButton>

                        </li>
                     <% }%>   
                     <% }%>  
                </ul>
                
            <!-- /#sidebar-wrapper -->
        </div>         
        </div>
        <div class="col-sm-9 col-md-9 col-lg-9">
                <div class="row">
                    <div class="col-md-8">
                        <section class="row">
                            <h2>Choose Report</h2>
                            <ul>
                                <li>
                                    <h5>Daily</h5>
                                    <ul>
                                        <li><a href="../AttReportsHome.aspx?reportname=edit_attendance">Edit Attendace Report</a></li>
                                        <li><a href="../AttReportsHome.aspx?reportname=detailed_att">Detailed Attendance</a></li>
                                        <li><a href="../AttReportsHome.aspx?reportname=present">Present</a></li>
                                        <li><a href="../AttReportsHome.aspx?reportname=absent">Absent</a></li>
                                        <li><a href="../AttReportsHome.aspx?reportname=late_in">Late In</a></li>
                                        <li><a href="../AttReportsHome.aspx?reportname=late_out">Late Out</a></li>
                                        <li><a href="../AttReportsHome.aspx?reportname=early_in">Early In</a></li>
                                        <li><a href="../AttReportsHome.aspx?reportname=early_out">Early Out</a></li>
                                        <li><a href="../AttReportsHome.aspx?reportname=overtime">Overtime</a></li>
                                        <li><a href="../AttReportsHome.aspx?reportname=missing_attendance">Missing Attendance</a></li>
                                        <li><a href="../AttReportsHome.aspx?reportname=multiple_in_out">Multiple In/Out</a></li>
                                        <li><a href="../AttReportsHome.aspx?reportname=emp_att">Employee Attendance</a></li>
                                        
                                    </ul>
                                </li>                                
                                <li>
                                    <h5>Monthly Reports</h5>
                                    <ul>
                                        <li><a href="../AttReportsHome.aspx?reportname=MonthlySheet_att">Monthly Sheet</a></li>
                                        <li><a href="../AttReportsHome.aspx?reportname=MonthlySummary_att">Monthly Summary</a></li>
                                    </ul>
                                </li>
                                <li>
                                    <h5>Visitors Reports</h5>
                                    <ul>
                                        <li><a href="../AttReportsHome.aspx?reportname=dailyvisitor">Daily Visitor Detail</a></li>
                                    </ul>
                                </li>                              
                            </ul>
                        </section>
                    </div>
                    <section class="col-md-4 selected-filters-wrapper">
                    <h2>Selected Filters...</h2>
                    <hr />
                    <div class="panel-group" id="accordion">
                        <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).CompanyFilter.Count > 0)
                            {
                                {
                                    int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).CompanyFilter.Count;
                                    Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a style = 'text-decoration: none !important;' data-toggle='collapse' data-parent='#accordion' href='#collapseOne'>Companies</a>  <span style ='float:right;' class='badge' id='CompanySpan'>" + d + "</span></h4></div><div id='collapseOne' class='panel-collapse collapse out'><div class='list-group'>");
                                }
                                foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).CompanyFilter)
                                {
                                    { Response.Write("<a class='list-group-item' id='Company'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-minier btn-danger' style='float:right;'>[X]</button></a> "); }
                                }
                                { Response.Write("</div></div></div>"); }
                            }%> 
                    </div>
                    <div class="panel-group" id="Div1">
                        <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).LocationFilter.Count > 0)
                            {
                                {
                                    int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).LocationFilter.Count;
                                    Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a style = 'text-decoration: none !important;' data-toggle='collapse' data-parent='#Div1' href='#collapseShift'>Stations</a>  <i class='ace-icon fa fa-arrow-down'></i><span style ='float:right;' class='badge' id ='LocationSpan'>" + d + "</span></h4></div><div id='collapseShift' class='panel-collapse collapse out'><div class='list-group'>");
                                }
                                foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).LocationFilter)
                                {
                                    { Response.Write("<a class='list-group-item' id ='Location'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-minier btn-danger' style='float:right;'>[X]</button></a>"); }
                                }
                                { Response.Write("</div></div></div>"); }
                            }%>
                    </div>
                    <div class="panel-group" id="Div3">
                        <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).ShiftFilter.Count > 0)
                            {
                                {
                                    int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).ShiftFilter.Count;
                                    Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a style = 'text-decoration: none !important;' data-toggle='collapse' data-parent='#Div3' href='#collapseType'>Shifts</a>   <i class='ace-icon fa fa-arrow-down'></i><span style ='float:right;' class='badge' id ='ShiftSpan'>" + d + "</span></h4></div><div id='collapseType' class='panel-collapse collapse out'><div class='list-group'>");
                                }
                                foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).ShiftFilter)
                                {
                                    { Response.Write("<a class='list-group-item' id='Shift'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-minier btn-danger' style='float:right;'>[X]</button></a>"); }
                                } 
                                    { Response.Write("</div></div></div>"); }
                            }%>
                    </div>
                    <div class="panel-group" id="Div4">
                        <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).DepartmentFilter.Count > 0)
                            {
                                {
                                    int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).DepartmentFilter.Count;
                                    Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a style = 'text-decoration: none !important;'  data-toggle='collapse' data-parent='#Div4' href='#collapseLocation'>Divisions<span  style ='float:right;' class='badge' id ='DepartmentSpan'>" + d + "</span></a> <i class='ace-icon fa fa-arrow-down'></i></h4></div><div id='collapseLocation' class='panel-collapse collapse out'><div class='list-group'>");
                                }
                                foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).DepartmentFilter)
                                {
                                    { Response.Write("<a class='list-group-item' id='Department'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-minier btn-danger' style='float:right;'>[X]</button></a>"); }
                                }
                                { Response.Write("</div></div></div>"); }
                            }%>
                    </div>
                    <div class="panel-group" id="Div5">
                        <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).TypeFilter.Count > 0)
                            {
                                {
                                    int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).TypeFilter.Count;
                                    Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a data-toggle='collapse' data-parent='#Div5' href='#collapseWing'>Employee Type</a> <i class='ace-icon fa fa-arrow-down'></i><span style ='float:right;' class='badge' id ='TypeSpan'>" + d + "</span></h4></div><div id='collapseWing' class='panel-collapse collapse out'><div class='list-group'>");
                                }
                                foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).TypeFilter)
                                {
                                    { Response.Write("<a class='list-group-item' id='Type'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-minier btn-danger' style='float:right;'>[X]</button></a>"); }
                                }
                                { Response.Write("</div></div></div>"); }
                            }%>
                    </div>
                    <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).SectionFilter.Count > 0)
                        {
                            {
                                int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).SectionFilter.Count;
                                Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a data-toggle='collapse' data-parent='#Div7' href='#collapseSection'>Departments</a> <i class='ace-icon fa fa-arrow-down'></i><span style ='float:right;' class='badge' id ='SectionSpan'>" + d + "</span></h4></div><div id='collapseSection' class='panel-collapse collapse out'><div class='list-group'>");
                            }
                            foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).SectionFilter)
                            {
                                { Response.Write("<a class='list-group-item' id='Section'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-minier btn-danger' style='float:right;'>[X]</button></a>"); }
                            }  { Response.Write("</div></div></div><div>"); }
                        }%>
                    <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).EmployeeFilter.Count > 0)
                        {
                            {
                                int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).EmployeeFilter.Count;
                                Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a data-toggle='collapse' data-parent='#Div7' href='#collapseEmployee'>Employees</a> <i class='ace-icon fa fa-arrow-down'></i><span style ='float:right;' class='badge' id ='EmployeeSpan'>" + d + "</span></h4></div><div id='collapseEmployee' class='panel-collapse collapse out'><div class='list-group'>");
                            }
                            foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).EmployeeFilter)
                            {
                                { Response.Write("<a class='list-group-item' id='Employee'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-minier btn-danger' style='float:right;'>[X]</button></a>"); }
                            }  { Response.Write("</div></div></div><div>"); }
                        }%>
                    <div class="panel-group" id="Div8">
                        <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).CMDesignationFilter.Count > 0)
                            {
                                {
                                    int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).CMDesignationFilter.Count;
                                    Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a data-toggle='collapse' data-parent='#Div6' href='#collapseCrew'>Common Designations</a> <i class='ace-icon fa fa-arrow-down'></i><span style ='float:right;' class='badge' id ='DesignationSpan'>" + d + "</span></h4></div><div id='collapseCrew' class='panel-collapse collapse out'><div class='list-group'>");
                                }
                                foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).CMDesignationFilter)
                                {
                                    { Response.Write("<a class='list-group-item' id='Designation'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-minier btn-danger' style='float:right;'>[X]</button></a>"); }
                                }   { Response.Write("</div></div></div>"); }
                            }%>
                    </div>
                </section>
                </div>
                <div class="row">
                    
                </div>
        </div>
    </section>
    <script src="~/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
       <script src="../../Scripts/Filters/DeleteSingleFilters.js"></script>
       <%--<script src="../../Scripts/ReportsFilters/DeleteSingleFilters.js"></script>--%>
</asp:Content>

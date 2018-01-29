<%@ Page Language="C#" MasterPageFile="~/ReportingEngine.Master" AutoEventWireup="true" CodeBehind="HRMSReports.aspx.cs" Inherits="PTAWMS.Reports.Filters.HRMSReports" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .hiddencol
        {
        display: none;
        }
    </style>
    <div class="page-content">      
        <div class="page-header">
            <h1>
                Reports
                <small>
                <i class="ace-icon fa fa-angle-double-right"></i>
                Select Report
                </small>
            </h1>
        </div>
        <!-- /.page-header -->
        <div class="row">
            <div class="col-md-12">
                <div class="col-md-8">
                    <div class="row">
                        <div class="col-md-6">
                            <span class="h5">From :</span>  <input id="dateFrom"  class="input-sm"  runat="server" type="date" />
                        </div>
                        <div class="col-md-6">
                            <span class="h5">To :</span>  <input id="dateTo" class="input-sm"  runat="server" type="date" />
                        </div>
                        <hr />
                    </div>
                    <br />
                    <div class="row col-md-12">
                        <% { %> <% PTAWMS.Models.ViewUserEmp user = (PTAWMS.Models.ViewUserEmp)HttpContext.Current.Session["LoggedUser"];%>
                        <% if(HttpContext.Current.Session["IsSupervisor"].ToString()=="0") %> 
                        <% {%>
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-success" title="Generate Reports" OnClick="btnStepSix_Click" >Generate Report</asp:LinkButton>
                         <% } else{%>
                         <% if(user.UserType=="A" || user.UserType=="H" ||user.UserType=="E") %> 
                        <% {%>    
                        <asp:LinkButton ID="btnStepOne"  runat="server" CssClass="btn btn-grey" OnClick="btnStepOne_Click" >Step One</asp:LinkButton>
                        <asp:LinkButton ID="btnStepTwo" runat="server" CssClass="btn btn-grey" OnClick="btnStepTwo_Click" >Step Two</asp:LinkButton>
                        <asp:LinkButton ID="btnStepThree" runat="server"  CssClass="btn btn-grey" OnClick="btnStepThree_Click" >Step Three</asp:LinkButton>
                        <asp:LinkButton ID="btnStepFour" runat="server" CssClass="btn btn-grey" OnClick="btnStepFour_Click" >Step Four</asp:LinkButton>
                        <asp:LinkButton ID="btnStepSix" runat="server" CssClass="btn btn-success" OnClick="btnStepSix_Click" >Generate Reports</asp:LinkButton>
                        <% } else
                            {%>
                        <%--<asp:LinkButton ID="LinkButton3"  title="Choose Departments & Common Designations" runat="server"  CssClass="btn btn-grey" OnClick="btnStepThree_Click" >Step One</asp:LinkButton>--%>
                        <asp:LinkButton ID="LinkButton4" runat="server" title="Choose Employees" CssClass="btn btn-grey" OnClick="btnStepFour_Click" >Step Two</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton5" runat="server" CssClass="btn btn-success" title="Generate Reports" OnClick="btnStepSix_Click" >Generate Report</asp:LinkButton>
                        <% }%>   
                        <% }%> 
                        <% }%> 
                        <hr />
                    </div>
                    <div class="row">
                        <section class="col-md-12">
                            <div id="Div2" class="accordion-style1 panel-group accordion-style2">
								<div class="panel panel-default">
									<div class="panel-heading">
										<h4 class="panel-title">
											<a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true">
												<i class="bigger-110 ace-icon fa fa-angle-down" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
												&nbsp;Attendance Reports
											</a>
										</h4>
									</div>

									<div class="panel-collapse collapse in" id="collapseOne" aria-expanded="true" style="">
										<div class="panel-body">
										    <ul class="text-primary">
                                                 <%if (Session["IsSupervisor"] =="0")
                                                    {%>
                                                        <li><a href="../RptLoader/AttReportsHome.aspx?reportname=emp_att">Employee Attendance</a></li>
                                                        <li><a href="../RptLoader/AttReportsHome.aspx?reportname=multiple_in_out">Multiple In/Out</a></li>
                                                   <% }
                                             else
                                                {%>
                                                <li><a href="../RptLoader/AttReportsHome.aspx?reportname=detailed_att">Detailed Attendance</a></li>
                                                <li><a href="../RptLoader/AttReportsHome.aspx?reportname=present">Present</a></li>
                                                <li><a href="../RptLoader/AttReportsHome.aspx?reportname=absent">Absent</a></li>
                                                <li><a href="../RptLoader/AttReportsHome.aspx?reportname=late_in">Late In</a></li>
                                                <li><a href="../RptLoader/AttReportsHome.aspx?reportname=late_out">Late Out</a></li>
                                                <li><a href="../RptLoader/AttReportsHome.aspx?reportname=early_in">Early In</a></li>
                                                <li><a href="../RptLoader/AttReportsHome.aspx?reportname=early_out">Early Out</a></li>
                                                <li><a href="../RptLoader/AttReportsHome.aspx?reportname=missing_attendance">Missing Attendance</a></li>
                                                <li><a href="../RptLoader/AttReportsHome.aspx?reportname=multiple_in_out">Multiple In/Out</a></li>
                                                <li><a href="../RptLoader/AttReportsHome.aspx?reportname=emp_att">Employee Attendance</a></li>
                                                <li><a href="../RptLoader/AttReportsHome.aspx?reportname=emp_att_chk">Employee Attendance check</a></li>
                                                <li><a href="../RptLoader/AttReportsHome.aspx?reportname=hrms_report">HRMS Report</a></li>
                                                <%}%>
                                            </ul>
										</div>
									</div>
								</div>

								<div class="panel panel-default">
									<div class="panel-heading">
										<h4 class="panel-title">
											<a class="accordion-toggle collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false">
												<i class="bigger-110 ace-icon fa fa-angle-right" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
												&nbsp;Overtime Reports
											</a>
										</h4>
									</div>

									<div class="panel-collapse collapse" id="collapseTwo" aria-expanded="false" style="height: 0px;">
										<div class="panel-body">
                                           <ul class="text-primary">
											<li><a href="../RptLoader/AttReportsHome.aspx?reportname=d_apc">Detailed Approved Overtime Claims</a></li>
                                        <li><a href="../RptLoader/AttReportsHome.aspx?reportname=a_ocs">Approved Overtime Claims Summary</a></li>
                                               </ul>
                                        </div>
									</div>
								</div>
                                <div class="panel panel-default">
									<div class="panel-heading">
										<h4 class="panel-title">
											<a class="accordion-toggle collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false">
												<i class="bigger-110 ace-icon fa fa-angle-right" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
												&nbsp;Visitor Reports
											</a>
										</h4>
									</div>

									<div class="panel-collapse collapse"  id="collapseThree" aria-expanded="false" style="height: 0px;">
										<div class="panel-body">
                                           <ul class="text-primary">
                                               <%if (Session["IsSupervisor"] =="0")
                                                    {%>
                                               
										      	        <li><a href="../RptLoader/AttReportsHome.aspx?reportname=dailyvisitor">Daily Visit Detail</a></li>
                                                  <% }
                                                       else
                                                {%>
										      	<li><a href="../RptLoader/AttReportsHome.aspx?reportname=dailyvisitor">Daily Visit Detail</a></li>
                                               	<li><a href="../RptLoader/AttReportsHome.aspx?reportname=visitorsummary">Visitor Summary</a></li>
                                               <li><a href="../RptLoader/AttReportsHome.aspx?reportname=empvisitorsummary">Employee Visitor Summary</a></li>
                                               <%}%>
                                         
                                               </ul>
                                        </div>
									</div>
								</div>                           
							
							</div>
                        </section>
                    </div>
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
        </div>
    </div>
</asp:Content>

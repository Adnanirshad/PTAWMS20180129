<%@ Page Title="" Language="C#" MasterPageFile="~/ReportingEngine.Master" AutoEventWireup="true" CodeBehind="StepFour.aspx.cs" Inherits="PTAWMS.Reports.Filters.StepFour" %>

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
                Select Employees
                </small>
            </h1>
        </div>
        <!-- /.page-header -->
        <div class="row">
            <div class="col-md-12">
                <div class="col-md-8">
                    <div class="row" id="divDate">
                        <div class="col-md-6">
                            <span class="h5">From :</span>  <input id="dateFrom"  class="input-sm"  runat="server" type="date" />
                        </div>
                        <div class="col-md-6">
                            <span class="h5">To :</span>  <input id="dateTo" class="input-sm"  runat="server" type="date" />
                        </div>
                        <hr />
                    </div>
                    <% if (Convert.ToString(Session["ReportMenu"]) == "HRMS")
                             {%>
                    <div class="checkbox">
													<label>
														<input name="form-field-checkbox" id="chkDate" type="checkbox" class="ace">
														<span class="lbl"> Apply Date Filters</span>
                                                        
													</label>
												</div>
                    <asp:HiddenField ID="hidDateFilter" runat="server" />
                    <% }%>
                    <br />
                    <div class="row col-md-12">
                        <% { %> <% PTAWMS.Models.ViewUserEmp user = (PTAWMS.Models.ViewUserEmp)HttpContext.Current.Session["LoggedUser"];%>
                        <% if (user.HRModule == true)
                             {%>
                        <asp:LinkButton ID="LinkButton1"  runat="server" CssClass="btn btn-sm  btn-grey" OnClick="btnStepOne_Click" ToolTip="Stations, Divisions">Location</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-sm  btn-grey" OnClick="btnStepTwo_Click" ToolTip="Types, Domicile, Shifts">Category</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton6" runat="server"  CssClass="btn btn-sm  btn-grey" OnClick="btnStepThree_Click" ToolTip="Department, Designations">Posting</asp:LinkButton>
                        <% if (Convert.ToString(Session["ReportMenu"]) == "HRMS")
                            {%>
                        <asp:LinkButton ID="LinkButton8" runat="server" CssClass="btn  btn-sm btn-grey" OnClick="btnHRQStep_Click" ToolTip="Degree, Institute">Education</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton10" runat="server" CssClass="btn btn-sm btn-primary wrap" OnClick="btnStepFour_Click" ToolTip="Employees">Employees</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton11" runat="server" CssClass="btn btn-sm  btn-success" OnClick="btnGenerateHRReport" >Generate Report</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton12" runat="server" CssClass="btn btn-sm  btn-success" OnClick="btnStepSix_Click" >Reports</asp:LinkButton>
                        <%}
    else
    { %>
                        <asp:LinkButton ID="LinkButton7" runat="server" CssClass="btn btn-sm btn-primary wrap" OnClick="btnStepFour_Click" ToolTip="Employees">Employees</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton9" runat="server" CssClass="btn btn-sm  btn-success" OnClick="btnStepSix_Click" >Reports</asp:LinkButton>
                        <%}
    } else if(user.UserType=="A" || user.UserType=="H" ||user.UserType=="E") %> 
                        <% {%>          
                        <asp:LinkButton ID="btnStepOne"  runat="server" CssClass="btn btn-sm btn-grey" OnClick="btnStepOne_Click" >Stations, Divisions</asp:LinkButton>
                        <asp:LinkButton ID="btnStepTwo" runat="server" CssClass="btn btn-sm  btn-grey" OnClick="btnStepTwo_Click" >Types, Shifts</asp:LinkButton>
                        <asp:LinkButton ID="btnStepThree" runat="server"  CssClass="btn  btn-sm btn-grey" OnClick="btnStepThree_Click" >Department, Designations</asp:LinkButton>
                        <asp:LinkButton ID="btnStepFour" runat="server" CssClass="btn btn-sm  btn-primary" OnClick="btnStepFour_Click" >Employees</asp:LinkButton>
                         <% if (user.HRModule == true)
                           {%>
                        <asp:LinkButton ID="LinkButton14" runat="server" CssClass="btn btn-sm  btn-grey" OnClick="btnHRQStep_Click" >Degree, Institute</asp:LinkButton>
                        <% }%>
                        <asp:LinkButton ID="btnStepSix" runat="server" CssClass="btn btn-sm  btn-success" OnClick="btnStepSix_Click" >Finish</asp:LinkButton>
                        <% } else
                            {%>
                       <%-- <asp:LinkButton ID="LinkButton3"  title="Choose Departments & Common Designations" runat="server"  CssClass="btn btn-sm  btn-grey" OnClick="btnStepThree_Click" >Department, Designations</asp:LinkButton>--%>
                        <asp:LinkButton ID="LinkButton4" runat="server" title="Choose Employees" CssClass="btn btn-sm  btn-primary" OnClick="btnStepFour_Click" >Employees</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton5" runat="server" CssClass="btn btn-sm  btn-success" title="Generate Reports" OnClick="btnStepSix_Click" >Finish</asp:LinkButton>
                        <% }%>   
                        <% }%> 
                        <hr />
                    </div>

                   

                    <div class="row">
                        <div class="filterHeader">
                            <span class="h4">Employee</span>
                            <span style="margin-left:10px">
                                <asp:TextBox ID="tbSearch_Employe1" CssClass="input-sm" runat="server" AutoPostBack="True" AutoCompleteType="Pager" />
                                <asp:Button ID="Button2" runat="server" Text="Search" CssClass="btn-primary" OnClick="ButtonSearchEmployee_Click" />
                                <asp:Button ID="Button3" runat="server" style="margin-top:18px" Text="Clear All Filters" CssClass="btn-warning" OnClick="ButtonDeleteAll_Click" />
                            </span>
                        </div>
                        <br />
                        <%-- <span style="margin-left:10px"><asp:TextBox ID="tbSearch_Employee" CssClass="input-field" runat="server" AutoPostBack="True" AutoCompleteType="Pager" /> <asp:Button ID="Button1" runat="server" Text="Search" CssClass="btn-primary" OnClick="ButtonSearchEmployee_Click" /></span></div>--%>
                        <section>
                            <asp:GridView ID="GridViewEmployee" runat="server" Width="600px" AutoGenerateColumns="False" PagerStyle-CssClass="pgr" CssClass="Grid"                              GridLines="None" AllowPaging="True" AllowSorting="True"                                                OnPageIndexChanging="GridViewEmployee_PageIndexChanging" ForeColor="Black" OnRowDataBound="GridViewEmployee_RowDataBound" ShowFooter="True"  >
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="EmployeeID" HeaderText="ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <%--<asp:CheckBox ID="CheckAll" runat="server" />--%>
                                            <input style="margin-left:6px" id="chkAll" onclick="javascript: SelectAllCheckboxes(this, 'GridViewEmployee');" 
                                                runat="server" type="checkbox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox style="margin-left:6px"  ID="CheckOne" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EmpNo" HeaderText="EmpNo" />
                                    <asp:BoundField DataField="FullName" HeaderText="Name" />
                                    <asp:BoundField DataField ="SectionName" HeaderText ="Department" />
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#EEEEEE" Font-Bold="False" ForeColor="Black" Wrap="False" />
                                <HeaderStyle BackColor="#EEEEEE" Font-Bold="False" ForeColor="Black" />
                                <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev" Mode="NextPreviousFirstLast" />
                                <PagerStyle BackColor="White" ForeColor="#0094FF" HorizontalAlign="Center" />
                                <RowStyle BackColor="White" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>
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
                    <div class="panel-group" id="Div9">
                        <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).DomicileFilter.Count > 0)
                            {
                                {
                                    int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).DomicileFilter.Count;
                                    Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a data-toggle='collapse' data-parent='#Div9' href='#collapseDomicile'>Domicile</a> <i class='ace-icon fa fa-arrow-down'></i><span style ='float:right;' class='badge' id ='DomicileSpan'>" + d + "</span></h4></div><div id='collapseDomicile' class='panel-collapse collapse out'><div class='list-group'>");
                                }
                                foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).DomicileFilter)
                                {
                                    { Response.Write("<a class='list-group-item' id='Domicile'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-minier btn-danger' style='float:right;'>[X]</button></a>"); }
                                }   { Response.Write("</div></div></div>"); }
                            }%>
                        </div>
                    <div class="panel-group" id="Div10">
                        <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).DegreeFilter.Count > 0)
                            {
                                {
                                    int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).DegreeFilter.Count;
                                    Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a data-toggle='collapse' data-parent='#Div10' href='#collapseDegree'>Degree</a> <i class='ace-icon fa fa-arrow-down'></i><span style ='float:right;' class='badge' id ='DegreeSpan'>" + d + "</span></h4></div><div id='collapseDegree' class='panel-collapse collapse out'><div class='list-group'>");
                                }
                                foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).DegreeFilter)
                                {
                                    { Response.Write("<a class='list-group-item' id='Degree'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-minier btn-danger' style='float:right;'>[X]</button></a>"); }
                                }   { Response.Write("</div></div></div>"); }
                            }%>
                        </div>
                    <div class="panel-group" id="Div11">
                        <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).InstituteFilter.Count > 0)
                            {
                                {
                                    int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).InstituteFilter.Count;
                                    Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a data-toggle='collapse' data-parent='#Div11' href='#collapseInstitute'>Institute</a> <i class='ace-icon fa fa-arrow-down'></i><span style ='float:right;' class='badge' id ='InstituteSpan'>" + d + "</span></h4></div><div id='collapseInstitute' class='panel-collapse collapse out'><div class='list-group'>");
                                }
                                foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).InstituteFilter)
                                {
                                    { Response.Write("<a class='list-group-item' id='Institute'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-minier btn-danger' style='float:right;'>[X]</button></a>"); }
                                }   { Response.Write("</div></div></div>"); }
                            }%>
                        </div>
                </section>
            </div>
        </div>
    </div>

</asp:Content>


